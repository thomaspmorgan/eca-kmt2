using System;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(CancelProgramExtensionValidator))]
    public class CancelProgramExtension
    {
        public bool printForm { get; set; }
        
        public DateTime NewPrgEndDate { get; set; }
        
        public string Remarks { get; set; }
    }
}
