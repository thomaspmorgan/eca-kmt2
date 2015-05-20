using ECA.Core.Service;
using System;
namespace ECA.WebApi.Security
{
    /// <summary>
    /// An IResourceAuthorizationHandler is a single point to handle crud operations on user permissions, roles, and resources.
    /// 
    /// The implementation should be used throughout the web api to modify user permissions.
    /// </summary>
    public interface IResourceAuthorizationHandler : ISaveable
    {
        /// <summary>
        /// Grants a permission to a user given the model.
        /// </summary>
        /// <param name="deletedPermission">The model containing the deleted permission.</param>
        /// <returns>The task.</returns>
        System.Threading.Tasks.Task DeletePermissionAsync(ECA.WebApi.Models.Security.IDeletedPermissionBindingModel deletedPermission);

        /// <summary>
        /// Grants a permission to a user given the model.
        /// </summary>
        /// <param name="grantedPermission">The model containing the granted permission.</param>
        /// <returns>The task.</returns>
        System.Threading.Tasks.Task GrantPermissionAsync(ECA.WebApi.Models.Security.IGrantedPermissionBindingModel grantedPermission);

        /// <summary>
        /// Grants a permission to a user given the model.
        /// </summary>
        /// <param name="revokedPermission">The model containing the revoked permission.</param>
        /// <returns>The task.</returns>
        System.Threading.Tasks.Task RevokePermissionAsync(ECA.WebApi.Models.Security.IRevokedPermissionBindingModel revokedPermission);
    }
}
