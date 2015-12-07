using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(AddAuthDropValidator))]
    public class AddAuthDrop
    {
        public bool printForm { get; set; }
        
        public string Reason { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string Remarks { get; set; }        
    }
}
