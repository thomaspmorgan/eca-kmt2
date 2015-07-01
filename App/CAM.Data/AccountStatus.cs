namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// An account status details a current user's account status within cam.
    /// </summary>
    [Table("CAM.AccountStatus")]
    public partial class AccountStatus
    {
        /// <summary>
        /// Creates a new account status.
        /// </summary>
        public AccountStatus()
        {
            UserAccounts = new HashSet<UserAccount>();
        }

        /// <summary>
        /// Gets or sets the account status id.
        /// </summary>
        [Key]
        public int AccountStatusId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the revised by.
        /// </summary>
        public int RevisedBy { get; set; }

        /// <summary>
        /// Gets or sets the revised on date.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; } 

        /// <summary>
        /// Gets or sets is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the user accounts.
        /// </summary>
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
