namespace CAM.Data
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.Contracts;

    [Table("CAM.UserAccount")]
    public partial class UserAccount : IValidatableObject
    {
        /// <summary>
        /// The user account Id of the system user.
        /// </summary>
        public const int SYSTEM_USER_ACCOUNT_ID = 1;

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
