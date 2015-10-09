using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
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
        private IParticipantPersonSevisService sevisService;

        /// <summary>
        /// Creates a new ParticipantsController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="sevisService">the service to retrieve sevis Info</param>
        public ParticipantsController(IParticipantService service, IParticipantPersonSevisService sevisService)
        {
            Contract.Requires(service != null, "The participant service must not be null.");
            Contract.Requires(service != null, "The participantPersonSevis service must not be null.");
            this.service = service;
            this.sevisService = sevisService;
        }

        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participants.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <returns>The list of participants.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleParticipantDTO>))]
        [Route("Participants")]
        public async Task<IHttpActionResult> GetParticipantsAsync([FromUri]PagingQueryBindingModel<SimpleParticipantDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantsAsync(
                    queryModel.ToQueryableOperator(DEFAULT_SORTER,
                    x => x.City,
                    x => x.Country,
                    x => x.Name));
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
        [ResponseType(typeof(PagedQueryResults<SimpleParticipantDTO>))]
        [Route("Projects/{projectId:int}/Participants")]
        public async Task<IHttpActionResult> GetParticipantsByProjectIdAsync(int projectId, [FromUri]PagingQueryBindingModel<SimpleParticipantDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantsByProjectIdAsync(projectId, queryModel.ToQueryableOperator(DEFAULT_SORTER));
                var commStatuses = sevisService.GetParticipantPersonSevisCommStatusesByParticipantIds(results.Results.Select(x => x.ParticipantId).ToArray());
                foreach (var result in results.Results)
                {
                    var commStatus = commStatuses.Where(p => p.ParticipantId == result.ParticipantId).FirstOrDefault();
                    result.SevisStatus = commStatus == null ? "None" : commStatus.SevisCommStatusName;
                } 
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
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
    }
}
