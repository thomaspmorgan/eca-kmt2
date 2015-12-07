using FluentValidation.Attributes;
using System;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(CancelAuthDropValidator))]
    public class CancelAuthDrop
    {
        public bool printForm { get; set; }
        
        public string Reason { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
    }
}
