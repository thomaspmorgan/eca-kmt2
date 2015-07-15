using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public abstract class AdditionalAddress<T> : EcaAddress 
        where T : class, IAddressable
    {
        public AdditionalAddress(
            User creator,
            int addressTypeId,
            string addressDisplayName,
            string street1,
            string street2,
            string street3,
            string postalCode,
            string locationName,
            int countryId,
            int cityId,
            int? divisionId
            )
            : base(addressTypeId, addressDisplayName, street1, street2, street3, postalCode, locationName, countryId, cityId, divisionId)
        {
            Contract.Requires(creator != null, "The creator must not be null.");
            this.Create = new Create(creator);
        }

        /// <summary>
        /// Gets or sets the audit.
        /// </summary>
        public Audit Create { get; private set; }

        public Location GetLocation()
        {
            Contract.Ensures(Contract.Result<Location>() != null);
            var location = new Location();
            location.CityId = this.CityId;
            location.CountryId = this.CountryId;
            location.DivisionId = this.DivisionId;
            location.LocationName = this.LocationName;
            location.LocationTypeId = this.LocationTypeId;
            location.PostalCode = this.PostalCode;
            location.Street1 = this.Street1;
            location.Street2 = this.Street2;
            location.Street3 = this.Street3;
            return location;
        }

        public Address GetAddress()
        {
            Contract.Ensures(Contract.Result<Address>() != null);
            var location = GetLocation();
            Contract.Assert(location != null, "The location must not be null.");
            var address = new Address();
            address.Location = location;
            address.AddressTypeId = this.AddressTypeId;
            address.DisplayName = this.AddressDisplayName;            
            return address;
        }

        public Address AddAddress(T addressable)
        {
            Contract.Requires(addressable != null, "The addressable object must not be null.");
            Contract.Ensures(Contract.Result<Address>() != null);
            var address = GetAddress();
            Contract.Assert(address != null, "The address must not be null.");
            Contract.Assert(address.Location != null, "The location must not be null.");
            Contract.Assert(addressable.Addresses != null, "The addressable entity's addresses property must not be null.");
            addressable.Addresses.Add(address);
            return address;
        }

        public abstract int GetAddressableEntityId();
    }
}
