using ECA.Business.Service;
using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// An PersonAddressBindingModel is used to add an address to a person.
    /// </summary>
    public class PersonAddressBindingModel : AddressBindingModelBase<ECA.Data.Person>
    {
        /// <summary>
        /// Returns the business layer AdditionalPersonAddress to be used to add a new Address to a person.
        /// </summary>
        /// <param name="creator">The user creating the address.</param>
        /// <returns>The additional address.</returns>
        public override AdditionalAddress<ECA.Data.Person> ToAdditionalAddress(User creator)
        {
            return new AdditionalPersonAddress(
                creator: creator,
                addressTypeId: this.AddressTypeId,
                addressDisplayName: this.AddressDisplayName,
                street1: this.Street1,
                street2: this.Street2,
                street3: this.Street3,
                postalCode: this.PostalCode,
                locationName: this.LocationName,
                countryId: this.CountryId,
                cityId: this.CityId,
                divisionId: this.DivisionId,
                personId: this.Id
                );
        }
    }
}