using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using ECA.Data;
using ECA.Business.Search;
using Microsoft.Azure.Search;
using System;
using ECA.Core.Settings;

namespace ECA.WebJobs.Search
{
    public class Functions
    {
        // This function will be triggered based on the schedule you have set for this WebJob
        // This function will enqueue a message on an Azure Queue called search
        [NoAutomaticTrigger]
        public static void ManualTrigger(TextWriter log)
        {
            Index(log);
        }

        public static void Index(TextWriter log)
        {
            var appSettings = new AppSettings();
            var serviceName = appSettings.SearchServiceName;
            var apiKey = appSettings.SearchApiKey;
            var connectionString = appSettings.EcaContextConnectionString;
            var configs = IndexService.GetAllConfigurations(typeof(ProgramDTODocumentConfiguration).Assembly).ToList();

            var notificationService = new TextWriterIndexNotificationService(log);
            using (var context = new EcaContext(connectionString.ConnectionString))
            using (var client = new SearchServiceClient(serviceName, new SearchCredentials(apiKey)))
            using (var indexService = new IndexService(client, configs))
            using (var programDocumentService = new ProgramDocumentService(context, indexService, notificationService))
            using (var projectDocumentService = new ProjectDocumentService(context, indexService, notificationService))
            {
                var documentServices = new List<IDocumentService>();
                documentServices.Add(programDocumentService);
                documentServices.Add(projectDocumentService);
                documentServices.ForEach((x) =>
                {
                    x.Process();
                });
            }
        }
    }
}
