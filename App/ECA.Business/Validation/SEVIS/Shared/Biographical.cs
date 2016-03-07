using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(BiographicalValidator))]
    public class Biographical
    {
        public Biographical()
        {
            FullName = new FullName();
        }

        /// <summary>
        /// Full name of person
        /// </summary>
        public FullName FullName { get; set; }

        /// <summary>
        /// Student date of birth.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gender code.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// City of birth
        /// </summary>
        public string BirthCity { get; set; }

        /// <summary>
        /// Country of birth
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string BirthCountryCode { get; set; }

        /// <summary>
        /// Country of citizenship
        /// </summary>
        public string CitizenshipCountryCode { get; set; }

        /// <summary>
        /// Country of legal permanent residence
        /// </summary>
        public string PermanentResidenceCountryCode { get; set; }

        /// <summary>
        /// Birth country reason (01 = U.S. - Born to foreign diplomat, 02 = U.S. - Expatriated)
        /// </summary>
        public string BirthCountryReason { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string EmailAddress { get; set; }
        
    }
}