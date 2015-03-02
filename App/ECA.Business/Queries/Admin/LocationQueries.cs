using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Data;
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
    }
}
