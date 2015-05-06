using ECA.Business.Service;
using ECA.Business.Service.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for address
    /// </summary>
    public class AddressBindingModel
    {
        /// <summary>
        /// Gets or sets street 1
        /// </summary>
        public string Street1 { get; set; }

        /// <summary>
        /// Gets or sets street 2
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// Gets or sets street 3
        /// </summary>
        public string Street3 { get; set; }

        /// <summary>
        /// Gets or sets city id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Gets or sets country id
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Converts binding model to business model
        /// </summary>
        /// <param name="user">The user updating the address</param>
        /// <returns>Home address business model</returns>
        public HomeAddress ToHomeAddress(User user)
        {
            return new HomeAddress(
                updatedBy: user,
                street1: this.Street1,
                street2: this.Street2,
                street3: this.Street3,
                cityId: this.CityId,
                countryId: this.CountryId,
                postalCode: this.PostalCode
            );
        }
    }
}