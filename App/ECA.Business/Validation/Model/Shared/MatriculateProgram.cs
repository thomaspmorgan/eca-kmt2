using System;
using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(MatriculateProgramValidator))]
    public class MatriculateProgram
    {
        public MatriculateProgram()
        { }

        [XmlAttribute(AttributeName = "printForm")]
        public bool printForm { get; set; }

        public DateTime NewPrgEndDate { get; set; }

        public string MatriculationCode { get; set; }
    }
}