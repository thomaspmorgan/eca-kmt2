using ECA.Business.Service.Lookup;
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
    /// The ProminentCategoryController provides clients with the Prominent Categories in the eca system.
    /// </summary>
    [Authorize]
    public class ProminentCategoriesController : ApiController
    {
        private static readonly ExpressionSorter<ProminentCategoryDTO> DEFAULT_SORTER = new ExpressionSorter<ProminentCategoryDTO>(x => x.Name, SortDirection.Ascending);

        private readonly IProminentCategoryService service;

        /// <summary>
        /// Creates a new ProminentCategoryController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public ProminentCategoriesController(IProminentCategoryService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the Prominent Categories in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The Prominent Categories.</returns>
        [ResponseType(typeof(PagedQueryResults<ProminentCategoryDTO>))]
        public async Task<IHttpActionResult> GetAsync([FromUri]PagingQueryBindingModel<ProminentCategoryDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var dtos = await service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(dtos);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
