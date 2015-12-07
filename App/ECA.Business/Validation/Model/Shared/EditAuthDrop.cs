using FluentValidation.Attributes;
using System;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(EditAuthDropValidator))]
    public class EditAuthDrop
    {
        public bool printForm { get; set; }
        
        public string Reason { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string Remarks { get; set; }
        
        public string NewReason { get; set; }
        
        public DateTime NewStartDate { get; set; }
        
        public DateTime NewEndDate { get; set; }

    }
}
