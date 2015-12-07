using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(AddOPTEmploymentValidator))]
    public class AddOPTEmployment
    {
        public bool printForm { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string FullPartTimeIndicator { get; set; }
        
        public string EmployerName { get; set; }

        public EmployerAddress employerAddress { get; set; }
        
        public string CourseRelevance { get; set; }

        public bool AcademicYearMet { get; set; }
        
        public string CompletionType { get; set; }
        
        public string StudentRemarks { get; set; }
        
        public string Remarks { get; set; }        
    }
}
