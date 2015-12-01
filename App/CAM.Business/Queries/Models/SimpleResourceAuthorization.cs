using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Queries.Models
{
    public class SimpleResourceAuthorization
    {
        /// <summary>
        /// Gets or sets the date the permission was assigned.
        /// </summary>
        public DateTimeOffset AssignedOn { get; set; }

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
        /// Gets or sets the role id this authorization is granted by.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the principal id.
        /// </summary>
        public int PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the permission id.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// Gets or sets is allowed.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}
