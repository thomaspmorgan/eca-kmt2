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
using ECA.Business.Validation;
using System.Collections.Generic;
using System.Diagnostics;
using ECA.Core.Exceptions;
using NLog;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Service to perform crud operations on projects
    /// </summary>
    public class ProjectService : EcaService, IProjectService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IBusinessValidator<ProjectServiceCreateValidationEntity, ProjectServiceUpdateValidationEntity> validator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The db context</param>
        public ProjectService(EcaContext context, IBusinessValidator<ProjectServiceCreateValidationEntity, ProjectServiceUpdateValidationEntity> validator)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(validator != null, "The validator must not be null.");
            this.validator = validator;
        }

        #region Create

        /// <summary>
        /// Creates and returns project
        /// </summary>
        /// <param name="draftProject">The project to create</param>
        /// <returns>The project that was created</returns>
        public Project Create(DraftProject draftProject)
        {
            
            var program = GetProgramById(draftProject.ProgramId);
            this.logger.Trace("Retrieved program by id {0}.", draftProject.ProgramId);
            validator.ValidateCreate(GetCreateValidationEntity(draftProject, program));
            var project = DoCreate(draftProject, program);
            this.logger.Trace("Created project {0}.", project);
            return project;
        }

        /// <summary>
        /// Creates and returns project asyncronously
        /// </summary>
        /// <param name="draftProject">The project to create</param>
        /// <returns>The project that was created</returns>
        public async Task<Project> CreateAsync(DraftProject draftProject)
        {
            var program = await GetProgramByIdAsync(draftProject.ProgramId);
            this.logger.Trace("Retrieved program by id {0}.", draftProject.ProgramId);
            validator.ValidateCreate(GetCreateValidationEntity(draftProject, program));
            var project = DoCreate(draftProject, program);
            this.logger.Trace("Created project {0}.", project);
            return project;
        }

        private ProjectServiceCreateValidationEntity GetCreateValidationEntity(DraftProject draftProject, Program program)
        {
            return new ProjectServiceCreateValidationEntity(
                name: draftProject.Name,
                description: draftProject.Description,
                program: program
                );
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
                Regions = program.Regions,
                Categories = program.Categories,
                Objectives = program.Objectives
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

        /// <summary>
        /// Updates the system's project with the given updated project.
        /// </summary>
        /// <param name="updatedProject">The updated project.</param>
        public void Update(PublishedProject updatedProject)
        {
            var projectToUpdate = GetProjectEntityById(updatedProject.ProjectId);
            if(projectToUpdate == null)
            {
                throw new ModelNotFoundException(String.Format("The project with id [{0}] was not found.", updatedProject.ProjectId));
            }
            this.logger.Trace("Retrieved project by id {0}.", updatedProject.ProjectId);

            var themesExist = CheckAllThemesExist(updatedProject.ThemeIds);
            this.logger.Trace("Check all themes with ids {0} existed.", String.Join(", ", updatedProject.ThemeIds));

            var goalsExist = CheckAllGoalsExist(updatedProject.GoalIds);
            this.logger.Trace("Check all goals with ids {0} existed.", String.Join(", ", updatedProject.GoalIds));

            var contactsExist = CheckAllContactsExist(updatedProject.PointsOfContactIds);
            this.logger.Trace("Check all contacts with ids {0} existed.", String.Join(", ", updatedProject.PointsOfContactIds));
            
            var categoriesExist = CheckAllCategoriesExist(updatedProject.CategoryIds);
            this.logger.Trace("Check all goals with ids {0} existed.", String.Join(", ", updatedProject.GoalIds));

            var objectivesExist = CheckAllObjectivesExist(updatedProject.ObjectiveIds);
            this.logger.Trace("Check all contacts with ids {0} existed.", String.Join(", ", updatedProject.PointsOfContactIds));

            Contract.Assert(updatedProject.CategoryIds != null, "The category ids must not be null.");
            Contract.Assert(updatedProject.ObjectiveIds != null, "The objective ids must not be null.");
            validator.ValidateUpdate(GetUpdateValidationEntity(
                publishedProject: updatedProject,
                projectToUpdate: projectToUpdate,
                goalsExist: goalsExist,
                themesExist: themesExist,
                pointsOfContactExist: contactsExist,
                categoriesExist: categoriesExist,
                objectivesExist: objectivesExist,
                numberOfCategories: updatedProject.CategoryIds.Count(),
                numberOfObjectives: updatedProject.ObjectiveIds.Count()));
            DoUpdate(updatedProject, projectToUpdate);
            
        }

        /// <summary>
        /// Updates the system's project with the given updated project.
        /// </summary>
        /// <param name="updatedProject">The updated project.</param>
        public async Task UpdateAsync(PublishedProject updatedProject)
        {
            var projectToUpdate = await GetProjectEntityByIdAsync(updatedProject.ProjectId);
            if (projectToUpdate == null)
            {
                throw new ModelNotFoundException(String.Format("The project with id [{0}] was not found.", updatedProject.ProjectId));
            }
            this.logger.Trace("Retrieved project by id {0}.", updatedProject.ProjectId);

            var themesExist = await CheckAllThemesExistAsync(updatedProject.ThemeIds);
            this.logger.Trace("Check all themes with ids {0} existed.", String.Join(", ", updatedProject.ThemeIds));

            var goalsExist = await CheckAllGoalsExistAsync(updatedProject.GoalIds);
            this.logger.Trace("Check all goals with ids {0} existed.", String.Join(", ", updatedProject.GoalIds));

            var contactsExist = await CheckAllContactsExistAsync(updatedProject.PointsOfContactIds);
            this.logger.Trace("Check all contacts with ids {0} existed.", String.Join(", ", updatedProject.PointsOfContactIds));
            
            var categoriesExist = CheckAllCategoriesExist(updatedProject.CategoryIds);
            this.logger.Trace("Check all goals with ids {0} existed.", String.Join(", ", updatedProject.GoalIds));

            var objectivesExist = CheckAllObjectivesExist(updatedProject.ObjectiveIds);
            this.logger.Trace("Check all contacts with ids {0} existed.", String.Join(", ", updatedProject.PointsOfContactIds));

            Contract.Assert(updatedProject.CategoryIds != null, "The category ids must not be null.");
            Contract.Assert(updatedProject.ObjectiveIds != null, "The objective ids must not be null.");
            validator.ValidateUpdate(GetUpdateValidationEntity(
                publishedProject: updatedProject,
                projectToUpdate: projectToUpdate,
                goalsExist: goalsExist,
                themesExist: themesExist,
                pointsOfContactExist: contactsExist,
                categoriesExist: categoriesExist,
                objectivesExist: objectivesExist,
                numberOfCategories: updatedProject.CategoryIds.Count(),
                numberOfObjectives: updatedProject.ObjectiveIds.Count()));

            DoUpdate(updatedProject, projectToUpdate);
        }

        private void DoUpdate(PublishedProject updatedProject, Project projectToUpdate)
        {
            Contract.Requires(updatedProject != null, "The updated project must not be null.");
            Contract.Requires(projectToUpdate != null, "The project to update must not be null.");
            SetPointOfContacts(updatedProject.PointsOfContactIds.ToList(), projectToUpdate);
            SetThemes(updatedProject.ThemeIds.ToList(), projectToUpdate);
            SetGoals(updatedProject.GoalIds.ToList(), projectToUpdate);
            SetCategories(updatedProject.CategoryIds.ToList(), projectToUpdate);
            SetObjectives(updatedProject.ObjectiveIds.ToList(), projectToUpdate);
            projectToUpdate.Name = updatedProject.Name;
            projectToUpdate.Description = updatedProject.Description;
            projectToUpdate.EndDate = updatedProject.EndDate;
            projectToUpdate.Name = updatedProject.Name;
            projectToUpdate.ProjectStatusId = updatedProject.ProjectStatusId;
            projectToUpdate.StartDate = updatedProject.StartDate;

            Contract.Assert(updatedProject.Audit != null, "The audit must not be null.");
            updatedProject.Audit.SetHistory(projectToUpdate);
        }

        private ProjectServiceUpdateValidationEntity GetUpdateValidationEntity(
            PublishedProject publishedProject,
            Project projectToUpdate,
            bool goalsExist,
            bool themesExist,
            bool pointsOfContactExist,
            bool categoriesExist,
            bool objectivesExist,
            int numberOfCategories,
            int numberOfObjectives)
        {
            return new ProjectServiceUpdateValidationEntity(
                updatedProject: publishedProject,
                projectToUpdate: projectToUpdate,
                goalsExist: goalsExist,
                themesExist: themesExist,
                pointsOfContactExist: pointsOfContactExist,
                categoriesExist: categoriesExist,
                objectivesExist: objectivesExist,
                numberOfCategories: numberOfCategories,
                numberOfObjectives: numberOfObjectives
                );
        }
        #endregion

        private Project GetProjectEntityById(int projectId)
        {
            return CreateGetProjectByIdQuery(projectId).FirstOrDefault();
        }

        private Task<Project> GetProjectEntityByIdAsync(int projectId)
        {
            return CreateGetProjectByIdQuery(projectId).FirstOrDefaultAsync();
        }

        private IQueryable<Project> CreateGetProjectByIdQuery(int projectId)
        {
            return Context.Projects       
                .Include(x => x.Themes)
                .Include(x => x.Goals)
                .Include(x => x.Contacts)
                .Include(x => x.Regions)
                .Include(x => x.Categories)
                .Include(x => x.Objectives)
                .Where(x => x.ProjectId == projectId);
        }

        private IQueryable<Program> CreateGetProgramByIdQuery(int programId)
        {
            return this.Context.Programs
                .Include(x => x.Themes)
                .Include(x => x.Goals)
                .Include(x => x.Contacts)
                .Include(x => x.Regions)
                .Include(x => x.Categories)
                .Include(x => x.Objectives)
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
            var projects = ProjectQueries.CreateGetProjectsByProgramQuery(this.Context, programId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved projects by program id {0} and query operator {1}.", programId, queryOperator);
            return projects;
        }

        /// <summary>
        /// Returns the sorted, filtered, and paged projects in the program with the given program id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted projects.</returns>
        public Task<PagedQueryResults<SimpleProjectDTO>> GetProjectsByProgramIdAsync(int programId, QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            var projects = ProjectQueries.CreateGetProjectsByProgramQuery(this.Context, programId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved projects by program id {0} and query operator {1}.", programId, queryOperator);
            return projects;
        }

        /// <summary>
        /// Returns a project by id asynchronously
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>Project</returns>
        public Task<ProjectDTO> GetProjectByIdAsync(int projectId)
        {
            var project = ProjectQueries.CreateGetProjectByIdQuery(this.Context, projectId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved project by id {0}.", projectId);
            return project;
        }

        /// <summary>
        /// Returns a project by id
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>Project</returns>
        public ProjectDTO GetProjectById(int projectId)
        {
            var project = ProjectQueries.CreateGetProjectByIdQuery(this.Context, projectId).FirstOrDefault();
            this.logger.Trace("Retrieved project by id {0}.", projectId);
            return project;
        }
        #endregion
    }
}
