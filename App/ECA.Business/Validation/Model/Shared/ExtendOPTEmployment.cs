using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class ExtendOPTEmployment
    {
        [Required]
        public bool printForm { get; set; }

        [MaxLength(10)]
        public DateTime StartDate { get; set; }

        [MaxLength(10)]
        public DateTime EndDate { get; set; }

        [StringLength(2)]
        public string FullPartTimeIndicator { get; set; }

        [MaxLength(100)]
        public string EmployerName { get; set; }

        public EmployerAddress employerAddress { get; set; }

        [MaxLength(500)]
        public string StudentRemarks { get; set; }

        [MaxLength(250)]
        public string Remarks { get; set; }
        
    }
}
