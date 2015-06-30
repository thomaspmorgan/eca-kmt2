namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CAM.Role")]
    public partial class Role
    {
        public Role()
        {
            PrincipalRoles = new HashSet<PrincipalRole>();
            RoleResourcePermissions = new HashSet<RoleResourcePermission>();
        }

        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(255)]
        public string RoleDescription { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset RevisedOn { get; set; }

        public int RevisedBy { get; set; }

        public bool IsActive { get; set; }

        public int? ResourceId { get; set; }

        public int? ResourceTypeId { get; set; }

        public virtual ICollection<PrincipalRole> PrincipalRoles { get; set; }

        public virtual Resource Resource { get; set; }

        public virtual ResourceType ResourceType { get; set; }

        public virtual ICollection<RoleResourcePermission> RoleResourcePermissions { get; set; }
    }
}
