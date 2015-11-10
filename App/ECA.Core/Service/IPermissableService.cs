using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Service
{
    /// <summary>
    /// An IPermissableService is capable of handling permissable objects.  Permissable objects are objects that are tracked
    /// by some kind of permissions service i.e. CAM.  The PermissableService that implements this interface will be given
    /// new and updated entities.
    /// </summary>
    [ContractClass(typeof(IPermissableServiceContract))]
    public interface IPermissableService
    {
        /// <summary>
        /// Will handle objects that have been created.
        /// </summary>
        /// <param name="addedEntities">The added entities.</param>
        List<AddedPermissableEntityResult> OnAdded(IList<IPermissable> addedEntities);

        /// <summary>
        /// Will handle objects that have been created.
        /// </summary>
        /// <param name="addedEntities">The added entities.</param>
        Task<List<AddedPermissableEntityResult>> OnAddedAsync(IList<IPermissable> addedEntities);

        /// <summary>
        /// Will handle objects that have been updated.
        /// </summary>
        /// <param name="updatedEntities">The updated entities.</param>
        void OnUpdated(IList<IPermissable> updatedEntities);

        /// <summary>
        /// Will handle objects that have been updated.
        /// </summary>
        /// <param name="updatedEntities">The updated entities.</param>
        Task OnUpdatedAsync(IList<IPermissable> updatedEntities);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IPermissableService))]
    public abstract class IPermissableServiceContract : IPermissableService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addedEntities"></param>
        public List<AddedPermissableEntityResult> OnAdded(IList<IPermissable> addedEntities)
        {
            Contract.Requires(addedEntities != null, "The added entities must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addedEntities"></param>
        /// <returns></returns>
        public Task<List<AddedPermissableEntityResult>> OnAddedAsync(IList<IPermissable> addedEntities)
        {
            Contract.Requires(addedEntities != null, "The added entities must not be null.");
            return Task.FromResult<List<AddedPermissableEntityResult>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedEntities"></param>
        public void OnUpdated(IList<IPermissable> updatedEntities)
        {
            Contract.Requires(updatedEntities != null, "The updated entities must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedEntities"></param>
        /// <returns></returns>
        public Task OnUpdatedAsync(IList<IPermissable> updatedEntities)
        {
            Contract.Requires(updatedEntities != null, "The updated entities must not be null.");
            return Task.FromResult<object>(null);
        }
    }
}
