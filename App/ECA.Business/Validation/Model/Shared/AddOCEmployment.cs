using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(AddOCEmploymentValidator))]
    public class AddOCEmployment
    {
        public bool printForm { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string EmploymentType { get; set; }
        
        public string Recommendation { get; set; }        
    }
}
