using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(EditOCEmploymentValidator))]
    public class EditOCEmployment
    {
        public bool printForm { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string EmploymentType { get; set; }
        
        public DateTime NewStartDate { get; set; }
        
        public DateTime NewEndDate { get; set; }
        
        public string NewEmploymentType { get; set; }
        
        public string Recommendation { get; set; }
    }
}
