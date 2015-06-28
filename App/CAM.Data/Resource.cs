//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Resource
    {
        public Resource()
        {
            this.Permissions = new HashSet<Permission>();
            this.PermissionAssignments = new HashSet<PermissionAssignment>();
            this.Roles = new HashSet<Role>();
            this.RoleResourcePermissions = new HashSet<RoleResourcePermission>();
            this.ChildResources = new HashSet<Resource>();
        }
    
        public int ResourceId { get; set; }
        public int ResourceTypeId { get; set; }
        public int ForeignResourceId { get; set; }
        public Nullable<int> ParentResourceId { get; set; }
    
        public virtual Application Application { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<PermissionAssignment> PermissionAssignments { get; set; }
        public virtual ResourceType ResourceType { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<RoleResourcePermission> RoleResourcePermissions { get; set; }
        public virtual ICollection<Resource> ChildResources { get; set; }
        public virtual Resource ParentResource { get; set; }
    }
}
