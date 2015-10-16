using ECA.Core.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace ECA.Business.Search
{
    /// <summary>
    /// A GenericDocumentSaveAction is used as a short hand for entities that do not have any special rules for indexing, for example,
    /// a Project is always going to have the same document type id and the method for selecting the project id is always going to be same.  Also,
    /// there is never a need to exclude projects from being indexed.  Therefore, a GenericDocumentSaveAction can be used to configured
    /// the Project entity's save action.
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericDocumentSaveAction<TEntity> : DocumentSaveAction<TEntity>
        where TEntity : class
    {
        private readonly Func<TEntity, object> idDelegate;
        private readonly Guid documentTypeId;

        /// <summary>
        /// Creates a new GenericDocumentSaveAction.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="documentTypeId">The TEntity's document type id.</param>
        /// <param name="idSelector">The expression to select the id from an instance of TEntity.</param>
        public GenericDocumentSaveAction(AppSettings appSettings, Guid documentTypeId, Expression<Func<TEntity, object>> idSelector) : base(appSettings)
        {
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            Contract.Requires(idSelector != null, "The id selector must not be null.");
            Contract.Requires(documentTypeId != Guid.Empty, "The document type id must not be null.");
            this.idDelegate = idSelector.Compile();
            this.documentTypeId = documentTypeId;
        }

        /// <summary>
        /// Returns the document key of TEntity.
        /// </summary>
        /// <param name="entity">The instance.</param>
        /// <param name="entityEntry">The DbContext entity entry.</param>
        /// <returns>The document key.</returns>
        public override DocumentKey GetDocumentKey(TEntity entity, DbEntityEntry<TEntity> entityEntry)
        {
            return new DocumentKey(this.documentTypeId, idDelegate(entity));
        }

        /// <summary>
        /// Returns false always.
        /// </summary>
        /// <param name="createdEntity">The entity.</param>
        /// <returns>False, the entity will never be excluded.</returns>
        public override bool IsCreatedEntityExcluded(TEntity createdEntity)
        {
            return false;
        }

        /// <summary>
        /// Returns false always.
        /// </summary>
        /// <param name="deletedEntity">The entity.</param>
        /// <returns>False, the entity will never be excluded.</returns>
        public override bool IsDeletedEntityExcluded(TEntity deletedEntity)
        {
            return false;
        }

        /// <summary>
        /// Returns false always.
        /// </summary>
        /// <param name="modifiedEntity">The entity.</param>
        /// <returns>False, the entity will never be excluded.</returns>
        public override bool IsModifiedEntityExcluded(TEntity modifiedEntity)
        {
            return false;
        }
    }
}
