using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
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

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The Project Type controller handles crud operations on project stati.
    /// </summary>
    [Authorize]
    public class MoneyFlowTypesController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of foci.
        /// </summary>
        private static readonly ExpressionSorter<MoneyFlowTypeDTO> DEFAULT_PROJECT_STATUS_DTO_SORTER = 
            new ExpressionSorter<MoneyFlowTypeDTO>(x => x.Name, SortDirection.Ascending);
        private IMoneyFlowTypeService service;

        /// <summary>
        /// Creates a new instance with the project Type service.
        /// </summary>
        /// <param name="service">The service.</param>
        public MoneyFlowTypesController(IMoneyFlowTypeService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the project stati currently in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The project stati currently in the system.</returns>
        [ResponseType(typeof(PagedQueryResults<MoneyFlowTypeDTO>))]
        public async Task<IHttpActionResult> GetMoneyFlowStati([FromUri]PagingQueryBindingModel<MoneyFlowTypeDTO> queryModel)
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
