using System;
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
using System.Xml.Linq;
using NLog;
using System.Xml.Serialization;
using System.IO.Compression;
using System.IO;

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
        private static Logger logger = LogManager.GetCurrentClassLogger();

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
            logger.Info(nextOccurrenceMessage);
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
    
        
            logger.Info("Starting SEVIS Batch Processing");

            //the staging of exchange visitors to send is now done in another web job, the xml will be pre populated
            //and returned in the dtos.

            var batchComm = new SevisComm(settings);
            var dtoToUpload = await service.GetNextBatchToUploadAsync();

            while (dtoToUpload != null)
            {
                //do the send here
                logger.Info("Sending Upload, BatchId: {0}", dtoToUpload.BatchId);
                var response = await batchComm.UploadAsync(XElement.Parse(dtoToUpload.SendString), dtoToUpload.BatchId, dtoToUpload.SevisOrgId, dtoToUpload.SevisUsername);
                
                //process response message
                if (response.IsSuccessStatusCode)
                {
                    string xmlString = await response.Content.ReadAsStringAsync();
                    await service.ProcessTransactionLogAsync(GetSystemUser(), dtoToUpload.BatchId, xmlString, GetFileProvider());
                    logger.Info("Processed Upload Response");
                }
                else
                {
                    logger.Error("Upload encountered an error, status code: {0}, reason: {1}", response.StatusCode.ToString(), response.ReasonPhrase);
                }
                dtoToUpload = await service.GetNextBatchToUploadAsync();
            }

            var dtoToDownload = await service.GetNextBatchToDownloadAsync();
            
            while (dtoToDownload != null)
            {
                // ask for download
                logger.Info("Getting Download, BatchId: {0}", dtoToDownload.BatchId);
                var response = await batchComm.DownloadAsync(dtoToDownload.BatchId, dtoToDownload.SevisOrgId, dtoToDownload.SevisUsername);

                //process response
                if (response.IsSuccessStatusCode)
                {
                    ZipArchive archive = new ZipArchive(await response.Content.ReadAsStreamAsync());
                    ZipArchiveEntry entryTransactionLog = archive.GetEntry("sevis_transaction_log.xml");
                    var xmlFileStreamReader = new StreamReader(entryTransactionLog.Open());
                    string transactionLogXml = await xmlFileStreamReader.ReadToEndAsync();
                    await service.ProcessTransactionLogAsync(GetSystemUser(), dtoToDownload.BatchId, transactionLogXml, GetFileProvider());
                }
                else
                {
                    logger.Error("Download encountered an error, status code: {0}, reason: {1}", response.StatusCode.ToString(), response.ReasonPhrase);
                }
                dtoToDownload = await service.GetNextBatchToDownloadAsync();
            }
            logger.Debug("Removing processed batches");
            await service.DeleteProcessedBatchesAsync();

            logger.Info("Finished Batch Processing.");

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
                    logger.Trace("Disposing of service " + this.service.GetType());
                    ((IDisposable)this.service).Dispose();
                    this.service = null;
                }
            }
        }

        #endregion
    }
}
