using ECA.Core.Data;
using ECA.Core.Service;
using ECA.Core.Settings;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// The DocumentSaveAction is used to handle created, modified, and deleted entities that 
    /// are the basis of an indexed document.  For example, the creation of a project should trigger a new project dto 
    /// document to be created.
    /// </summary>
    /// <typeparam name="TEntity">The entity type in the context that has been created, modified, or deleted.</typeparam>
    public abstract class DocumentSaveAction<TEntity> : ISaveAction
        where TEntity : class
    {
        private AppSettings appSettings;

        /// <summary>
        /// Creates a new save action with the document type id and the app settings.
        /// </summary>
        /// <param name="documentTypeId">The document type id.  This id should correspond to the same guid
        /// as the document type in the document configuration.</param>
        /// <param name="appSettings">The app settings.</param>
        public DocumentSaveAction(AppSettings appSettings)
        {
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            this.appSettings = appSettings;
            this.CreatedEntities = new List<TEntity>();
            this.DeletedEntities = new List<TEntity>();
            this.ModifiedEntities = new List<TEntity>();
        }

        /// <summary>
        /// Gets the added entities.
        /// </summary>
        public List<TEntity> CreatedEntities { get; private set; }

        /// <summary>
        /// Gets the modified entities.
        /// </summary>
        public List<TEntity> ModifiedEntities { get; private set; }

        /// <summary>
        /// Gets the deleted entities.
        /// </summary>
        public List<TEntity> DeletedEntities { get; private set; }

        /// <summary>
        /// Gets or sets the Context used in this save action.  It will automatically get set by the BeforeSaveChanges methods.
        /// </summary>
        public DbContext Context { get; set; }

        /// <summary>
        /// Retrieves the added entities from the given context.
        /// </summary>
        /// <param name="context">The context to retrieve added entities.</param>
        /// <returns>The added entities.</returns>
        public IList<TEntity> GetCreatedDocumentEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var createdDocuments = GetDocumentEntities(context, EntityState.Added)
                .Where(x => !IsCreatedEntityExcluded(x))
                .ToList();
            return createdDocuments;
        }

        /// <summary>
        /// Retrieves the modified entities from the given context.
        /// </summary>
        /// <param name="context">The context to retrieve modified entities.</param>
        /// <returns>The modified entities.</returns>
        public IList<TEntity> GetModifiedDocumentEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var modifiedDocuments = GetDocumentEntities(context, EntityState.Modified)
                .Where(x => !IsModifiedEntityExcluded(x))
                .ToList();
            return modifiedDocuments;
        }

        /// <summary>
        /// Retrieves the modified entities from the given context.
        /// </summary>
        /// <param name="context">The context to retrieve modified entities.</param>
        /// <returns>The modified entities.</returns>
        public IList<TEntity> GetDeletedDocumentEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var deletedDocuments = GetDocumentEntities(context, EntityState.Deleted)
                .Where(x => !IsDeletedEntityExcluded(x))
                .ToList();
            return deletedDocuments;
        }

        /// <summary>
        /// Retrieves the entities from the given context with the entity state.
        /// </summary>
        /// <param name="context">The context to retrieve the entities from.</param>
        /// <param name="state">The entity state.</param>
        /// <returns>The entities with the given state.</returns>
        public IList<TEntity> GetDocumentEntities(DbContext context, EntityState state)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var changedEntities = context.ChangeTracker.Entries().Where(x => x.State == state).ToList();
            var documentType = typeof(TEntity);
            var changeDocumentEntities = changedEntities
                .Where(a => documentType.IsAssignableFrom(a.Entity.GetType()))
                .Select(x => (TEntity)x.Entity)
                .ToList();
            return changeDocumentEntities;
        }

        private void OnBeforeSaveChanges(DbContext context)
        {
            this.Context = context;
            this.CreatedEntities = GetCreatedDocumentEntities(context).ToList();
            this.ModifiedEntities = GetModifiedDocumentEntities(context).ToList();
            this.DeletedEntities = GetDeletedDocumentEntities(context).ToList();
        }

        #region ISaveAction

        /// <summary>
        /// Finds created, modified, and deleted entities.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void BeforeSaveChanges(DbContext context)
        {
            OnBeforeSaveChanges(context);
        }

        /// <summary>
        /// Finds created, modified, and deleted entities.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual Task BeforeSaveChangesAsync(DbContext context)
        {
            OnBeforeSaveChanges(context);
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Sends a message to the azure queue if there are entities whose documents should be updated.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void AfterSaveChanges(DbContext context)
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
        public virtual async Task AfterSaveChangesAsync(DbContext context)
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
        public virtual IndexDocumentBatchMessage GetBatchMessage()
        {
            var batch = new IndexDocumentBatchMessage();
            batch.CreatedDocuments = this.CreatedEntities.Select(x => GetDocumentKey(x, GetEntityEntry(x)).ToString()).ToList();
            batch.DeletedDocuments = this.DeletedEntities.Select(x => GetDocumentKey(x, GetEntityEntry(x)).ToString()).ToList();
            batch.ModifiedDocuments = this.ModifiedEntities.Select(x => GetDocumentKey(x, GetEntityEntry(x)).ToString()).ToList();
            return batch;
        }

        /// <summary>
        /// Returns the entity entry from the context.
        /// </summary>
        /// <param name="entity">The entity to get the entry for.</param>
        /// <returns>The DbEntityEntry for the given entity.</returns>
        protected DbEntityEntry<TEntity> GetEntityEntry(TEntity entity)
        {
            return this.Context.Entry<TEntity>(entity);
        }
        #endregion

        /// <summary>
        /// Returns true if the given entity should be excluded from the indexing process.
        /// </summary>
        /// <param name="createdEntity">The created entity.</param>
        /// <returns>True, if the given created entity should be excluded from the indexing process.</returns>
        public abstract bool IsCreatedEntityExcluded(TEntity createdEntity);

        /// <summary>
        /// Returns true if the given entity should be excluded from the indexing process.
        /// </summary>
        /// <param name="modifiedEntity">The modified entity.</param>
        /// <returns>True, if the given modified entity should be excluded from the indexing process.</returns>
        public abstract bool IsModifiedEntityExcluded(TEntity modifiedEntity);

        /// <summary>
        /// Returns true if the given entity should be excluded from the indexing process.
        /// </summary>
        /// <param name="deletedEntity">The deleted entity.</param>
        /// <returns>True, if the given deleted entity should be excluded from the indexing process.</returns>
        public abstract bool IsDeletedEntityExcluded(TEntity deletedEntity);

        /// <summary>
        /// Returns the document key of the given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityEntry">The entity entry from the DbContext.</param>
        /// <returns>The document key.</returns>
        public abstract DocumentKey GetDocumentKey(TEntity entity, DbEntityEntry<TEntity> entityEntry);
    }
}
