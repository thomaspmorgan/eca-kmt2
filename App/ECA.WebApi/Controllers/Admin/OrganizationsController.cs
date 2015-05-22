using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.WebApi.Models.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

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
        /// 
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
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
    }
}
