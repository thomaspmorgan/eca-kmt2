using ECA.Business.Service.Sevis;
using ECA.Core.Settings;
using ECA.WebJobs.Core;
using ECA.WebJobs.Sevis.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Practices.Unity;

namespace ECA.WebJobs.Sevis.Staging
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    public class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        public static void Main()
        {
            
            var unityContainer = new SevisUnityContainer(new AppSettings());
            var config = new JobHostConfiguration
            {
                JobActivator = new UnityWebJobActivator(unityContainer),
            };
            config.UseTimers();
            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
