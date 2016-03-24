using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using ECA.Business.Service.Sevis;
using ECA.Business.Service;

namespace ECA.WebJobs.Sevis.Staging
{
    public class Functions
    {
        // This function will be triggered based on the schedule you have set for this WebJob
        // This function will enqueue a message on an Azure Queue called queue
        [NoAutomaticTrigger]
        public static async Task ManualTrigger(TextWriter log, ISevisBatchProcessingService service)
        {
            await StageSevisBatchesAsync(log, service);
        }

        public static async Task StageSevisBatchesAsync(TextWriter log, ISevisBatchProcessingService service)
        {
            log.WriteLine("Staging participants into sevis batches...");
            var user = new User(1);
            await service.StageBatchesAsync(user);
            log.WriteLine("Finished staging participants into sevis batches.");
        }
    }
}
