using System;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(TravelInfoValidator))]
    public class TravelInfo
    {
        public TravelInfo()
        { }

        [XmlElement(IsNullable = true)]
        public string PassportNumber { get; set; }

        [XmlElement(IsNullable = true)]
        public string PassportIssuingCntry { get; set; }

        [XmlElement(IsNullable = true)]
        public DateTime PassportExpDate { get; set; }

        [XmlElement(IsNullable = true)]
        public string VisaNumber { get; set; }

        [XmlElement(IsNullable = true)]
        public string VisaIssuingCntry { get; set; }

        [XmlElement(IsNullable = true)]
        public DateTime VisaIssueDate { get; set; }

        [XmlElement(IsNullable = true)]
        public DateTime VisaExpDate { get; set; }

        [XmlElement(IsNullable = true)]
        public string PortOfEntry { get; set; }

        [XmlElement(IsNullable = true)]
        public DateTime DateOfEntry { get; set; }        
    }
}
