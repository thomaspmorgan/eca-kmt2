using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using ECA.Business.Service.Sevis;
using ECA.Core.Settings;
using System.IO;
using System.Diagnostics.Contracts;
using ECA.Business.Sevis;
using System.Net.Http;


namespace ECA.WebJobs.Sevis.Comm
{
    public class Functions : IDisposable
    {
        // This function will be triggered based on the schedule you have set for this WebJob

        /// <summary>
        /// The manual trigger method.
        /// </summary>
        /// <param name="service">The SEVIS batch services to get and update batches.</param>
        /// <param name="settings">The app settings.</param>
        [NoAutomaticTrigger]
        public void ManualTrigger(ISevisBatchProcessingService service, AppSettings settings)
        {
            Contract.Requires(service != null, "The SEVIS service must not be null.");
            Contract.Requires(settings != null, "The settings must not be null.");
            var task = Process(service, settings);
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

            // Check for Uploads to send
 
            var batchesToBeSent = service.GetSevisBatchesToUpload();
            Console.WriteLine(String.Format("Sevis Batches to Upload: [{0}]", batchesToBeSent.Count()));

            var batchComm = new SevisComm(settings);

            foreach(var batch in batchesToBeSent)
            {
                HttpResponseMessage status = new HttpResponseMessage();
                try
                {
                    status = await batchComm.UploadAsync(batch.SendXml, batch.BatchId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(String.Format("Exception: {0}", e.Message));
                }
                if (status.IsSuccessStatusCode)
                {
                    var updatedBatch = new UpdatedSevisBatchProcessing(batch);
                    updatedBatch.SubmitDate = DateTime.Now;
                    await service.UpdateAsync(updatedBatch);
                    Console.WriteLine("SEVIS Upload successful");
                }
                else
                    Console.WriteLine("SEVIS Upload failed, error: {0}", status.StatusCode.ToString());
            }

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
