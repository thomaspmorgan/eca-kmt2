namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CAM.AccountStatus")]
    public partial class AccountStatus
    {
        public AccountStatus()
        {
            UserAccounts = new HashSet<UserAccount>();
        }

        [Key]
        public int AccountStatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int RevisedBy { get; set; }

        public DateTimeOffset RevisedOn { get; set; } 

        public bool IsActive { get; set; }

        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
