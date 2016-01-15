using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(SiteOfActivitySeekingGempValidator))]
    public class SiteOfActivitySeekingGemp
    {
        public SiteOfActivitySeekingGemp()
        { }

        /// <summary>
        /// Print request indicator
        /// </summary>
        [XmlAttribute(AttributeName = "printForm")]
        public bool printForm { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
    }
}