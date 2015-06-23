using System.Collections.Generic;
using System.Linq;
using CAM.Data;
using NLog.Interface;
using CAM.Business;
using System.Threading.Tasks;

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
        public PermissionStore(CamModel model, IPermissionModelService permissionModelService, IResourceService resourceService)
            : base(model, permissionModelService, resourceService)
        {
            ResourceId = null;
            PrincipalId = null;
            Permissions = new List<IPermission>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load all the permissions for a given User or Group Id
        /// </summary>
        /// <param name="principleId">User or Group Id</param>
        public void LoadUserPermissions(int principleId)
        {
            Permissions = GetUserPermissions(principleId);
        }
        #endregion

        /// <summary>
        /// Load all the permissions for a given User or Group Id
        /// </summary>
        /// <param name="principleId">User or Group Id</param>
        public async Task LoadUserPermissionsAsync(int principalId)
        {
            Permissions = await GetUserPermissionsAsync(principalId);
        }
    }
}
