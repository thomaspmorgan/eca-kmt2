using ECA.Business.Models.Programs;
using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Programs;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// A ProgramService is capable of performing crud operations on a program using entity framework.
    /// </summary>
    public class ProgramService : EcaService, IProgramService
    {
        private const string GET_PROGRAMS_SPROC_NAME = "GetPrograms";
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IBusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity> validator;

        /// <summary>
        /// Creates a new ProgramService with the given context to operator against.
        /// </summary>
        /// <param name="context">The context to operate on.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="programServiceValidator">The program service validator.</param>
        public ProgramService(EcaContext context, IBusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity> programServiceValidator)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(programServiceValidator != null, "The program service validator must not be null.");
            this.validator = programServiceValidator;
        }

        #region Get

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        public PagedQueryResults<SimpleProgramDTO> GetPrograms(QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            var results = ProgramQueries.CreateGetSimpleProgramDTOsQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved programs by query operator [{0}].", queryOperator);
            return results;
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        public async Task<PagedQueryResults<SimpleProgramDTO>> GetProgramsAsync(QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            var results = await ProgramQueries.CreateGetSimpleProgramDTOsQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved programs by query operator [{0}].", queryOperator);
            return results;
        }

        public PagedQueryResults<OrganizationProgramDTO> GetProgramsHierarchy(QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var results = CreateGetProgramsHierarchySqlQuery().ToArray();
            var pagedResults = GetPagedQueryResults(results, queryOperator);
            this.logger.Trace("Retrieved program hierarchy by query operator [{0}].", queryOperator);
            return pagedResults;
        }

        public async Task<PagedQueryResults<OrganizationProgramDTO>> GetProgramsHierarchyAsync(QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var results = (await CreateGetProgramsHierarchySqlQuery().ToArrayAsync());
            var pagedResults = GetPagedQueryResults(results, queryOperator);
            this.logger.Trace("Retrieved program hierarchy by query operator [{0}].", queryOperator);
            return pagedResults;
        }

        private PagedQueryResults<T> GetPagedQueryResults<T>(IEnumerable<T> enumerable, QueryableOperator<T> queryOperator) where T : class
        {
            var queryable = enumerable.AsQueryable<T>();
            queryable = queryable.Apply(queryOperator);
            return queryable.ToPagedQueryResults<T>(queryOperator.Start, queryOperator.Limit);
        }
        /// <summary>
        /// Returns the program with the given id, or null if it does not exist.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The program, or null if it doesn't exist.</returns>
        public ProgramDTO GetProgramById(int programId)
        {
            var dto = ProgramQueries.CreateGetPublishedProgramByIdQuery(this.Context, programId).FirstOrDefault();
            this.logger.Trace("Retrieved program by id [{0}].", programId);
            return dto;
        }

        /// <summary>
        /// Returns the program with the given id, or null if it does not exist.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The program, or null if it doesn't exist.</returns>
        public async Task<ProgramDTO> GetProgramByIdAsync(int programId)
        {
            var dto = await ProgramQueries.CreateGetPublishedProgramByIdQuery(this.Context, programId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved program by id [{0}].", programId);
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
            var regionTypeIds = GetLocationTypeIds(draftProgram.RegionIds);
            this.logger.Trace("Retrieved region type by region ids [{0}].", String.Join(", ", draftProgram.RegionIds));

            var owner = GetOrganizationById(draftProgram.OwnerOrganizationId);
            this.logger.Trace("Retrieved owner organization by id [{0}].", draftProgram.OwnerOrganizationId);

            var parentProgramId = draftProgram.ParentProgramId;
            Program parentProgram = parentProgramId.HasValue ? GetProgramEntityById(draftProgram.ParentProgramId.Value) : null;
            var program = DoCreate(draftProgram, GetValidationEntity(draftProgram, owner, parentProgram, regionTypeIds));
            this.logger.Trace("Created program.");
            
            return program;
        }

        /// <summary>
        /// Creates a new program in the ECA system with a status of draft.
        /// </summary>
        /// <param name="draftProgram">The draft program.</param>
        /// <returns>The saved program.</returns>
        public async Task<Program> CreateAsync(DraftProgram draftProgram)
        {
            var regionTypeIds = await GetLocationTypeIdsAsync(draftProgram.RegionIds);
            this.logger.Trace("Retrieved region type by region ids [{0}].", String.Join(", ", draftProgram.RegionIds));

            var owner = await GetOrganizationByIdAsync(draftProgram.OwnerOrganizationId);
            this.logger.Trace("Retrieved owner organization by id [{0}].", draftProgram.OwnerOrganizationId);

            var parentProgramId = draftProgram.ParentProgramId;
            Program parentProgram = parentProgramId.HasValue ? await GetProgramEntityByIdAsync(draftProgram.ParentProgramId.Value) : null;
            
            var program = DoCreate(draftProgram, GetValidationEntity(draftProgram, owner, parentProgram, regionTypeIds));
            this.logger.Trace("Created program.");
            return program;
        }

        private Program DoCreate(DraftProgram draftProgram, ProgramServiceValidationEntity validationEntity)
        {
            Contract.Requires(draftProgram != null, "The draft program must not be null.");
            validator.ValidateCreate(validationEntity);
            var owner = GetOrganizationById(draftProgram.OwnerOrganizationId);
            var program = new Program
            {
                Description = draftProgram.Description,
                EndDate = draftProgram.EndDate,
                Name = draftProgram.Name,

                ParentProgram = draftProgram.ParentProgramId.HasValue ? GetProgramEntityById(draftProgram.ParentProgramId.Value) : null,
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
            SetCategories(draftProgram.FocusCategoryIds, program);
            SetObjectives(draftProgram.JustificationObjectiveIds, program);

            Debug.Assert(draftProgram.Audit != null, "The audit must not be null.");
            draftProgram.Audit.SetHistory(program);
            this.Context.Programs.Add(program);
            return program;
        }

        #endregion

        #region Update

        private Task<Program> GetProgramEntityByIdAsync(int programId)
        {
            return GetProgramByIdQuery(programId).FirstOrDefaultAsync();
        }

        private IQueryable<Program> GetProgramByIdQuery(int programId)
        {
            return this.Context.Programs
                .Include(x => x.Themes)
                .Include(x => x.Goals)
                .Include(x => x.Contacts)
                .Include(x => x.Regions)
                .Include(x => x.Categories)
                .Include(x => x.Objectives)
                .Include(x => x.Owner)
                .Where(x => x.ProgramId == programId);
        }

        /// <summary>
        /// Updates the system's program with the given updated program.
        /// </summary>
        /// <param name="updatedProgram">The updated program.</param>
        public void Update(EcaProgram updatedProgram)
        {
            var programToUpdate = GetProgramEntityById(updatedProgram.Id);
            this.logger.Trace("Retrieved program with id [{0}].", updatedProgram.Id);

            var regionTypeIds = GetLocationTypeIds(updatedProgram.RegionIds);
            this.logger.Trace("Retrieved region type by region ids [{0}].", String.Join(", ", updatedProgram.RegionIds));

            var owner = GetOrganizationById(updatedProgram.OwnerOrganizationId);
            this.logger.Trace("Retrieved owner organization by id [{0}].", updatedProgram.OwnerOrganizationId);

            var parentProgramId = updatedProgram.ParentProgramId;
            Program parentProgram = parentProgramId.HasValue ? GetProgramEntityById(updatedProgram.ParentProgramId.Value) : null;
            if (programToUpdate != null)
            {
                DoUpdate(programToUpdate, updatedProgram, GetValidationEntity(updatedProgram, owner, parentProgram, regionTypeIds));
                this.logger.Trace("Performed update on program.");
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
            var programToUpdate = await GetProgramEntityByIdAsync(updatedProgram.Id);
            this.logger.Trace("Retrieved program with id [{0}].", updatedProgram.Id);

            var regionTypeIds = await GetLocationTypeIdsAsync(updatedProgram.RegionIds);
            this.logger.Trace("Retrieved region type by region ids [{0}].", String.Join(", ", updatedProgram.RegionIds));

            var owner = GetOrganizationById(updatedProgram.OwnerOrganizationId);
            this.logger.Trace("Retrieved owner organization by id [{0}].", updatedProgram.OwnerOrganizationId);

            var parentProgramId = updatedProgram.ParentProgramId;
            Program parentProgram = parentProgramId.HasValue ? await GetProgramEntityByIdAsync(updatedProgram.ParentProgramId.Value) : null;

            if (programToUpdate != null)
            {
                DoUpdate(programToUpdate, updatedProgram, GetValidationEntity(updatedProgram,owner, parentProgram, regionTypeIds));
                this.logger.Trace("Performed update on program.");
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
            programToUpdate.Description = updatedProgram.Description;
            programToUpdate.EndDate = updatedProgram.EndDate;
            programToUpdate.Name = updatedProgram.Name;
            programToUpdate.Owner = owner;
            programToUpdate.OwnerId = owner.OrganizationId;
            //parent program should already be loaded in the context via the Find method
            programToUpdate.ParentProgram = updatedProgram.ParentProgramId.HasValue ? GetProgramEntityById(updatedProgram.ParentProgramId.Value) : null;
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
            Contract.Requires(locationIds != null, "The location ids must not be null.");
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

        /// <summary>
        /// Returns the program with the given id.
        /// </summary>
        /// <param name="parentProgramId">The program id.</param>
        /// <returns>The program.</returns>
        protected Program GetProgramEntityById(int parentProgramId)
        {
            return this.Context.Programs.Find(parentProgramId);
        }

        /// <summary>
        /// Returns the program with the given id.
        /// </summary>
        /// <param name="parentProgramId">The program id.</param>
        /// <returns>The program.</returns>

        private ProgramServiceValidationEntity GetValidationEntity(EcaProgram program, Organization owner, Program parentProgram, List<int> regionTypesIds)
        {
            return new ProgramServiceValidationEntity(
                name: program.Name,
                description: program.Description,
                regionLocationTypeIds: regionTypesIds,
                contactIds: program.ContactIds,
                owner: owner,
                themeIds: program.ThemeIds,
                goalIds: program.GoalIds,
                regionIds: program.RegionIds,
                categoryIds: program.FocusCategoryIds,
                objectiveIds: program.JustificationObjectiveIds,
                parentProgramId: program.ParentProgramId,
                parentProgram: parentProgram
                );
        }

        private DbRawSqlQuery<OrganizationProgramDTO> CreateGetProgramsHierarchySqlQuery()
        {
            return this.Context.Database.SqlQuery<OrganizationProgramDTO>(GET_PROGRAMS_SPROC_NAME);
        }
    }
}
