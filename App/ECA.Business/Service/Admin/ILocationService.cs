using System;
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
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Admin.LocationDTO> GetLocations(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Admin.LocationDTO> queryOperator);

        /// <summary>
        /// Returns a list of locations.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>A list of locations in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Admin.LocationDTO>> GetLocationsAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Admin.LocationDTO> queryOperator);
    }
}
