using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    /// <summary>
    /// Service to perform crud operations on projects
    /// </summary>
    public class ProjectService : EcaService, IProjectService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IBusinessValidator<ProjectServiceCreateValidationEntity, ProjectServiceUpdateValidationEntity> validator;
        private readonly IOfficeService officeService;

        private Action<int, Project> throwIfProjectDoesNotExist;
        private Action<ParticipantType> throwIfParticipantTypeDoesNotExist;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The db context</param>
        /// <param name="officeService">The office service.</param>
        /// <param name="validator">The project business validator.</param>
        public ProjectService(EcaContext context, IOfficeService officeService, IBusinessValidator<ProjectServiceCreateValidationEntity, ProjectServiceUpdateValidationEntity> validator)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(validator != null, "The validator must not be null.");
            this.validator = validator;
            this.officeService = officeService;
            throwIfProjectDoesNotExist = (projectId, project) =>
            {
                if (project == null)
                {
                    throw new ModelNotFoundException(String.Format("The project with id [{0}] does not exist.", projectId));
                }
            };
            throwIfParticipantTypeDoesNotExist = (participantType) =>
            {
                if (participantType == null)
                {
                    throw new ModelNotFoundException("The participant type does not exist.");
                }
            };
        }

        #region Participants

        /// <summary>
        /// Adds the participant to the project.
        /// </summary>
        /// <param name="additionalParticipant">The additional participant.</param>
        public void AddParticipant(AdditionalProjectParticipant additionalParticipant)
        {
            HandleAdditionalProjectParticipant(additionalParticipant);
        }

        /// <summary>
        /// Adds the participant to the project.
        /// </summary>
        /// <param name="additionalParticipant">The additional participant.</param>
        public Task AddParticipantAsync(AdditionalProjectParticipant additionalParticipant)
        {
            return HandleAdditionalProjectParticipantAsync(additionalParticipant);
        }

        private void HandleAdditionalProjectParticipant(AdditionalProjectParticipant additionalParticipant)
        {
            if (additionalParticipant is AdditionalPersonProjectParticipant)
            {
                HandleAdditionalPersonParticipant(additionalParticipant as AdditionalPersonProjectParticipant);
            }
            else if (additionalParticipant is AdditionalOrganizationProjectParticipant)
            {
                HandleAdditionalOrganizationParticipant(additionalParticipant as AdditionalOrganizationProjectParticipant);
            }
            else
            {
                throw new NotSupportedException("The additional participant is not supported.");
            }
        }

        private Task HandleAdditionalProjectParticipantAsync(AdditionalProjectParticipant additionalParticipant)
        {
            if (additionalParticipant is AdditionalPersonProjectParticipant)
            {
                return HandleAdditionalPersonParticipantAsync(additionalParticipant as AdditionalPersonProjectParticipant);
            }
            else if (additionalParticipant is AdditionalOrganizationProjectParticipant)
            {
                return HandleAdditionalOrganizationParticipantAsync(additionalParticipant as AdditionalOrganizationProjectParticipant);
            }
            else
            {
                throw new NotSupportedException("The additional participant is not supported.");
            }
        }

        private void HandleAdditionalOrganizationParticipant(AdditionalOrganizationProjectParticipant additionalOrganizationProjectParticipant)
        {
            var organization = CreateGetOrganizationQuery(additionalOrganizationProjectParticipant.OrganizationId).FirstOrDefault();
            if (organization == null)
            {
                throw new ModelNotFoundException(String.Format("The organization with id [{0}] does not exist.", additionalOrganizationProjectParticipant.OrganizationId));
            }
            var project = CreateGetProjectByIdQuery(additionalOrganizationProjectParticipant.ProjectId).FirstOrDefault();
            throwIfProjectDoesNotExist(additionalOrganizationProjectParticipant.ProjectId, project);

            var participantType = CreateGetParticipantTypeQuery().FirstOrDefault();
            throwIfParticipantTypeDoesNotExist(participantType);

            var existingParticipant = CreateGetParticipantByProjectIdAndOrganizationId(project.ProjectId, organization.OrganizationId).FirstOrDefault();
            if (existingParticipant == null)
            {
                DoHandleAdditionalProjectParticipant(additionalOrganizationProjectParticipant, participantType);
            }
        }

        private async Task HandleAdditionalOrganizationParticipantAsync(AdditionalOrganizationProjectParticipant additionalOrganizationProjectParticipant)
        {
            var organization = await CreateGetOrganizationQuery(additionalOrganizationProjectParticipant.OrganizationId).FirstOrDefaultAsync();
            if (organization == null)
            {
                throw new ModelNotFoundException(String.Format("The organization with id [{0}] does not exist.", additionalOrganizationProjectParticipant.OrganizationId));
            }
            var project = await CreateGetProjectByIdQuery(additionalOrganizationProjectParticipant.ProjectId).FirstOrDefaultAsync();
            throwIfProjectDoesNotExist(additionalOrganizationProjectParticipant.ProjectId, project);

            var participantType = await CreateGetParticipantTypeQuery().FirstOrDefaultAsync();
            throwIfParticipantTypeDoesNotExist(participantType);
            var existingParticipant = CreateGetParticipantByProjectIdAndOrganizationId(project.ProjectId, organization.OrganizationId).FirstOrDefault();
            if (existingParticipant == null)
            {
                DoHandleAdditionalProjectParticipant(additionalOrganizationProjectParticipant, participantType);
            }
        }

        private void HandleAdditionalPersonParticipant(AdditionalPersonProjectParticipant additionalPersonProjectParticipant)
        {
            var person = CreateGetPersonQuery(additionalPersonProjectParticipant.PersonId).FirstOrDefault();
            if (person == null)
            {
                throw new ModelNotFoundException(String.Format("The person with id [{0}] does not exist.", additionalPersonProjectParticipant.PersonId));
            }
            var project = CreateGetProjectByIdQuery(additionalPersonProjectParticipant.ProjectId).FirstOrDefault();
            throwIfProjectDoesNotExist(additionalPersonProjectParticipant.ProjectId, project);

            var participantType = CreateGetParticipantTypeQuery().FirstOrDefault();
            throwIfParticipantTypeDoesNotExist(participantType);

            var existingParticipant = CreateGetParticipantByProjectIdAndPersonId(project.ProjectId, person.PersonId).FirstOrDefault();
            if (existingParticipant == null)
            {
                DoHandleAdditionalProjectParticipant(additionalPersonProjectParticipant, participantType);
            }
        }

        private async Task HandleAdditionalPersonParticipantAsync(AdditionalPersonProjectParticipant additionalPersonProjectParticipant)
        {
            var person = await CreateGetPersonQuery(additionalPersonProjectParticipant.PersonId).FirstOrDefaultAsync();
            if (person == null)
            {
                throw new ModelNotFoundException(String.Format("The person with id [{0}] does not exist.", additionalPersonProjectParticipant.PersonId));
            }
            var project = await CreateGetProjectByIdQuery(additionalPersonProjectParticipant.ProjectId).FirstOrDefaultAsync();
            throwIfProjectDoesNotExist(additionalPersonProjectParticipant.ProjectId, project);

            var participantType = await CreateGetParticipantTypeQuery().FirstOrDefaultAsync();
            throwIfParticipantTypeDoesNotExist(participantType);

            var existingParticipant = await CreateGetParticipantByProjectIdAndPersonId(project.ProjectId, person.PersonId).FirstOrDefaultAsync();
            if (existingParticipant == null)
            {
                DoHandleAdditionalProjectParticipant(additionalPersonProjectParticipant, participantType);
            }
        }

        private Participant DoHandleAdditionalProjectParticipant(AdditionalProjectParticipant additionalProjectParticipant, ParticipantType participantType)
        {
            var participant = new Participant();
            additionalProjectParticipant.UpdateParticipant(participant, participantType);
            additionalProjectParticipant.Audit.SetHistory(participant);
            Context.Participants.Add(participant);
            return participant;
        }

        private IQueryable<Participant> CreateGetParticipantByProjectIdAndPersonId(int projectId, int personId)
        {
            return Context.Participants.Where(x => x.PersonId == personId);
        }

        private IQueryable<Participant> CreateGetParticipantByProjectIdAndOrganizationId(int projectId, int organizationId)
        {
            return Context.Participants.Where(x => x.OrganizationId == organizationId);
        }

        private IQueryable<ParticipantType> CreateGetParticipantTypeQuery()
        {
            return this.Context.ParticipantTypes.Where(x => x.ParticipantTypeId == ParticipantType.ForeignEducationalInstitution.Id);
        }

        private IQueryable<Organization> CreateGetOrganizationQuery(int organizationId)
        {
            return this.Context.Organizations.Where(x => x.OrganizationId == organizationId);
        }

        private IQueryable<Person> CreateGetPersonQuery(int personId)
        {
            return this.Context.People.Where(x => x.PersonId == personId);
        }

        #endregion

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

            var office = CreateGetOrganizationByProjectIdQuery(updatedProject.ProjectId).FirstOrDefault();
            Contract.Assert(office != null, "The project must have an office.");
            var officeSettings = officeService.GetOfficeSettings(office.OrganizationId);

            var allowedCategoryIds = CreateGetAllowedCategoryIdsQuery(projectToUpdate.ProgramId).ToList();
            this.logger.Trace("Loaded allowed category ids [{0}] for program with id [{1}].", String.Join(", ", allowedCategoryIds), projectToUpdate.ProgramId);

            var allowedObjectiveIds = CreateGetAllowedObjectiveIdsQuery(projectToUpdate.ProgramId).ToList();
            this.logger.Trace("Loaded allowed objective ids [{0}] for program with id [{1}].", String.Join(", ", allowedCategoryIds), projectToUpdate.ProgramId);

            validator.ValidateUpdate(GetUpdateValidationEntity(
                publishedProject: updatedProject,
                projectToUpdate: projectToUpdate,
                goalsExist: goalsExist,
                themesExist: themesExist,
                pointsOfContactExist: contactsExist,
                settings: officeSettings,
                allowedCategoryIds: allowedCategoryIds,
                allowedObjectiveIds: allowedObjectiveIds,
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
            
            var categoriesExist = await CheckAllCategoriesExistAsync(updatedProject.CategoryIds);
            this.logger.Trace("Check all goals with ids {0} existed.", String.Join(", ", updatedProject.GoalIds));

            var objectivesExist = await CheckAllObjectivesExistAsync(updatedProject.ObjectiveIds);
            this.logger.Trace("Check all contacts with ids {0} existed.", String.Join(", ", updatedProject.PointsOfContactIds));

            var office = await CreateGetOrganizationByProjectIdQuery(updatedProject.ProjectId).FirstOrDefaultAsync();
            Contract.Assert(office != null, "The project must have an office.");
            var officeSettings = await officeService.GetOfficeSettingsAsync(office.OrganizationId);

            var allowedCategoryIds = await CreateGetAllowedCategoryIdsQuery(projectToUpdate.ProgramId).ToListAsync();
            this.logger.Trace("Loaded allowed category ids [{0}] for program with id [{1}].", String.Join(", ", allowedCategoryIds), projectToUpdate.ProgramId);

            var allowedObjectiveIds = await CreateGetAllowedObjectiveIdsQuery(projectToUpdate.ProgramId).ToListAsync();
            this.logger.Trace("Loaded allowed objective ids [{0}] for program with id [{1}].", String.Join(", ", allowedCategoryIds), projectToUpdate.ProgramId);

            validator.ValidateUpdate(GetUpdateValidationEntity(
               publishedProject: updatedProject,
               projectToUpdate: projectToUpdate,
               goalsExist: goalsExist,
               themesExist: themesExist,
               pointsOfContactExist: contactsExist,
               settings: officeSettings,
               allowedCategoryIds: allowedCategoryIds,
               allowedObjectiveIds: allowedObjectiveIds,
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
            int numberOfObjectives,
            List<int> allowedCategoryIds,
            List<int> allowedObjectiveIds,
            OfficeSettings settings)
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
                numberOfObjectives: numberOfObjectives,
                officeSettings: settings,
                allowedCategoryIds: allowedCategoryIds,
                allowedObjectiveIds: allowedObjectiveIds
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

        private IQueryable<Organization> CreateGetOrganizationByProjectIdQuery(int projectId)
        {
            var query = Context.Projects.Where(x => x.ProjectId == projectId).Select(x => x.ParentProgram.Owner);
            return query;
        }

        private IQueryable<int> CreateGetAllowedCategoryIdsQuery(int programId)
        {
            return FocusCategoryQueries.CreateGetFocusCategoryDTOByProgramIdQuery(this.Context, programId).Select(x => x.Id);
        }

        private IQueryable<int> CreateGetAllowedObjectiveIdsQuery(int programId)
        {
            return JustificationObjectiveQueries.CreateGetJustificationObjectiveDTOByProgramIdQuery(this.Context, programId).Select(x => x.Id);
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
