namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// A Permission is singular action that is granted or revoked in the cam system and usually assigned to a principal and resource
    /// or a role and a resource.
    /// </summary>
    [Table("CAM.Permission")]
    public partial class Permission
    {
        /// <summary>
        /// Creates a new permission.
        /// </summary>
        public Permission()
        {
            PermissionAssignments = new HashSet<PermissionAssignment>();
            RoleResourcePermissions = new HashSet<RoleResourcePermission>();
        }

        /// <summary>
        /// Gets or sets the Permission Id.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the permission.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string PermissionName { get; set; }

        /// <summary>
        /// Gets the date created on.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the id of the creator.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the revised on date.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the revised by user id.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string RevisedBy { get; set; }

        /// <summary>
        /// Gets or sets is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the resource type id.
        /// </summary>
        public int? ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the parent resource type id.
        /// </summary>
        public int? ParentResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        public int? ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the permission description.
        /// </summary>
        [StringLength(255)]
        public string PermissionDescription { get; set; }

        /// <summary>
        /// Gets or sets the resource type.
        /// </summary>
        public virtual ResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        public virtual Resource Resource { get; set; }

        /// <summary>
        /// Gets or sets the resource type.
        /// </summary>
        public virtual ResourceType ParentResourceType { get; set; }

        /// <summary>
        /// Gets or sets the permission assignments.
        /// </summary>
        public virtual ICollection<PermissionAssignment> PermissionAssignments { get; set; }

        /// <summary>
        /// Gets or sets the role resource permissions.
        /// </summary>
        public virtual ICollection<RoleResourcePermission> RoleResourcePermissions { get; set; }
    }
}
