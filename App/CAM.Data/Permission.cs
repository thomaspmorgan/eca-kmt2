namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CAM.Permission")]
    public partial class Permission
    {
        public Permission()
        {
            PermissionAssignments = new HashSet<PermissionAssignment>();
            RoleResourcePermissions = new HashSet<RoleResourcePermission>();
        }

        public int PermissionId { get; set; }

        [Required]
        [StringLength(50)]
        public string PermissionName { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset RevisedOn { get; set; }

        [Required]
        [StringLength(50)]
        public string RevisedBy { get; set; }

        public bool IsActive { get; set; }

        public int? ResourceTypeId { get; set; }

        public int? ParentResourceTypeId { get; set; }

        public int? ResourceId { get; set; }

        [StringLength(255)]
        public string PermissionDescription { get; set; }

        public virtual ResourceType ResourceType { get; set; }

        public virtual Resource Resource { get; set; }

        public virtual ResourceType ParentResourceType { get; set; }

        public virtual ICollection<PermissionAssignment> PermissionAssignments { get; set; }

        public virtual ICollection<RoleResourcePermission> RoleResourcePermissions { get; set; }
    }
}
