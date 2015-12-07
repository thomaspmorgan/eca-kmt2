using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(StudentUpdateValidator))]
    public class StudentUpdate
    {
        public string sevisID { get; set; }
        
        public string requestID { get; set; }
        
        public string userID { get; set; }
        
        public string statusCode { get; set; }
        
        public string UserDefinedA { get; set; }
        
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
