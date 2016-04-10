using ECA.Business.Search;
using ECA.Core.Settings;
using ECA.WebJobs.Core;
using ECA.WebJobs.Search.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace ECA.WebJobs.Search.Index.All
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976

    /// <summary>
    /// The Program to execute as a webjob.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Please set the following connection strings in app.config for this WebJob to run: AzureWebJobsDashboard and AzureWebJobsStorage
        /// </summary>
        public static void Main()
        {
            var unityContainer = new SearchUnityContainer(new AppSettings());
            var config = new JobHostConfiguration
            {
                JobActivator = new UnityWebJobActivator(unityContainer)
            };
            var services = unityContainer.Resolve<IList<IDocumentService>>();
            var indexService = unityContainer.Resolve<IIndexService>();
            var host = new JobHost();
            host.Call(typeof(Functions).GetMethod("ManualTrigger"), new { documentServices = services, indexService = indexService, settings = new AppSettings()});
        }
    }
}
