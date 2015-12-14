using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(DeferProgramAttendenceValidator))]
    public class DeferProgramAttendence
    {
        public bool printForm { get; set; }
        
        public DateTime NewPrgStartDate { get; set; }
        
        public DateTime NewPrgEndDate { get; set; }
        
        public string Remarks { get; set; }        
    }
}
