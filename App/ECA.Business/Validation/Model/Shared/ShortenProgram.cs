using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(ShortenProgramValidator))]
    public class ShortenProgram
    {
        [XmlAttribute(AttributeName = "printForm")]
        public bool printForm { get; set; }
        
        public DateTime NewPrgEndDate { get; set; }
        
        public string Remarks { get; set; }

        public TippPhaseDates TippPhaseDates { get; set; }
    }
}
