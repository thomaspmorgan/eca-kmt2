using CAM.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Security
{
    /// <summary>
    /// The GrantedPermissionBindingModel represents a permission that must be given to a user.
    /// </summary>
    public class GrantedPermissionBindingModel
    {
        /// <summary>
        /// The principal id of the user receiving the granted permission.
        /// </summary>
        public int GranteePrincipalId { get; set; }

        /// <summary>
        /// The resource type the permission is for.
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// The key of the resource the permission for e.g. the Project or Program id.
        /// </summary>
        public int ForeignResourceId { get; set; }

        /// <summary>
        /// The permission id.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Returns a GrantedPermission from this instance.
        /// </summary>
        /// <param name="grantorUserId">The grantor user id.</param>
        /// <returns>The GrantedPermission.</returns>
        public GrantedPermission ToGrantedPermission(int grantorUserId)
        {
            return new GrantedPermission(this.GranteePrincipalId, this.PermissionId, this.ForeignResourceId, this.ResourceType, grantorUserId);
        }
    }
}