using System.Linq;
using System.Data.Entity;
using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using ECA.Core.Logging;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Service to perform crud operations on projects
    /// </summary>
    public class ProjectService : DbContextService<EcaContext>, IProjectService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The db context</param>
        /// <param name="logger">The logger</param>
        public ProjectService(EcaContext context, ILogger logger) : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Create

        /// <summary>
        /// Creates and returns project
        /// </summary>
        /// <param name="project">The project to create</param>
        /// <returns>The project that was created</returns>
        public Project Create(DraftProject draftProject)
        {
            Contract.Requires(draftProject != null, "The draft project must not be null.");
            var program = GetProgramById(draftProject.ProgramId);
            return DoCreate(draftProject, program);
        }

        /// <summary>
        /// Creates and returns project asyncronously
        /// </summary>
        /// <param name="project">The project to create</param>
        /// <returns>The project that was created</returns>
        public async Task<Project> CreateAsync(DraftProject draftProject)
        {
            Contract.Requires(draftProject != null, "The draft project must not be null.");
            var program = await GetProgramByIdAsync(draftProject.ProgramId);
            return DoCreate(draftProject, program);
        }

        private Project DoCreate(DraftProject draftProject, Program program)
        {
            var project = new Project
            {
                Name = draftProject.Name,
                Description = draftProject.Description,
                ProjectStatusId = draftProject.StatusId,
                ProjectTypeId = 0, // need to figure this out
                ProgramId = draftProject.ProgramId,
                Themes = program.Themes,
                Goals = program.Goals,
                FocusArea = program.Focus.FocusName,
                Contacts = program.Contacts,
                Regions = program.Regions
            };
            draftProject.Audit.SetHistory(project);
            this.Context.Projects.Add(project);
            return project;
        }

        /// <summary>
        /// Gets program by id 
        /// </summary>
        /// <param name="programId">The program id to lookup</param>
        /// <returns>The program</returns>
        protected Program GetProgramById(int programId)
        {
            return this.Context.Programs.Find(programId);
        }

        /// <summary>
        /// Get program by id asyncronously
        /// </summary>
        /// <param name="programId">The program id to lookup</param>
        /// <returns>The program</returns>
        protected async Task<Program> GetProgramByIdAsync(int programId)
        {
            return await this.Context.Programs
                                         .Include("Themes")
                                         .Include("Goals")
                                         .Include("Contacts")
                                         .Include("Regions")
                                         .FirstOrDefaultAsync(p => p.ProgramId == programId);
        }

        #endregion

        #region Query

        /// <summary>
        /// Returns the sorted, filtered, and paged projects in the program with the given program id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted projects.</returns>
        public PagedQueryResults<SimpleProjectDTO> GetProjectsByProgramId(int programId, QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            return ProjectQueries.CreateGetProjectsByProgramQuery(this.Context, programId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the sorted, filtered, and paged projects in the program with the given program id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted projects.</returns>
        public Task<PagedQueryResults<SimpleProjectDTO>> GetProjectsByProgramIdAsync(int programId, QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            return ProjectQueries.CreateGetProjectsByProgramQuery(this.Context, programId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns a project by id asynchronously
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>Project</returns>
        public Task<ProjectDTO> GetProjectByIdAsync(int projectId)
        {
            return ProjectQueries.CreateGetProjectByIdQuery(this.Context, projectId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns a project by id
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>Project</returns>
        public ProjectDTO GetProjectById(int projectId)
        {
            return ProjectQueries.CreateGetProjectByIdQuery(this.Context, projectId).FirstOrDefault();
        }

        #endregion
    }
}
