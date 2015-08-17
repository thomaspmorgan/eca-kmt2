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

            var query = from location in context.Locations
                        let locationType = location.LocationType

                        let hasCountry = location.Country != null
                        let country = location.Country

                        let hasDivision = location.Division != null
                        let division = location.Division

                        let hasCity = location.City != null
                        let city = location.City

                        let hasRegion = location.Region != null
                        let region = location.Region

                        select new LocationDTO
                        {
                            Country = hasCountry ? country.LocationName : null,
                            CountryIso = hasCountry ? country.LocationIso : null,
                            CountryIso2 = hasCountry ? country.LocationIso2 : null,
                            CountryId = location.CountryId,
                            Division = hasDivision ? division.LocationName : null,
                            DivisionIso = hasDivision ? division.LocationIso : null,
                            DivisionIso2 = hasDivision ? division.LocationIso2 : null,
                            DivisionId = location.DivisionId,
                            Id = location.LocationId,
                            LocationTypeId = locationType.LocationTypeId,
                            LocationTypeName = locationType.LocationTypeName,
                            Name = location.LocationName,
                            Region = hasRegion ? region.LocationName : null,
                            RegionIso = hasRegion ? region.LocationIso : null,
                            RegionIso2 = hasRegion ? region.LocationIso2 : null,
                            RegionId = location.RegionId,
                            City = hasCity ? city.LocationName : null,
                            CityId = location.CityId,
                            Longitude = location.Longitude,
                            Latitude = location.Latitude,
                            LocationIso = location.LocationIso,
                            LocationIso2 = location.LocationIso2
                        };

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
