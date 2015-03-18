using ECA.Business.Models.Programs;
using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Programs;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.Exceptions;
using ECA.Core.Logging;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// A ProgramService is capable of performing crud operations on a program using entity framework.
    /// </summary>
    public class ProgramService : DbContextService<EcaContext>, IProgramService
    {
        private static readonly string COMPONENT_NAME = typeof(ProgramService).FullName;

        private readonly ILogger logger;
        private readonly IBusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity> validator;

        /// <summary>
        /// Creates a new ProgramService with the given context to operator against.
        /// </summary>
        /// <param name="context">The context to operate on.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="programServiceValidator">The program service validator.</param>
        public ProgramService(EcaContext context, ILogger logger, IBusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity> programServiceValidator)
            : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
            Contract.Requires(programServiceValidator != null, "The program service validator must not be null.");
            this.validator = programServiceValidator;
            this.logger = logger;
        }

        #region Get

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        public PagedQueryResults<SimpleProgramDTO> GetPrograms(QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            var stopwatch = Stopwatch.StartNew();
            var results = ProgramQueries.CreateGetSimpleProgramDTOsQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object>{{"queryOperator", queryOperator}});
            return results;
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        public async Task<PagedQueryResults<SimpleProgramDTO>> GetProgramsAsync(QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            var stopwatch = Stopwatch.StartNew();
            var results = await ProgramQueries.CreateGetSimpleProgramDTOsQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "queryOperator", queryOperator } });
            return results;
        }

        /// <summary>
        /// Returns the program with the given id, or null if it does not exist.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The program, or null if it doesn't exist.</returns>
        public ProgramDTO GetProgramById(int programId)
        {
            var stopwatch = Stopwatch.StartNew();
            var dto = ProgramQueries.CreateGetPublishedProgramByIdQuery(this.Context, programId).FirstOrDefault();
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "programId", programId } });
            return dto;
        }

        /// <summary>
        /// Returns the program with the given id, or null if it does not exist.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The program, or null if it doesn't exist.</returns>
        public async Task<ProgramDTO> GetProgramByIdAsync(int programId)
        {
            var stopwatch = Stopwatch.StartNew();
            var dto = await ProgramQueries.CreateGetPublishedProgramByIdQuery(this.Context, programId).FirstOrDefaultAsync();
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "programId", programId } });
            return dto;
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates a new program in the ECA system with a status of draft.
        /// </summary>
        /// <param name="draftProgram">The draft program.</param>
        /// <returns>The saved program.</returns>
        public Program Create(DraftProgram draftProgram)
        {
            var stopwatch = Stopwatch.StartNew();
            var regionTypeIds = GetLocationTypeIds(draftProgram.RegionIds);
            var focus = GetFocusById(draftProgram.FocusId);
            var owner = GetOrganizationById(draftProgram.OwnerOrganizationId);
            var parentProgramId = draftProgram.ParentProgramId;
            Program parentProgram = parentProgramId.HasValue ? GetParentProgramById(draftProgram.ParentProgramId.Value) : null;
            var program = DoCreate(draftProgram, GetValidationEntity(draftProgram, focus, owner, parentProgram, regionTypeIds));
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed);
            return program;
        }

        /// <summary>
        /// Creates a new program in the ECA system with a status of draft.
        /// </summary>
        /// <param name="draftProgram">The draft program.</param>
        /// <returns>The saved program.</returns>
        public async Task<Program> CreateAsync(DraftProgram draftProgram)
        {
            var stopwatch = Stopwatch.StartNew();
            var regionTypeIds = await GetLocationTypeIdsAsync(draftProgram.RegionIds);
            var focus = await GetFocusByIdAsync(draftProgram.FocusId);
            var owner = await GetOrganizationByIdAsync(draftProgram.OwnerOrganizationId);
            var parentProgramId = draftProgram.ParentProgramId;
            Program parentProgram = parentProgramId.HasValue ? await GetParentProgramByIdAsync(draftProgram.ParentProgramId.Value) : null;
            var program = DoCreate(draftProgram, GetValidationEntity(draftProgram, focus, owner, parentProgram, regionTypeIds));
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed);
            return program;
        }

        private Program DoCreate(DraftProgram draftProgram, ProgramServiceValidationEntity validationEntity)
        {
            Contract.Requires(draftProgram != null, "The draft program must not be null.");
            validator.ValidateCreate(validationEntity);
            var owner = GetOrganizationById(draftProgram.OwnerOrganizationId);
            var focus = GetFocusById(draftProgram.FocusId);
            var program = new Program
            {
                Description = draftProgram.Description,
                EndDate = draftProgram.EndDate,
                Focus = focus,
                Name = draftProgram.Name,
                ParentProgram = draftProgram.ParentProgramId.HasValue ? GetParentProgramById(draftProgram.ParentProgramId.Value) : null,
                ProgramType = null,
                ProgramStatusId = draftProgram.ProgramStatusId,
                StartDate = draftProgram.StartDate,
                OwnerId = owner.OrganizationId,
                Owner = owner,
                Website = draftProgram.Website,
            };
            SetGoals(draftProgram.GoalIds, program);
            SetPointOfContacts(draftProgram.ContactIds, program);
            SetThemes(draftProgram.ThemeIds, program);
            SetRegions(draftProgram.RegionIds, program);
            Debug.Assert(draftProgram.Audit != null, "The audit must not be null.");
            draftProgram.Audit.SetHistory(program);
            this.Context.Programs.Add(program);
            return program;
        }

        #endregion

        #region Update
        private IQueryable<Program> CreateGetProgramByIdQuery(int programId)
        {
            return this.Context.Programs.Where(x => x.ProgramId == programId);
        }

        /// <summary>
        /// Updates the system's program with the given updated program.
        /// </summary>
        /// <param name="updatedProgram">The updated program.</param>
        public void Update(EcaProgram updatedProgram)
        {
            var stopwatch = Stopwatch.StartNew();
            var programToUpdate = CreateGetProgramByIdQuery(updatedProgram.Id).FirstOrDefault();
            var regionTypeIds = GetLocationTypeIds(updatedProgram.RegionIds);
            var focus = GetFocusById(updatedProgram.FocusId);
            var owner = GetOrganizationById(updatedProgram.OwnerOrganizationId);
            var parentProgramId = updatedProgram.ParentProgramId;
            Program parentProgram = parentProgramId.HasValue ? GetParentProgramById(updatedProgram.ParentProgramId.Value) : null;
            if (programToUpdate != null)
            {
                DoUpdate(programToUpdate, updatedProgram, GetValidationEntity(updatedProgram, focus, owner, parentProgram, regionTypeIds));
                stopwatch.Stop();
                this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed);
            }
            else
            {
                throw new ModelNotFoundException(String.Format("The program with the given Id [{0}] was not found.", updatedProgram.Id));
            }
        }

        /// <summary>
        /// Updates the system's program with the given updated program.
        /// </summary>
        /// <param name="updatedProgram">The updated program.</param>
        public async Task UpdateAsync(EcaProgram updatedProgram)
        {
            var stopwatch = Stopwatch.StartNew();
            var programToUpdate = await CreateGetProgramByIdQuery(updatedProgram.Id).FirstOrDefaultAsync();
            var regionTypeIds = await GetLocationTypeIdsAsync(updatedProgram.RegionIds);
            var focus = await GetFocusByIdAsync(updatedProgram.FocusId);
            var owner = GetOrganizationById(updatedProgram.OwnerOrganizationId);
            var parentProgramId = updatedProgram.ParentProgramId;
            Program parentProgram = parentProgramId.HasValue ? await GetParentProgramByIdAsync(updatedProgram.ParentProgramId.Value) : null;
            
            if (programToUpdate != null)
            {
                DoUpdate(programToUpdate, updatedProgram, GetValidationEntity(updatedProgram, focus, owner, parentProgram, regionTypeIds));
                stopwatch.Stop();
                this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed);
            }
            else
            {
                throw new ModelNotFoundException(String.Format("The program with the given Id [{0}] was not found.", updatedProgram.Id));
            }
        }

        private void DoUpdate(Program programToUpdate, EcaProgram updatedProgram, ProgramServiceValidationEntity validationEntity)
        {
            Contract.Requires(updatedProgram != null, "The updated program must not be null.");
            validator.ValidateUpdate(validationEntity);
            var owner = GetOrganizationById(updatedProgram.OwnerOrganizationId);
            var focus = GetFocusById(updatedProgram.FocusId);
            programToUpdate.Description = updatedProgram.Description;
            programToUpdate.EndDate = updatedProgram.EndDate;
            programToUpdate.Focus = focus;
            programToUpdate.Name = updatedProgram.Name;
            programToUpdate.Owner = owner;
            programToUpdate.OwnerId = owner.OrganizationId;
            programToUpdate.ParentProgram = updatedProgram.ParentProgramId.HasValue ? GetParentProgramById(updatedProgram.ParentProgramId.Value) : null;
            programToUpdate.ProgramStatusId = updatedProgram.ProgramStatusId;
            programToUpdate.RowVersion = updatedProgram.RowVersion;
            programToUpdate.StartDate = updatedProgram.StartDate;
            programToUpdate.Website = updatedProgram.Website;

            updatedProgram.Audit.SetHistory(programToUpdate);

            SetGoals(updatedProgram.GoalIds, programToUpdate);
            SetPointOfContacts(updatedProgram.ContactIds, programToUpdate);
            SetThemes(updatedProgram.ThemeIds, programToUpdate);
            SetRegions(updatedProgram.RegionIds, programToUpdate);
        }
        #endregion

        private ProgramServiceValidationEntity GetValidationEntity(EcaProgram program, Focus focus, Organization owner, Program parentProgram, List<int> regionTypesIds)
        {
            return new ProgramServiceValidationEntity(
                name: program.Name,
                description: program.Description,
                regionLocationTypeIds: regionTypesIds,
                contactIds: program.ContactIds,
                focus: focus,
                owner: owner,
                themeIds: program.ThemeIds,
                goalIds: program.GoalIds,
                regionIds: program.RegionIds,
                parentProgramId: program.ParentProgramId,
                parentProgram: parentProgram
                );
        }

        /// <summary>
        /// Updates the goals on the given program to the goals with the given ids.
        /// </summary>
        /// <param name="goalIds">The goal ids.</param>
        /// <param name="programEntity">The program.</param>
        public void SetGoals(List<int> goalIds, Program programEntity)
        {
            Contract.Requires(goalIds != null, "The goal ids must not be null.");
            Contract.Requires(programEntity != null, "The program entity must not be null.");
            programEntity.Goals.Clear();
            goalIds.ForEach(x =>
            {
                var goal = new Goal { GoalId = x };
                this.Context.Goals.Attach(goal);
                programEntity.Goals.Add(goal);
                goal.Programs.Add(programEntity);
            });
        }

        /// <summary>
        /// Updates the points of contacts on the given program to the pocs with the given ids.
        /// </summary>
        /// <param name="pointOfContactIds">The points of contacts by id.</param>
        /// <param name="programEntity">The program to update.</param>
        public void SetPointOfContacts(List<int> pointOfContactIds, Program programEntity)
        {
            Contract.Requires(pointOfContactIds != null, "The list of poc ids must not be null.");
            Contract.Requires(programEntity != null, "The program entity must not be null.");
            programEntity.Contacts.Clear();
            pointOfContactIds.ForEach(x =>
            {
                var contact = new Contact { ContactId = x };
                this.Context.Contacts.Attach(contact);
                programEntity.Contacts.Add(contact);
                contact.Programs.Add(programEntity);
            });
        }

        /// <summary>
        /// Updates the themes on the given program to the themes with the given ids.
        /// </summary>
        /// <param name="themeIds">The themes by id.</param>
        /// <param name="programEntity">The program to update.</param>
        public void SetThemes(List<int> themeIds, Program programEntity)
        {
            Contract.Requires(themeIds != null, "The theme ids must not be null.");
            Contract.Requires(programEntity != null, "The program entity must not be null.");
            programEntity.Themes.Clear();
            themeIds.ForEach(x =>
            {
                var theme = new Theme { ThemeId = x };
                this.Context.Themes.Attach(theme);
                programEntity.Themes.Add(theme);
                theme.Programs.Add(programEntity);
            });
        }

        /// <summary>
        /// Updates the regions on the given program to the regions with the given ids.
        /// </summary>
        /// <param name="regionIds">The regions by id.</param>
        /// <param name="programEntity">The program to update.</param>
        public void SetRegions(List<int> regionIds, Program programEntity)
        {
            Contract.Requires(regionIds != null, "The theme ids must not be null.");
            Contract.Requires(programEntity != null, "The program entity must not be null.");            
            programEntity.Regions.Clear();
            regionIds.ForEach(x =>
            {
                var location = new Location { LocationId = x };
                this.Context.Locations.Attach(location);
                programEntity.Regions.Add(location);
            });
        }

        #region Validation
        /// <summary>
        /// Returns the program with the given id.
        /// </summary>
        /// <param name="parentProgramId">The program id.</param>
        /// <returns>The program.</returns>
        protected Program GetParentProgramById(int parentProgramId)
        {
            return this.Context.Programs.Find(parentProgramId);
        }

        /// <summary>
        /// Returns the program with the given id.
        /// </summary>
        /// <param name="parentProgramId">The program id.</param>
        /// <returns>The program.</returns>
        protected async Task<Program> GetParentProgramByIdAsync(int parentProgramId)
        {
            return await this.Context.Programs.FindAsync(parentProgramId);
        }

        /// <summary>
        /// Returns the focus with the given id.
        /// </summary>
        /// <param name="focusId">The focus id.</param>
        /// <returns>The focus.</returns>
        protected Focus GetFocusById(int focusId)
        {
            return this.Context.Foci.Find(focusId);
        }

        /// <summary>
        /// Returns the focus with the given id.
        /// </summary>
        /// <param name="focusId">The focus id.</param>
        /// <returns>The focus.</returns>
        protected async Task<Focus> GetFocusByIdAsync(int focusId)
        {
            return await this.Context.Foci.FindAsync(focusId);
        }

        /// <summary>
        /// Returns the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The organization.</returns>
        protected Organization GetOrganizationById(int organizationId)
        {
            return this.Context.Organizations.Find(organizationId);
        }

        /// <summary>
        /// Returns the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The organization.</returns>
        protected async Task<Organization> GetOrganizationByIdAsync(int organizationId)
        {
            return await this.Context.Organizations.FindAsync(organizationId);
        }

        /// <summary>
        /// Returns the location types for the given location ids.
        /// </summary>
        /// <param name="locationIds">The list of location ids.</param>
        /// <returns>The list of location type ids.</returns>
        public List<int> GetLocationTypeIds(List<int> locationIds)
        {
            return LocationQueries.CreateGetLocationTypeIdsQuery(this.Context, locationIds).ToList();
        }

        /// <summary>
        /// Returns the location types for the given location ids.
        /// </summary>
        /// <param name="locationIds">The list of location ids.</param>
        /// <returns>The list of location type ids.</returns>
        public async Task<List<int>> GetLocationTypeIdsAsync(List<int> locationIds)
        {
            return await LocationQueries.CreateGetLocationTypeIdsQuery(this.Context, locationIds).ToListAsync();
        }

        #endregion
    }
}
