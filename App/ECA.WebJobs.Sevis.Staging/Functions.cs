using ECA.Business.Service;
using ECA.Business.Service.Sevis;
using ECA.Core.Settings;
using Microsoft.Azure.WebJobs;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.WebJobs.Sevis.Staging
{
    /// <summary>
    /// The Functions class is executed in a WebJob to stage sevis batches.
    /// </summary>
    public class Functions : IDisposable
    {
        private ISevisBatchProcessingService service;
        private AppSettings appSettings;

        /// <summary>
        /// Creates a new instance with the service to use and the app settings.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="appSettings">The app settings.</param>
        public Functions(ISevisBatchProcessingService service, AppSettings appSettings)
        {
            Contract.Requires(service != null, "The service must not be null.");
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            this.service = service;
            this.appSettings = appSettings;
        }

        //https://azure.microsoft.com/en-us/documentation/articles/websites-dotnet-webjobs-sdk/#schedule
        //https://github.com/Azure/azure-webjobs-sdk-extensions

        //run at startup and once every 20 secs in debug or once every 5 mins in release

        /// <summary>
        /// Stages sevis batches using the service.  In a debug build this will run every 20 seconds.  In a release build it will run every 5 mins.
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
            Console.WriteLine(String.Format("Processing sevis batches as system user with id [{0}] and max created exchange visitors [{1}] and max updated exchange visitors [{2}].", 
                this.appSettings.SystemUserId, 
                this.appSettings.MaxCreateExchangeVisitorRecordsPerBatch, 
                this.appSettings.MaxUpdateExchangeVisitorRecordsPerBatch));
            await this.service.StageBatchesAsync();
            var nextOccurrenceMessage = info.FormatNextOccurrences(1);
            Console.WriteLine(nextOccurrenceMessage);
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
