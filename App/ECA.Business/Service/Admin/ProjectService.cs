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
using ECA.Business.Validation;
using System.Collections.Generic;
using System.Diagnostics;
using ECA.Core.Exceptions;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Service to perform crud operations on projects
    /// </summary>
    public class ProjectService : EcaService, IProjectService
    {
        private static readonly string COMPONENT_NAME = typeof(ProjectService).FullName;

        private readonly ILogger logger;
        private IBusinessValidator<ProjectServiceCreateValidationEntity, ProjectServiceUpdateValidationEntity> validator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The db context</param>
        /// <param name="logger">The logger</param>
        public ProjectService(EcaContext context, ILogger logger, IBusinessValidator<ProjectServiceCreateValidationEntity, ProjectServiceUpdateValidationEntity> validator)
            : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
            Contract.Requires(validator != null, "The validator must not be null.");
            this.validator = validator;
            this.logger = logger;
        }

        #region Create

        /// <summary>
        /// Creates and returns project
        /// </summary>
        /// <param name="draftProject">The project to create</param>
        /// <returns>The project that was created</returns>
        public Project Create(DraftProject draftProject)
        {
            var stopwatch = Stopwatch.StartNew();
            var program = GetProgramById(draftProject.ProgramId);
            var project = DoCreate(draftProject, program);
            stopwatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed);
            return project;
        }

        /// <summary>
        /// Creates and returns project asyncronously
        /// </summary>
        /// <param name="draftProject">The project to create</param>
        /// <returns>The project that was created</returns>
        public async Task<Project> CreateAsync(DraftProject draftProject)
        {
            var stopwatch = Stopwatch.StartNew();
            var program = await GetProgramByIdAsync(draftProject.ProgramId);
            var project = DoCreate(draftProject, program);
            stopwatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed);
            return project;
        }

        private Project DoCreate(DraftProject draftProject, Program program)
        {
            var project = new Project
            {
                Name = draftProject.Name,
                Description = draftProject.Description,
                StartDate = DateTimeOffset.Now,
                ProjectStatusId = draftProject.StatusId,
                ProjectTypeId = 0, // need to figure this out
                ProgramId = draftProject.ProgramId,
                Themes = program.Themes,
                Goals = program.Goals,
                Focus = program.Focus,
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
            return CreateGetProgramByIdQuery(programId).FirstOrDefault();
        }

        /// <summary>
        /// Get program by id asyncronously
        /// </summary>
        /// <param name="programId">The program id to lookup</param>
        /// <returns>The program</returns>
        protected async Task<Program> GetProgramByIdAsync(int programId)
        {
            return await CreateGetProgramByIdQuery(programId).FirstOrDefaultAsync();
        }

        #endregion

        #region Update

        public void Update(PublishedProject updatedProject)
        {
            var projectToUpdate = GetProjectEntityById(updatedProject.ProjectId);
            if(projectToUpdate == null)
            {
                throw new ModelNotFoundException(String.Format("The project with id [{0}] was not found.", updatedProject.ProjectId));
            }
            var stopwatch = Stopwatch.StartNew();
            var themesExist = CheckAllThemesExist(updatedProject.ThemeIds);
            var goalsExist = CheckAllGoalsExist(updatedProject.GoalIds);
            var contactsExist = CheckAllContactsExist(updatedProject.PointsOfContactIds);
            var focus = GetFocusById(updatedProject.FocusId);            
            validator.ValidateUpdate(GetUpdateValidationEntity(
                publishedProject: updatedProject,
                projectToUpdate: projectToUpdate,
                focus: focus,
                goalsExist: goalsExist,
                themesExist: themesExist,
                pointsOfContactExist: contactsExist));
            DoUpdate(updatedProject, projectToUpdate, focus);
            stopwatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed);
        }

        public async Task UpdateAsync(PublishedProject updatedProject)
        {
            var projectToUpdate = await GetProjectEntityByIdAsync(updatedProject.ProjectId);
            if (projectToUpdate == null)
            {
                throw new ModelNotFoundException(String.Format("The project with id [{0}] was not found.", updatedProject.ProjectId));
            }
            var stopwatch = Stopwatch.StartNew();
            var themesExist = await CheckAllThemesExistAsync(updatedProject.ThemeIds);
            var goalsExist = await CheckAllGoalsExistAsync(updatedProject.GoalIds);
            var contactsExist = await CheckAllContactsExistAsync(updatedProject.PointsOfContactIds);
            var focus = await GetFocusByIdAsync(updatedProject.FocusId);
            validator.ValidateUpdate(GetUpdateValidationEntity(
                publishedProject: updatedProject,
                projectToUpdate: projectToUpdate,
                focus: focus,
                goalsExist: goalsExist,
                themesExist: themesExist,
                pointsOfContactExist: contactsExist));

            DoUpdate(updatedProject, projectToUpdate, focus);
            stopwatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed);
        }

        private void DoUpdate(PublishedProject updatedProject, Project projectToUpdate, Focus focus)
        {
            Contract.Requires(updatedProject != null, "The updated project must not be null.");
            Contract.Requires(projectToUpdate != null, "The project to update must not be null.");
            SetPointOfContacts(updatedProject.PointsOfContactIds.ToList(), projectToUpdate);
            SetThemes(updatedProject.ThemeIds.ToList(), projectToUpdate);
            SetGoals(updatedProject.GoalIds.ToList(), projectToUpdate);
            projectToUpdate.Name = updatedProject.Name;
            projectToUpdate.Description = updatedProject.Description;
            projectToUpdate.EndDate = updatedProject.EndDate;
            projectToUpdate.Focus = focus;
            projectToUpdate.Name = updatedProject.Name;
            projectToUpdate.ProjectStatusId = updatedProject.ProjectStatusId;
            projectToUpdate.StartDate = updatedProject.StartDate;

            Contract.Assert(updatedProject.Audit != null, "The audit must not be null.");
            updatedProject.Audit.SetHistory(projectToUpdate);
        }

        private ProjectServiceUpdateValidationEntity GetUpdateValidationEntity(
            PublishedProject publishedProject,
            Project projectToUpdate,
            Focus focus,
            bool goalsExist,
            bool themesExist,
            bool pointsOfContactExist)
        {
            return new ProjectServiceUpdateValidationEntity(
                updatedProject: publishedProject,
                projectToUpdate: projectToUpdate,
                focus: focus,
                goalsExist: goalsExist,
                themesExist: themesExist,
                pointsOfContactExist: pointsOfContactExist
                );
        }
        #endregion

        private Project GetProjectEntityById(int projectId)
        {
            return Context.Projects.Find(projectId);
        }

        private Task<Project> GetProjectEntityByIdAsync(int projectId)
        {
            return Context.Projects.FindAsync(projectId);
        }

        private IQueryable<Program> CreateGetProgramByIdQuery(int programId)
        {
            return this.Context.Programs
                .Include("Themes")
                .Include("Goals")
                .Include("Contacts")
                .Include("Regions")
                .Where(x => x.ProgramId == programId);
        }

        #region Get

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
