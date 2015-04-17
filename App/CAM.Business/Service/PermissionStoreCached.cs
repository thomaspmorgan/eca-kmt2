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
        public PermissionStoreCached()
        {
            LoadPermissionsLookup();
            ResourceId = null;
            PrincipalId = null;
            Permissions = new List<IPermission>();
        }

        /// <summary>
        /// Instantiates the PermissionsStore and loads a cached version of the PermissionLookups
        /// </summary>
        /// <param name="applicationId">Sets the ApplicationResourceId property given the ApplicationId</param>
        public PermissionStoreCached(int applicationId)
        {
            ResourceId = GetResourceIdForApplicationId(applicationId);
            PrincipalId = null;
            LoadPermissionsLookup();
        }

        /// <summary>
        /// Instantiates the PermissionsStore and loads a cached version of the PermissionLookups
        /// </summary>
        /// <param name="applicationId">Sets the ApplicationResourceId property</param>
        /// <param name="principalId">Sets the PrincipalId property</param>
        public PermissionStoreCached(int applicationId, int principalId)
        {
            ResourceId = GetResourceIdForApplicationId(applicationId);
            PrincipalId = principalId;
            LoadPermissionsLookup();
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

        #region Private Methods

        /// <summary>
        /// used in PermissionStoreCached constructor to load a cached list (property PermissionLookup) to facilitate lookup by Id or Name
        /// </summary>
        private void LoadPermissionsLookup()
        {
            logger.Trace("Loading PermissionLookup");

            string key = "PermissionsLookup";

            PermissionLookup = CacheManager.Get<List<PermissionModel>>(key);

            logger.Debug("PermissionLookup found in cache? {0}", (PermissionLookup != null) ? "Yes" : "No");

            if (PermissionLookup == null)
            {
                PermissionLookup = GetPermissionLookup();
                if (PermissionLookup != null)
                    CacheManager.Add<List<PermissionModel>>(PermissionLookup, key);
            }
        }

        #endregion
    }

}
