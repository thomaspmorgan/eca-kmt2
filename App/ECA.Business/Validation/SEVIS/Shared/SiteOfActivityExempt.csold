using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(SiteOfActivityExemptValidator))]
    public class SiteOfActivityExempt
    {
        public SiteOfActivityExempt()
        { }

        [XmlElement(IsNullable = true)]
        public string Remarks { get; set; }
    }
}