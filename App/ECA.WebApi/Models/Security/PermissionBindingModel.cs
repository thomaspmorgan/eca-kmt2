using CAM.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Security
{
    /// <summary>
    /// A DeletedPermissionBindingModel is used to delete a permission from a principal.
    /// </summary>
    public class PermissionBindingModel : IDeletedPermissionBindingModel, IGrantedPermissionBindingModel, IRevokedPermissionBindingModel
    {
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
        /// The principal id of the user to remove the permission from.
        /// </summary>
        public int PrincipalId { get; set; }

        /// <summary>
        /// Returns a deleted permission for use in the business layer.
        /// </summary>
        /// <param name="grantorUserId">The user id of the user deleting the permission.</param>
        /// <returns>A DeletedPermission instance.</returns>
        public DeletedPermission ToDeletedPermission(int grantorUserId)
        {
            return new DeletedPermission(this.PrincipalId, this.ForeignResourceId, this.PermissionId, this.ResourceType);
        }

        /// <summary>
        /// Returns a GrantedPermission from this instance.
        /// </summary>
        /// <param name="grantorUserId">The grantor user id.</param>
        /// <returns>The GrantedPermission.</returns>
        public GrantedPermission ToGrantedPermission(int grantorUserId)
        {
            return new GrantedPermission(this.PrincipalId, this.PermissionId, this.ForeignResourceId, this.ResourceType, grantorUserId);
        }

        /// <summary>
        /// Returns a RevokedPermission from this instance.
        /// </summary>
        /// <param name="revokerUserId">The revoker user id.</param>
        /// <returns>The RevokedPermission.</returns>
        public RevokedPermission ToRevokedPermission(int revokerUserId)
        {
            return new RevokedPermission(this.PrincipalId, this.PermissionId, this.ForeignResourceId, this.ResourceType, revokerUserId);
        }
    }
}