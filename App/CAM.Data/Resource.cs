namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CAM.Resource")]
    public partial class Resource
    {
        public Resource()
        {
            Permissions = new HashSet<Permission>();
            PermissionAssignments = new HashSet<PermissionAssignment>();
            ChildResources = new HashSet<Resource>();
            Roles = new HashSet<Role>();
            RoleResourcePermissions = new HashSet<RoleResourcePermission>();
        }

        public int ResourceId { get; set; }

        public int ResourceTypeId { get; set; }

        public int ForeignResourceId { get; set; }

        public int? ParentResourceId { get; set; }

        public virtual Application Application { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }

        public virtual ICollection<PermissionAssignment> PermissionAssignments { get; set; }

        public virtual ICollection<Resource> ChildResources { get; set; }

        public virtual Resource ParentResource { get; set; }

        public virtual ResourceType ResourceType { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<RoleResourcePermission> RoleResourcePermissions { get; set; }
    }
}
