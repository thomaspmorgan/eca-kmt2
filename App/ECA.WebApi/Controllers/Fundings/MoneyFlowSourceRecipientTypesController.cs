using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Fundings;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Fundings
{
    /// <summary>
    /// The money flow source recipient types controller handles crud operations on money flow source recipient types.
    /// </summary>
    [Authorize]
    public class MoneyFlowSourceRecipientTypesController : ApiController
    {

        private static readonly ExpressionSorter<MoneyFlowSourceRecipientTypeDTO> DEFAULT_PROJECT_STATUS_DTO_SORTER =
            new ExpressionSorter<MoneyFlowSourceRecipientTypeDTO>(x => x.Name, SortDirection.Ascending);
        private IMoneyFlowSourceRecipientTypeService service;

        /// <summary>
        /// Creates a new instance with the project Type service.
        /// </summary>
        /// <param name="service">The service.</param>
        public MoneyFlowSourceRecipientTypesController(IMoneyFlowSourceRecipientTypeService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the money flow source recipient types currently in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The money flow source recipient types currently in the system.</returns>
        [ResponseType(typeof(PagedQueryResults<MoneyFlowSourceRecipientTypeDTO>))]
        public async Task<IHttpActionResult> GetMoneyFlowSourceRecipientTypesAsync([FromUri]PagingQueryBindingModel<MoneyFlowSourceRecipientTypeDTO> queryModel)
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
