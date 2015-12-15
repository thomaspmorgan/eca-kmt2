using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Contains all student details
    /// </summary>
    [Validator(typeof(StudentValidator))]
    public class Student
    {
        public Student()
        {
            personalInfo = new PersonalInfo();
            usAddress = new USAddress();
            foreignAddress = new ForeignAddress();
            educationalInfo = new EducationalInfo();
            financialInfo = new FinancialInfo();
            createDependent = new CreateDependent();
        }
        
        public bool isNew { get; set; }

        public string requestID { get; set; }
        
        public string userID { get; set; }
        
        public bool printForm { get; set; }
        
        public string UserDefinedA { get; set; }
        
        public string UserDefinedB { get; set; }

        public PersonalInfo personalInfo { get; set; }
        
        public string IssueReason { get; set; }
        
        public USAddress usAddress { get; set; }

        public ForeignAddress foreignAddress { get; set; }

        public EducationalInfo educationalInfo { get; set; }

        public FinancialInfo financialInfo { get; set; }

        public CreateDependent createDependent { get; set; }
        
        public string Remarks { get; set; }
    }
}
