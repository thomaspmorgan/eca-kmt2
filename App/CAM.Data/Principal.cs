namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CAM.Principal")]
    public partial class Principal
    {
        public Principal()
        {
            PermissionAssignments = new HashSet<PermissionAssignment>();
            PrincipalRoles = new HashSet<PrincipalRole>();
        }

        public int PrincipalId { get; set; }

        public int? PrincipalTypeId { get; set; }

        public virtual ICollection<PermissionAssignment> PermissionAssignments { get; set; }

        public virtual PrincipalType PrincipalType { get; set; }

        public virtual ICollection<PrincipalRole> PrincipalRoles { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}
