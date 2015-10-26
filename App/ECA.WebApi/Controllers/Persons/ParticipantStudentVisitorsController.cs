using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// The ParticipantStudentVisitorsController handles crud operations on ECA participant that are persons and their student visitor info
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class ParticipantStudentVisitorsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of participant student visitors.
        /// </summary>
        private static readonly ExpressionSorter<ParticipantStudentVisitorDTO> DEFAULT_SORTER = new ExpressionSorter<ParticipantStudentVisitorDTO>(x => x.ParticipantId, SortDirection.Ascending);

        private IParticipantStudentVisitorService service;

        /// <summary>
        /// Creates a new ParticipantStudentVisitorsController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public ParticipantStudentVisitorsController(IParticipantStudentVisitorService service)
        {
            Contract.Requires(service != null, "The participantPersonSevis service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participantStudentVisitors.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <returns>The list of participantStudentVisitors.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantStudentVisitorDTO>))]
        [Route("ParticipantStudentVisitors")]
        public async Task<IHttpActionResult> GetParticipantPersonsAsync([FromUri]PagingQueryBindingModel<ParticipantStudentVisitorDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantStudentVisitorsAsync(queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participants and there student visitor info by project id.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <param name="projectId">The id of the project to get participants for.</param>
        /// <returns>The list of participantStudentVisitors.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantStudentVisitorDTO>))]
        [Route("Projects/{projectId:int}/ParticipantStudentVisitors")]
        public async Task<IHttpActionResult> GetParticipantStudentVisitorsByProjectIdAsync(int projectId, [FromUri]PagingQueryBindingModel<ParticipantStudentVisitorDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantStudentVisitorsByProjectIdAsync(projectId, queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Retrieves the participantStudentVisitor with the given id.
        /// </summary>
        /// <param name="participantId">The id of the participant.</param>
        /// <returns>The participantStudentVisitor with the given id.</returns>
        [ResponseType(typeof(ParticipantStudentVisitorDTO))]
        [Route("ParticipantStudentVisitors/{participantId:int}")]
        public async Task<IHttpActionResult> GetParticipantStudentVisitorByIdAsync(int participantId) 
        {
            var participantPerson = await service.GetParticipantStudentVisitorByIdAsync(participantId);
            if (participantPerson != null)
            {
                return Ok(participantPerson);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
