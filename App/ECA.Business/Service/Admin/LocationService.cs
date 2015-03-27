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
using ECA.Core.Logging;
using ECA.Business.Service.Lookup;

namespace ECA.Business.Service.Admin
{
    public class LocationService : LookupService<LocationDTO>, ILocationService
    {
        public LocationService(EcaContext context, ILogger logger) : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
        }

        #region Get
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
