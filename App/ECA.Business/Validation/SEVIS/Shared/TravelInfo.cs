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
        
        public DateTime PassportExpDate { get; set; }

        [XmlElement(IsNullable = true)]
        public string VisaNumber { get; set; }

        [XmlElement(IsNullable = true)]
        public string VisaIssuingCntry { get; set; }
        
        public DateTime VisaIssueDate { get; set; }
        
        public DateTime VisaExpDate { get; set; }

        [XmlElement(IsNullable = true)]
        public string PortOfEntry { get; set; }
        
        public DateTime DateOfEntry { get; set; }        
    }
}
