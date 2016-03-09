
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Sevis.Bio
{
    [Validator(typeof(BiographyValidator))]
    public class PersonBiography : Biography
    {
        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        public string PhoneNumber { get; set; }

    }
}