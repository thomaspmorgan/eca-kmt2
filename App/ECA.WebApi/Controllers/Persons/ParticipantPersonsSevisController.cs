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

        private IParticipantPersonsSevisService service;
        private SevisValidationService validation;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ParticipantPersonsSevisController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="userProvider">user provider</param>
        public ParticipantPersonsSevisController(IParticipantPersonsSevisService service, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The participantPersonSevis service must not be null.");
            this.service = service;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participantPersonSevises.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <returns>The list of participantPersonSevises.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantPersonSevisDTO>))]
        [Route("ParticipantPersonsSevis")]
        public async Task<IHttpActionResult> GetParticipantPersonsSevisAsync([FromUri]PagingQueryBindingModel<ParticipantPersonSevisDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantPersonsSevisAsync(queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participants and there SEVIS info by project id.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <param name="projectId">The id of the project to get participants for.</param>
        /// <returns>The list of participantPersonSevises.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantPersonSevisDTO>))]
        [Route("Projects/{projectId:int}/ParticipantPersonsSevis")]
        public async Task<IHttpActionResult> GetParticipantPersonsSevisByProjectIdAsync(int projectId, [FromUri]PagingQueryBindingModel<ParticipantPersonSevisDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantPersonsSevisByProjectIdAsync(projectId, queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Retrieves the participantPersonSevis with the given id.
        /// </summary>
        /// <param name="participantId">The id of the participant.</param>
        /// <returns>The participantPersonSevis with the given id.</returns>
        [ResponseType(typeof(ParticipantPersonSevisDTO))]
        [Route("ParticipantPersonsSevis/{participantId:int}")]
        public async Task<IHttpActionResult> GetParticipantPersonsSevisByIdAsync(int participantId) 
        {
            var participantPerson = await service.GetParticipantPersonsSevisByIdAsync(participantId);
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
        /// <returns>The saved email address.</returns>
        [Route("ParticipantPersonsSevis")]
        public async Task<IHttpActionResult> PutParticipantPersonsSevisAsync([FromBody]UpdatedParticipantPersonSevisBindingModel model)
        {   
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var participantPersonSevisDTO = await service.UpdateAsync(model.ToUpdatedParticipantPersonSevis(businessUser));
                await service.SaveChangesAsync();
                participantPersonSevisDTO = await service.GetParticipantPersonsSevisByIdAsync(participantPersonSevisDTO.ParticipantId);
                return Ok(participantPersonSevisDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participant's SEVIS comm statuses.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <param name="participantId">The id of the project to get participants for.</param>
        /// <returns>The list of participantPerson Sevis Comm Statuses.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantPersonSevisCommStatusDTO>))]
        [Route("ParticipantPersonsSevis/{participantId:int}/SevisCommStatuses")]
        public async Task<IHttpActionResult> GetParticipantPersonsSevisCommStatusesByProjectIdAsync(int participantId, [FromUri]PagingQueryBindingModel<ParticipantPersonSevisCommStatusDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantPersonsSevisCommStatusesByIdAsync(participantId, queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
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
        /// <returns>Success or error</returns>
        [Route("ParticipantPersonsSevis/SendToSevis")]
        public async Task<IHttpActionResult> PostSendToSevisAsync(int[] participantIds)
        {
            if (ModelState.IsValid)
            {
                var statuses = await service.SendToSevis(participantIds);
                await service.SaveChangesAsync();
                return Ok(statuses);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns>validation result</returns>
        //[Route("ParticipantPersonsSevis/ValidateSevis")]
        //public async Task<IHttpActionResult> ValidateSevisAsync(UpdatedParticipantPersonSevisValidationEntity validationEntity)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var statuses = await validation.TestSevisValidation(validationEntity);
        //        return Ok(statuses);
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}


    }
}
