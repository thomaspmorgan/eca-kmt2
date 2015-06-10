using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Projects;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
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

namespace ECA.WebApi.Controllers.Projects
{
    /// <summary>
    /// The Project Status controller handles crud operations on project stati.
    /// </summary>
    [Authorize]
    public class ProjectStatusesController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of foci.
        /// </summary>
        private static readonly ExpressionSorter<ProjectStatusDTO> DEFAULT_PROJECT_STATUS_DTO_SORTER = 
            new ExpressionSorter<ProjectStatusDTO>(x => x.Name, SortDirection.Ascending);
        private IProjectStatusService service;

        /// <summary>
        /// Creates a new instance with the project status service.
        /// </summary>
        /// <param name="service">The service.</param>
        public ProjectStatusesController(IProjectStatusService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the project stati currently in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The project stati currently in the system.</returns>
        [ResponseType(typeof(PagedQueryResults<ProjectStatusDTO>))]
        public async Task<IHttpActionResult> GetProjectStati([FromUri]PagingQueryBindingModel<ProjectStatusDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_PROJECT_STATUS_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
