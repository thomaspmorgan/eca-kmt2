
namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Contains all student details
    /// </summary>
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
        
        public string requestID { get; set; }
        
        public string userID { get; set; }
        
        public bool printForm { get; set; }
        
        public string UserDefinedA { get; set; }
        
        public string UserDefinedB { get; set; }

        /// <summary>
        /// Personal information object
        /// </summary>
        public PersonalInfo personalInfo { get; set; }
        
        /// <summary>
        /// Issue reason
        /// </summary>
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
        public string Remarks { get; set; }        
    }
}
