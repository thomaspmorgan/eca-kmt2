using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class StudentPersonalInfo
    {
        [Required]
        public bool printForm { get; set; }

        /// <summary>
        /// Full name of person
        /// </summary>
        [Required]
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
        /// Commuter flag
        /// </summary>
        public bool Commuter { get; set; }

        /// <summary>
        /// US address
        /// </summary>
        public USAddress usAddress { get; set; }

        /// <summary>
        /// Foreign address
        /// </summary>
        public ForeignAddress foreignAddress { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }
        
    }
}
