using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ReprintFormValidator))]
    public class ReprintForm
    {
        public ReprintForm()
        { }

        /// <summary>
        /// Print request indicator
        /// </summary>
        [XmlAttribute(AttributeName = "printForm")]
        public bool printForm { get; set; }

        public string Reason { get; set; }

        [XmlElement(IsNullable = true)]
        public string OtherRemarks { get; set; }

        [XmlElement(IsNullable = true)]
        public string Remarks { get; set; }
    }
}
