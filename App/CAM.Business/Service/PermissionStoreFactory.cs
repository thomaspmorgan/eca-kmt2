using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAM.Business.Service
{
    public class PermissionStoreFactory
    {
        /// <summary>
        /// Returns and instance of an IPermissionStore, for a given principalId.
        /// </summary>
        /// <param name="principalId">User or Group ID</param>
        /// <param name="cached">Optional parameter (default true) for whether the permissions are cached.  See the CacheManager <see cref="CAM.Business.CacheManager">CacheManager</see> class to see how the permissions are cached.  Default cache timeout is 10 minutes.</param>
        /// <returns>An instance of a IPermissionStore object having all the permission for the user/group</returns>
        public static IPermissionStore<IPermission> GetPermissionStore(int principalId, bool cached = true)
        {
            IPermissionStore<IPermission> permissionStore;
            if (cached)
                permissionStore = new PermissionStoreCached();
            else
                permissionStore = new PermissionStore();

            permissionStore.LoadUserPermissions(principalId);
            permissionStore.PrincipalId = principalId;
            return permissionStore;
        }

        /// <summary>
        /// Returns and instance of an IPermissionStore, for a given principalId and resourceId
        /// </summary>
        /// <param name="principalId">User or Group ID</param>
        /// <param name="resourceId">Resource ID, from table Resource</param>
        /// <param name="cached">Optional parameter (default true) for whether the permissions are cached.  See <see cref="CAM.Business.CacheManager">CacheManager</see> class to see how the permissions are cached.  Default cache timeout is 10 minutes.</param>
        /// <returns>An instance of a IPermissionStore object having all the permissions for the user/group for an application</returns>
        public static IPermissionStore<IPermission> GetPermissionStoreForUserByResource(int principalId, int resourceId, bool cached = true)
        {
            IPermissionStore<IPermission> permissionStore;
            if (cached)
                permissionStore = new PermissionStoreCached();
            else
                permissionStore = new PermissionStore();

            permissionStore.LoadUserPermissionsForResource(principalId,
                                                           resourceId);
            permissionStore.PrincipalId = principalId;
            permissionStore.ResourceId = resourceId;
            return permissionStore;
        }

    }
}

