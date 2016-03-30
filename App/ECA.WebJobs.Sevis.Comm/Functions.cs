using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using ECA.Business.Service.Sevis;
using ECA.Core.Settings;
using System.Diagnostics.Contracts;
using System.Net.Http;
using ECA.WebJobs.Sevis;
using ECA.Business.Sevis;
using ECA.Business.Service;
using ECA.WebJobs.Sevis.Core;

namespace ECA.WebJobs.Sevis.Comm
{
    /// <summary>
    /// The Function class is responsible for processing sevis batches on a schedule.
    /// </summary>
    public class Functions : IDisposable
    {
        // This function will be triggered based on the schedule you have set for this WebJob

        private ISevisBatchProcessingService service;
        private AppSettings appSettings;

        /// <summary>
        /// Creates a new Functions instance.
        /// </summary>
        /// <param name="service">The sevis batch processing service.</param>
        /// <param name="appSettings">The app settings.</param>
        public Functions(ISevisBatchProcessingService service, AppSettings appSettings)
        {
            this.service = service;
            this.appSettings = appSettings;

        }

        /// <summary>
        /// Send, recieve, and process sevis batches using the service.  In a debug build this will run every 20 seconds.  In a release build it will run every 5 mins.
        /// </summary>
        /// <param name="info">The timer trigger instance.</param>
        /// <returns>The task<./returns>
        public async Task ProcessTimer(
#if DEBUG
            [TimerTrigger("00:00:20", RunOnStartup = true)] TimerInfo info
#else
            [TimerTrigger("00:05:00", RunOnStartup = true)] TimerInfo info
#endif   
            )
        {
            await ProcessAsync(this.service, this.appSettings);
            var nextOccurrenceMessage = info.FormatNextOccurrences(1);
            Console.WriteLine(nextOccurrenceMessage);
        }

        /// <summary>
        /// Performs processing of the SEVIS batch records.
        /// </summary>
        /// <param name="service">The SEVIS batch services to get and update batches.</param>
        /// <param name="settings">The app settings.</param>
        public async Task ProcessAsync(ISevisBatchProcessingService service, AppSettings settings)
        {
            Contract.Requires(service != null, "The SEVIS service must not be null.");
            Contract.Requires(settings != null, "The settings must not be null.");

            Console.WriteLine("Starting SEVIS Batch Processing");
            var systemUser = new User(Int32.Parse(settings.SystemUserId));

            //the staging of exchange visitors to send is now done in another web job, the xml will be pre populated
            //and returned in the dtos.

            var batchComm = new SevisComm(settings);
            var dtoToUpload = await service.GetNextBatchToUploadAsync();
            while (dtoToUpload != null)
            {
                //do the send here

                //string transactionLogXml = null;
                //var fileProvider = GetFileProvider();
                //await service.ProcessTransactionLogAsync(systemUser, transactionLogXml, fileProvider);
                dtoToUpload = await service.GetNextBatchToUploadAsync();
            }

            var batchByIdToDownload = await service.GetNextBatchByBatchIdToDownloadAsync();
            while (batchByIdToDownload != null)
            {

                //processing methods here or possibly another webjob
                //await service.ProcessTransactionLogAsync(string.Empty);
                batchByIdToDownload = await service.GetNextBatchByBatchIdToDownloadAsync();
            }

            //foreach (var batch in batchesToBeSent)
            //{
            //    HttpResponseMessage status = new HttpResponseMessage();
            //    try
            //    {
            //        status = await batchComm.UploadAsync(batch.SendXml, batch.BatchId);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(String.Format("Exception: {0}", e.Message));
            //    }
            //    if (status.IsSuccessStatusCode)
            //    {
            //        var updatedBatch = new UpdatedSevisBatchProcessing(batch);
            //        updatedBatch.SubmitDate = DateTime.Now;
            //        await service.UpdateAsync(updatedBatch);
            //        Console.WriteLine("SEVIS Upload successful");
            //    }
            //    else
            //        Console.WriteLine("SEVIS Upload failed, error: {0}", status.StatusCode.ToString());
            //}

            // Check for Downloads to get

            //var batchesToDownload = service.GetSevisBatchesToDownload();

            // Process any Downloads that have not been processed

            //var batchesToProcess = service.GetSevisBatchesToProcess();

            Console.WriteLine("Finished Batch Processing.");

        }

        /// <summary>
        /// Returns a DS2019FileProvider.  Change this method to provide the list of files when ready.
        /// </summary>
        /// <returns>The file provider.</returns>
        public IDS2019FileProvider GetFileProvider()
        {
            return new ZipArchiveDS2019FileProvider();
        }

        /// <summary>
        /// Returns the system user.
        /// </summary>
        /// <returns>The system user.</returns>
        public User GetSystemUser()
        {
            return new User(Int32.Parse(this.appSettings.SystemUserId));
        }

        #region IDispose

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.service is IDisposable)
                {
                    Console.WriteLine("Disposing of service " + this.service.GetType());
                    ((IDisposable)this.service).Dispose();
                    this.service = null;
                }
            }
        }

        #endregion
    }
}
