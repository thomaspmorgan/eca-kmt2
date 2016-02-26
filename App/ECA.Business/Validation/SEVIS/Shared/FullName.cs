using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(FullNameValidator))]
    public class FullName
    {
        public FullName()
        { }

        /// <summary>
        /// Person last name.
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string LastName { get; set; }

        /// <summary>
        /// Person first name.
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string FirstName { get; set; }

        /// <summary>
        /// Person passport name.
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string PassportName { get; set; }

        /// <summary>
        /// Person preferred name.
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string PreferredName { get; set; }

        /// <summary>
        /// Person name suffix.
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string Suffix { get; set; }
    }
}
