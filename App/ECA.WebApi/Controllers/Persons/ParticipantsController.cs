using CAM.Data;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// The Participants controller handles crud operations on ECA participants.
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class ParticipantsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of participants.
        /// </summary>
        private static readonly ExpressionSorter<SimpleParticipantDTO> DEFAULT_SORTER = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Ascending);

        private IParticipantService service;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ParticipantsController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="userProvider">The user provider.</param>
        public ParticipantsController(IParticipantService service, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The participant service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.service = service;
            this.userProvider = userProvider;
        }
        
        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participants by project id.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <param name="projectId">The id of the project to get participants for.</param>
        /// <returns>The list of participants.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleParticipantDTO>))]
        [Route("Projects/{projectId:int}/Participants")]
        public async Task<IHttpActionResult> GetParticipantsByProjectIdAsync(int projectId, [FromUri]PagingQueryBindingModel<SimpleParticipantDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantsByProjectIdAsync(projectId, queryModel.ToQueryableOperator(DEFAULT_SORTER, x => x.Name, x => x.ParticipantType, x => x.ParticipantStatus, x => x.SevisStatus));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Deletes the participant from the project.
        /// </summary>
        /// <param name="id">The id of the participant.</param>
        /// <param name="projectId">The id of the project to get participants for.</param>
        /// <returns>An Ok Result.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleParticipantDTO>))]
        [Route("Projects/{projectId:int}/Participants/{id:int}")]
        [ResourceAuthorize(Permission.EDIT_PROJECT_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        public async Task<IHttpActionResult> DeleteParticipantAsync(int projectId, int id)
        {
            var user = this.userProvider.GetCurrentUser();
            var businessUser = this.userProvider.GetBusinessUser(user);
            await this.service.DeleteAsync(new DeletedParticipant(businessUser, projectId, id));
            await this.service.SaveChangesAsync();
            return Ok();
        }
        
        /// <summary>
        /// Retrieves the participant with the given id.
        /// </summary>
        /// <param name="participantId">The id of the participant.</param>
        /// <returns>The participant with the given id.</returns>
        [ResponseType(typeof(SimpleParticipantDTO))]
        [Route("Participants/{participantId:int}")]
        public async Task<IHttpActionResult> GetParticipantByIdAsync(int participantId)
        {
            var participant = await this.service.GetParticipantByIdAsync(participantId);
            if (participant != null)
            {
                return Ok(participant);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Retrieves the participant associated with a person
        /// </summary>
        /// <param name="personId">The id of the person</param>
        /// <returns></returns>
        [ResponseType(typeof(List<SimpleParticipantDTO>))]
        [Route("Person/Participant/{personId:int}")]
        public async Task<IHttpActionResult> GetParticipantByPersonIdAsync(int personId)
        {
            var participant = await this.service.GetParticipantByPersonIdAsync(personId);
            if (participant != null)
            {
                return Ok(participant);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
