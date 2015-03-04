using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Admin
{
    /// <summary>
    /// Contains queries for locations against a db context.
    /// </summary>
    public static class LocationQueries
    {
        /// <summary>
        /// Returns a query to retrieve filtered and sorted location dtos from the system.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The queryable operator.</param>
        /// <returns>The query to retrieve filtered and sorted locations.</returns>
        public static IQueryable<LocationDTO> CreateGetLocationsQuery(EcaContext context, QueryableOperator<LocationDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = context.Locations.Select(x => new LocationDTO
            {
                Id = x.LocationId,
                LocationTypeId = x.LocationTypeId,
                LocationTypeName = x.LocationType.LocationTypeName,
                Name = x.LocationName
            });
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a query to get all location types given the location ids.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="locationIds">The location ids to get types for.</param>
        /// <returns>The query to get the distinct list of location types.</returns>
        public static IQueryable<int> CreateGetLocationTypeIdsQuery(EcaContext context, List<int> locationIds)
        {
            return context.Locations
                .Where(x => locationIds.Contains(x.LocationId))
                .Select(x => x.LocationTypeId)
                .OrderBy(x => x)
                .Distinct();
        }
    }
}
