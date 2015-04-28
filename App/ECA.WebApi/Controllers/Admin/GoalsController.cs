using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The goals controller is capable of performing operations on goals in the ECA system.
    /// </summary>
    [Authorize]
    public class GoalsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of goals.
        /// </summary>
        private static readonly ExpressionSorter<GoalDTO> DEFAULT_GOAL_DTO_SORTER = new ExpressionSorter<GoalDTO>(x => x.Name, SortDirection.Ascending);

        private IGoalService service;

        /// <summary>
        /// Creates a new GoalsController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public GoalsController(IGoalService service)
        {
            Contract.Requires(service != null, "The goal service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns a listing of the goals.
        /// </summary>
        /// <param name="queryModel">The page, filter and sort information.</param>
        /// <returns>The list of goals.</returns>
        [ResponseType(typeof(PagedQueryResults<GoalDTO>))]
        public async Task<IHttpActionResult> GetLocationsAsync([FromUri]PagingQueryBindingModel<GoalDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_GOAL_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
