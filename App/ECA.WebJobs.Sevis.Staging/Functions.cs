using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using ECA.Business.Service.Sevis;
using ECA.Business.Service;
using System;
using System.Diagnostics.Contracts;
using Microsoft.Azure.WebJobs.Extensions.Timers;

namespace ECA.WebJobs.Sevis.Staging
{
    public class Functions : IDisposable
    {
        private ISevisBatchProcessingService service;
        public Functions(ISevisBatchProcessingService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        //https://azure.microsoft.com/en-us/documentation/articles/websites-dotnet-webjobs-sdk/#schedule
        //https://github.com/Azure/azure-webjobs-sdk-extensions
       
        //run at startup and once every 20 secs in debug or once every 15 mins in release
        public async Task ProcessTimer(
#if DEBUG
            [TimerTrigger("00:00:20", RunOnStartup = true)] TimerInfo info
#else
            [TimerTrigger("00:15:00", RunOnStartup = true)] TimerInfo info
#endif   
            )
        {
            await StageSevisBatchesAsync();
            var nextOccurrenceMessage = info.FormatNextOccurrences(1);
            Console.WriteLine(nextOccurrenceMessage);
        }

        public async Task StageSevisBatchesAsync()
        {
            Console.WriteLine("Staging participants into sevis batches...");
            var user = new User(1);
            await this.service.StageBatchesAsync(user);
            Console.WriteLine("Finished staging participants into sevis batches.");
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
                }
            }
        }

        #endregion
    }
}
