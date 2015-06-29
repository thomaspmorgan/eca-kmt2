using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Service
{
    /// <summary>
    /// The PermissableSaveAction is a save action that will retrieve and track added or updated entities and 
    /// provide them to the given IPermissableService.
    /// </summary>
    public class PermissableSaveAction : ISaveAction
    {
        private readonly IPermissableService service;

        /// <summary>
        /// Creates a new PermissableSaveAction with the given service.
        /// </summary>
        /// <param name="permissableService">The service to handle created or updated entities in a DbContext.</param>
        public PermissableSaveAction(IPermissableService permissableService)
        {
            Contract.Requires(permissableService != null, "The permissable service must not be null.");
            this.service = permissableService;
            this.AddedEntities = new List<IPermissable>();
            this.ModifiedEntities = new List<IPermissable>();
        }

        /// <summary>
        /// Gets the added entities.
        /// </summary>
        public List<IPermissable> AddedEntities { get; private set; }

        /// <summary>
        /// Gets the modified entities.
        /// </summary>
        public List<IPermissable> ModifiedEntities { get; private set; }

        /// <summary>
        /// Retrieves the added and updated permissable entities in the DbContext.
        /// </summary>
        /// <param name="context">The context to retrieve new and updated IPermissable entities.</param>
        public void BeforeSaveChanges(DbContext context)
        {
            OnBeforeSaveChanges(context);
        }

        /// <summary>
        /// Retrieves the added and updated permissable entities in the DbContext.
        /// </summary>
        /// <param name="context">The context to retrieve new and updated IPermissable entities.</param>
        public Task BeforeSaveChangesAsync(DbContext context)
        {
            OnBeforeSaveChanges(context);
            return Task.FromResult<object>(null);
        }

        private void OnBeforeSaveChanges(DbContext context)
        {
            this.AddedEntities = GetAddedPermissableEntities(context).ToList();
            this.ModifiedEntities = GetUpdatedPermissableEntities(context).ToList();
        }

        /// <summary>
        /// Passes the added and modified permissable entities to the permissable service.
        /// </summary>
        /// <param name="context">The DbContext with the permissable entities.</param>
        public void AfterSaveChanges(DbContext context)
        {
            this.service.OnAdded(this.AddedEntities);
            this.service.OnUpdated(this.ModifiedEntities);
        }

        /// <summary>
        /// Passes the added and modified permissable entities to the permissable service.
        /// </summary>
        /// <param name="context">The DbContext with the permissable entities.</param>
        public async Task AfterSaveChangesAsync(DbContext context)
        {
            await this.service.OnAddedAsync(this.AddedEntities);
            await this.service.OnUpdatedAsync(this.ModifiedEntities);
        }

        /// <summary>
        /// Retrieves the added permissable entities from the given context.
        /// </summary>
        /// <param name="context">The context to retrieve added permissable entities.</param>
        /// <returns>The added entities.</returns>
        public IList<IPermissable> GetAddedPermissableEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return GetPermissableEntities(context, EntityState.Added);
        }

        /// <summary>
        /// Retrieves the modified permissable entities from the given context.
        /// </summary>
        /// <param name="context">The context to retrieve modified permissable entities.</param>
        /// <returns>The modified entities.</returns>
        public IList<IPermissable> GetUpdatedPermissableEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return GetPermissableEntities(context, EntityState.Modified);
        }

        /// <summary>
        /// Retrieves the entities from the given context with the entity state.
        /// </summary>
        /// <param name="context">The context to retrieve the permissable entities from.</param>
        /// <param name="state">The entity state.</param>
        /// <returns>The permissable entities with the given state.</returns>
        public IList<IPermissable> GetPermissableEntities(DbContext context, EntityState state)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var addedEntities = context.ChangeTracker.Entries().Where(x => x.State == state).ToList();
            var permissableType = typeof(IPermissable);
            var addedPermissableEntities = addedEntities
                .Where(a => permissableType.IsAssignableFrom(a.Entity.GetType()))
                .Select(x => (IPermissable)x.Entity)
                .ToList();
            return addedPermissableEntities;
        }
    }
}
