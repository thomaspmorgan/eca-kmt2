using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class Student
    {
        /// <summary>
        /// Request id.
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string requestID { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string userID { get; set; }

        /// <summary>
        /// Print form flag
        /// </summary>
        [Required]
        public bool printForm { get; set; }

        [MaxLength(10)]
        public string UserDefinedA { get; set; }

        [MaxLength(14)]
        public string UserDefinedB { get; set; }

        /// <summary>
        /// Personal information object
        /// </summary>
        public PersonalInfo personalInfo { get; set; }
        
        /// <summary>
        /// Issue reason
        /// </summary>
        [StringLength(1)]
        [Required(ErrorMessage = "Issue reason is required")]
        public string IssueReason { get; set; }
                
        /// <summary>
        /// US address
        /// </summary>
        public USAddress usAddress { get; set; }

        /// <summary>
        /// Foreign address
        /// </summary>
        public ForeignAddress foreignAddress { get; set; }

        /// <summary>
        /// Educational information
        /// </summary>
        public EducationalInfo educationalInfo { get; set; }

        /// <summary>
        /// Financial information
        /// </summary>
        public FinancialInfo financialInfo { get; set; }

        /// <summary>
        /// Dependents
        /// </summary>
        public CreateDependent createDependent { get; set; }
        
        /// <summary>
        /// Student record remarks
        /// </summary>
        [MaxLength(500)]
        public string Remarks { get; set; }
        
    }
}
