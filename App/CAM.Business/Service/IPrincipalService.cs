using System;
namespace CAM.Business.Service
{
    /// <summary>
    /// The PrincipalService is responsible for handling operations for cam principals.
    /// </summary>
    public interface IPrincipalService
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
    }
}
