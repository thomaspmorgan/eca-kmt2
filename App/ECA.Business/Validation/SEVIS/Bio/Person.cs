
using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Sevis.Bio
{
    [Validator(typeof(PersonValidator))]
    public class Person : Biography
    {
        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Returns an EVPersonTypeBiographical instance for use in a new exchange visitor's biographical information.
        /// </summary>
        /// <returns>Returns an EVPersonTypeBiographical instance for use in a new exchange visitor's biographical information.</returns>
        public EVPersonTypeBiographical GetEVPersonTypeBiographical()
        {
            Contract.Requires(this.BirthDate.HasValue, "The birth date must have a value.");
            Contract.Requires(this.FullName != null, "The full name should be specified.");
            Contract.Requires(this.BirthCountryCode != null, "The BirthCountryCode should be specified.");
            Contract.Requires(this.CitizenshipCountryCode != null, "The CitizenshipCountryCode should be specified.");
            Contract.Requires(this.PermanentResidenceCountryCode != null, "The PermanentResidenceCountryCode should be specified.");
            Contract.Requires(this.Gender != null, "The Gender should be specified.");
            return new EVPersonTypeBiographical
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode.GetBirthCntryCodeType(),
                BirthCountryReasonSpecified = false,
                BirthDate = this.BirthDate.Value,
                CitizenshipCountryCode = this.CitizenshipCountryCode.GetCountryCodeWithType(),
                EmailAddress = this.EmailAddress,
                FullName = this.FullName.GetNameType(),
                Gender = this.Gender.GetEVGenderCodeType(),
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode.GetCountryCodeWithType(),
                PhoneNumber = this.PhoneNumber,
            };
        }
    }
}