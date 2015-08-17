using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
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
    public class LocationsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of locations.
        /// </summary>
        private static readonly ExpressionSorter<LocationDTO> DEFAULT_LOCATION_DTO_SORTER = new ExpressionSorter<LocationDTO>(x => x.Name, SortDirection.Ascending);

        private ILocationService service;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ProjectsController with the given location service.
        /// </summary>
        /// <param name="service">The service.</param>
        public LocationsController(ILocationService service, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The location service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.userProvider = userProvider;
            this.service = service;
        }

        /// <summary>
        /// Returns a listing of the locations.
        /// </summary>
        /// <param name="queryModel">The page, filter and sort information.</param>
        /// <returns>The list of locations.</returns>
        [ResponseType(typeof(PagedQueryResults<LocationDTO>))]
        public async Task<IHttpActionResult> GetLocationsAsync([FromUri]PagingQueryBindingModel<LocationDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_LOCATION_DTO_SORTER));
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
        public async Task<IHttpActionResult> PostCreateLocationAsync([FromBody]LocationBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(currentUser);
                var location = await this.service.CreateAsync(model.ToAdditionalLocation(businessUser));
                await this.service.SaveChangesAsync();
                var dto = await this.service.GetLocationByIdAsync(location.LocationId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// UPdate a location and saves it to the system.
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
                await this.service.UpdateAsync(model.ToUpdatedLocation(businessUser));
                await this.service.SaveChangesAsync();
                var dto = await this.service.GetLocationByIdAsync(model.Id);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
