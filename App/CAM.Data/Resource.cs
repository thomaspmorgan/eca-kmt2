namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// A Resource is a tracking mechanism for detailing an object that must be protected by permissions.
    /// 
    /// For example, a project may be a resource, and therefore its resource type would be project and its
    /// foreign resource id would the id of the project in ECA.
    /// </summary>
    [Table("CAM.Resource")]
    public partial class Resource
    {
        /// <summary>
        /// Creates a new resource.
        /// </summary>
        public Resource()
        {
            Permissions = new HashSet<Permission>();
            PermissionAssignments = new HashSet<PermissionAssignment>();
            ChildResources = new HashSet<Resource>();
            Roles = new HashSet<Role>();
            RoleResourcePermissions = new HashSet<RoleResourcePermission>();
        }

        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the resource type id.
        /// </summary>
        public int ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the foreign resource id, i.e. the primary key of the resource not in the cam system.
        /// </summary>
        public int ForeignResourceId { get; set; }

        /// <summary>
        /// Gets or sets the parent resource id.
        /// </summary>
        public int? ParentResourceId { get; set; }

        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        public virtual Application Application { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        public virtual ICollection<Permission> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the permission assignments.
        /// </summary>
        public virtual ICollection<PermissionAssignment> PermissionAssignments { get; set; }

        /// <summary>
        /// Gets or sets the child resources.
        /// </summary>
        public virtual ICollection<Resource> ChildResources { get; set; }

        /// <summary>
        /// Gets or sets the parent resource.
        /// </summary>
        public virtual Resource ParentResource { get; set; }

        /// <summary>
        /// Gets or sets the resource type.
        /// </summary>
        public virtual ResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// Gets or sets the role resource permissions.
        /// </summary>
        public virtual ICollection<RoleResourcePermission> RoleResourcePermissions { get; set; }
    }
}
