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

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The LocationService is a lookup service that performs crud operations on Locations.
    /// </summary>
    public class LocationService : LookupService<LocationDTO>, ILocationService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new location service with the context to operate against.
        /// </summary>
        /// <param name="context">The context.</param>
        public LocationService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
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
    }
}
