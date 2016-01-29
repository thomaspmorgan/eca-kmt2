using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using GeoTimeZone;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An ILocationService is capable of performing crud operations on a location.
    /// </summary>
    [ContractClass(typeof(LocationServiceContract))]
    public interface ILocationService : ISaveable
    {
        /// <summary>
        /// Returns a list of locations.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>A list of locations in the system.</returns>
        PagedQueryResults<LocationDTO> Get(QueryableOperator<LocationDTO> queryOperator);

        /// <summary>
        /// Returns a list of locations.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>A list of locations in the system.</returns>
        Task<PagedQueryResults<LocationDTO>> GetAsync(QueryableOperator<LocationDTO> queryOperator);

        /// <summary>
        /// Returns the distinct list of location types for the given locations by id.
        /// </summary>
        /// <param name="locationIds">The locations by id.</param>
        /// <returns>The list of location type ids.</returns>
        List<int> GetLocationTypeIds(List<int> locationIds);

        /// <summary>
        /// Returns the distinct list of location types for the given locations by id.
        /// </summary>
        /// <param name="locationIds">The locations by id.</param>
        /// <returns>The list of location type ids.</returns>
        Task<List<int>> GetLocationTypeIdsAsync(List<int> locationIds);
        
        /// <summary>
        /// Updates an existing address.
        /// </summary>
        /// <param name="updatedAddress">The updated address.</param>
        void Update(UpdatedEcaAddress updatedAddress);

        /// <summary>
        /// Updates an existing address.
        /// </summary>
        /// <param name="updatedAddress">The updated address.</param>
        Task UpdateAsync(UpdatedEcaAddress updatedAddress);

        /// <summary>
        /// Creates a new address in the system.
        /// </summary>
        /// <typeparam name="T">The type that is IAddressable, such as Organization.</typeparam>
        /// <param name="additionalAddress">The new address.</param>
        /// <returns>The address entity.</returns>
        Address Create<T>(AdditionalAddress<T> additionalAddress)
            where T : class, IAddressable;

        /// <summary>
        /// Creates a new address in the system.
        /// </summary>
        /// <typeparam name="T">The type that is IAddressable, such as Organization.</typeparam>
        /// <param name="additionalAddress">The new address.</param>
        /// <returns>The address entity.</returns>
        Task<Address> CreateAsync<T>(AdditionalAddress<T> additionalAddress)
            where T : class, IAddressable;

        /// <summary>
        /// Returns the address with the given id.
        /// </summary>
        /// <param name="addressId">The address id.</param>
        /// <returns>The address.</returns>
        AddressDTO GetAddressById(int addressId);

        /// <summary>
        /// Returns the address with the given id.
        /// </summary>
        /// <param name="addressId">The address id.</param>
        /// <returns>The address.</returns>
        Task<AddressDTO> GetAddressByIdAsync(int addressId);

        /// <summary>
        /// Removes the address from the context.
        /// </summary>
        /// <param name="addressId">The id of the address to delete.</param>
        void Delete(int addressId);

        /// <summary>
        /// Removes the address from the context.
        /// </summary>
        /// <param name="addressId">The id of the address to delete.</param>
        Task DeleteAsync(int addressId);

        /// <summary>
        /// Returns the location with the given id, or null if it is not found.
        /// </summary>
        /// <param name="locationId">The id of the location.</param>
        /// <returns>The location, or null if it does not exist.</returns>
        LocationDTO GetLocationById(int locationId);

        /// <summary>
        /// Returns the location with the given id, or null if it is not found.
        /// </summary>
        /// <param name="locationId">The id of the location.</param>
        /// <returns>The location, or null if it does not exist.</returns>
        Task<LocationDTO> GetLocationByIdAsync(int locationId);

        /// <summary>
        /// Updates the system's location with the given updated location.
        /// </summary>
        /// <param name="updatedLocation">The updated location.</param>
        void Update(UpdatedLocation updatedLocation);

        /// <summary>
        /// Updates the system's location with the given updated location.
        /// </summary>
        /// <param name="updatedLocation">The updated location.</param>
        Task UpdateAsync(UpdatedLocation updatedLocation);

        /// <summary>
        /// Adds the given location to the eca system.
        /// </summary>
        /// <param name="additionalLocation">The new location.</param>
        /// <returns>The task.</returns>
        Location Create(AdditionalLocation additionalLocation);

        /// <summary>
        /// Adds the given location to the eca system.
        /// </summary>
        /// <param name="additionalLocation">The new location.</param>
        /// <returns>The task.</returns>
        Task<Location> CreateAsync(AdditionalLocation additionalLocation);

        /// <summary>
        /// Returns timezone results for the location with the given latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The timezone details.</returns>
        TimeZoneResult GetIANATimezone(double latitude, double longitude);

        /// <summary>
        /// Returns timezone results for the location with the given location id.
        /// </summary>
        /// <param name="locationId">The id of the location.</param>
        /// <returns>The timezone results, or null if not found.</returns>
        TimeZoneResult GetIANATimezone(int locationId);

        /// <summary>
        /// Returns timezone results for the location with the given location id.
        /// </summary>
        /// <param name="locationId">The id of the location.</param>
        /// <returns>The timezone results, or null if not found.</returns>
        Task<TimeZoneResult> GetIANATimezoneAsync(int locationId);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(ILocationService))]
    public abstract class LocationServiceContract : ILocationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<LocationDTO> Get(QueryableOperator<LocationDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<LocationDTO>> GetAsync(QueryableOperator<LocationDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<LocationDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationIds"></param>
        /// <returns></returns>
        public List<int> GetLocationTypeIds(List<int> locationIds)
        {
            Contract.Requires(locationIds != null, "The location ids must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationIds"></param>
        /// <returns></returns>
        public Task<List<int>> GetLocationTypeIdsAsync(List<int> locationIds)
        {
            Contract.Requires(locationIds != null, "The location ids must not be null.");
            return Task.FromResult<List<int>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedAddress"></param>
        public void Update(UpdatedEcaAddress updatedAddress)
        {
            Contract.Requires(updatedAddress != null, "The updated address must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedAddress"></param>
        /// <returns></returns>
        public Task UpdateAsync(UpdatedEcaAddress updatedAddress)
        {
            Contract.Requires(updatedAddress != null, "The updated address must not be null.");
            return Task.FromResult<Object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="additionalAddress"></param>
        /// <returns></returns>
        public Address Create<T>(AdditionalAddress<T> additionalAddress) where T : class, IAddressable
        {
            Contract.Requires(additionalAddress != null, "The additional address must not be null.");
            Contract.Ensures(Contract.Result<Address>() != null, "The address returned must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="additionalAddress"></param>
        /// <returns></returns>
        public Task<Address> CreateAsync<T>(AdditionalAddress<T> additionalAddress) where T : class, IAddressable
        {
            Contract.Requires(additionalAddress != null, "The additional address must not be null.");
            Contract.Ensures(Contract.Result<Task<Address>>() != null, "The address returned must not be null.");
            return Task.FromResult<Address>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public AddressDTO GetAddressById(int addressId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public Task<AddressDTO> GetAddressByIdAsync(int addressId)
        {
            return Task.FromResult<AddressDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult<int>(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addressId"></param>
        public void Delete(int addressId)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public Task DeleteAsync(int addressId)
        {
            return Task.FromResult<object>(null);   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public LocationDTO GetLocationById(int locationId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public Task<LocationDTO> GetLocationByIdAsync(int locationId)
        {
            return Task.FromResult<LocationDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedLocation"></param>
        public void Update(UpdatedLocation updatedLocation)
        {
            Contract.Requires(updatedLocation != null, "The updated location must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedLocation"></param>
        /// <returns></returns>
        public Task UpdateAsync(UpdatedLocation updatedLocation)
        {
            Contract.Requires(updatedLocation != null, "The updated location must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="additionalLocation"></param>
        /// <returns></returns>
        public Location Create(AdditionalLocation additionalLocation)
        {
            Contract.Requires(additionalLocation != null, "The additional location must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="additionalLocation"></param>
        /// <returns></returns>
        public Task<Location> CreateAsync(AdditionalLocation additionalLocation)
        {
            Contract.Requires(additionalLocation != null, "The additional location must not be null.");
            return Task.FromResult<Location>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public TimeZoneResult GetIANATimezone(double latitude, double longitude)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public TimeZoneResult GetIANATimezone(int locationId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public Task<TimeZoneResult> GetIANATimezoneAsync(int locationId)
        {
            return Task.FromResult<TimeZoneResult>(null);
        }
    }
}
