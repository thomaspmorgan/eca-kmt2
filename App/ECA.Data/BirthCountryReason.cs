using System.ComponentModel.DataAnnotations;

namespace ECA.Data
{
    public class BirthCountryReason
    {
        /// <summary>
        /// Gets or sets the birth country reason id.
        /// </summary>
        [Key]
        public int BirthCountryReasonId { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason code.
        /// </summary>
        [Required]
        public string ReasonCode { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason name.
        /// </summary>
        public string Description { get; set; }

        public History History { get; set; }
    }
}
