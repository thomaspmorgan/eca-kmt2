using ECA.Core.Settings;
using ECA.WebJobs.Core;
using ECA.WebJobs.Search.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Practices.Unity;

namespace ECA.WebJobs.Search.Index
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var unityContainer = new UnityContainer();
            var config = new JobHostConfiguration
            {
                JobActivator = new UnityWebJobActivator(new SearchUnityContainer(new AppSettings())),
                NameResolver = new QueueNameResolver(new AppSettings()),
            };
            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
