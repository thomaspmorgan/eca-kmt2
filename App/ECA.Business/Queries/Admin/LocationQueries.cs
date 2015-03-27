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
        /// Returns a query to retrieve location dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query.</returns>
        public static IQueryable<LocationDTO> CreateGetLocationsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = context.Locations.Select(x => new LocationDTO
            {
                Id = x.LocationId,
                LocationTypeId = x.LocationTypeId,
                LocationTypeName = x.LocationType.LocationTypeName,
                Name = x.LocationName
            });
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
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(locationIds != null, "The location ids must not be null.");
            return context.Locations
                .Where(x => locationIds.Contains(x.LocationId))
                .Select(x => x.LocationTypeId)
                .OrderBy(x => x)
                .Distinct();
        }
    }
}
