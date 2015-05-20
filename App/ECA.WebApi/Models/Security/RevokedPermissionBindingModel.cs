using CAM.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Security
{
    /// <summary>
    /// A RevokedPermissionBindingModel is used to explicity revoke a permission from a principal via the client.
    /// </summary>
    public class RevokedPermissionBindingModel : GrantedPermissionBindingModel, IRevokedPermissionBindingModel
    {
        /// <summary>
        /// Returns a RevokedPermission from this instance.
        /// </summary>
        /// <param name="revokerUserId">The revoker user id.</param>
        /// <returns>The RevokedPermission.</returns>
        public RevokedPermission ToRevokedPermission(int revokerUserId)
        {
            return new RevokedPermission(this.GranteePrincipalId, this.PermissionId, this.ForeignResourceId, this.ResourceType, revokerUserId);
        }
    }
}