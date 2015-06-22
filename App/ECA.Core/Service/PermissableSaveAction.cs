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
    public class PermissableSaveAction : ISaveAction
    {
        private readonly IPermissableService service;

        public PermissableSaveAction(IPermissableService permissableService)
        {
            Contract.Requires(permissableService != null, "The permissable service must not be null.");
            this.service = permissableService;
            this.AddedEntities = new List<IPermissable>();
            this.ModifiedEntities = new List<IPermissable>();
        }

        public List<IPermissable> AddedEntities { get; private set; }

        public List<IPermissable> ModifiedEntities { get; private set; }

        public void BeforeSaveChanges(DbContext context)
        {
            OnBeforeSaveChanges(context);
        }

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


        public void AfterSaveChanges(DbContext context)
        {
            this.service.OnAdded(this.AddedEntities);
            this.service.OnUpdated(this.ModifiedEntities);
        }

        public async Task AfterSaveChangesAsync(DbContext context)
        {
            await this.service.OnAddedAsync(this.AddedEntities);
            await this.service.OnUpdatedAsync(this.ModifiedEntities);
        }

        public IList<IPermissable> GetAddedPermissableEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return GetPermissableEntities(context, EntityState.Added);
        }

        public IList<IPermissable> GetUpdatedPermissableEntities(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return GetPermissableEntities(context, EntityState.Modified);
        }

        public IList<IPermissable> GetPermissableEntities(DbContext context, EntityState state)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(state != null, "The state must not be null.");
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
