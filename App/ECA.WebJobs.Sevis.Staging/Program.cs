using ECA.Business.Service.Sevis;
using ECA.Core.Settings;
using ECA.WebJobs.Core;
using ECA.WebJobs.Sevis.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Practices.Unity;

namespace ECA.WebJobs.Sevis.Staging
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var host = new JobHost();
            var unityContainer = new SevisUnityContainer(new AppSettings());
            var config = new JobHostConfiguration
            {
                JobActivator = new UnityWebJobActivator(unityContainer)
            };
            var service = unityContainer.Resolve<ISevisBatchProcessingService>();
            // The following code will invoke a function called ManualTrigger and 
            // pass in data (value in this case) to the function
            host.Call(typeof(Functions).GetMethod("ManualTrigger"), new { service = service });
        }
    }
}
