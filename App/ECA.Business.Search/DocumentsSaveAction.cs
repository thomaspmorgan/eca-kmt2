using ECA.Core.Data;
using ECA.Core.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public class DocumentsSaveAction<TDocument> : ISaveAction//, IDisposable
        where TDocument : class
    {
        private IDocumentService documentService;

        public DocumentsSaveAction(IDocumentService documentService)
        {
            Contract.Requires(documentService != null, "The document service must not be null.");
            this.documentService = documentService;
        }

        /// <summary>
        /// Gets the added documents.
        /// </summary>
        public List<TDocument> AddedDocuments { get; private set; }

        /// <summary>
        /// Gets the modified documents.
        /// </summary>
        public List<TDocument> ModifiedDocuments { get; private set; }

        /// <summary>
        /// Retrieves the added permissable entities from the given context.
        /// </summary>
        /// <param name="context">The context to retrieve added permissable entities.</param>
        /// <returns>The added entities.</returns>
        public IList<TDocument> GetAddedDocumentEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return GetDocumentEntities(context, EntityState.Added);
        }

        /// <summary>
        /// Retrieves the modified permissable entities from the given context.
        /// </summary>
        /// <param name="context">The context to retrieve modified permissable entities.</param>
        /// <returns>The modified entities.</returns>
        public IList<TDocument> GetModifiedDocumentEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return GetDocumentEntities(context, EntityState.Modified);
        }

        /// <summary>
        /// Retrieves the entities from the given context with the entity state.
        /// </summary>
        /// <param name="context">The context to retrieve the permissable entities from.</param>
        /// <param name="state">The entity state.</param>
        /// <returns>The permissable entities with the given state.</returns>
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
            this.AddedDocuments = GetAddedDocumentEntities(context).ToList();
            this.ModifiedDocuments = GetModifiedDocumentEntities(context).ToList();
        }

        #region ISaveAction
        public void BeforeSaveChanges(DbContext context)
        {
            OnBeforeSaveChanges(context);
        }

        public Task BeforeSaveChangesAsync(DbContext context)
        {
            OnBeforeSaveChanges(context);
            return Task.FromResult<object>(null);
        }

        public void AfterSaveChanges(DbContext context)
        {
            
        }

        public Task AfterSaveChangesAsync(DbContext context)
        {
            throw new Exception();
        }
        
        #endregion
    }
}
