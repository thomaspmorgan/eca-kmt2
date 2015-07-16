using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// An AddressBindingModelBase is used to bind an address from a client to an IAddressable entity.
    /// </summary>
    /// <typeparam name="T">The addressable entity type.</typeparam>
    public abstract class AddressBindingModelBase<T>  
        where T : class, IAddressable
    {
        /// <summary>
        /// Gets or sets the address display name.
        /// </summary>
        public string AddressDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the address type id.
        /// </summary>
        public int AddressTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Street 1 address information.
        /// </summary>
        public string Street1 { get; set; }

        /// <summary>
        /// Gets or sets the Street 2 address information.
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// Gets or sets the Street 3 address information.
        /// </summary>
        public string Street3 { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string LocationName { get; set; }

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

        /// <summary>
        /// Returns the AdditionalAddress
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <returns>The additional address.</returns>
        public abstract AdditionalAddress<T> ToAdditionalAddress(User creator);
    }
}