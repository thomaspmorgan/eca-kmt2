using FluentValidation.Attributes;
using System;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(CancelOCEmploymentValidator))]
    public class CancelOCEmployment
    {
        public bool printForm { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string EmploymentType { get; set; }        
    }
}
