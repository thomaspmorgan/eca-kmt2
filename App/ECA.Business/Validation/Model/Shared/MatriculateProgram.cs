using System;

namespace ECA.Business.Validation.Model
{
    public class MatriculateProgram
    {
        public MatriculateProgram()
        { }

        public bool printForm { get; set; }

        public DateTime NewPrgEndDate { get; set; }

        public string MatriculationCode { get; set; }
    }
}