using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Models.Person;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ECA.WebApi.Security;

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
        private IParticipantService participantService;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ParticipantStudentVisitorsController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="participantService">the participant service.</param>
        /// <param name="userProvider">the user provider service.</param>
        public ParticipantStudentVisitorsController(IParticipantStudentVisitorService service, IParticipantService participantService, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The participantPersonSevis service must not be null.");
            this.service = service;
            this.participantService = participantService;
            this.userProvider = userProvider;
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
                var participant = await participantService.GetParticipantByIdAsync(participantId);
                if (participant != null)
                {
                    var participantStudentVisitor = await NewParticipantStudentVisitorAsync(participantId);
                    return Ok(participantStudentVisitor);
                }
                else
                    return NotFound();
            }
        }


        /// <summary>
        /// Updates the new participantStudentVisitor with the given participantId.
        /// </summary>
        /// <param name="model">The new participantStudentVisitor.</param>
        /// <returns>The saved participantStudentVisitor.</returns>
        [Route("ParticipantStudentVisitors")]
        public async Task<IHttpActionResult> PutParticipantStudentVisitorAsync([FromBody]UpdatedParticipantStudentVisitorBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var participantStudentVisitorDTO = await service.UpdateAsync(model.ToUpdatedParticipantStudentVisitor(businessUser));
                await service.SaveChangesAsync();
                participantStudentVisitorDTO = await service.GetParticipantStudentVisitorByIdAsync(participantStudentVisitorDTO.ParticipantId);
                return Ok(participantStudentVisitorDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
        private async Task<ParticipantStudentVisitorDTO> NewParticipantStudentVisitorAsync(int participantId)
        {
            var currentUser = userProvider.GetCurrentUser();
            var businessUser = userProvider.GetBusinessUser(currentUser);
            await service.CreateParticipantStudentVisitor(participantId, businessUser);
            await service.SaveChangesAsync();
            return await service.GetParticipantStudentVisitorByIdAsync(participantId);
        }
    }
}
