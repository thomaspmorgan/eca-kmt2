using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// U.S. physical address
    /// </summary>
    public class USAddress
    { 
        /// <summary>
        /// Creates a new USAddress instance.
        /// </summary>
        /// <param name="address1">The address 1 value.</param>
        /// <param name="address2">The address 2 value.</param>
        /// <param name="city">The city of the address.</param>
        /// <param name="state">The state of the address.</param>
        /// <param name="postalCode">The postal code of the address.</param>
        /// <param name="explanationCode">The explanation code.</param>
        /// <param name="explanation">The explanation.</param>
        public USAddress(string address1, string address2, string city, string state, string postalCode, string explanationCode, string explanation)
        {
            this.Address1 = address1;
            this.Address2 = address2;
            this.City = city;
            this.State = state;
            this.PostalCode = postalCode;
            this.Explanation = explanation;
            this.ExplanationCode = explanationCode;
        }

        /// <summary>
        /// Gets or sets the street 1 address.
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the street 2 address.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        public string PostalCode { get; set; }
        
        /// <summary>
        /// Gets or sets the explanation code.
        /// </summary>
        public string ExplanationCode { get; set; }

        /// <summary>
        /// Gets or sets the explanation.
        /// </summary>
        public string Explanation { get; set; }

        /// <summary>
        /// Returns a Sevis USAddress instance from this address.
        /// </summary>
        /// <returns>A Sevis USAddress instance from this address.</returns>
        public USAddrDoctorType GetUSAddressDoctorType()
        {
            Func<string, bool> isCodeSpecified = (value) =>
            {
                return !string.IsNullOrWhiteSpace(value);
            };

            USAddrDoctorTypeExplanationCode? explanationCodeInstance = null;
            if (isCodeSpecified(this.ExplanationCode))
            {
                explanationCodeInstance = this.ExplanationCode.GetUSAddrDoctorTypeExplanationCode();
            }
            return new USAddrDoctorType
            {
                Address1 = this.Address1,
                Address2 = this.Address2,
                City = this.City,
                PostalCode = this.PostalCode,
                State = this.State.GetStateCodeType(),
                Explanation = this.Explanation,
                ExplanationCodeSpecified = isCodeSpecified(this.ExplanationCode),
                ExplanationCode = explanationCodeInstance
            };
        }
    }
}
