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
    
    public partial class Principal
    {
        public Principal()
        {
            this.PermissionAssignments = new HashSet<PermissionAssignment>();
            this.PrincipalRoles = new HashSet<PrincipalRole>();
        }
    
        public int PrincipalId { get; set; }
        public Nullable<int> PrincipalTypeId { get; set; }
    
        public virtual ICollection<PermissionAssignment> PermissionAssignments { get; set; }
        public virtual PrincipalType PrincipalType { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<PrincipalRole> PrincipalRoles { get; set; }
    }
}
