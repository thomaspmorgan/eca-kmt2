using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using NLog;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// The EvaluationNoteController provides clients with the Evaluation Notes for a person in the eca system.
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class EvaluationNoteController : ApiController
    {
        private static readonly ExpressionSorter<EvaluationNoteDTO> DEFAULT_SORTER = new ExpressionSorter<EvaluationNoteDTO>(x => x.AddedOn, SortDirection.Descending);

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IEvaluationNoteService service;
        private readonly IUserProvider userProvider;

        /// <summary>
        /// Creates a new EvaluationNoteController with the given service.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userProvider"></param>
        public EvaluationNoteController(IEvaluationNoteService service, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The service must not be null.");
            Contract.Requires(userProvider != null, "The userProvider must not be null.");
            this.service = service;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Returns the EvaluationNotes in the system
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [ResponseType(typeof(PagedQueryResults<EvaluationNoteDTO>))]
        public async Task<IHttpActionResult> GetAsync([FromUri]PagingQueryBindingModel<EvaluationNoteDTO> queryModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var dtos = await service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_SORTER));
            return Ok(dtos);
        }

        /// <summary>
        /// Add a new evaluation note in the eca system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ResponseType(typeof(EvaluationNoteDTO))]
        [Route("People/{personId:int}/EvaluationNote")]
        public async Task<IHttpActionResult> PostEvaluationNoteAsync(NewPersonEvaluationNoteBindingModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = userProvider.GetCurrentUser();
            var businessUser = userProvider.GetBusinessUser(currentUser);
            var evalnote = await service.CreateAsync(model.ToPersonEvaluationNote(businessUser));
            await service.SaveChangesAsync();
            var dto = await service.GetByIdAsync(evalnote.EvaluationNoteId);
            return Ok(dto);
        }

        /// <summary>
        /// Updates an evaluation note to the person
        /// </summary>
        /// <param name="model">The updated evaluation note</param>
        /// <returns>void</returns>
        [ResponseType(typeof(EvaluationNoteDTO))]
        [Route("People/{personId:int}/EvaluationNote")]
        public async Task<IHttpActionResult> PutEvalutionNoteAsync(UpdatedPersonEvaluationNoteBindingModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = userProvider.GetCurrentUser();
            var businessUser = userProvider.GetBusinessUser(currentUser);
            await service.UpdateAsync(model.ToUpdatedPersonEvaluationNote(businessUser));
            await service.SaveChangesAsync();
            var dto = await service.GetByIdAsync(model.EvaluationNoteId);
            return Ok(dto);
        }

        /// <summary>
        /// Deletes the evaluation note with the given id.
        /// </summary>
        /// <param name="id">The id of the evaluation note.</param>
        /// <returns>An ok response.</returns>
        [ResponseType(typeof(OkResult))]
        [Route("People/{personId:int}/EvaluationNote/{id:int}")]
        public async Task<IHttpActionResult> DeleteEvaluationNote(int id)
        {
            await service.DeleteAsync(id);
            await service.SaveChangesAsync();
            return Ok();
        }

    }
}