using CAM.Data;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Business.Validation.Sevis;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Security;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// The ParticipantPersonsSevisController handles crud operations on ECA participants that are persons and their SEVIS info
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class ParticipantPersonsSevisController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of participants.
        /// </summary>
        private static readonly ExpressionSorter<ParticipantPersonSevisDTO> DEFAULT_SORTER = new ExpressionSorter<ParticipantPersonSevisDTO>(x => x.ParticipantId, SortDirection.Ascending);

        private IParticipantPersonsSevisService participantService;
        private IExchangeVisitorService visitorService;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ParticipantPersonsSevisController with the given service.
        /// </summary>
        /// <param name="participantService">participant person sevis service.</param>
        /// <param name="userProvider">user provider</param>
        /// <param name="visitorService">The exchange visitor service.</param>
        public ParticipantPersonsSevisController(IParticipantPersonsSevisService participantService, IExchangeVisitorService visitorService, IUserProvider userProvider)
        {
            Contract.Requires(participantService != null, "The participantPersonSevis service must not be null.");
            Contract.Requires(visitorService != null, "The visitor service must not be null.");
            this.visitorService = visitorService;
            this.participantService = participantService;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Retrieves the participantPersonSevis with the given id.
        /// </summary>
        /// <param name="participantId">The id of the participant.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersonSevis with the given id.</returns>
        [ResponseType(typeof(ParticipantPersonSevisDTO))]
        [Route("Project/{projectId:int}/ParticipantPersonsSevis/{participantId:int}")]
        public async Task<IHttpActionResult> GetParticipantPersonsSevisByIdAsync(int projectId, int participantId)
        {
            var participantPerson = await participantService.GetParticipantPersonsSevisByIdAsync(projectId, participantId);
            if (participantPerson != null)
            {
                return Ok(participantPerson);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Updates the new participantPersonSevis with the given participantId.
        /// </summary>
        /// <param name="model">The new email address.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The saved email address.</returns>
        [HttpPut]
        [Route("Project/{projectId:int}/ParticipantPersonsSevis")]
        [ResourceAuthorize(Permission.EDIT_SEVIS_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        [ResponseType(typeof(ParticipantPersonSevisDTO))]
        public async Task<IHttpActionResult> PutParticipantPersonsSevisAsync(int projectId, [FromBody]UpdatedParticipantPersonSevisBindingModel model)
        {   
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await participantService.UpdateAsync(model.ToUpdatedParticipantPersonSevis(businessUser));
                await participantService.SaveChangesAsync();
                var dto = await participantService.GetParticipantPersonsSevisByIdAsync(projectId, model.ParticipantId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Send to sevis async
        /// </summary>
        /// <param name="participantIds">The participant ids to send to sevis</param>
        /// <param name="projectId">The project id of the participants.</param>
        /// <returns>Success or error</returns>
        [Route("Project/{projectId:int}/ParticipantPersonsSevis/SendToSevis")]
        [ResourceAuthorize(Permission.SEND_TO_SEVIS_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        public async Task<IHttpActionResult> PostSendToSevisAsync(int projectId, int[] participantIds)
        {
            if (ModelState.IsValid)
            {
                var statuses = await participantService.SendToSevisAsync(projectId, participantIds);
                await participantService.SaveChangesAsync();
                return Ok(statuses);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Manually run the participant service verification.
        /// </summary>
        /// <param name="projectId">The project id the participant is on.</param>
        /// <param name="participantId">The participant to verify by id.</param>
        /// <returns>An ok result.</returns>
        [Route("Project/{projectId:int}/Participant/{participantId:int}/Profile")]
        //[ResourceAuthorize(Permission.EDIT_SEVIS_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        //[ResponseType(typeof(ExchangeVisitor))]
        public async Task<IHttpActionResult> GetExchangeVisitorProfileAsync(int projectId, int participantId)
        {
            var user = this.userProvider.GetCurrentUser();
            var businessUser = this.userProvider.GetBusinessUser(user);
            var model = await visitorService.GetExchangeVisitorAsync(businessUser, projectId, participantId);            
            return Ok(model);
        } 
    }
}
