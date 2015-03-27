using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An ILocationService is capable of performing crud operations on a location.
    /// </summary>
    public interface ILocationService
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
    }
}
