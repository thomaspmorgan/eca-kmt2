using System;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(CancelCptEmploymentValidator))]
    public class CancelCptEmployment
    {
        public bool printForm { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string FullPartTimeIndicator { get; set; }
        
        public string EmployerName { get; set; }
    }
}
