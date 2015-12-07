using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(AddCptEmploymentValidator))]
    public class AddCptEmployment
    {
        public bool printForm { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string FullPartTimeIndicator { get; set; }
        
        public string EmployerName { get; set; }

        public EmployerAddress employerAddress { get; set; }
        
        public string CourseRelevance { get; set; }
        
        public string Remarks { get; set; }
    }
}
