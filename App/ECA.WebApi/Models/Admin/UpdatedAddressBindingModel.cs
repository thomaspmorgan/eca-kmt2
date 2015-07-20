using ECA.Business.Service;
using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The UpdatedAddressBindingModel is used to update an eca system address with a client's updated address.
    /// </summary>
    public class UpdatedAddressBindingModel : AddressBindingModelBase
    {
        /// <summary>
        /// Gets the id of the address.
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// Returns an eca business entity to update an address.
        /// </summary>
        /// <param name="user">The user performing the update.</param>
        /// <returns>The updated eca address for the business layer.</returns>
        public UpdatedEcaAddress ToUpdatedEcaAddress(User user)
        {
            return new UpdatedEcaAddress(
                updator: user,
                addressId: this.AddressId,
                addressTypeId: this.AddressTypeId,
                addressDisplayName: this.AddressDisplayName,
                street1: this.Street1,
                street2: this.Street2,
                street3: this.Street3,
                postalCode: this.PostalCode,
                locationName: this.AddressDisplayName,
                countryId: this.CountryId,
                cityId: this.CityId,
                divisionId: this.DivisionId
                );
        }
    }
}