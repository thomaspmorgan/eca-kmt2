using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(AddDependentValidator))]
    public class AddDependent
    {
        public AddDependent()
        {
            FullName = new FullName();
        }
        
        [XmlAttribute(AttributeName = "printForm")]
        public bool printForm { get; set; }

        public FullName FullName { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        public string Gender { get; set; }

        public string BirthCity { get; set; }

        public string BirthCountryCode { get; set; }
        
        public string CitizenshipCountryCode { get; set; }

        public string PermanentResidenceCountryCode { get; set; }

        [XmlElement(IsNullable = true)]
        public string BirthCountryReason { get; set; }

        [XmlElement(IsNullable = true)]
        public string EmailAddress { get; set; }

        public string Relationship { get; set; }

        [XmlElement(IsNullable = true)]
        public string UserDefinedA { get; set; }

        [XmlElement(IsNullable = true)]
        public string UserDefinedB { get; set; }
    }
}
