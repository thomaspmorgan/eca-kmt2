using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// U.S. physical address
    /// </summary>
    [Validator(typeof(USAddressValidator))]
    public class USAddress
    {
        public USAddress()
        { }

        public string Address1 { get; set; }

        [XmlElement(IsNullable = true)]
        public string Address2 { get; set; }

        [XmlElement(IsNullable = true)]
        public string City { get; set; }

        [XmlElement(IsNullable = true)]
        public string State { get; set; }

        public string PostalCode { get; set; }

        [XmlElement(IsNullable = true)]
        public string ExplanationCode { get; set; }

        [XmlElement(IsNullable = true)]
        public string Explanation { get; set; }
    }
}
