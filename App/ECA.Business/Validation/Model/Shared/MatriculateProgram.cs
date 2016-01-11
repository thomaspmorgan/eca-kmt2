using System;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(MatriculateProgramValidator))]
    public class MatriculateProgram
    {
        public MatriculateProgram()
        { }

        public bool printForm { get; set; }

        public DateTime NewPrgEndDate { get; set; }

        public string MatriculationCode { get; set; }
    }
}