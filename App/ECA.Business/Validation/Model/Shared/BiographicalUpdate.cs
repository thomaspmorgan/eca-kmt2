using ECA.Business.Validation.Model.CreateEV;
using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// Edit exchange visitor biographical information
    /// </summary>
    [Validator(typeof(BiographicalUpdateValidator))]
    public class BiographicalUpdate : Biographical
    {
        public BiographicalUpdate()
        {
            USAddress = new USAddress();
            MailAddress = new USAddress();
        }

        /// <summary>
        /// Print request indicator
        /// </summary>
        [XmlAttribute(AttributeName = "printForm")]
        public bool printForm { get; set; }

        /// <summary>
        /// Current phone number
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Position held in home country
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// US physical address
        /// </summary>
        public USAddress USAddress { get; set; }

        /// <summary>
        /// US mailing address
        /// </summary>
        public USAddress MailAddress { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string Remarks { get; set; }
    }
}
