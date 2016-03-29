﻿using System;
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
    public class Functions : IDisposable
    {
        // This function will be triggered based on the schedule you have set for this WebJob

        private ISevisBatchProcessingService service;
        private AppSettings appSettings;
        private ZipArchiveDS2019FileProvider fileProvider;

        public Functions(ISevisBatchProcessingService service, AppSettings appSettings, ZipArchiveDS2019FileProvider fileProvider)
        {
            this.service = service;
            this.appSettings = appSettings;
            this.fileProvider = fileProvider;

        }

        /// <summary>
        /// Process batch sevis batches using the service.  In a debug build this will run every 20 seconds.  In a release build it will run every 5 mins.
        /// </summary>
        /// <param name="info">The timer trigger instance.</param>
        /// <returns>The task</returns>
        public async Task ProcessTimer(
#if DEBUG
            [TimerTrigger("00:00:20", RunOnStartup = true)] TimerInfo info
#else
            [TimerTrigger("00:05:00", RunOnStartup = true)] TimerInfo info
#endif   
            )
        {
            await Process(this.service, this.appSettings);
            var nextOccurrenceMessage = info.FormatNextOccurrences(1);
            Console.WriteLine(nextOccurrenceMessage);
        }

        /// <summary>
        /// Performs processing of the SEVIS batch records.
        /// </summary>
        /// <param name="service">The SEVIS batch services to get and update batches.</param>
        /// <param name="settings">The app settings.</param>
        public async Task Process(ISevisBatchProcessingService service, AppSettings settings)
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

                string transactionLogXml = null;
                await service.ProcessTransactionLogAsync(systemUser, transactionLogXml, this.fileProvider);
                await service.SaveChangesAsync();
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
            //if (disposing)
            //{
            //        if (service is IDisposable)
            //        {
            //            Console.WriteLine("Disposing of service " + service.GetType());
            //            ((IDisposable)service).Dispose();
            //        }
            //    }
            //}
        }

        #endregion
    }
}
