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
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ParticipantPersonsSevisController with the given service.
        /// </summary>
        /// <param name="participantService">The participant person sevis service.</param>
        /// <param name="userProvider">The user provider</param>
        public ParticipantPersonsSevisController(IParticipantPersonsSevisService participantService, IUserProvider userProvider)
        {
            Contract.Requires(participantService != null, "The participantPersonSevis service must not be null.");
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
        /// <param name="model">The participants to send to sevis.</param>
        /// <param name="projectId">The project id of the participants.</param>
        /// <param name="applicationId">The application id for which the participants belong.</param>
        /// <returns>Success or error</returns>
        [Route("Application/{applicationId:int}/Project/{projectId:int}/ParticipantPersonsSevis/SendToSevis")]
        [ResourceAuthorize(Permission.SEND_TO_SEVIS_VALUE, ResourceType.APPLICATION_VALUE, "applicationId")]
        public async Task<IHttpActionResult> PostSendToSevisAsync(int applicationId, int projectId, SendToSevisBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();                
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var hasSevisCredentials = await userProvider.HasSevisUserAccountAsync(currentUser, model.SevisUsername, model.SevisOrgId);
                if (!hasSevisCredentials)
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
                }
                var businessModel = model.ToParticipantsToBeSentToSevis(businessUser, projectId);
                var statuses = await participantService.SendToSevisAsync(businessModel);
                await participantService.SaveChangesAsync();
                return Ok(statuses);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
