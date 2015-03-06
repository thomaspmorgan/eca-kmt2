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
    [RoutePrefix("api")]
    public class MoneyFlowsController : ApiController
    {
        private static readonly ExpressionSorter<MoneyFlowDTO> DEFAULT_MONEY_FLOW_DTO_SORTER = new ExpressionSorter<MoneyFlowDTO>(x => x.SourceName, SortDirection.Ascending);

        private IMoneyFlowService moneyFlowService;

        public MoneyFlowsController(IMoneyFlowService moneyFlowService)
        {
            Debug.Assert(moneyFlowService != null, "The money flow service must not be null.");
            this.moneyFlowService = moneyFlowService;
        }

        [ResponseType(typeof(PagedQueryResults<MoneyFlowDTO>))]
        [Route("Projects/{projectId:int}/MoneyFlows")]
        public async Task<IHttpActionResult> GetMoneyFlowsByProjectId(int projectId, [FromUri]PagingQueryBindingModel queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.moneyFlowService.GetMoneyFlowsByProjectIdAsync(projectId, queryModel.ToQueryableOperator<MoneyFlowDTO>(DEFAULT_MONEY_FLOW_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /**
        [Route("api/Projects/{id:int}/MoneyFlows")]
        public IEnumerable<MoneyFlowDTO> GetMoneyFlowsByProject(int id)
        {
            var moneyOutFlows = db.MoneyFlows
                .Include(m => m.RecipientParticipant)
                .Include(m => m.RecipientParticipant.Organization)
                //.Include(m => m.RecipientParticipant.Person.Names)
                .Include(m => m.RecipientAccommodation)
                .Include(m => m.RecipientTransportation)
                .Include(m => m.SourceProject)
                .Include(m => m.SourceType)
                .Include(m => m.RecipientType)
                .Where(m => m.SourceProjectId == id);

            var moneyInFlows = db.MoneyFlows
                .Include(m => m.SourceProgram)
                .Include(m => m.RecipientProject)
                .Where(m => m.RecipientProjectId == id);

            var moneyFlows = moneyInFlows.Union(moneyOutFlows);

            var moneyFlowDTOs = Mapper.Map<IEnumerable<MoneyFlow>, IEnumerable<MoneyFlowDTO>>(moneyFlows);

            return moneyFlowDTOs;
        }
        */
    }
}