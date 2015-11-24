using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class AddOCEmployment
    {
        [Required]
        public bool printForm { get; set; }

        [MaxLength(10)]
        public DateTime StartDate { get; set; }

        [MaxLength(10)]
        public DateTime EndDate { get; set; }

        [StringLength(2)]
        public string EmploymentType { get; set; }

        [MaxLength(250)]
        public string Recommendation { get; set; }
        
    }
}
