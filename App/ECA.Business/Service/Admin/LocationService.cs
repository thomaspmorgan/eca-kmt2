using System.Data.Entity;
using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Service.Lookup;
using System.Diagnostics;
using NLog;
using ECA.Core.Exceptions;
using ECA.Business.Validation;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The LocationService is a lookup service that performs crud operations on Locations.
    /// </summary>
    public class LocationService : LookupService<LocationDTO>, ILocationService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<int, object, Type> throwIfEntityNotFound;
        private readonly Action<int, Location, string> throwIfLocationNotFound;
        private readonly IBusinessValidator<EcaAddressValidationEntity, EcaAddressValidationEntity> addressValidator;

        /// <summary>
        /// Creates a new location service with the context to operate against.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="addressValidator">The address create and update validator.</param>
        public LocationService(EcaContext context, IBusinessValidator<EcaAddressValidationEntity, EcaAddressValidationEntity> addressValidator)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(addressValidator != null, "The address validator must not be null.");
            this.addressValidator = addressValidator;
            throwIfEntityNotFound = (id, instance, t) =>
            {
                if (instance == null)
                {
                    throw new ModelNotFoundException(String.Format("The model type [{0}] with Id [{1}] was not found.", t.Name, id));
                }
            };
            throwIfLocationNotFound = (id, location, locationTypeName) =>
            {
                if (location == null)
                {
                    throw new ModelNotFoundException(String.Format("The [{0}] with id [{1}] was not found.", locationTypeName, id));
                }
            };
        }

        #region Get
        /// <summary>
        /// Returns a query to get dtos.
        /// </summary>
        /// <returns>The query to get location dtos.</returns>
        protected override IQueryable<LocationDTO> GetSelectDTOQuery()
        {
            return LocationQueries.CreateGetLocationsQuery(this.Context);
        }
        #endregion

        #region Get By Id

        /// <summary>
        /// Returns the location with the given id, or null if it is not found.
        /// </summary>
        /// <param name="locationId">The id of the location.</param>
        /// <returns>The location, or null if it does not exist.</returns>
        public LocationDTO GetLocationById(int locationId)
        {
            var location = LocationQueries.CreateGetLocationsQuery(this.Context).Where(x => x.Id == locationId).FirstOrDefault();
            logger.Debug("Retrieved location = [{0}] by id [{1}].", location, locationId);
            return location;
        }

        /// <summary>
        /// Returns the location with the given id, or null if it is not found.
        /// </summary>
        /// <param name="locationId">The id of the location.</param>
        /// <returns>The location, or null if it does not exist.</returns>
        public async Task<LocationDTO> GetLocationByIdAsync(int locationId)
        {
            var location = await LocationQueries.CreateGetLocationsQuery(this.Context).Where(x => x.Id == locationId).FirstOrDefaultAsync();
            logger.Debug("Retrieved location = [{0}] by id [{1}].", location, locationId);
            return location;
        }
        #endregion

        #region Validation

        /// <summary>
        /// Returns the distinct list of location types for the given locations by id.
        /// </summary>
        /// <param name="locationIds">The locations by id.</param>
        /// <returns>The list of location type ids.</returns>
        public List<int> GetLocationTypeIds(List<int> locationIds)
        {
            var ids = LocationQueries.CreateGetLocationTypeIdsQuery(this.Context, locationIds).ToList();
            this.logger.Trace("Retrieved location types for location ids {0}.", String.Join(", ", locationIds));
            return ids;
        }

        /// <summary>
        /// Returns the distinct list of location types for the given locations by id.
        /// </summary>
        /// <param name="locationIds">The locations by id.</param>
        /// <returns>The list of location type ids.</returns>
        public async Task<List<int>> GetLocationTypeIdsAsync(List<int> locationIds)
        {
            var ids = await LocationQueries.CreateGetLocationTypeIdsQuery(this.Context, locationIds).ToListAsync();
            this.logger.Trace("Retrieved location types for location ids {0}.", String.Join(", ", locationIds));
            return ids;
        }
        #endregion

        #region Addresses

        private IQueryable<Address> CreateGetAddressToDeleteByIdQuery(int addressId)
        {
            return Context.Addresses.Where(x => x.AddressId == addressId).Include(x => x.Location);
        }

        /// <summary>
        /// Removes the address from the context.
        /// </summary>
        /// <param name="addressId">The id of the address to delete.</param>
        public void Delete(int addressId)
        {
            var address = CreateGetAddressToDeleteByIdQuery(addressId).FirstOrDefault();
            DoDelete(address);
        }

        /// <summary>
        /// Removes the address from the context.
        /// </summary>
        /// <param name="addressId">The id of the address to delete.</param>
        public async Task DeleteAsync(int addressId)
        {
            var address = await CreateGetAddressToDeleteByIdQuery(addressId).FirstOrDefaultAsync();
            DoDelete(address);
        }

        private void DoDelete(Address addressToDelete)
        {
            if (addressToDelete != null)
            {
                if (addressToDelete.Location != null)
                {
                    Context.Locations.Remove(addressToDelete.Location);
                }
                Context.Addresses.Remove(addressToDelete);
            }
        }

        /// <summary>
        /// Returns the address with the given id.
        /// </summary>
        /// <param name="addressId">The address id.</param>
        /// <returns>The address.</returns>
        public AddressDTO GetAddressById(int addressId)
        {
            var address = AddressQueries.CreateGetAddressDTOByIdQuery(this.Context, addressId).FirstOrDefault();
            logger.Info("Retreived address with the given address id [{0}].", addressId);
            return address;
        }

        /// <summary>
        /// Returns the address with the given id.
        /// </summary>
        /// <param name="addressId">The address id.</param>
        /// <returns>The address.</returns>
        public async Task<AddressDTO> GetAddressByIdAsync(int addressId)
        {
            var address = await AddressQueries.CreateGetAddressDTOByIdQuery(this.Context, addressId).FirstOrDefaultAsync();
            logger.Info("Retreived address with the given address id [{0}].", addressId);
            return address;
        }

        /// <summary>
        /// Creates a new address in the system.
        /// </summary>
        /// <typeparam name="T">The type that is IAddressable, such as Organization.</typeparam>
        /// <param name="additionalAddress">The new address.</param>
        /// <returns>The address entity.</returns>
        public Address Create<T>(AdditionalAddress<T> additionalAddress)
            where T : class, IAddressable 
        {
            var dbSet = Context.Set<T>();
            var id = additionalAddress.GetAddressableEntityId();
            var entity = dbSet.Find(id);
            throwIfEntityNotFound(id, entity, typeof(T));

            var city = Context.Locations.Find(additionalAddress.CityId);
            throwIfLocationNotFound(additionalAddress.CityId, city, "City");

            var country = Context.Locations.Find(additionalAddress.CountryId);
            throwIfLocationNotFound(additionalAddress.CountryId, country, "Country");

            var division = Context.Locations.Find(additionalAddress.DivisionId);
            throwIfLocationNotFound(additionalAddress.DivisionId, division, "Division");

            var existingAddresses = additionalAddress.CreateGetAddressesQuery(this.Context).ToList();

            return DoCreateAddress<T>(entity, additionalAddress, existingAddresses);
        }

        /// <summary>
        /// Creates a new address in the system.
        /// </summary>
        /// <typeparam name="T">The type that is IAddressable, such as Organization.</typeparam>
        /// <param name="additionalAddress">The new address.</param>
        /// <returns>The address entity.</returns>
        public async Task<Address> CreateAsync<T>(AdditionalAddress<T> additionalAddress)
            where T : class, IAddressable
        {
            var dbSet = Context.Set<T>();
            var id = additionalAddress.GetAddressableEntityId();
            var entity = await dbSet.FindAsync(id);
            throwIfEntityNotFound(id, entity, typeof(T));

            var city = await Context.Locations.FindAsync(additionalAddress.CityId);
            throwIfLocationNotFound(additionalAddress.CityId, city, "City");

            var country = await Context.Locations.FindAsync(additionalAddress.CountryId);
            throwIfLocationNotFound(additionalAddress.CountryId, country, "Country");

            var division = await Context.Locations.FindAsync(additionalAddress.DivisionId);
            throwIfLocationNotFound(additionalAddress.DivisionId, division, "Division");

            var existingAddresses = await additionalAddress.CreateGetAddressesQuery(this.Context).ToListAsync();

            return DoCreateAddress<T>(entity, additionalAddress, existingAddresses);
        }

        private void SetAllAddressNotPrimary(ICollection<Address> addresses)
        {
            addresses.ToList().ForEach(x => x.IsPrimary = false);
        }

        private Address DoCreateAddress<T>(T entity, AdditionalAddress<T> additionalAddress, ICollection<Address> existingAddresses)
            where T : class, IAddressable
        {
            logger.Info("Adding an additional address to the [{0}] entity with id [{1}].", typeof(T).Name, additionalAddress.GetAddressableEntityId());
            addressValidator.ValidateCreate(ToEcaAddressValidationEntity(additionalAddress));
            var address = additionalAddress.AddAddress(entity);
            Context.Addresses.Add(address);
            Context.Locations.Add(address.Location);
            additionalAddress.Create.SetHistory(address);
            additionalAddress.Create.SetHistory(address.Location);
            if (additionalAddress.IsPrimary)
            {
                SetAllAddressNotPrimary(existingAddresses);
            }
            return address;
        }

        /// <summary>
        /// Updates an existing address.
        /// </summary>
        /// <param name="updatedAddress">The updated address.</param>
        public void Update(UpdatedEcaAddress updatedAddress)
        {
            var address = Context.Addresses.Find(updatedAddress.AddressId);
            throwIfEntityNotFound(updatedAddress.AddressId, address, typeof(Address));
            var location = Context.Locations.Find(address.LocationId);
            Contract.Assert(location != null, "The address must have a location.");
            var city = Context.Locations.Find(updatedAddress.CityId);
            throwIfLocationNotFound(updatedAddress.CityId, city, "City");

            var country = Context.Locations.Find(updatedAddress.CountryId);
            throwIfLocationNotFound(updatedAddress.CountryId, country, "Country");

            var division = Context.Locations.Find(updatedAddress.DivisionId);
            throwIfLocationNotFound(updatedAddress.DivisionId, division, "Division");

            var otherAddresses = CreateGetOtherEntityAddressesQuery(address).ToList();

            DoUpdate(updatedAddress, location, address, otherAddresses);
        }

        /// <summary>
        /// Updates an existing address.
        /// </summary>
        /// <param name="updatedAddress">The updated address.</param>
        public async Task UpdateAsync(UpdatedEcaAddress updatedAddress)
        {
            var address = await Context.Addresses.FindAsync(updatedAddress.AddressId);
            throwIfEntityNotFound(updatedAddress.AddressId, address, typeof(Address));
            var location = await Context.Locations.FindAsync(address.LocationId);
            Contract.Assert(location != null, "The address must have a location.");

            var city = await Context.Locations.FindAsync(updatedAddress.CityId);
            throwIfLocationNotFound(updatedAddress.CityId, city, "City");

            var country = await Context.Locations.FindAsync(updatedAddress.CountryId);
            throwIfLocationNotFound(updatedAddress.CountryId, country, "Country");

            var division = await Context.Locations.FindAsync(updatedAddress.DivisionId);
            throwIfLocationNotFound(updatedAddress.DivisionId, division, "Division");

            var otherAddresses = await CreateGetOtherEntityAddressesQuery(address).ToListAsync();

            DoUpdate(updatedAddress, location, address, otherAddresses);
        }

        private IQueryable<Address> CreateGetOtherEntityAddressesQuery(Address address)
        {
            return Context.Addresses.Where(x => 
                x.PersonId == address.PersonId 
                && x.OrganizationId == address.OrganizationId 
                && x.AddressId != address.AddressId);
        }

        private void DoUpdate(UpdatedEcaAddress updatedAddress, Location location, Address address, ICollection<Address> otherAddresses)
        {
            Contract.Requires(updatedAddress != null, "The updated address must not be null.");
            Contract.Requires(location != null, "The location must not be null.");
            Contract.Requires(address != null, "The address must not be null.");
            logger.Info("Updating the address with id [{0}].", updatedAddress.AddressId);
            addressValidator.ValidateUpdate(ToEcaAddressValidationEntity(updatedAddress));
            location.CityId = updatedAddress.CityId;
            location.CountryId = updatedAddress.CountryId;
            location.DivisionId = updatedAddress.DivisionId;
            location.LocationName = updatedAddress.LocationName;
            location.LocationTypeId = updatedAddress.LocationTypeId;
            location.PostalCode = updatedAddress.PostalCode;
            location.Street1 = updatedAddress.Street1;
            location.Street2 = updatedAddress.Street2;
            location.Street3 = updatedAddress.Street3;
            address.AddressTypeId = updatedAddress.AddressTypeId;
            address.IsPrimary = updatedAddress.IsPrimary;
            updatedAddress.Update.SetHistory(location);
            updatedAddress.Update.SetHistory(address);
            if (updatedAddress.IsPrimary)
            {
                SetAllAddressNotPrimary(otherAddresses);
            }
        }

        private EcaAddressValidationEntity ToEcaAddressValidationEntity(EcaAddress address)
        {
            return new EcaAddressValidationEntity(addressTypeId: address.AddressTypeId);
        }


        #endregion
    }
}
