using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using ECA.Business.Search;
using System.Diagnostics.Contracts;

namespace ECA.WebJobs.Search.Index
{
    public class Functions
    {
        private IList<IDocumentService> documentServices;
        private Action<IDocumentService, Guid> throwIfDocumentServiceDoesNotExist;

        /// <summary>
        /// The web job functions.
        /// </summary>
        /// <param name="documentServices">The document services.</param>
        public Functions(IList<IDocumentService> documentServices)
        {
            Contract.Requires(documentServices != null, "The document services must not be null.");
            this.documentServices = documentServices;
            throwIfDocumentServiceDoesNotExist = (service, documentTypeId) =>
            {
                if (service == null)
                {
                    throw new NotSupportedException(String.Format("A service as not found for the document type id [{0}].", documentTypeId));
                }
            };
        }

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called %searchindexqueue%.

        /// <summary>
        /// Accepts the message and handles the documents that must be created, updated, or deleted for the entities whose document keys are given
        /// in the message.
        /// </summary>
        /// <param name="message">The message containing the document keys for entities whos documents must be created, updated, or deleted.</param>
        /// <param name="log"></param>
        public void ProcessQueueMessage(
            [QueueTrigger("%searchindexqueue%")] IndexDocumentBatchMessage message,
            TextWriter log)
        {
            Contract.Requires(message != null, "The message batch must not be null.");
            HandleCreatedDocuments(message.CreatedDocuments.Distinct().ToList());
            HandleUpdatedDocuments(message.ModifiedDocuments.Distinct().ToList());
            HandleDeletedDocuments(message.DeletedDocuments.Distinct().ToList());
        }

        /// <summary>
        /// Returns the document service for the given document key as a string.
        /// </summary>
        /// <param name="documentKeyAsString">the document key as a string.</param>
        /// <returns>The document service.</returns>
        public IDocumentService GetDocumentService(string documentKeyAsString)
        {
            return GetDocumentService(new DocumentKey(documentKeyAsString));
        }

        /// <summary>
        /// Returns the document service for the given document key.
        /// </summary>
        /// <param name="key">The document key.</param>
        /// <returns>The document service.</returns>
        public IDocumentService GetDocumentService(DocumentKey key)
        {
            return GetDocumentService(key.DocumentTypeId);

        }

        /// <summary>
        /// Returns the document service for the document type id.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <returns>The document service.</returns>
        public IDocumentService GetDocumentService(Guid documentTypeId)
        {
            var documentService = this.documentServices.Where(x => x.GetDocumentTypeId() == documentTypeId).FirstOrDefault();
            throwIfDocumentServiceDoesNotExist(documentService, documentTypeId);
            return documentService;
        }

        /// <summary>
        /// Creates the documents with the given document keys as strings.
        /// </summary>
        /// <param name="updatedDocumentKeysAsStrings">The document keys as strings.</param>
        public void HandleCreatedDocuments(List<string> createdDocumentKeysAsStrings)
        {
            foreach (var documentKeyAsString in createdDocumentKeysAsStrings)
            {
                var service = GetDocumentService(documentKeyAsString);
                var documentKey = new DocumentKey(documentKeyAsString);
                service.AddOrUpdateDocument(documentKey.Value);
            }
        }

        /// <summary>
        /// Updates the documents with the given document keys as strings.
        /// </summary>
        /// <param name="updatedDocumentKeysAsStrings">The document keys as strings.</param>
        public void HandleUpdatedDocuments(List<string> updatedDocumentKeysAsStrings)
        {
            HandleCreatedDocuments(updatedDocumentKeysAsStrings);
        }

        /// <summary>
        /// Deletes the documents with the given keys using the appropriate document services.
        /// </summary>
        /// <param name="deletedDocumentKeysAsStrings">The document keys as strings of documents to delete.</param>
        public void HandleDeletedDocuments(List<string> deletedDocumentKeysAsStrings)
        {
            if (deletedDocumentKeysAsStrings.Count == 0)
            {
                return;
            }
            else
            {
                var documentKeys = deletedDocumentKeysAsStrings.Select(x => new DocumentKey(x)).ToList();
                var groupedByDocTypeIdKeys = from documentKey in documentKeys
                                             group documentKey by documentKey.DocumentTypeId into g
                                             select new
                                             {
                                                 DocumentTypeId = g.Key,
                                                 Keys = g.Select(x => x).ToList()
                                             };
                foreach (var group in groupedByDocTypeIdKeys)
                {
                    var service = GetDocumentService(group.DocumentTypeId);
                    service.DeleteDocuments(group.Keys.Select(x => x.Value).ToList());
                }
            }
        }
    }
}
