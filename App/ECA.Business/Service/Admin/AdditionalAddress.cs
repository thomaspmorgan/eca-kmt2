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
    /// <summary>
    /// An AdditionalAddress is a class contains the necessary information to add a new address to an Eca entity.  Extend
    /// this class to handle the different IAddresssable entities.
    /// </summary>
    /// <typeparam name="T">The IAddressable ECA Entity type.</typeparam>
    public abstract class AdditionalAddress<T> : EcaAddress 
        where T : class, IAddressable
    {
        /// <summary>
        /// Creates a new AdditionalAddress instance.
        /// </summary>
        /// <param name="creator">The user creating a new address.</param>
        /// <param name="addressTypeId">The address type.</param>
        /// <param name="isPrimary">True, if the address is the primary address.</param>
        /// <param name="street1">The street 1.</param>
        /// <param name="street2">The street 2.</param>
        /// <param name="street3">The street 3.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="locationName">The location name.</param>
        /// <param name="countryId">The country id.</param>
        /// <param name="cityId">The city id.</param>
        /// <param name="divisionId">The division id.</param>
        public AdditionalAddress(
            User creator,
            int addressTypeId,
            bool isPrimary,
            string street1,
            string street2,
            string street3,
            string postalCode,
            string locationName,
            int countryId,
            int cityId,
            int? divisionId
            )
            : base(addressTypeId, isPrimary, street1, street2, street3, postalCode, locationName, countryId, cityId, divisionId)
        {
            Contract.Requires(creator != null, "The creator must not be null.");
            this.Create = new Create(creator);
        }

        /// <summary>
        /// Gets or sets the audit.
        /// </summary>
        public Audit Create { get; private set; }

        /// <summary>
        /// Returns the location of the new address.
        /// </summary>
        /// <returns>The location of the new address.</returns>
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
            this.Create.SetHistory(location);
            return location;
        }

        /// <summary>
        /// Returns the new address entity represented by this additional address.
        /// </summary>
        /// <returns>The address to add to the eca system.</returns>
        public Address GetAddress()
        {
            Contract.Ensures(Contract.Result<Address>() != null);
            var location = GetLocation();
            Contract.Assert(location != null, "The location must not be null.");
            var address = new Address();
            address.Location = location;
            address.AddressTypeId = this.AddressTypeId;
            address.IsPrimary = this.IsPrimary;
            this.Create.SetHistory(address);
            return address;
        }

        /// <summary>
        /// Adds the address and location represented by this AdditionalAddress to the given
        /// IAddressable entity.
        /// </summary>
        /// <param name="addressable">The addressable entity.</param>
        /// <returns>The address to add to the ECA Context.</returns>
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

        /// <summary>
        /// Returns the Id of the IAddressable entity.
        /// </summary>
        /// <returns>The id of the IAddressable entity.</returns>
        public abstract int GetAddressableEntityId();

        /// <summary>
        /// Returns a query that retrieves the addresses of this entity from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve all addresses.</returns>
        public abstract IQueryable<Address> CreateGetAddressesQuery(EcaContext context);
    }
}
