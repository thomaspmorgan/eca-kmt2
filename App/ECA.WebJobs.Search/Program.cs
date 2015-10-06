using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Practices.Unity;
using ECA.Core.Settings;
using ECA.Business.Search;
using ECA.Data;
using System.Data.Entity;
using Microsoft.Azure.Search;

namespace ECA.WebJobs.Search
{

    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976

    /// <summary>
    /// The Program to execute as a webjob.
    /// </summary>
    public class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        public static void Main()
        {
            var unityContainer = new UnityContainer();
            var config = new JobHostConfiguration
            {
                JobActivator = new UnityWebJobActivator(GetUnityContainer(unityContainer))
            };
            var services = unityContainer.Resolve<IList<IDocumentService>>();
            var host = new JobHost();
            host.Call(typeof(Functions).GetMethod("ManualTrigger"), new { documentServices = services });
        }

        /// <summary>
        /// Returns the unity container with class registrations..
        /// </summary>
        /// <param name="container">The unity container to register to.</param>
        /// <returns>The filled unity container.</returns>
        public static IUnityContainer GetUnityContainer(IUnityContainer container)
        {
            var appSettings = new AppSettings();
            var apiKey = GetAzureSearchApiKey(appSettings);
            var serviceName = GetSeachServiceName(appSettings);
            var indexName = GetIndexName(appSettings);
            var connectionString = GetConnectionString(appSettings);

            //Register document configurations.
            container.RegisterType<IList<IDocumentConfiguration>>(new InjectionFactory((c) =>
            {
                return IndexService.GetAllConfigurations(typeof(ProgramDTODocumentConfiguration).Assembly).ToList();
            }));

            //Register ECA Context
            container.RegisterType<EcaContext>(new HierarchicalLifetimeManager(), new InjectionConstructor(connectionString));
            container.RegisterType<DbContext, EcaContext>(new HierarchicalLifetimeManager(), new InjectionConstructor(connectionString));

            //Register the notification service
            container.RegisterType<IIndexNotificationService, TextWriterIndexNotificationService>();

            //Register the search service client
            container.RegisterType<SearchServiceClient>(new HierarchicalLifetimeManager(), new InjectionFactory((c) =>
            {
                return new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            }));
            container.RegisterType<IIndexService>(new HierarchicalLifetimeManager(), new InjectionFactory((c) =>
            {
                var configs = IndexService.GetAllConfigurations(typeof(ProgramDTODocumentConfiguration).Assembly).ToList();
                var client = c.Resolve<SearchServiceClient>();
                var indexService = new IndexService(indexName, client, configs);
                return indexService;
            }));

            //Register the document services
            container.RegisterType<IList<IDocumentService>>(new HierarchicalLifetimeManager(), new InjectionFactory((c) =>
            {
                var list = new List<IDocumentService>();
                var context = c.Resolve<EcaContext>();
                var indexService = c.Resolve<IIndexService>();
                var notificationService = c.Resolve<IIndexNotificationService>();
                list.Add(new ProgramDocumentService(context, indexService, notificationService, 300));
                list.Add(new ProjectDocumentService(context, indexService, notificationService, 300));
                return list;
            }));

            return container;
        }

        /// <summary>
        /// Returns the azure search api key.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <returns>The azure search api key.</returns>
        public static string GetAzureSearchApiKey(AppSettings appSettings)
        {
            var apiKey = appSettings.SearchApiKey;
            LogMessage(String.Format("The azure search api key is [{0}].", apiKey));
            return apiKey;
        }

        /// <summary>
        /// Returns the name of the azure search service.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <returns>The azure search service name.</returns>
        public static string GetSeachServiceName(AppSettings appSettings)
        {
            var searchServiceName = appSettings.SearchServiceName;
            LogMessage(String.Format("The azure search service name is [{0}].", searchServiceName));
            return searchServiceName;
        }

        /// <summary>
        /// Returns the index name to operate against.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <returns>The index name.</returns>
        public static string GetIndexName(AppSettings appSettings)
        {
            var indexName = appSettings.SearchIndexName;
            LogMessage(String.Format("The index name to operate against [{0}].", indexName));
            return indexName;
        }

        /// <summary>
        /// Returns the eca connection string.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <returns>The connection string.</returns>
        public static string GetConnectionString(AppSettings appSettings)
        {
            var connectionString = appSettings.EcaContextConnectionString.ConnectionString;
            LogMessage(String.Format("Using the connection string [{0}] to retrieve entities for documentation.", connectionString));
            return connectionString;
        }

        /// <summary>
        /// Writes the given message to the console.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
