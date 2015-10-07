using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// An IIndexNotificationService is used to present notifications about the index service to a target.
    /// </summary>
    public interface IIndexNotificationService
    {
        /// <summary>
        /// The method to call when a document type has begun processing.
        /// </summary>
        /// <param name="documentTypeName">The document type name.</param>
        void Started(string documentTypeName);

        /// <summary>
        /// The method to call when a batch of documents has been processed.
        /// </summary>
        /// <param name="documentTypeName">The document type name.</param>
        /// <param name="totalDocumentsToProcess">the total number of documents to process.</param>
        /// <param name="documentsProcessed">The number of documents processed.</param>
        void Processed(string documentTypeName, int totalDocumentsToProcess, int documentsProcessed);

        /// <summary>
        /// The method to call when a document type has finished processing.
        /// </summary>
        /// <param name="documentTypeName">The document type.</param>
        void Finished(string documentTypeName);

        /// <summary>
        /// The method to call when a single document has started updating.
        /// </summary>
        /// <param name="documentTypeName">The document type name.</param>
        /// <param name="id">The document id.</param>
        void UpdateStarted(string documentTypeName, object id);

        /// <summary>
        /// The method to call when a single document has finished updating.
        /// </summary>
        /// <param name="documentTypeName">The document type name.</param>
        /// <param name="id">The document id.</param>
        void UpdateFinished(string documentTypeName, object id);
    }
}
