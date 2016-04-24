using System;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using ECA.Business.Service.Sevis;
using ECA.Core.Settings;
using System.Diagnostics.Contracts;
using ECA.Business.Service;
using ECA.WebJobs.Sevis.Core;
using ECA.Net;
using System.Xml.Linq;
using NLog;
using ECA.Business.Queries.Models.Sevis;

namespace ECA.WebJobs.Sevis.Comm
{
    /// <summary>
    /// The Function class is responsible for processing sevis batches on a schedule.
    /// </summary>
    public class Functions : IDisposable
    {
        // This function will be triggered based on the schedule you have set for this WebJob

        private ISevisBatchProcessingService service;
        private ISevisApiResponseHandler responseHandler;
        private IEcaHttpMessageHandlerService ecaHttpMessageHandlerService;
        private AppSettings appSettings;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new Functions instance.
        /// </summary>
        /// <param name="service">The sevis batch processing service.</param>
        /// <param name="appSettings">The app settings.</param>
        public Functions(ISevisBatchProcessingService service, ISevisApiResponseHandler responseHandler, IEcaHttpMessageHandlerService theEcaHttpMessageHandlerService, AppSettings appSettings)
        {
            Contract.Requires(service != null, "The service must not be null.");
            Contract.Requires(responseHandler != null, "The response handler must not be null.");
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            Contract.Requires(theEcaHttpMessageHandlerService != null, "The app settings must not be null.");
            this.responseHandler = responseHandler;
            this.service = service;
            this.ecaHttpMessageHandlerService = theEcaHttpMessageHandlerService;
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
            var sevisComm = new SevisComm(this.appSettings, this.ecaHttpMessageHandlerService);
            await ProcessAsync(this.service, sevisComm, this.appSettings);
            var nextOccurrenceMessage = info.FormatNextOccurrences(1);
            logger.Info(nextOccurrenceMessage);
        }

        /// <summary>
        /// Performs processing of the SEVIS batch records.
        /// </summary>
        /// <param name="service">The SEVIS batch services to get and update batches.</param>
        /// <param name="sevisComm">The sevis batch comm object.</param>
        /// <param name="settings">The app settings.</param>
        public async Task ProcessAsync(ISevisBatchProcessingService service, SevisComm sevisComm, AppSettings settings)
        {
            Contract.Requires(service != null, "The SEVIS service must not be null.");
            Contract.Requires(settings != null, "The settings must not be null.");
            Contract.Requires(sevisComm != null, "The sevis comm must not be null.");
            logger.Info("Starting SEVIS Batch Processing");
            await DownloadBatchesAsync(sevisComm);
            await UploadBatchesAsync(sevisComm);

            logger.Debug("Removing processed batches");
            await service.DeleteProcessedBatchesAsync();
            logger.Info("Finished Batch Processing.");
        }

        #region Upload
        /// <summary>
        /// Uploads batches to the sevis api.
        /// </summary>
        /// <param name="batchComm">The communication class instance.</param>
        /// <returns>The task.</returns>
        public async Task UploadBatchesAsync(SevisComm batchComm)
        {
            Contract.Requires(batchComm != null, "The batchComm must not be null.");
            var dtoToUpload = await service.GetNextBatchToUploadAsync();
            while (dtoToUpload != null)
            {
                await UploadBatchAsync(batchComm, dtoToUpload);
                dtoToUpload = await service.GetNextBatchToUploadAsync();
            }
        }

        /// <summary>
        /// Uploads a batch to the sevis api.
        /// </summary>
        /// <param name="batchComm">The sevis communication instance.</param>
        /// <param name="dtoToUpload">The batch to upload.</param>
        /// <returns></returns>
        public async Task UploadBatchAsync(SevisComm batchComm, SevisBatchProcessingDTO dtoToUpload)
        {
            Contract.Requires(batchComm != null, "The batchComm must not be null.");
            Contract.Requires(dtoToUpload != null, "The dto to upload must not be null.");
            //do the send here
            logger.Info("Sending Upload, BatchId: {0}", dtoToUpload.BatchId);
            var response = await batchComm.UploadAsync(XElement.Parse(dtoToUpload.SendString), dtoToUpload.BatchId, dtoToUpload.SevisOrgId, dtoToUpload.SevisUsername);

            //process response message
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                await responseHandler.HandleUploadResponseStreamAsync(GetSystemUser(), dtoToUpload, stream);
                logger.Info("Processed Upload Response");
            }
            else
            {
                logger.Error("Upload encountered an error, status code: {0}, reason: {1}", response.StatusCode.ToString(), response.ReasonPhrase);
                await service.HandleFailedUploadBatchAsync(dtoToUpload.Id, null);
            }
        }
        #endregion

        #region Download

        /// <summary>
        /// Downloads results from the sevis api.
        /// </summary>
        /// <param name="batchComm">The batch communication instance.</param>
        /// <returns>The task.</returns>
        public async Task DownloadBatchesAsync(SevisComm batchComm)
        {
            Contract.Requires(batchComm != null, "The batchComm must not be null.");
            var dtoToDownload = await service.GetNextBatchToDownloadAsync();
            while (dtoToDownload != null)
            {
                await DownloadBatchAsync(batchComm, dtoToDownload);
                dtoToDownload = await service.GetNextBatchToDownloadAsync();
            }
        }

        /// <summary>
        /// Calls the sevis api for results to the sevis batch.
        /// </summary>
        /// <param name="batchComm">The sevis comm instance.</param>
        /// <param name="dtoToDownload">The batch to get download results for.</param>
        /// <returns>The task.</returns>
        public async Task DownloadBatchAsync(SevisComm batchComm, SevisBatchProcessingDTO dtoToDownload)
        {
            Contract.Requires(batchComm != null, "The batchComm must not be null.");
            Contract.Requires(dtoToDownload != null, "The dto to download must not be null.");
            // ask for download
            logger.Info("Getting Download, BatchId: {0}", dtoToDownload.BatchId);
            var response = await batchComm.DownloadAsync(dtoToDownload.BatchId, dtoToDownload.SevisOrgId, dtoToDownload.SevisUsername);

            //process response
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                await responseHandler.HandleDownloadResponseStreamAsync(GetSystemUser(), dtoToDownload, stream);
                logger.Info("Processed Download Response");
            }
            else
            {
                logger.Error("Download encountered an error, status code: {0}, reason: {1}", response.StatusCode.ToString(), response.ReasonPhrase);
                await service.HandleFailedDownloadBatchAsync(dtoToDownload.Id, null);
            }
        }
        #endregion

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
                if (this.responseHandler is IDisposable)
                {
                    logger.Trace("Disposing of response handler " + this.responseHandler.GetType());
                    ((IDisposable)this.responseHandler).Dispose();
                    this.responseHandler = null;
                }
                if (this.ecaHttpMessageHandlerService is IDisposable)
                {
                    logger.Trace("Disposing of response handler " + this.ecaHttpMessageHandlerService.GetType());
                    ((IDisposable)this.ecaHttpMessageHandlerService).Dispose();
                    this.ecaHttpMessageHandlerService = null;
                }
            }
        }

        #endregion
    }
}
