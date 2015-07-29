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
    /// An OrganizationAddressBindingModel is used to add an address to an organization.
    /// </summary>
    public class OrganizationAddressBindingModel : AddressBindingModelBase<Organization>
    {
        /// <summary>
        /// Returns the business layer AdditionalOrganizationAddress to be used to add a new Address to an organization.
        /// </summary>
        /// <param name="creator">The user creating the address.</param>
        /// <returns>The additional address.</returns>
        public override AdditionalAddress<Organization> ToAdditionalAddress(User creator)
        {
            return new AdditionalOrganizationAddress(
                creator: creator,
                addressTypeId: this.AddressTypeId,
                isPrimary: this.IsPrimary,
                street1: this.Street1,
                street2: this.Street2,
                street3: this.Street3,
                postalCode: this.PostalCode,
                locationName: null,
                countryId: this.CountryId,
                cityId: this.CityId,
                divisionId: this.DivisionId,
                organizationId: this.Id
                );
        }
    }
}