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
    public class Functions : IDisposable
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
            Contract.Requires(documentKeyAsString != null, "The key must not be null.");
            return GetDocumentService(new DocumentKey(documentKeyAsString));
        }

        /// <summary>
        /// Returns the document service for the given document key.
        /// </summary>
        /// <param name="key">The document key.</param>
        /// <returns>The document service.</returns>
        public IDocumentService GetDocumentService(DocumentKey key)
        {
            Contract.Requires(key != null, "The key must not be null.");
            return GetDocumentService(key.DocumentTypeId);

        }

        /// <summary>
        /// Returns the document service for the document type id.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <returns>The document service.</returns>
        public IDocumentService GetDocumentService(Guid documentTypeId)
        {
            Contract.Requires(documentTypeId != Guid.Empty, "The document type id must not be the empty guid.");
            var documentService = this.documentServices.Where(x => x.GetDocumentTypeId() == documentTypeId).FirstOrDefault();
            throwIfDocumentServiceDoesNotExist(documentService, documentTypeId);
            return documentService;
        }

        /// <summary>
        /// Creates the documents with the given document keys as strings.
        /// </summary>
        /// <param name="updatedDocumentKeysAsStrings">The document keys as strings.</param>
        public void HandleCreatedDocuments(IEnumerable<string> createdDocumentKeysAsStrings)
        {
            Contract.Requires(createdDocumentKeysAsStrings != null, "The createdDocumentKeysAsStrings must not be null.");
            foreach (var documentKeyAsString in createdDocumentKeysAsStrings)
            {   
                var service = GetDocumentService(documentKeyAsString);
                var documentKey = new DocumentKey(documentKeyAsString);
                Console.WriteLine(String.Format("Handling created or updated document with key {0}.", documentKey));
                service.AddOrUpdateDocument(documentKey.Value);
            }
        }

        /// <summary>
        /// Updates the documents with the given document keys as strings.
        /// </summary>
        /// <param name="updatedDocumentKeysAsStrings">The document keys as strings.</param>
        public void HandleUpdatedDocuments(IEnumerable<string> updatedDocumentKeysAsStrings)
        {
            Contract.Requires(updatedDocumentKeysAsStrings != null, "The updatedDocumentKeysAsStrings must not be null.");
            HandleCreatedDocuments(updatedDocumentKeysAsStrings);
        }

        /// <summary>
        /// Deletes the documents with the given keys using the appropriate document services.
        /// </summary>
        /// <param name="deletedDocumentKeysAsStrings">The document keys as strings of documents to delete.</param>
        public void HandleDeletedDocuments(IEnumerable<string> deletedDocumentKeysAsStrings)
        {
            Contract.Requires(deletedDocumentKeysAsStrings != null, "The updatedDocumentKeysAsStrings must not be null.");
            if (deletedDocumentKeysAsStrings.Count() == 0)
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
                    var ids = group.Keys.Select(x => x.Value).ToList();
                    Console.WriteLine(String.Format("Handling deleted documents of type [{0}] with ids [{1}].", group.DocumentTypeId, String.Join(", ", ids)));
                    service.DeleteDocuments(ids);
                }
            }
        }

        #region IDispose

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var service in this.documentServices)
                {
                    if (service is IDisposable)
                    {
                        Console.WriteLine("Disposing of service " + service.GetType());
                        ((IDisposable)service).Dispose();
                    }
                }
            }
        }

        #endregion
    }
}
