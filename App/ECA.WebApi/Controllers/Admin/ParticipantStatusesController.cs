using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The ParticipantTypesController provides clients with the participant types in the eca system.
    /// </summary>
    [Authorize]
    public class ParticipantStatusesController : ApiController
    {
        private static readonly ExpressionSorter<ParticipantStatusDTO> DEFAULT_SORTER = new ExpressionSorter<ParticipantStatusDTO>(x => x.Name, SortDirection.Ascending);

        private readonly IParticipantStatusService service;

        /// <summary>
        /// Creates a new ParticipantTypesController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public ParticipantStatusesController(IParticipantStatusService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Returns the participant statii in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The participant statii.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantTypeDTO>))]
        public async Task<IHttpActionResult> GetAsync([FromUri]PagingQueryBindingModel<ParticipantStatusDTO> queryModel)
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
