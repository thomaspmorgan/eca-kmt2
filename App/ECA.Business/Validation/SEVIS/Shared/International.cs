using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// International organization funding
    /// </summary>
    [Validator(typeof(InternationalValidator))]
    public class International
    {
        public International()
        { }

        /// <summary>
        /// International organization 1
        /// </summary>
        public string Org1 { get; set; }

        /// <summary>
        /// Other International organization 1
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string OtherName1 { get; set; }

        /// <summary>
        /// International organization 1 funding amount
        /// </summary>
        public string Amount1 { get; set; }

        /// <summary>
        /// International organization 2
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string Org2 { get; set; }

        /// <summary>
        /// Other International organization 2
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string OtherName2 { get; set; }

        /// <summary>
        /// International organization 2 funding amount
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string Amount2 { get; set; }
    }
}