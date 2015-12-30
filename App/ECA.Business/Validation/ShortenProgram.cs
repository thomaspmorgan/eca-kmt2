using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ShortenProgramValidator))]
    public class ShortenProgram
    {
        public bool printForm { get; set; }
        
        public DateTime NewPrgEndDate { get; set; }
        
        public string Remarks { get; set; }

        public TippPhaseDates TippPhaseDates { get; set; }
    }
}
