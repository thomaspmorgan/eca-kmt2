using System;
using System.Collections.Generic;
using System.Data;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// Controller for managing moneyflows
    /// </summary>
    [RoutePrefix("api")]
    public class MoneyFlowsController : ApiController
    {
        /// <summary>
        /// The default sorter
        /// </summary>
        private static readonly ExpressionSorter<MoneyFlowDTO> DEFAULT_MONEY_FLOW_DTO_SORTER = new ExpressionSorter<MoneyFlowDTO>(x => x.TransactionDate, SortDirection.Descending);

        /// <summary>
        /// The injected moneyflow service
        /// </summary>
        private IMoneyFlowService moneyFlowService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="moneyFlowService">The moneyflow service</param>
        public MoneyFlowsController(IMoneyFlowService moneyFlowService)
        {
            Debug.Assert(moneyFlowService != null, "The money flow service must not be null.");
            this.moneyFlowService = moneyFlowService;
        }

        /// <summary>
        /// Gets moneyflows by the project id
        /// </summary>
        /// <param name="projectId">The project id to query for associated moneyflows</param>
        /// <param name="queryModel">The page, sort, and filter info</param>
        /// <returns>Returns a list of moneyflows that are paged, filtered, and sorted</returns>
        [ResponseType(typeof(PagedQueryResults<MoneyFlowDTO>))]
        [Route("Projects/{projectId:int}/MoneyFlows")]
        public async Task<IHttpActionResult> GetMoneyFlowsByProjectId(int projectId, [FromUri]PagingQueryBindingModel<MoneyFlowDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.moneyFlowService.GetMoneyFlowsByProjectIdAsync(projectId, queryModel.ToQueryableOperator(DEFAULT_MONEY_FLOW_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}