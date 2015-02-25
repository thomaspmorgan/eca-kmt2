using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The ProjectsController is used for managing projects in the ECA system.
    /// </summary>
    [RoutePrefix("api")]
    public class ProjectsController : ApiController
    {
        private static ExpressionSorter<SimpleProjectDTO> DEFAULT_PROGRAM_PROJECT_SORTER = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectName, SortDirection.Ascending);

        private IProjectService projectService;

        /// <summary>
        /// Creates a new ProjectsController with the given project service.
        /// </summary>
        /// <param name="projectService">The project service.</param>
        public ProjectsController(IProjectService projectService)
        {
            Debug.Assert(projectService != null, "The project service must not be null.");
            this.projectService = projectService;
        }

        /// <summary>
        /// Returns a listing of the projects by program.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryModel">The page, filter and sort information.</param>
        /// <returns>The list of projects by program.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleProjectDTO>))]
        [Route("Programs/{programId:int}/Projects")]
        public async Task<IHttpActionResult> GetProjectsByProgramAsync(int programId, PagingQueryBindingModel queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.projectService.GetProjectsByProgramIdAsync(programId, queryModel.ToQueryableOperator<SimpleProjectDTO>(DEFAULT_PROGRAM_PROJECT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }


    /*public class ProjectsController : ApiController
    {
        private EcaContext db = new EcaContext();

        // GET: api/Projects
        public IEnumerable<ProjectDTO> GetProjects()
        {
            var projects = db.Projects;
            var projectDTOs = Mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDTO>>(projects);
            return projectDTOs;
        }
        // GET: api/Programs/3/Projects
        [Route("api/Programs/{id:int}/Projects")]
        public IEnumerable<ProjectDTO> GetProjectsByProgram(int id)
        {
            var projects = db.Projects.Include("ParentProgram").Include("Regions").Include("Status").Where(s => s.ParentProgram.ProgramId == id);
            var projectDTOs = Mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDTO>>(projects);
            return projectDTOs;
        }
        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> GetProject(int id)
        {
            Project project = await db.Projects.Include(p => p.ParentProgram).Include(p => p.Regions).Include(p => p.Goals).Include(p => p.Themes).Include(p => p.ParentProgram.Owner).SingleOrDefaultAsync(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            var projectDTO = Mapper.Map<Project, ProjectDTO>(project);
            return Ok(projectDTO);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Projects
        [ResponseType(typeof(ProjectDTO))]
        public async Task<IHttpActionResult> PostProject(ProjectDTO projectDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var now = DateTimeOffset.Now;

            var historyDTO = new HistoryDTO();
            historyDTO.CreatedBy = 0;
            historyDTO.CreatedOn = now;
            historyDTO.RevisedBy = 0;
            historyDTO.RevisedOn = now;
            projectDTO.History = historyDTO;

            var project = Mapper.Map<ProjectDTO, Project>(projectDTO);
            project.ProjectStatusId = (await db.ProjectStatuses.FirstOrDefaultAsync(p => p.Status == "Draft")).ProjectStatusId;
            db.Projects.Add(project);
            await db.SaveChangesAsync();
            // doesn't work, why? 
            // projectDTO = Mapper.Map<Project, ProjectDTO>(project);
            projectDTO.ProjectId = project.ProjectId;

            return CreatedAtRoute("DefaultApi", new { id = projectDTO.ProjectId }, projectDTO);
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> DeleteProject(int id)
        {
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            await db.SaveChangesAsync();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ProjectId == id) > 0;
        }
    }*/
}