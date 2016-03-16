using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Model.Sevis;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// U.S. physical address
    /// </summary>
    [Validator(typeof(USAddressValidator))]
    public class USAddress
    {
        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public USAddress()
        {
            
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
            return new USAddrDoctorType
            {
                Address1 = this.Address1,
                Address2 = this.Address2,
                City = this.City,
                PostalCode = this.PostalCode,
                State = this.State.GetStateCodeType(),
                Explanation = this.Explanation,
                ExplanationCodeSpecified = false
            };
        }
    }
}
