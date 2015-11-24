using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class AddDependent
    {
        [Required]
        public bool printForm { get; set; }

        public FullName fullName { get; set; }
        
        /// <summary>
        /// Student date of birth.
        /// </summary>
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gender code.
        /// </summary>
        [StringLength(1)]
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        /// <summary>
        /// Country of birth
        /// </summary>
        [StringLength(2)]
        [Required(ErrorMessage = "Country of birth is required")]
        public string BirthCountryCode { get; set; }

        /// <summary>
        /// Country of citizenship
        /// </summary>
        [StringLength(2)]
        [Required(ErrorMessage = "Country of citizenship is required")]
        public string CitizenshipCountryCode { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        [MaxLength(255)]
        public string Email { get; set; }
        
        /// <summary>
        /// Visa type
        /// </summary>
        [StringLength(2)]
        [Required(ErrorMessage = "Visa type is required")]
        public string VisaType { get; set; }

        [StringLength(2)]
        public string Relationship { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

    }
}
