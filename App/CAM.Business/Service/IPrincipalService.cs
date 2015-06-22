using ECA.Core.Service;
using System;
using System.Diagnostics.Contracts;
namespace CAM.Business.Service
{
    /// <summary>
    /// The PrincipalService is responsible for handling operations for cam principals.
    /// </summary>
    [ContractClass(typeof(PrincipalServiceContract))]
    public interface IPrincipalService : ISaveable
    {
        /// <summary>
        /// Grants a permission to a principal in the system.  If the permission had been previously granted it is set active.
        /// </summary>
        /// <param name="grantedPermission">The permission granted to a principal by another principal.</param>
        void GrantPermission(CAM.Business.Model.GrantedPermission grantedPermission);

        /// <summary>
        /// Grants a permission to a principal in the system.  If the permission had been previously granted it is set active.
        /// </summary>
        /// <param name="grantedPermission">The permission granted to a principal by another principal.</param>
        System.Threading.Tasks.Task GrantPermissionsAsync(CAM.Business.Model.GrantedPermission grantedPermission);

        /// <summary>
        /// Revoke a permission explicity from a user.
        /// </summary>
        /// <param name="revokedPermission">The revoked permission.</param>
        void RevokePermission(CAM.Business.Model.RevokedPermission revokedPermission);

        /// <summary>
        /// Revoke a permission explicity from a user.
        /// </summary>
        /// <param name="revokedPermission">The revoked permission.</param>
        System.Threading.Tasks.Task RevokePermissionAsync(CAM.Business.Model.RevokedPermission revokedPermission);

        /// <summary>
        /// Deletes a permission assignment.
        /// </summary>
        /// <param name="permission">The deleted permission.</param>
        void DeletePermission(CAM.Business.Model.DeletedPermission permission);

        /// <summary>
        /// Deletes a permission assignment.
        /// </summary>
        /// <param name="permission">The deleted permission.</param>
        System.Threading.Tasks.Task DeletePermissionAsync(CAM.Business.Model.DeletedPermission permission);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IPrincipalService))]
    public abstract class PrincipalServiceContract : IPrincipalService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grantedPermission"></param>
        public void GrantPermission(Model.GrantedPermission grantedPermission)
        {
            Contract.Requires(grantedPermission != null, "The granted permission must not be null.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grantedPermission"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task GrantPermissionsAsync(Model.GrantedPermission grantedPermission)
        {
            Contract.Requires(grantedPermission != null, "The granted permission must not be null.");
            return System.Threading.Tasks.Task.FromResult<object>(null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="revokedPermission"></param>
        public void RevokePermission(Model.RevokedPermission revokedPermission)
        {
            Contract.Requires(revokedPermission != null, "The revoked permission must not be null.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="revokedPermission"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task RevokePermissionAsync(Model.RevokedPermission revokedPermission)
        {
            Contract.Requires(revokedPermission != null, "The revoked permission must not be null.");
            return System.Threading.Tasks.Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveActions"></param>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveActions"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            return System.Threading.Tasks.Task.FromResult<int>(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permission"></param>
        public void DeletePermission(Model.DeletedPermission permission)
        {
            Contract.Requires(permission != null, "The permission must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task DeletePermissionAsync(Model.DeletedPermission permission)
        {
            Contract.Requires(permission != null, "The permission must not be null.");
            return System.Threading.Tasks.Task.FromResult<object>(null);
        }
    }
}
