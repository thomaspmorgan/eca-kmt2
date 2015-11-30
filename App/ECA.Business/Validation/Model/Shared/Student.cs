using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class Student
    {
        public const int ID_MAX_LENGTH = 20;

        public const int USERDEFINEDA_MAX_LENGTH = 10;

        public const int USERDEFINEDB_MAX_LENGTH = 14;

        public const int ISSUE_REASON_STRING_LENGTH = 1;

        public const int REMARKS_MAX_LENGTH = 500;
        
        [MaxLength(ID_MAX_LENGTH)]
        [Required(ErrorMessage = "Request id is required")]
        public string requestID { get; set; }

        [MaxLength(ID_MAX_LENGTH)]
        [Required(ErrorMessage = "User id is required")]
        public string userID { get; set; }

        [Required(ErrorMessage = "Print form is required")]
        public bool printForm { get; set; }

        [MaxLength(USERDEFINEDA_MAX_LENGTH)]
        public string UserDefinedA { get; set; }

        [MaxLength(USERDEFINEDB_MAX_LENGTH)]
        public string UserDefinedB { get; set; }

        /// <summary>
        /// Personal information object
        /// </summary>
        public PersonalInfo personalInfo { get; set; }
        
        /// <summary>
        /// Issue reason
        /// </summary>
        [StringLength(ISSUE_REASON_STRING_LENGTH)]
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
        [MaxLength(REMARKS_MAX_LENGTH)]
        public string Remarks { get; set; }
        
    }
}
