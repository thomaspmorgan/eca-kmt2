using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog.Interface;
using CAM.Data;

namespace CAM.Business.Service
{
    public class PermissionStoreCached : PermissionStoreBase, IPermissionStore<IPermission>
    {
        #region Properties

        private readonly ILogger logger = new LoggerAdapter(NLog.LogManager.GetCurrentClassLogger());

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the PermissionsStore and loads a cached version of the PermissionLookups
        /// </summary>
        public PermissionStoreCached(CamModel model, IPermissionModelService permissionModelService, IResourceService resourceService)
            : base(model, permissionModelService, resourceService)
        {
            ResourceId = null;
            PrincipalId = null;
            Permissions = new List<IPermission>();
        }

        /// <summary>
        /// Instantiates the PermissionsStore and loads a cached version of the PermissionLookups
        /// </summary>
        /// <param name="applicationId">Sets the ApplicationResourceId property given the ApplicationId</param>
        public PermissionStoreCached(int applicationId, CamModel model, IPermissionModelService permissionModelService, IResourceService resourceService)
            : base(model, permissionModelService, resourceService)
        {
            ResourceId = resourceService.GetResourceIdForApplicationId(applicationId);
            PrincipalId = null;
        }

        /// <summary>
        /// Instantiates the PermissionsStore and loads a cached version of the PermissionLookups
        /// </summary>
        /// <param name="applicationId">Sets the ApplicationResourceId property</param>
        /// <param name="principalId">Sets the PrincipalId property</param>
        public PermissionStoreCached(int applicationId, int principalId, CamModel model, IPermissionModelService permissionModelService, IResourceService resourceService)
            : base(model, permissionModelService, resourceService)
        {
            ResourceId = resourceService.GetResourceIdForApplicationId(applicationId);
            PrincipalId = principalId;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the Permissions list property with permission for the given User or Group Id and ResourceId, permissions are cached based on settings or the CacheManager class
        /// </summary>
        /// <param name="principleId">User or Group Id</param>
        /// <param name="resourceId">Id of the Resource</param>
        public void LoadUserPermissionsForResource(int principleId, int resourceId)
        {
            logger.Trace("Getting User (PrincipalId={0}) Permissions for Resource={1}", principleId, resourceId);


            string key = principleId.ToString() + "-" + resourceId.ToString();

            Permissions = CacheManager.Get<List<IPermission>>(key);

            logger.Debug("Permissions found in cache? {0}", (Permissions != null) ? "Yes" : "No");

            if (Permissions == null)
            {
                Permissions = GetUserPermissionsForResource(principleId, resourceId);
                if (Permissions != null)
                    CacheManager.Add<List<IPermission>>(Permissions, key);
            }
        }

        /// <summary>
        /// Load all the permissions for a given User or Group Id.  Permissions are cached based on settings of the CacheManager class
        /// </summary>
        /// <param name="principleId">User or Group Id</param>
        public void LoadUserPermissions(int principleId)
        {
            logger.Trace("Getting User (PrincipalId={0}) Permissions", principleId);

            string key = principleId.ToString();

            Permissions = CacheManager.Get<List<IPermission>>(key);

            logger.Debug("Permissions found in cache? {0}", (Permissions != null) ? "Yes" : "No");

            if (Permissions == null)
            {
                Permissions = GetUserPermissions(principleId);
                if (Permissions != null)
                    CacheManager.Add<List<IPermission>>(Permissions, key);
            }
        }

   
        #endregion
    }

}
