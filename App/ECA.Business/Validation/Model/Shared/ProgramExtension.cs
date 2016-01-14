using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ProgramExtensionValidator))]
    public class ProgramExtension
    {
        public ProgramExtension()
        {
            TippPhaseDates = new TippPhaseDates();
        }

        [XmlAttribute(AttributeName = "printForm")]
        public bool printForm { get; set; }
        
        public DateTime NewPrgEndDate { get; set; }

        [XmlElement(IsNullable = true)]
        public string Remarks { get; set; }
        
        public TippPhaseDates TippPhaseDates { get; set; }
    }
}
