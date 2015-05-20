using CAM.Business.Model;
using CAM.Data;
using ECA.WebApi.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Projects
{
    /// <summary>
    /// An AddCollaboratorBindingModel instance is used to add a collaborator to a project.
    /// </summary>
    public class AddCollaboratorBindingModel : IGrantedPermissionBindingModel
    {
        /// <summary>
        /// The principal id of the user to add as a collaborator.
        /// </summary>
        public int CollaboratorPrincipalId { get; set; }

        /// <summary>
        /// The id of the project to add a collaborator to.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Returns a granted permission instance from this object.
        /// </summary>
        /// <param name="grantorUserId">The user id of the user granting the permission.</param>
        /// <returns>The granted permission.</returns>
        public GrantedPermission ToGrantedPermission(int grantorUserId)
        {
            return new GrantedPermission(
                this.CollaboratorPrincipalId, 
                Permission.Editproject.Id,
                this.ProjectId,
                ResourceType.Project.Value,
                grantorUserId);
        }
    }
}