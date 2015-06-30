namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CAM.PrincipalType")]
    public partial class PrincipalType
    {
        public PrincipalType()
        {
            Principals = new HashSet<Principal>();
        }

        public int PrincipalTypeId { get; set; }

        [Required]
        [StringLength(10)]
        public string PrincipalTypeName { get; set; }

        [StringLength(255)]
        public string PrincipalTypeDescription { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int RevisedBy { get; set; }

        public DateTimeOffset RevisedOn { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Principal> Principals { get; set; }
    }
}
