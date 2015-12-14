using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(EditDependentValidator))]
    public class EditDependent
    {
        public string dependentSevisID { get; set; }

        public bool PrintForm { get; set; }
        
        public FullName fullName { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        public string Gender { get; set; }
        
        public string BirthCountryCode { get; set; }

        public string CitizenshipCountryCode { get; set; }
        
        public string Email { get; set; }
        
        public string Relationship { get; set; }
        
        public string Remarks { get; set; }        
    }
}
