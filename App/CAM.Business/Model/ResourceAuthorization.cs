using CAM.Business.Service;
using CAM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Model
{
    /// <summary>
    /// A ResourceAuthorization object details a single permission granted to a resource for a user either by a permission
    /// or a role.
    /// </summary>
    public class ResourceAuthorization : SimplePermission
    {
        /// <summary>
        /// Gets or sets the date the permission was assigned.
        /// </summary>
        public DateTimeOffset AssignedOn { get; set; }

        /// <summary>
        /// Gets or sets the Display Name of the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the Resource Type.
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the Permission Name.
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// Gets or sets the permission description.
        /// </summary>
        public string PermissionDescription { get; set; }

        /// <summary>
        /// Gets or sets whether or not this resource authorization is granted by a role.
        /// </summary>
        public bool IsGrantedByRole { get; set; }

        /// <summary>
        /// Gets or sets whether or not this resource authorization is granted by a permission explicity.
        /// </summary>
        public bool IsGrantedByPermission { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether this permission is granted by a resource's parent permission.
        /// </summary>
        public bool IsGrantedByInheritance { get; set; }

        /// <summary>
        /// Gets or sets the name of the role this authorization is granted.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the role id this authorization is granted by.
        /// </summary>
        public int RoleId { get; set; }
    }
}
