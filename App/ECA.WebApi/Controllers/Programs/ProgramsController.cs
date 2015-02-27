using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
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
        [ResponseType(typeof(EcaProgram))]
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

        //private EcaContext db = new EcaContext();

        //// GET: api/Programs
        //public IEnumerable<ProgramDTO> GetPrograms(int limit = 200, int offset = 0)
        //{
        //    db.Configuration.LazyLoadingEnabled = false;
        //    var programDTOs = (from p in db.Programs
        //                       orderby p.Name
        //                       select new ProgramDTO
        //                           {
        //                               ProgramId = p.ProgramId,
        //                               Name = p.Name,
        //                               Description = p.Description,
        //                               Owner = new OrganizationDTO
        //                               {
        //                                   OrganizationId = p.Owner.OrganizationId
        //                               }
        //                           }); //.Skip(offset).Take(limit);

        //    //var programDTOs = Mapper.Map<IEnumerable<Program>, IEnumerable<ProgramDTO>>(programs);
        //    return programDTOs;
        //}

        //// GET: api/Programs/5
        //[ResponseType(typeof(ProgramDTO))]
        //public async Task<IHttpActionResult> GetProgram(int id)
        //{
        //    Program program = await db.Programs
        //        .Include(p => p.Themes)
        //        .Include(p => p.Owner)
        //        .Include(p =>p.Regions)
        //        .Include(p => p.Goals)
        //        .Include(p => p.Contacts)
        //            .SingleOrDefaultAsync(i => i.ProgramId == id);
        //    if (program == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(GetProgramDTO(program));
        //}

        //private ProgramDTO GetProgramDTO(Program program)
        //{
        //    var programDTO = Mapper.Map<Program, ProgramDTO>(program);
        //    return programDTO;
        //}

        //// PUT: api/Programs/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutProgram(int id, Program program)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != program.ProgramId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(program).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProgramExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Programs
        //[ResponseType(typeof(Program))]
        //public async Task<IHttpActionResult> PostProgram(Program program)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Programs.Add(program);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = program.ProgramId }, program);
        //}

        //// DELETE: api/Programs/5
        //[ResponseType(typeof(Program))]
        //public async Task<IHttpActionResult> DeleteProgram(int id)
        //{
        //    Program program = await db.Programs.FindAsync(id);
        //    if (program == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Programs.Remove(program);
        //    await db.SaveChangesAsync();

        //    return Ok(program);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool ProgramExists(int id)
        //{
        //    return db.Programs.Count(e => e.ProgramId == id) > 0;
        //}
    }
}