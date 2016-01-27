using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using GeoTimeZone;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The LocationsController is capable of performing crud operations on locations.
    /// </summary>
    [Authorize]
    [RoutePrefix("api")]
    public class LocationsController : ApiController
    {
        /// <summary>
        /// The default sorter for location types.
        /// </summary>
        private static readonly ExpressionSorter<SimpleLookupDTO> DEFAULT_LOCATION_TYPE_SORTER = new ExpressionSorter<SimpleLookupDTO>(x => x.Value, SortDirection.Ascending);

        /// <summary>
        /// The default sorter for a list of locations.
        /// </summary>
        private static readonly ExpressionSorter<LocationDTO> DEFAULT_LOCATION_DTO_SORTER = new ExpressionSorter<LocationDTO>(x => x.Name, SortDirection.Ascending);

        private ILocationService locationService;
        private ILocationTypeService locationTypeService;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ProjectsController with the given location service.
        /// </summary>
        /// <param name="locationService">The service.</param>
        /// <param name="locationTypeService">The location type service.</param>
        /// <param name="userProvider">The user provider.</param>
        public LocationsController(ILocationService locationService, ILocationTypeService locationTypeService, IUserProvider userProvider)
        {
            Contract.Requires(locationService != null, "The location service must not be null.");
            Contract.Requires(locationTypeService != null, "The location type service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.userProvider = userProvider;
            this.locationService = locationService;
            this.locationTypeService = locationTypeService;
        }

        /// <summary>
        /// Returns a listing of the locations.
        /// </summary>
        /// <param name="queryModel">The page, filter and sort information.</param>
        /// <returns>The list of locations.</returns>
        [ResponseType(typeof(PagedQueryResults<LocationDTO>))]
        [Route("Locations")]
        public async Task<IHttpActionResult> GetLocationsAsync([FromUri]PagingQueryBindingModel<LocationDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.locationService.GetAsync(queryModel.ToQueryableOperator(
                    DEFAULT_LOCATION_DTO_SORTER,
                    x => x.City,
                    x => x.Country,
                    x => x.Division,
                    x => x.Region,
                    x => x.Name,
                    x => x.LocationIso,
                    x => x.LocationIso2,
                    x => x.LocationTypeName));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Creates a new location and saves it to the system.
        /// </summary>
        /// <param name="model">The new location.</param>
        /// <returns>The saved location.</returns>
        [ResponseType(typeof(LocationDTO))]
        [Route("Locations")]
        public async Task<IHttpActionResult> PostCreateLocationAsync([FromBody]LocationBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(currentUser);
                var location = await this.locationService.CreateAsync(model.ToAdditionalLocation(businessUser));
                await this.locationService.SaveChangesAsync();
                var dto = await this.locationService.GetLocationByIdAsync(location.LocationId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Update a location and saves it to the system.
        /// </summary>
        /// <param name="model">The updated location.</param>
        /// <returns>The saved location.</returns>
        [ResponseType(typeof(LocationDTO))]
        public async Task<IHttpActionResult> PutUpdateLocationAsync([FromBody]UpdatedLocationBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(currentUser);
                await this.locationService.UpdateAsync(model.ToUpdatedLocation(businessUser));
                await this.locationService.SaveChangesAsync();
                var dto = await this.locationService.GetLocationByIdAsync(model.Id);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the location types in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The types in the system.</returns>
        [ResponseType(typeof(PagedQueryResults<object>))]
        [Route("Locations/Types")]
        public async Task<IHttpActionResult> GetLocationTypesAsync([FromUri]PagingQueryBindingModel<SimpleLookupDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var types = await this.locationTypeService.GetAsync(queryModel.ToQueryableOperator(DEFAULT_LOCATION_TYPE_SORTER));
                return Ok(types);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #region Timezone

        /// <summary>
        /// Returns timezone information for the given latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The timezone result.</returns>
        [Route("Location/Timezone")]
        [ResponseType(typeof(TimeZoneResult))]
        public IHttpActionResult GetTimezoneByLatitudeAndLongitude(double latitude, double longitude)
        {
            return Ok(locationService.GetIANATimezone(latitude, longitude));
        }

        /// <summary>
        /// Returns timezone information for the location with the given id.
        /// </summary>
        /// <param name="id">The id of the location.</param>
        /// <returns>The timezone information for the location with the given id.</returns>
        [Route("Location/{id:int}/Timezone")]
        public async Task<IHttpActionResult> GetTimezoneByLocationIdAsync(int id)
        {
            var result = await locationService.GetIANATimezoneAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
        #endregion
    }
}
