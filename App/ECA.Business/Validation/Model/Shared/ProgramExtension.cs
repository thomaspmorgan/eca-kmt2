using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ProgramExtensionValidator))]
    public class ProgramExtension
    {
        public bool printForm { get; set; }
        
        public DateTime NewPrgEndDate { get; set; }
        
        public string Remarks { get; set; }
        
        public string Explanation { get; set; }
    }
}
