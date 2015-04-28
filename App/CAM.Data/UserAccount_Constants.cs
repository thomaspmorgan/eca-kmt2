using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Data
{
    public partial class UserAccount : IValidatableObject
    {
        /// <summary>
        /// The user account Id of the system user.
        /// </summary>
        public const int SYSTEM_USER_ACCOUNT_ID = 1;

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
