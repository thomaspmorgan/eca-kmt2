using ECA.Core.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// A RelatedEntityDocumentSaveAction is used to update documents that are related to TEntity.  For
    /// example, if an address is created, then the related Organization document must be updated.  
    /// 
    /// Deleted and updated TEntity instances will have already had their scoped instance updated therefore,
    /// the entity's previous values are loaded from the database before the changes are applied.  This is
    /// useful, for example, if a Relationship has changed and two documents must be updated, e.g. a relationship
    /// was changed and one entity lost the relationship and another gained the relationship.
    /// </summary>
    /// <typeparam name="TEntity">The type of Entity that is being added, updated, or deleted.</typeparam>
    public abstract class RelatedEntityDocumentSaveAction<TEntity> : DocumentSaveAction<TEntity>
       where TEntity : class
    {
        /// <summary>
        /// Creats a new RelatedEntityDocumentSaveAction with the given app settings.
        /// </summary>
        /// <param name="settings">The app settings.</param>
        public RelatedEntityDocumentSaveAction(AppSettings settings) : base(settings)
        {
            Contract.Requires(settings != null, "The settings must not be null.");
            this.RelatedEntityDocumentKeys = new List<DocumentKey>();
        }

        /// <summary>
        /// Gets the document keys of all entities that are affected by TEntity's changes.
        /// </summary>
        public List<DocumentKey> RelatedEntityDocumentKeys { get; private set; }

        private List<TEntity> GetAllModifiedEntities()
        {
            return this.CreatedEntities.Union(this.ModifiedEntities).Union(this.DeletedEntities).Distinct().ToList();
        }

        private List<DbEntityEntry<TEntity>> GetAllModifiedAddressDbEntityEntries()
        {
            var entities = GetAllModifiedEntities();
            return entities.Select(x => GetEntityEntry(x)).ToList();
            
        }

        /// <summary>
        /// Locates created, deleted and modified TEntity instances and then create document keys
        /// based on the entity state.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void BeforeSaveChanges(DbContext context)
        {
            base.BeforeSaveChanges(context);
            var entries = GetAllModifiedAddressDbEntityEntries().ToList();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    HandleAddedEntityEntry(entry);
                }
                else
                {
                    var propertyValues = entry.GetDatabaseValues();
                    HandleNonAddedEntity(entry, propertyValues);
                }
            }
        }

        /// <summary>
        /// Locates created, deleted and modified TEntity instances and then create document keys
        /// based on the entity state.
        /// </summary>
        /// <param name="context">The context.</param>
        public override async Task BeforeSaveChangesAsync(DbContext context)
        {
            await base.BeforeSaveChangesAsync(context);
            var entries = GetAllModifiedAddressDbEntityEntries().ToList();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    await HandleAddedEntityEntryAsync(entry);
                }
                else
                {
                    var propertyValues = await entry.GetDatabaseValuesAsync();
                    await HandleNonAddedEntityAsync(entry, propertyValues);
                }
            }
        }

        private void HandleAddedEntityEntry(DbEntityEntry<TEntity> entry)
        {
            Contract.Assert(entry.State == EntityState.Added, "The entry entity state should be added.");
            this.RelatedEntityDocumentKeys = GetRelatedEntityDocumentKeysOfAddedEntity((TEntity)entry.Entity);
        }

        private async Task HandleAddedEntityEntryAsync(DbEntityEntry<TEntity> entry)
        {
            Contract.Assert(entry.State == EntityState.Added, "The entry entity state should be added.");
            this.RelatedEntityDocumentKeys = await GetRelatedEntityDocumentKeysOfAddedEntityAsync((TEntity)entry.Entity);
        }

        private void HandleNonAddedEntity(DbEntityEntry<TEntity> entry, DbPropertyValues propertyValues)
        {
            Contract.Assert(entry.State != EntityState.Added, "The entry entity state should not be added.");
            this.RelatedEntityDocumentKeys = GetRelatedEntityDocumentKeysOfModifiedEntity(entry.Entity, propertyValues);
        }

        private async Task HandleNonAddedEntityAsync(DbEntityEntry<TEntity> entry, DbPropertyValues propertyValues)
        {
            Contract.Assert(entry.State != EntityState.Added, "The entry entity state should not be added.");
            this.RelatedEntityDocumentKeys = await GetRelatedEntityDocumentKeysOfModifiedEntityAsync(entry.Entity, propertyValues);
        }

        /// <summary>
        /// Creates a new batch message that only contains modified documents because the create, delete, or update
        /// of an address implies another entity had a modified, created, or deleted address related to it.  Therefore,
        /// the organization, person, etc must be re-indexed.
        /// </summary>
        /// <returns>The batch message detailing the person or organization indexed document must be updated.</returns>
        public override IndexDocumentBatchMessage GetBatchMessage()
        {
            var baseMessage = new IndexDocumentBatchMessage();
            baseMessage.ModifiedDocuments = this.RelatedEntityDocumentKeys.Select(x => x.ToString()).ToList();
            return baseMessage;
        }

        /// <summary>
        /// This method is overridden and not used.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityEntry"></param>
        /// <returns></returns>
        public override DocumentKey GetDocumentKey(TEntity entity, DbEntityEntry<TEntity> entityEntry)
        {
            throw new NotSupportedException("This method intentionally not implemented.");
        }

        /// <summary>
        /// Returns the list of document keys for documents that must be modified based on the given TEntity entity that was
        /// created.
        /// </summary>
        /// <param name="addedEntity">The newly created entity.</param>
        /// <returns>The list of document keys for documents that must be updated.</returns>
        public abstract List<DocumentKey> GetRelatedEntityDocumentKeysOfAddedEntity(TEntity addedEntity);

        /// <summary>
        /// Returns the list of document keys for documents that must be modified based on the given TEntity entity that was
        /// created.
        /// </summary>
        /// <param name="addedEntity">The newly created entity.</param>
        /// <returns>The list of document keys for documents that must be updated.</returns>
        public abstract Task<List<DocumentKey>> GetRelatedEntityDocumentKeysOfAddedEntityAsync(TEntity addedEntity);

        /// <summary>
        /// Returns the list of document keys for documents that must be updated based on TEntity's modification
        /// or removal.
        /// </summary>
        /// <param name="originalValues">The original property values of the TEntity instance
        /// before the changes are applied to the database.</param>
        /// <param name="updatedOrDeletedEntity">The entity that has been updated or deleted.  Its values will have already been changed
        /// by business logic.  Therefore, old values must be obtained via the property values.  New values should be obtained from this instance.</param>
        /// <returns>The list of document keys for documents that must be updated.</returns>
        public abstract List<DocumentKey> GetRelatedEntityDocumentKeysOfModifiedEntity(TEntity updatedOrDeletedEntity, DbPropertyValues originalValues);

        /// <summary>
        /// Returns the list of document keys for documents that must be updated based on TEntity's modification
        /// or removal.
        /// </summary>
        /// <param name="originalValues">The original property values of the TEntity instance
        /// before the changes are applied to the database.</param>
        /// <param name="updatedOrDeletedEntity">The entity that has been updated or deleted.  Its values will have already been changed
        /// by business logic.  Therefore, old values must be obtained via the property values.  New values should be obtained from this instance.</param>
        /// <returns>The list of document keys for documents that must be updated.</returns>
        public abstract Task<List<DocumentKey>> GetRelatedEntityDocumentKeysOfModifiedEntityAsync(TEntity updatedOrDeletedEntity, DbPropertyValues originalValues);
    }
}
