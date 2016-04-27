using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using ECA.WebJobs.Core;
using ECA.WebJobs.Sevis.Core;
using ECA.Core.Settings;

namespace ECA.WebJobs.Sevis.Validation
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var unityContainer = new SevisUnityContainer(new AppSettings());
            var config = new JobHostConfiguration
            {
                JobActivator = new UnityWebJobActivator(unityContainer),
            };
#if DEBUG
            config.UseDevelopmentSettings();
#endif
            config.UseTimers();
            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
