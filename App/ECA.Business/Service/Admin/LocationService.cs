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

namespace ECA.Business.Service.Admin
{
    public class LocationService : DbContextService<EcaContext>, ILocationService
    {

        public LocationService(EcaContext context) : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get
        /// <summary>
        /// Returns a list of locations.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>A list of locations in the system.</returns>
        public PagedQueryResults<LocationDTO> GetLocations(QueryableOperator<LocationDTO> queryOperator)
        {
            return LocationQueries.CreateGetLocationsQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns a list of locations.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>A list of locations in the system.</returns>
        public Task<PagedQueryResults<LocationDTO>> GetLocationsAsync(QueryableOperator<LocationDTO> queryOperator)
        {
            return LocationQueries.CreateGetLocationsQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
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
            return LocationQueries.CreateGetLocationTypeIdsQuery(this.Context, locationIds).ToList();
        }

        /// <summary>
        /// Returns the distinct list of location types for the given locations by id.
        /// </summary>
        /// <param name="locationIds">The locations by id.</param>
        /// <returns>The list of location type ids.</returns>
        public Task<List<int>> GetLocationTypeIdsAsync(List<int> locationIds)
        {   
            return LocationQueries.CreateGetLocationTypeIdsQuery(this.Context, locationIds).ToListAsync();
        }
        #endregion

    }
}
