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
    
    public partial class PrincipleRole
    {
        public int PrincipalId { get; set; }
        public int RoleId { get; set; }
        public int AssignedBy { get; set; }
        public Nullable<System.DateTimeOffset> AssignedOn { get; set; }
    
        public virtual Principal Principal { get; set; }
        public virtual Role Role { get; set; }
    }
}
