using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using ECA.Business.Search;
using System.Diagnostics.Contracts;
using System;
using ECA.Core.Settings;

namespace ECA.WebJobs.Search.Index.All
{
    /// <summary>
    /// The starter class for the indexing operations to run in the azure search webjob.
    /// </summary>
    public class Functions
    {
        // This function will be triggered based on the schedule you have set for this WebJob
        // This function will enqueue a message on an Azure Queue called search

        /// <summary>
        /// The manual trigger method.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="documentServices">The document services to index with.</param>
        /// <param name="indexService">The index service.</param>
        /// <param name="settings">The app settings.</param>
        [NoAutomaticTrigger]
        public void ManualTrigger(TextWriter log, IList<IDocumentService> documentServices, IIndexService indexService, AppSettings settings)
        {
            Contract.Requires(documentServices != null, "The document services must not be null.");
            Contract.Requires(indexService != null, "The index service must not be null.");
            Contract.Requires(settings != null, "The settings must not be null.");
            Index(documentServices, indexService, settings);
        }

        /// <summary>
        /// Performs the indexing via all document services provided.
        /// </summary>
        /// <param name="documentServices">The document services to use for indexing.</param>
        /// <param name="indexService">The index service.</param>
        /// <param name="settings">The app settings.</param>
        public void Index(IList<IDocumentService> documentServices, IIndexService indexService, AppSettings settings)
        {
            Contract.Requires(documentServices != null, "The document services must not be null.");
            Contract.Requires(indexService != null, "The index service must not be null.");
            Contract.Requires(settings != null, "The settings must not be null.");
            var indexName = settings.SearchIndexName;

            Console.WriteLine(String.Format("Deleting search index named {0}...", indexName));
            indexService.DeleteIndex(indexName);
            Console.WriteLine("Deleted search index.");
            var list = documentServices.ToList();
            var totalDocuments = 0;
            list.ForEach(x =>
            {
                totalDocuments += x.GetDocumentCount();
            });
            Console.WriteLine(String.Format("Found a total of {0} documents to process.", totalDocuments));
            list.ForEach(x =>
            {
                Console.WriteLine(String.Format("Processing documents via {0}.", x.GetType()));
                x.AddOrUpdateAll();
            });
            list.ForEach(x =>
            {
                if (x is IDisposable)
                {
                    Console.WriteLine(String.Format("Disposing {0}.", x.GetType()));
                    (x as IDisposable).Dispose();
                }
            });
            if(indexService is IDisposable)
            {
                Console.WriteLine(String.Format("Disposing {0}.", indexService.GetType()));
                ((IDisposable)indexService).Dispose();
            }
            Console.WriteLine(String.Format("Finished indexing {0} documents.", totalDocuments));
        }
    }
}
