using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Projects
{
    /// <summary>
    /// The Project Status controller handles crud operations on sevis comm statuses.
    /// </summary>
    [Authorize]
    public class SevisCommStatusesController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of sevis comm statuses.
        /// </summary>
        private static readonly ExpressionSorter<SevisCommStatusDTO> DEFAULT_SEVIS_COMM_STATUS_DTO_SORTER = 
            new ExpressionSorter<SevisCommStatusDTO>(x => x.Name, SortDirection.Ascending);
        private ISevisCommStatusService service;

        /// <summary>
        /// Creates a new instance with the SEVIS Comm status service.
        /// </summary>
        /// <param name="service">The service.</param>
        public SevisCommStatusesController(ISevisCommStatusService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the SEVIS communication statuses currently in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The project SEVIS communication statuses currently in the system.</returns>
        [ResponseType(typeof(PagedQueryResults<SevisCommStatusDTO>))]
        public async Task<IHttpActionResult> GetSevisCommStatusesAsync([FromUri]PagingQueryBindingModel<SevisCommStatusDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_SEVIS_COMM_STATUS_DTO_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
