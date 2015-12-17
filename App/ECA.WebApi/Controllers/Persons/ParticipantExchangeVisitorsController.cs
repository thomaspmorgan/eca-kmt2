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
    /// The ParticipantExchangeVisitorsController handles crud operations on ECA participant that are persons and their exchange visitor info
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class ParticipantExchangeVisitorsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of participant student visitors.
        /// </summary>
        private static readonly ExpressionSorter<ParticipantExchangeVisitorDTO> DEFAULT_SORTER = new ExpressionSorter<ParticipantExchangeVisitorDTO>(x => x.ParticipantId, SortDirection.Ascending);

        private IParticipantExchangeVisitorService service;
        private IParticipantService participantService;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ParticipantExchangeVisitorsController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="participantService">the participant service.</param>
        /// <param name="userProvider">the user provider service.</param>
        public ParticipantExchangeVisitorsController(IParticipantExchangeVisitorService service, IParticipantService participantService, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The participantPersonSevis service must not be null.");
            this.service = service;
            this.participantService = participantService;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participantExchangeVisitors
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <returns>The list of participantExchangeVisitors.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantExchangeVisitorDTO>))]
        [Route("ParticipantExchangeVisitors")]
        public async Task<IHttpActionResult> GetParticipantExchangeVisitorsAsync([FromUri]PagingQueryBindingModel<ParticipantExchangeVisitorDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantExchangeVisitorsAsync(queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participants and there exchange visitor info by project id.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <param name="projectId">The id of the project to get participants for.</param>
        /// <returns>The list of participantExchangeVisitors.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantExchangeVisitorDTO>))]
        [Route("Projects/{projectId:int}/ParticipantExchangeVisitors")]
        public async Task<IHttpActionResult> GetParticipantExchangeVisitorsByProjectIdAsync(int projectId, [FromUri]PagingQueryBindingModel<ParticipantExchangeVisitorDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantExchangeVisitorsByProjectIdAsync(projectId, queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Retrieves the participantExchangeVisitor with the given id.
        /// </summary>
        /// <param name="participantId">The id of the participant.</param>
        /// <returns>The participantExchangeVisitor with the given id.</returns>
        [ResponseType(typeof(ParticipantExchangeVisitorDTO))]
        [Route("ParticipantExchangeVisitors/{participantId:int}")]
        public async Task<IHttpActionResult> GetParticipantExchangeVisitorByIdAsync(int participantId) 
        {
            var participantExchangeVisitor = await service.GetParticipantExchangeVisitorByIdAsync(participantId);
            if (participantExchangeVisitor != null)
            {
                return Ok(participantExchangeVisitor);
            }
            else
            {
                var participant = await participantService.GetParticipantByIdAsync(participantId);
                if (participant != null)
                {
                    var newParticipantExchangeVisitor = await NewParticipantExchangeVisitorAsync(participantId);
                    return Ok(newParticipantExchangeVisitor);
                }
                else
                    return NotFound();
            }
        }


        /// <summary>
        /// Updates the new participantExchangeVisitor with the given participantId.
        /// </summary>
        /// <param name="model">The new participantExchangeVisitor.</param>
        /// <returns>The saved participantExchangeVisitor.</returns>
        [Route("ParticipantExchangeVisitors")]
        public async Task<IHttpActionResult> PutParticipantStudentVisitorAsync([FromBody]UpdatedParticipantExchangeVisitorBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var participantStudentVisitorDTO = await service.UpdateAsync(model.ToUpdatedParticipantExchangeVisitor(businessUser));
                await service.SaveChangesAsync();
                participantStudentVisitorDTO = await service.GetParticipantExchangeVisitorByIdAsync(participantStudentVisitorDTO.ParticipantId);
                return Ok(participantStudentVisitorDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
        private async Task<ParticipantExchangeVisitorDTO> NewParticipantExchangeVisitorAsync(int participantId)
        {
            var currentUser = userProvider.GetCurrentUser();
            var businessUser = userProvider.GetBusinessUser(currentUser);
            await service.CreateParticipantExchangeVisitor(participantId, businessUser);
            await service.SaveChangesAsync();
            return await service.GetParticipantExchangeVisitorByIdAsync(participantId);
        }
    }
}
