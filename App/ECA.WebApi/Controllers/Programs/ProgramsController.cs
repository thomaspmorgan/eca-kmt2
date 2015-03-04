using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Programs;
using ECA.WebApi.Models.Query;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Programs
{
    /// <summary>
    /// The ProgramsController is capable of handling program requests from a client.
    /// </summary>
    public class ProgramsController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of programs.
        /// </summary>
        private static readonly ExpressionSorter<SimpleProgramDTO> DEFAULT_PROGRAM_SORTER = new ExpressionSorter<SimpleProgramDTO>(x => x.Name, SortDirection.Ascending);

        private IProgramService programService;

        /// <summary>
        /// Creates a new ProgramController with the given program service.
        /// </summary>
        /// <param name="programService">The program service.</param>
        public ProgramsController(IProgramService programService)
        {
            Debug.Assert(programService != null, "The program service must not be null.");
            this.programService = programService;
        }

        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of programs.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <returns>The list of programs.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleProgramDTO>))]
        public async Task<IHttpActionResult> GetProgramsAsync([FromUri]PagingQueryBindingModel queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.programService.GetProgramsAsync(queryModel.ToQueryableOperator<SimpleProgramDTO>(DEFAULT_PROGRAM_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the program with the given id.
        /// </summary>
        /// <returns>The program with the given id.</returns>
        [ResponseType(typeof(ProgramDTO))]
        public async Task<IHttpActionResult> GetProgramByIdAsync(int id)
        {
            var program = await this.programService.GetProgramByIdAsync(id);
            if (program != null)
            {
                return Ok(program);
            }
            else
            {
                return NotFound();
            }            
        }

        /// <summary>
        /// Creates a new draft program and returns the saved program.
        /// </summary>
        /// <param name="model">The new draft program.</param>
        /// <returns>The saved program.</returns>
        [ResponseType(typeof(ProgramDTO))]
        public async Task<IHttpActionResult> PostProgramAsync(DraftProgramBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = 0;
                var program = programService.Create(model.ToDraftProgram(userId));
                await programService.SaveChangesAsync();
                var dto = await programService.GetProgramByIdAsync(program.ProgramId);
                return Ok(dto);

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Updates the system's program with the given program details.
        /// </summary>
        /// <param name="model">The updated program.</param>
        /// <returns>The system's updated program.</returns>
        [ResponseType(typeof(ProgramDTO))]
        public async Task<IHttpActionResult> PutProgramAsync(ProgramBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = 0;
                await programService.UpdateAsync(model.ToEcaProgram(userId));
                await programService.SaveChangesAsync();
                var dto = await programService.GetProgramByIdAsync(model.Id);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }
    }
}