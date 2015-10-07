using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using ECA.Business.Search;
using System.Diagnostics.Contracts;
using System;

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
        [NoAutomaticTrigger]
        public void ManualTrigger(TextWriter log, IList<IDocumentService> documentServices)
        {
            Index(log, documentServices);
        }

        /// <summary>
        /// Performs the indexing via all document services provided.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="documentServices">The document services to use for indexing.</param>
        public void Index(TextWriter log, IList<IDocumentService> documentServices)
        {
            Contract.Requires(documentServices != null, "The document services must not be null.");
            var list = documentServices.ToList();
            list.ForEach(x =>
            {
                x.AddOrUpdateAll();
            });
            list.ForEach(x =>
            {
                if (x is IDisposable)
                {
                    (x as IDisposable).Dispose();
                }
            });
        }
    }
}
