using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog.Interface;
using CAM.Data;
using System.Threading.Tasks;

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
        #endregion

        #region Public Methods

        /// <summary>
        /// Load all the permissions for a given User or Group Id.  Permissions are cached based on settings of the CacheManager class
        /// </summary>
        /// <param name="principleId">User or Group Id</param>
        public void LoadUserPermissions(int principleId)
        {
            logger.Trace("Getting User (PrincipalId={0}) Permissions", principleId);
            var key = GetKey(principleId);
            Permissions = CacheManager.Get<List<IPermission>>(key);
            logger.Debug("Permissions found in cache? {0}", (Permissions != null) ? "Yes" : "No");
            if (Permissions == null)
            {
                Permissions = GetUserPermissions(principleId);
                DoCache(principleId, Permissions);
            }
        }

        /// <summary>
        /// Load all the permissions for a given User or Group Id.  Permissions are cached based on settings of the CacheManager class
        /// </summary>
        /// <param name="principleId">User or Group Id</param>
        public async Task LoadUserPermissionsAsync(int principleId)
        {
            logger.Trace("Getting User (PrincipalId={0}) Permissions", principleId);
            var key = GetKey(principleId);
            Permissions = CacheManager.Get<List<IPermission>>(key);
            logger.Debug("Permissions found in cache? {0}", (Permissions != null) ? "Yes" : "No");
            if (Permissions == null)
            {
                Permissions = await GetUserPermissionsAsync(principleId);
                DoCache(principleId, Permissions);
            }
        }

        private string GetKey(int principalId)
        {
            return principalId.ToString();
        }

        private void DoCache(int principalId, List<IPermission> permissions)
        {
            var key = GetKey(principalId);
            CacheManager.Add<List<IPermission>>(permissions, key);
        }
   
        #endregion
    }

}
