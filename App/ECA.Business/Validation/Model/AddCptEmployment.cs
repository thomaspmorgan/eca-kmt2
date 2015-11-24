using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class AddCptEmployment
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

        [MaxLength(250)]
        public string CourseRelevance { get; set; }
        
        [MaxLength(200)]
        public string Remarks { get; set; }

    }
}
