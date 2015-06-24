using System.Collections.Generic;
using System.Linq;
using CAM.Data;

namespace CAM.Business.Service
{
    /// <summary>
    /// A PermissionModel is a simple dto used to demonstrate permission relationships.
    /// </summary>
    public class PermissionModel
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }        

        /// <summary>
        /// Gets or sets the resource type id.
        /// </summary>
        public int? ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the parent resource type id.
        /// </summary>
        public int? ParentResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Parent Permission Id, by the parent resource type id.
        /// </summary>
        public int? ParentPermissionId { get; set; }
    }
}
