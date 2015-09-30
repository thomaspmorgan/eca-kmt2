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

namespace ECA.WebJobs.Search
{
    public class Functions
    {
        // This function will be triggered based on the schedule you have set for this WebJob
        // This function will enqueue a message on an Azure Queue called search
        [NoAutomaticTrigger]
        public static void ManualTrigger(TextWriter log, int value, [Queue("search")] out string message)
        {
            log.WriteLine("Function is invoked with value={0}", value);
            message = value.ToString();
            Index(log);
            log.WriteLine("Following message will be written on the Queue={0}", message);
        }

        public static void Index(TextWriter log)
        {
            var serviceName = AppSettings.SearchServiceName;
            var apiKey = AppSettings.SearchApiKey;
            var connectionString = AppSettings.EcaContextConnectionString;
            var configs = IndexService.GetAllConfigurations(typeof(ProgramDTODocumentConfiguration).Assembly).ToList();

            var notificationService = new TextWriterIndexNotificationService(log);
            using (var context = new EcaContext(connectionString.ConnectionString))
            using (var client = new SearchServiceClient(serviceName, new SearchCredentials(apiKey)))
            using (var indexService = new IndexService(client, configs))
            using (var programDocumentService = new ProgramDocumentService(context, indexService, notificationService))
            using (var projectDocumentService = new ProjectDocumentService(context, indexService, notificationService))
            {
                Console.WriteLine(context.Database.Connection.ConnectionString);
                Console.WriteLine(apiKey);
                Console.WriteLine(serviceName);
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
