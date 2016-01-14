using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.CreateEV
{
    /// <summary>
    /// Site of activity address
    /// </summary>
    [Validator(typeof(SiteOfActivitySOAValidator))]
    public class SiteOfActivitySOA
    {
        public SiteOfActivitySOA()
        { }

        [XmlAttribute(AttributeName = "printForm")]
        public bool printForm { get; set; }

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

        public string SiteName { get; set; }

        public bool PrimarySite { get; set; }

        [XmlElement(IsNullable = true)]
        public string Remarks { get; set; }
    }
}