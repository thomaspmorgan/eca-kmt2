using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ECA.Data
{
    /// <summary>
    /// The birth country reason entity is used to detail the dependent's US birth country reason.
    /// </summary>
    public partial class BirthCountryReason
    {
        /// <summary>
        /// Creates a new default instance and initializes the people collection.
        /// </summary>
        public BirthCountryReason()
        {
            this.Dependents = new HashSet<PersonDependent>();
        }

        /// <summary>
        /// Gets or sets the birth country reason id.
        /// </summary>
        [Key]
        public int BirthCountryReasonId { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason code.
        /// </summary>
        [Required]
        public string BirthReasonCode { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason name.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the people of this person type.
        /// </summary>
        public virtual ICollection<PersonDependent> Dependents { get; set; }
    }
}
