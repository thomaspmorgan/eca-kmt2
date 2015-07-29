using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The AddressBindingModelBase represents the data needed for a client to either create or update an address.
    /// </summary>
    public abstract class AddressBindingModelBase
    {
        /// <summary>
        /// Gets or sets whether this address should be the primary address.
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets the address type id.
        /// </summary>
        public int AddressTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Street 1 address information.
        /// </summary>
        [MaxLength(Location.STREET_MAX_LENGTH)]
        public string Street1 { get; set; }
         
        /// <summary>
        /// Gets or sets the Street 2 address information.
        /// </summary>
        [MaxLength(Location.STREET_MAX_LENGTH)]
        public string Street2 { get; set; }

        /// <summary>
        /// Gets or sets the Street 3 address information.
        /// </summary>
        [MaxLength(Location.STREET_MAX_LENGTH)]
        public string Street3 { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        [MaxLength(Location.POSTAL_CODE_MAX_LENGTH)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country id
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the city id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Gets or sets the division id
        /// </summary>
        public int DivisionId { get; set; }
    }

    /// <summary>
    /// An AddressBindingModelBase is used to bind an address from a client to an IAddressable entity.
    /// </summary>
    /// <typeparam name="T">The addressable entity type.</typeparam>
    public abstract class AddressBindingModelBase<T> : AddressBindingModelBase
        where T : class, IAddressable
    {
        /// <summary>
        /// Gets or sets the addressable entity id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Returns the AdditionalAddress
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <returns>The additional address.</returns>
        public abstract AdditionalAddress<T> ToAdditionalAddress(User creator);
    }
}