using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Queries.Models
{
    /// <summary>
    /// A ResourcePermissionDTO represents a possible permission that can be applied to a resource in CAM.
    /// </summary>
    public class ResourcePermissionDTO
    {
        /// <summary>
        /// Gets or sets the Permission Id.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the permission description.
        /// </summary>
        public string PermissionDescription { get; set; }

        /// <summary>
        /// Gets or sets the permission name.
        /// </summary>
        public string PermissionName { get; set; }

    }
}
