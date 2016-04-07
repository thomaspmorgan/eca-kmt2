namespace CAM.Data
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A UserAccount is a representation of an Azure Active Directory user that is tracked within CAM.
    /// </summary>
    [Table("CAM.UserAccount")]
    public partial class UserAccount : IValidatableObject
    {
        /// <summary>
        /// The user account Id of the system user.
        /// </summary>
        public const int SYSTEM_USER_ACCOUNT_ID = 1;

        /// <summary>
        /// Gets or sets the principal id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the sevis user name.
        /// </summary>
        [MaxLength(10)]
        public string SevisUsername { get; set; }

        /// <summary>
        /// Gets or sets the azure active directory GUID.
        /// </summary>
        public Guid AdGuid { get; set; }

        /// <summary>
        /// Gets or sets the last accessed date.
        /// </summary>
        public DateTimeOffset? LastAccessed { get; set; }

        /// <summary>
        /// Gets the created on date.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the created by user id.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the revised on date.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the revised by user id.
        /// </summary>
        public int RevisedBy { get; set; }

        /// <summary>
        /// Gets or sets the account status id.
        /// </summary>
        public int AccountStatusId { get; set; }

        /// <summary>
        /// Gets or sets the permissions revised on date.
        /// </summary>
        public DateTimeOffset? PermissionsRevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        [StringLength(2000)]
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        [Required]
        [StringLength(101)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [StringLength(50)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the expired on date.
        /// </summary>
        public DateTimeOffset? ExpiredDate { get; set; }

        /// <summary>
        /// Gets or sets the suspended date.
        /// </summary>
        public DateTimeOffset? SuspendedDate { get; set; }

        /// <summary>
        /// Gets or sets the revoked date.
        /// </summary>
        public DateTimeOffset? RevokedDate { get; set; }

        /// <summary>
        /// Gets or sets the restored date.
        /// </summary>
        public DateTimeOffset? RestoredDate { get; set; }

        /// <summary>
        /// Gets or sets the account status.
        /// </summary>
        public virtual AccountStatus AccountStatus { get; set; }

        /// <summary>
        /// Gets or sets the principal.
        /// </summary>
        public virtual Principal Principal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Contract.Assert(validationContext != null, "The validation context must not be null.");
            Contract.Assert(validationContext.Items.ContainsKey(CamModel.VALIDATABLE_CONTEXT_KEY), "The validation context must have a context to query.");
            var context = validationContext.Items[CamModel.VALIDATABLE_CONTEXT_KEY] as CamModel;
            Contract.Assert(context != null, "The context must not be null.");

            var existingUser = context.UserAccounts.Where(x => x.AdGuid == this.AdGuid && x.PrincipalId != this.PrincipalId).FirstOrDefault();
            if (existingUser != null)
            {
                yield return new ValidationResult(String.Format("The user with ad id [{0}] already exists.", this.AdGuid), new List<string> { "AdGuid" });
            }
        }
    }
}
