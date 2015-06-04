using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System;
using System.Collections.Generic;
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
    /// The organizations controller
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class OrganizationsController : ApiController
    {
        private static readonly ExpressionSorter<SimpleOrganizationDTO> DEFAULT_SORTER = new ExpressionSorter<SimpleOrganizationDTO>(x => x.Name, SortDirection.Ascending);
        private IOrganizationService service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">The service</param>
        public OrganizationsController(IOrganizationService service)
        {
            Contract.Requires(service != null, "The organization service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the organizations in the system.
        /// </summary>
        /// <param name="queryModel">The query operator.</param>
        /// <returns>The organizations in the system.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleOrganizationDTO>))]
        public async Task<IHttpActionResult> GetOrganizationsAsync([FromUri]PagingQueryBindingModel<SimpleOrganizationDTO> queryModel) {
            if (ModelState.IsValid)
            {
                var results = await service.GetOrganizationsAsync(queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the organization with the given id.
        /// </summary>
        /// <param name="id">The id of the organization.</param>
        /// <returns>The organization.</returns>
        [ResponseType(typeof(OrganizationDTO))]
        public async Task<IHttpActionResult> GetOrganizationByIdAsync(int id)
        {
            var results = await service.GetOrganizationByIdAsync(id);
            if (results != null)
            {
                return Ok(results);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
