using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
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
    }
}
