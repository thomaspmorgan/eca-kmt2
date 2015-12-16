using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(AddDependentValidator))]
    public class AddDependent
    {
        public FullName fullName { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        public string Gender { get; set; }

        public string BirthCity { get; set; }

        public string BirthCountryCode { get; set; }
        
        public string CitizenshipCountryCode { get; set; }

        public string PermanentResidenceCountryCode { get; set; }

        public string BirthCountryReason { get; set; }

        public string Email { get; set; }
        
        public string VisaType { get; set; }
        
        public string Relationship { get; set; }
        
        public string Remarks { get; set; }

        public string UserDefinedA { get; set; }

        public string UserDefinedB { get; set; }
    }
}
