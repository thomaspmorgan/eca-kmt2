using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ExtendOPTEmploymentValidator))]
    public class ExtendOPTEmployment
    {
        public bool printForm { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string FullPartTimeIndicator { get; set; }
        
        public string EmployerName { get; set; }

        public EmployerAddress employerAddress { get; set; }
        
        public string StudentRemarks { get; set; }
        
        public string Remarks { get; set; }        
    }
}
