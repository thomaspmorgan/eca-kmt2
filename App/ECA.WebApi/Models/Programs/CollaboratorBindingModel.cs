using CAM.Data;
using ECA.WebApi.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Programs
{
    public class CollaboratorBindingModel : IDeletedPermissionBindingModel, IRevokedPermissionBindingModel, IGrantedPermissionBindingModel
    {
        /// <summary>
        /// The principal id of the user to add as a collaborator.
        /// </summary>
        public int PrincipalId { get; set; }

        /// <summary>
        /// The id of the project to add a collaborator to.
        /// </summary>
        public int ProgramId { get; set; }

        /// <summary>
        /// The id of the permission to grant.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Returns a DeletedPermission.
        /// </summary>
        /// <param name="grantorUserId">The user removing the permission by principal id.</param>
        /// <returns>The deleted permission.</returns>
        public CAM.Business.Model.DeletedPermission ToDeletedPermission(int grantorUserId)
        {
            return new CAM.Business.Model.DeletedPermission(this.PrincipalId, this.ProgramId, this.PermissionId, ResourceType.Program.Value);
        }

        /// <summary>
        /// Returns a revoked permission.
        /// </summary>
        /// <param name="grantorUserId">The grantor user id.</param>
        /// <returns>The revoked permission.</returns>
        public CAM.Business.Model.RevokedPermission ToRevokedPermission(int grantorUserId)
        {
            return new CAM.Business.Model.RevokedPermission(this.PrincipalId, this.PermissionId, this.ProgramId, ResourceType.Program.Value, grantorUserId);
        }

        /// <summary>
        /// Returns a granted permission instance from this object.
        /// </summary>
        /// <param name="grantorUserId">The user id of the user granting the permission.</param>
        /// <returns>The granted permission.</returns>
        public CAM.Business.Model.GrantedPermission ToGrantedPermission(int grantorUserId)
        {
            return new CAM.Business.Model.GrantedPermission(this.PrincipalId, this.PermissionId, this.ProgramId, ResourceType.Program.Value, grantorUserId);
        }
    }
}