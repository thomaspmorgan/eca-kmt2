using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class StudentUpdate
    {
        /// <summary>
        /// Sevis id.
        /// </summary>
        [MaxLength(11)]
        [Required]
        public string sevisID { get; set; }

        /// <summary>
        /// Request id
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string requestID { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        [MaxLength(10)]
        [Required]
        public string userID { get; set; }

        /// <summary>
        /// Status code
        /// </summary>
        [MaxLength(10)]
        [Required]
        public string statusCode { get; set; }

        [MaxLength(10)]
        public string UserDefinedA { get; set; }

        [MaxLength(14)]
        public string UserDefinedB { get; set; }

        public AuthDropBelowFC authDropBelowFC { get; set; }
        
        public CPTEmployment cptEmployment { get; set; }

        public UpdatedDependent updatedDependent { get; set; }

        public DisciplinaryAction disciplinaryAction { get; set; }

        public EducationLevel educationLevel { get; set; }

        public FinancialInfo financialInfo { get; set; }

        public OffCampusEmployment offCampusEmployment { get; set; }

        public OPTEmployment optEmployment { get; set; }

        public StudentPersonalInfo personalInfo { get; set; }

        public Program program { get; set; }
        
        public StudentRegistration registration { get; set; }

        public StudentReprint reprint { get; set; }

        public StudentRequest request { get; set; }

        public StudentStatus status { get; set; }
    }
}
