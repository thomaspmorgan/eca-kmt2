namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CAM.UserAccount")]
    public partial class UserAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PrincipalId { get; set; }

        public Guid AdGuid { get; set; }

        public DateTimeOffset? LastAccessed { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset RevisedOn { get; set; }

        public int RevisedBy { get; set; }

        public int AccountStatusId { get; set; }

        public DateTimeOffset? PermissionsRevisedOn { get; set; }

        [StringLength(2000)]
        public string Note { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(101)]
        public string DisplayName { get; set; }

        [StringLength(50)]
        public string EmailAddress { get; set; }

        public DateTimeOffset? ExpiredDate { get; set; }

        public DateTimeOffset? SuspendedDate { get; set; }

        public DateTimeOffset? RevokedDate { get; set; }

        public DateTimeOffset? RestoredDate { get; set; }

        public virtual AccountStatus AccountStatus { get; set; }

        public virtual Principal Principal { get; set; }
    }
}
