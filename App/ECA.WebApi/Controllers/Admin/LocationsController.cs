using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Admin
{
    public class LocationsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of projects.
        /// </summary>
        private static readonly ExpressionSorter<LocationDTO> DEFAULT_LOCATION_DTO_SORTER = new ExpressionSorter<LocationDTO>(x => x.Name, SortDirection.Ascending);

        private ILocationService service;

        /// <summary>
        /// Creates a new ProjectsController with the given project service.
        /// </summary>
        /// <param name="service">The service.</param>
        public LocationsController(ILocationService service)
        {
            Debug.Assert(service != null, "The project service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns a listing of the projects by program.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryModel">The page, filter and sort information.</param>
        /// <returns>The list of projects by program.</returns>
        [ResponseType(typeof(PagedQueryResults<LocationDTO>))]
        public async Task<IHttpActionResult> GetLocationsAsync(PagingQueryBindingModel queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetLocationsAsync(queryModel.ToQueryableOperator<LocationDTO>(DEFAULT_LOCATION_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
