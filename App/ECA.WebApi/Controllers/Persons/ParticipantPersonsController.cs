using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Person;
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
    /// The ParticipantPersons controller handles crud operations on ECA participants that are persons, handling project specific info for persons vice organizations
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class ParticipantPersonsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of participants.
        /// </summary>
        private static readonly ExpressionSorter<SimpleParticipantPersonDTO> DEFAULT_SORTER = new ExpressionSorter<SimpleParticipantPersonDTO>(x => x.ParticipantId, SortDirection.Ascending);

        private IParticipantPersonService service;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ParticipantPersonsController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="userProvider">The user provider.</param>
        public ParticipantPersonsController(IParticipantPersonService service, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The participant service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.userProvider = userProvider;
            this.service = service;
        }

        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participantPersons.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <returns>The list of participantPersons.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleParticipantPersonDTO>))]
        [Route("ParticipantPersons")]
        public async Task<IHttpActionResult> GetParticipantPersonsAsync([FromUri]PagingQueryBindingModel<SimpleParticipantPersonDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantPersonsAsync(queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participants by project id.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <param name="projectId">The id of the project to get participants for.</param>
        /// <returns>The list of participants.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleParticipantPersonDTO>))]
        [Route("Projects/{projectId:int}/ParticipantPersons")]
        public async Task<IHttpActionResult> GetParticipantPersonsByProjectIdAsync(int projectId, [FromUri]PagingQueryBindingModel<SimpleParticipantPersonDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantPersonsByProjectIdAsync(projectId, queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Retrieves the participantPerson with the given id.
        /// </summary>
        /// <param name="participantId">The id of the participant.</param>
        /// <returns>The participantPerson with the given id.</returns>
        [ResponseType(typeof(SimpleParticipantPersonDTO))]
        [Route("ParticipantPersons/{participantId:int}")]
        public async Task<IHttpActionResult> GetParticipantPersonByIdAsync(int participantId) 
        {
            var participantPerson = await this.service.GetParticipantPersonByIdAsync(participantId);
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
        /// Updates a project's participant person details with the given client information.
        /// </summary>
        /// <param name="model">The updated participant person.</param>
        /// <returns>An ok result.</returns>
        [ResponseType(typeof(SimpleParticipantPersonDTO))]
        [Route("ParticipantPersons/")]
        public async Task<IHttpActionResult> PutCreateOrUpdateParticipantPersonAsync([FromBody]UpdatedParticipantPersonBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(currentUser);
                await this.service.CreateOrUpdateAsync(model.ToUpdatedParticipantPerson(businessUser));
                await this.service.SaveChangesAsync();
                var participantPerson = await this.service.GetParticipantPersonByIdAsync(model.ParticipantId);
                return Ok(participantPerson);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
