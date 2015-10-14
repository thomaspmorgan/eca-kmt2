using ECA.Business.Search;
using ECA.Core.Settings;
using ECA.Data;
using Microsoft.Azure.Search;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebJobs.Search.Core
{
    /// <summary>
    /// The SearchUnityContainer is a unity container that can be used across multiple webjobs dealing with indexing.
    /// </summary>
    public class SearchUnityContainer : UnityContainer
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        public SearchUnityContainer(AppSettings appSettings)
        {
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            var apiKey = GetAzureSearchApiKey(appSettings);
            var serviceName = GetSeachServiceName(appSettings);
            var indexName = GetIndexName(appSettings);
            var connectionString = GetConnectionString(appSettings);

            //Register document configurations.
            this.RegisterType<IList<IDocumentConfiguration>>(new InjectionFactory((c) =>
            {
                return IndexService.GetAllConfigurations(typeof(ProgramDTODocumentConfiguration).Assembly).ToList();
            }));

            //Register ECA Context
            this.RegisterType<EcaContext>(new InjectionConstructor(connectionString));
            this.RegisterType<DbContext, EcaContext>(new InjectionConstructor(connectionString));

            //Register the notification service
            this.RegisterType<IIndexNotificationService, TextWriterIndexNotificationService>();

            //Register the search service client
            this.RegisterType<SearchServiceClient>(new InjectionFactory((c) =>
            {
                return new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            }));
            this.RegisterType<IIndexService>(new InjectionFactory((c) =>
            {
                var configs = IndexService.GetAllConfigurations(typeof(ProgramDTODocumentConfiguration).Assembly).ToList();
                var client = c.Resolve<SearchServiceClient>();
                var indexService = new IndexService(indexName, client, configs);
                return indexService;
            }));

            //Register the document services
            this.RegisterType<IList<IDocumentService>>(new InjectionFactory((c) =>
            {
                var list = new List<IDocumentService>();
                var context = c.Resolve<EcaContext>();
                var indexService = c.Resolve<IIndexService>();
                var notificationService = c.Resolve<IIndexNotificationService>();
                list.Add(new ProgramDocumentService(context, indexService, notificationService, 300));
                list.Add(new ProjectDocumentService(context, indexService, notificationService, 300));
                list.Add(new OfficeDocumentService(context, indexService, notificationService, 300));
                list.Add(new OrganizationDocumentService(context, indexService, notificationService, 300));
                return list;
            }));
        }
        
        /// <summary>
        /// Returns the azure search api key.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <returns>The azure search api key.</returns>
        public string GetAzureSearchApiKey(AppSettings appSettings)
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
        public string GetSeachServiceName(AppSettings appSettings)
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
        public string GetIndexName(AppSettings appSettings)
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
        public string GetConnectionString(AppSettings appSettings)
        {
            var connectionString = appSettings.EcaContextConnectionString.ConnectionString;
            LogMessage(String.Format("Using the connection string [{0}] to retrieve entities for documentation.", connectionString));
            return connectionString;
        }

        /// <summary>
        /// Writes the given message to the console.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
