using ECA.Core.Data;
using ECA.Core.Service;
using ECA.Core.Settings;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// The DocumentsSaveAction is used to handle created, modified, and deleted entities that 
    /// are also used as azure search document entities, such as program, or project.
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public class DocumentsSaveAction<TDocument> : ISaveAction
        where TDocument : class, IIdentifiable
    {
        private Guid documentTypeId;
        private AppSettings appSettings;

        /// <summary>
        /// Creates a new save action with the document type id and the app settings.
        /// </summary>
        /// <param name="documentTypeId">The document type id.  This id should correspond to the same guid
        /// as the document type in the document configuration.</param>
        /// <param name="appSettings">The app settings.</param>
        public DocumentsSaveAction(Guid documentTypeId, AppSettings appSettings, DocumentSaveActionConfiguration<TDocument> configuration = null)
        {
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            Contract.Requires(documentTypeId != Guid.Empty, "The document type id must not be empty.");
            this.documentTypeId = documentTypeId;
            this.appSettings = appSettings;
            this.CreatedDocuments = new List<TDocument>();
            this.DeletedDocuments = new List<TDocument>();
            this.ModifiedDocuments = new List<TDocument>();
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the save action configuration.
        /// </summary>
        public DocumentSaveActionConfiguration<TDocument> Configuration { get; private set; }

        /// <summary>
        /// Gets the added documents.
        /// </summary>
        public List<TDocument> CreatedDocuments { get; private set; }

        /// <summary>
        /// Gets the modified documents.
        /// </summary>
        public List<TDocument> ModifiedDocuments { get; private set; }

        /// <summary>
        /// Gets the deleted documents.
        /// </summary>
        public List<TDocument> DeletedDocuments { get; private set; }

        /// <summary>
        /// Retrieves the added entities from the given context.
        /// </summary>
        /// <param name="context">The context to retrieve added entities.</param>
        /// <returns>The added entities.</returns>
        public IList<TDocument> GetCreatedDocumentEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var createdDocuments = GetDocumentEntities(context, EntityState.Added).ToList();
            if (this.Configuration != null && this.Configuration.CreatedExclusionRules != null)
            {
                var excludedDocuments = GetDocumentsToExclude(createdDocuments, this.Configuration.CreatedExclusionRules.ToList());
                excludedDocuments.ForEach(x => createdDocuments.Remove(x));
            }
            return createdDocuments;
        }

        /// <summary>
        /// Retrieves the modified entities from the given context.
        /// </summary>
        /// <param name="context">The context to retrieve modified entities.</param>
        /// <returns>The modified entities.</returns>
        public IList<TDocument> GetModifiedDocumentEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var modifiedDocuments = GetDocumentEntities(context, EntityState.Modified).ToList();
            if (this.Configuration != null && this.Configuration.ModifiedExclusionRules != null)
            {
                var excludedDocuments = GetDocumentsToExclude(modifiedDocuments, this.Configuration.ModifiedExclusionRules.ToList());
                excludedDocuments.ForEach(x => modifiedDocuments.Remove(x));
            }
            return modifiedDocuments;
        }

        /// <summary>
        /// Retrieves the modified entities from the given context.
        /// </summary>
        /// <param name="context">The context to retrieve modified entities.</param>
        /// <returns>The modified entities.</returns>
        public IList<TDocument> GetDeletedDocumentEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var deletedDocuments = GetDocumentEntities(context, EntityState.Deleted).ToList();
            if (this.Configuration != null && this.Configuration.DeletedExclusionRules != null)
            {
                var excludedDocuments = GetDocumentsToExclude(deletedDocuments, this.Configuration.DeletedExclusionRules.ToList());
                excludedDocuments.ForEach(x => deletedDocuments.Remove(x));
            }
            return deletedDocuments;
        }

        /// <summary>
        /// Retrieves the entities from the given context with the entity state.
        /// </summary>
        /// <param name="context">The context to retrieve the entities from.</param>
        /// <param name="state">The entity state.</param>
        /// <returns>The entities with the given state.</returns>
        public IList<TDocument> GetDocumentEntities(DbContext context, EntityState state)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var changedEntities = context.ChangeTracker.Entries().Where(x => x.State == state).ToList();
            var documentType = typeof(TDocument);
            var changeDocumentEntities = changedEntities
                .Where(a => documentType.IsAssignableFrom(a.Entity.GetType()))
                .Select(x => (TDocument)x.Entity)
                .ToList();
            return changeDocumentEntities;
        }


        private void OnBeforeSaveChanges(DbContext context)
        {
            this.CreatedDocuments = GetCreatedDocumentEntities(context).ToList();
            this.ModifiedDocuments = GetModifiedDocumentEntities(context).ToList();
            this.DeletedDocuments = GetDeletedDocumentEntities(context).ToList();
        }

        private List<TDocument> GetDocumentsToExclude(List<TDocument> allDocuments, List<Func<TDocument, bool>> exclusionRules)
        {
            var excludedDocuments = new List<TDocument>();
            foreach (var document in allDocuments)
            {
                foreach (var exclusionRule in exclusionRules)
                {
                    var exclude = exclusionRule(document);
                    if (exclude)
                    {
                        excludedDocuments.Add(document);
                        break;
                    }
                }
            }
            return excludedDocuments;
        }

        #region ISaveAction

        /// <summary>
        /// Finds created, modified, and deleted entities.
        /// </summary>
        /// <param name="context">The context.</param>
        public void BeforeSaveChanges(DbContext context)
        {
            OnBeforeSaveChanges(context);
        }

        /// <summary>
        /// Finds created, modified, and deleted entities.
        /// </summary>
        /// <param name="context">The context.</param>
        public Task BeforeSaveChangesAsync(DbContext context)
        {
            OnBeforeSaveChanges(context);
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Sends a message to the azure queue if there are entities whose documents should be updated.
        /// </summary>
        /// <param name="context">The context.</param>
        public void AfterSaveChanges(DbContext context)
        {
            var batchMessage = GetBatchMessage();
            if (batchMessage.HasDocumentsToHandle())
            {
                var queue = GetCloudQueueClient();
                queue.CreateIfNotExists();
                queue.AddMessage(GetCloudQueueMessage(batchMessage));
            }
        }

        /// <summary>
        /// Sends a message to the azure queue if there are entities whose documents should be updated.
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task AfterSaveChangesAsync(DbContext context)
        {
            var batchMessage = GetBatchMessage();
            if (batchMessage.HasDocumentsToHandle())
            {
                var queue = GetCloudQueueClient();
                await queue.CreateIfNotExistsAsync();
                await queue.AddMessageAsync(GetCloudQueueMessage(batchMessage));
            }
        }

        private CloudQueue GetCloudQueueClient()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(appSettings.AzureWebJobsStorageConnectionString.ConnectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(appSettings.SearchDocumentQueueName);
            return queue;
        }

        private CloudQueueMessage GetCloudQueueMessage(IndexDocumentBatchMessage message)
        {
            return new CloudQueueMessage(JsonConvert.SerializeObject(message));
        }

        /// <summary>
        /// Returns the message to send to the azure queue.
        /// </summary>
        /// <returns>The azure queue message.</returns>
        public IndexDocumentBatchMessage GetBatchMessage()
        {
            var batch = new IndexDocumentBatchMessage();
            batch.CreatedDocuments = this.CreatedDocuments.Select(x => new DocumentKey(documentTypeId, x.GetId()).ToString()).ToList();
            batch.DeletedDocuments = this.DeletedDocuments.Select(x => new DocumentKey(documentTypeId, x.GetId()).ToString()).ToList();
            batch.ModifiedDocuments = this.ModifiedDocuments.Select(x => new DocumentKey(documentTypeId, x.GetId()).ToString()).ToList();
            return batch;
        }

        #endregion
    }
}
