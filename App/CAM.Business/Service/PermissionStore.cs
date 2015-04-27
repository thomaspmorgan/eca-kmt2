using System.Collections.Generic;
using System.Linq;
using CAM.Data;
using NLog.Interface;
using CAM.Business;

namespace CAM.Business.Service
{
    public class PermissionStore : PermissionStoreBase, IPermissionStore<IPermission>
    {
        #region Properties

        private readonly ILogger logger = new LoggerAdapter(NLog.LogManager.GetCurrentClassLogger());

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the PermissionsStore
        /// </summary>
        public PermissionStore(CamModel model)
            : base(model)
        {
            ResourceId = null;
            PrincipalId = null;
            Permissions = new List<IPermission>();
        }

        /// <summary>
        /// Instantiates the PermissionsStore 
        /// </summary>
        /// <param name="applicationId">Sets the ApplicationResourceId property given the ApplicationId</param>
        public PermissionStore(int resourceId, CamModel model)
            : base(model)
        {
            ResourceId = GetResourceIdForApplicationId(resourceId);
            PrincipalId = null;
        }

        /// <summary>
        /// Instantiates the PermissionsStore 
        /// </summary>
        /// <param name="resourceId">Sets the ApplicationResourceId property</param>
        /// <param name="principalId">Sets the PrincipalId property</param>
        public PermissionStore(int resourceId, int principalId, CamModel model)
            : base(model)
        {
            ResourceId = GetResourceIdForApplicationId(resourceId);
            PrincipalId = principalId;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the Permissions list property with permission for the given User or Group Id and ResourceId
        /// </summary>
        /// <param name="principleId">User or Group Id</param>
        /// <param name="resourceId">Id of the Resource</param>
        public void LoadUserPermissionsForResource(int principleId, int resourceId)
        {
                    Permissions = GetUserPermissionsForResource(principleId, resourceId);
        }

        /// <summary>
        /// Load all the permissions for a given User or Group Id
        /// </summary>
        /// <param name="principleId">User or Group Id</param>
        public void LoadUserPermissions(int principleId)
        {
                    Permissions = GetUserPermissions(principleId);
        }

        /// <summary>
        /// Gets a permission Id given the Name of the permission
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>PermissionId</returns>

        #endregion

        #region Private Methods

        /// <summary>
        /// used in PermissionStoreCached constructor to load a cached list (property PermissionLookup) to facilitate lookup by Id or Name
        /// </summary>
        private void LoadPermissionsLookup()
        {
            logger.Trace("Loading PermissionLookup");
                PermissionLookup = GetPermissionLookup();
        }
        #endregion
    }
}
