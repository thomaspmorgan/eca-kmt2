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
using ECA.Business.Service.Admin;
using ECA.Business.Service.Lookup;

namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// A ProgramService is capable of performing crud operations on a program using entity framework.
    /// </summary>
    public class ProgramService : EcaService, IProgramService
    {
        /// <summary>
        /// The character used to seperate the path in the program hierarchy sql stored procedure.
        /// </summary>
        public static char[] PROGRAM_HIERARCHY_SPLIT_CHARS = new char[] { '-' };
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IBusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity> validator;
        private readonly IOfficeService officeService;

        /// <summary>
        /// Creates a new ProgramService with the given context to operator against.
        /// </summary>
        /// <param name="officeService">The office service.</param>
        /// <param name="context">The context to operate on.</param>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="programServiceValidator">The program service validator.</param>
        public ProgramService(
            EcaContext context,
            IOfficeService officeService,
            IBusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity> programServiceValidator,
            List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(programServiceValidator != null, "The program service validator must not be null.");
            Contract.Requires(officeService != null, "The office service must not be null.");
            this.validator = programServiceValidator;
            this.officeService = officeService;
        }

        #region Get

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        public PagedQueryResults<OrganizationProgramDTO> GetPrograms(QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var results = ProgramQueries.CreateGetOrganizationProgramDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved program with query operator [{0}].", queryOperator);
            return results;
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        public async Task<PagedQueryResults<OrganizationProgramDTO>> GetProgramsAsync(QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var results = await ProgramQueries.CreateGetOrganizationProgramDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved program with query operator [{0}].", queryOperator);
            return results;
        }

        public async Task<PagedQueryResults<OrganizationProgramDTO>> GetSubprogramsByProgramAsync(int programId, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var dto = await ProgramQueries.CreateGetSubprogramsQuery(this.Context, programId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved subprograms of program id [{0}].", programId);
            return dto;
        }

        public PagedQueryResults<OrganizationProgramDTO> GetSubprogramsByProgram(int programId, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var dto = ProgramQueries.CreateGetSubprogramsQuery(this.Context, programId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved subprograms of program id  [{0}].", programId);
            return dto;
        }

        /// <summary>
        /// Returns the list of programs in a hierarchy.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The programs.</returns>
        public PagedQueryResults<OrganizationProgramDTO> GetProgramsHierarchy(QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var results = CreateGetProgramsHierarchySqlQuery().ToArray();
            var pagedResults = GetPagedQueryResults(results, queryOperator);
            this.logger.Trace("Retrieved program hierarchy by query operator [{0}].", queryOperator);
            return pagedResults;
        }

        /// <summary>
        /// Returns the list of programs in a hierarchy.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The programs.</returns>
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

        /// <summary>
        /// Returns a list of parent programs to the program with the given id.  The root program will be first.
        /// </summary>
        /// <param name="programId">The id of the program to get parent programs for.</param>
        /// <returns>The list of parent programs, root first.</returns>
        public List<OrganizationProgramDTO> GetParentPrograms(int programId)
        {
            var hierarchyPrograms = CreateGetProgramsHierarchySqlQuery().ToArray();
            return DoGetParentPrograms(programId, hierarchyPrograms);
        }

        /// <summary>
        /// Returns a list of parent programs to the program with the given id.  The root program will be first.
        /// </summary>
        /// <param name="programId">The id of the program to get parent programs for.</param>
        /// <returns>The list of parent programs, root first.</returns>
        public async Task<List<OrganizationProgramDTO>> GetParentProgramsAsync(int programId)
        {
            var hierarchyPrograms = await CreateGetProgramsHierarchySqlQuery().ToArrayAsync();
            return DoGetParentPrograms(programId, hierarchyPrograms);
        }

        private List<OrganizationProgramDTO> DoGetParentPrograms(int programId, IEnumerable<OrganizationProgramDTO> hierarchyPrograms)
        {
            var childProgram = hierarchyPrograms.Where(x => x.ProgramId == programId).FirstOrDefault();
            if (childProgram == null)
            {
                return new List<OrganizationProgramDTO>();
            }
            else
            {
                var parentPrograms = new List<OrganizationProgramDTO>();
                var paths = childProgram.Path.Split(PROGRAM_HIERARCHY_SPLIT_CHARS, StringSplitOptions.RemoveEmptyEntries);
                //need to skip the last path value because that belongs to the program with the given program id.
                paths = paths.Take(paths.Length - 1).ToArray();
                var path = "";
                for (var i = 0; i < paths.Length; i++)
                {
                    if (i == 0)
                    {
                        path = paths[i];
                    }
                    else
                    {
                        path += PROGRAM_HIERARCHY_SPLIT_CHARS[0] + paths[i];
                    }
                    var parentProgram = hierarchyPrograms.Where(x => x.Path == path).FirstOrDefault();
                    Contract.Assert(parentProgram != null, String.Format("A Program with the path [{0}] should exist.", path));
                    parentPrograms.Add(parentProgram);
                }
                return parentPrograms;
            }
        }

        /// <summary>
        /// Returns all child programs of the program with the given id.
        /// </summary>
        /// <param name="programId">The id of the program to retrieve all child programs for.</param>
        /// <returns>All child, grand child, etc programs of the program with the given id.</returns>
        public List<OrganizationProgramDTO> GetAllChildPrograms(int programId)
        {
            var hierarchyPrograms = CreateGetProgramsHierarchySqlQuery().ToArray();
            return DoGetAllChildPrograms(programId, hierarchyPrograms);
        }

        /// <summary>
        /// Returns all child programs of the program with the given id.
        /// </summary>
        /// <param name="programId">The id of the program to retrieve all child programs for.</param>
        /// <returns>All child, grand child, etc programs of the program with the given id.</returns>
        public async Task<List<OrganizationProgramDTO>> GetAllChildProgramsAsync(int programId)
        {
            var hierarchyPrograms = await CreateGetProgramsHierarchySqlQuery().ToArrayAsync();
            return DoGetAllChildPrograms(programId, hierarchyPrograms);
        }

        private List<OrganizationProgramDTO> DoGetAllChildPrograms(int programId, IEnumerable<OrganizationProgramDTO> hierarchyPrograms)
        {
            var program = hierarchyPrograms.Where(x => x.ProgramId == programId).FirstOrDefault();
            if (program == null)
            {
                return new List<OrganizationProgramDTO>();
            }
            else
            {
                var childPrograms = hierarchyPrograms.Where(x => x.Path.IndexOf(program.Path) == 0 && x.ProgramId != program.ProgramId).ToList();
                return childPrograms;
            }
        }

        /// <summary>
        /// Returns all child programs of the program with the given id, including the parent.
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public List<OrganizationProgramDTO> GetAllChildProgramsWithParent(int programId)
        {
            var hierarchyPrograms = CreateGetProgramsHierarchySqlQuery().ToArray();
            return DoGetAllChildProgramsWithParent(programId, hierarchyPrograms);
        }

        /// <summary>
        /// Returns all child programs of the program with the given id, including the parent.
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<List<OrganizationProgramDTO>> GetAllChildProgramsWithParentAsync(int programId)
        {
            var hierarchyPrograms = await CreateGetProgramsHierarchySqlQuery().ToArrayAsync();
            return DoGetAllChildProgramsWithParent(programId, hierarchyPrograms);
        }

        private List<OrganizationProgramDTO> DoGetAllChildProgramsWithParent(int programId, IEnumerable<OrganizationProgramDTO> hierarchyPrograms)
        {
            var program = hierarchyPrograms.Where(x => x.ProgramId == programId).FirstOrDefault();
            if (program == null)
            {
                return new List<OrganizationProgramDTO>();
            }
            else
            {
                var childPrograms = hierarchyPrograms.Where(x => x.Path.IndexOf(program.Path) == 0).ToList();
                return childPrograms;
            }
        }


        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs that could be a parent to the program with the given id.
        /// </summary>
        /// <param name="programId">The id of the program to get valid parent programs for.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of programs that could be a parent to the program with the given id.</returns>
        public PagedQueryResults<OrganizationProgramDTO> GetValidParentPrograms(int programId, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var results = CreateGetProgramsHierarchySqlQuery().ToArray();
            return DoGetValidParentPrograms(programId, results, queryOperator);
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs that could be a parent to the program with the given id.
        /// </summary>
        /// <param name="programId">The id of the program to get valid parent programs for.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of programs that could be a parent to the program with the given id.</returns>
        public async Task<PagedQueryResults<OrganizationProgramDTO>> GetValidParentProgramsAsync(int programId, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var results = await CreateGetProgramsHierarchySqlQuery().ToArrayAsync();
            return DoGetValidParentPrograms(programId, results, queryOperator);
        }

        private PagedQueryResults<OrganizationProgramDTO> DoGetValidParentPrograms(int programId, IEnumerable<OrganizationProgramDTO> hierarchyPrograms, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var program = hierarchyPrograms.Where(x => x.ProgramId == programId).FirstOrDefault();
            if (program == null)
            {
                return new PagedQueryResults<OrganizationProgramDTO>(0, new List<OrganizationProgramDTO>());
            }
            else
            {
                var childPrograms = DoGetAllChildPrograms(programId, hierarchyPrograms);
                var validParentPrograms = hierarchyPrograms
                    .Where(x => x.Owner_OrganizationId == program.Owner_OrganizationId && x.ProgramId != programId)
                    .ToList()
                    .Except(childPrograms, new OrganizationProgramDTOComparer())
                    .ToList();
                var pagedResults = GetPagedQueryResults(validParentPrograms, queryOperator);
                return pagedResults;
            }

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

            var ownerOfficeSettings = officeService.GetOfficeSettings(owner.OrganizationId);
            this.logger.Trace("Retrieved office settings for owner organization with id [{0}].", owner.OrganizationId);

            var parentProgramId = draftProgram.ParentProgramId;
            Program parentProgram = parentProgramId.HasValue ? GetProgramEntityById(draftProgram.ParentProgramId.Value) : null;

            var inactiveRegionIds = GetLocationsById(draftProgram.RegionIds).Where(x => !x.IsActive).Select(x => x.LocationId).ToList();

            var program = DoCreate(draftProgram, GetValidationEntity(
                    program: draftProgram,
                    owner: owner,
                    ownerOfficeSettings: ownerOfficeSettings,
                    parentProgram: parentProgram,
                    regionTypesIds: regionTypeIds,
                    inactiveRegionIds: inactiveRegionIds,
                    parentProgramParentPrograms: new List<OrganizationProgramDTO>()));
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

            var ownerOfficeSettings = await officeService.GetOfficeSettingsAsync(owner.OrganizationId);
            this.logger.Trace("Retrieved office settings for owner organization with id [{0}].", owner.OrganizationId);

            var parentProgramId = draftProgram.ParentProgramId;
            Program parentProgram = parentProgramId.HasValue ? await GetProgramEntityByIdAsync(draftProgram.ParentProgramId.Value) : null;

            var inactiveRegionIds = (await GetLocationsByIdAsync(draftProgram.RegionIds)).Where(x => !x.IsActive).Select(x => x.LocationId).ToList();

            var program = DoCreate(draftProgram, GetValidationEntity(
                    program: draftProgram,
                    owner: owner,
                    ownerOfficeSettings: ownerOfficeSettings,
                    parentProgram: parentProgram,
                    regionTypesIds: regionTypeIds,
                    inactiveRegionIds: inactiveRegionIds,
                    parentProgramParentPrograms: new List<OrganizationProgramDTO>()));
            this.logger.Trace("Created program.");
            return program;
        }

        private Program DoCreate(DraftProgram draftProgram, ProgramServiceValidationEntity validationEntity)
        {
            Contract.Requires(draftProgram != null, "The draft program must not be null.");
            validator.ValidateCreate(validationEntity);
            var program = new Program
            {
                Description = draftProgram.Description,
                EndDate = draftProgram.EndDate,
                Name = draftProgram.Name,
                ParentProgramId = draftProgram.ParentProgramId,
                ProgramType = null,
                ProgramStatusId = draftProgram.ProgramStatusId,
                StartDate = draftProgram.StartDate,
                OwnerId = draftProgram.OwnerOrganizationId
            };
            SetGoals(draftProgram.GoalIds, program);
            SetPointOfContacts(draftProgram.ContactIds, program);
            SetThemes(draftProgram.ThemeIds, program);
            SetRegions(draftProgram.RegionIds, program);
            SetCategories(draftProgram.FocusCategoryIds, program);
            SetObjectives(draftProgram.JustificationObjectiveIds, program);
            SetWebsitesToCreate(draftProgram.Websites, program, draftProgram);

            Debug.Assert(draftProgram.Audit != null, "The audit must not be null.");
            draftProgram.Audit.SetHistory(program);
            this.Context.Programs.Add(program);
            return program;
        }

        private void SetWebsitesToCreate(List<WebsiteDTO> websiteList, Program program, DraftProgram draftProgram)
        {
            var distinctWebsites = websiteList.Select(x => x.Value).Distinct().ToList();
            var websites = new List<Website>();
            foreach (string websiteValue in distinctWebsites)
            {
                var website = new Website { WebsiteValue = websiteValue };
                draftProgram.Audit.SetHistory(website);
                websites.Add(website);
            }
            program.Websites = websites;
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
                .Include(x => x.Websites)
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

            if (programToUpdate != null)
            {
                var regionTypeIds = GetLocationTypeIds(updatedProgram.RegionIds);
                this.logger.Trace("Retrieved region type by region ids [{0}].", String.Join(", ", updatedProgram.RegionIds));

                var owner = GetOrganizationById(updatedProgram.OwnerOrganizationId);
                this.logger.Trace("Retrieved owner organization by id [{0}].", updatedProgram.OwnerOrganizationId);

                var ownerOfficeSettings = officeService.GetOfficeSettings(owner.OrganizationId);
                this.logger.Trace("Retrieved office settings for owner organization with id [{0}].", owner.OrganizationId);

                var parentProgramId = updatedProgram.ParentProgramId;
                Program parentProgram = parentProgramId.HasValue ? GetProgramEntityById(updatedProgram.ParentProgramId.Value) : null;

                List<OrganizationProgramDTO> parentProgramParentPrograms = new List<OrganizationProgramDTO>();
                if (parentProgram != null)
                {
                    parentProgramParentPrograms = GetParentPrograms(parentProgram.ProgramId);
                }

                var inactiveRegionIds = GetNewInactiveProgramLocations(updatedProgram.Id, updatedProgram.RegionIds).Select(x => x.LocationId).ToList();

                DoUpdate(programToUpdate, updatedProgram, GetValidationEntity(
                    program: updatedProgram,
                    owner: owner,
                    ownerOfficeSettings: ownerOfficeSettings,
                    parentProgram: parentProgram,
                    regionTypesIds: regionTypeIds,
                    inactiveRegionIds: inactiveRegionIds,
                    parentProgramParentPrograms: parentProgramParentPrograms));
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

            if (programToUpdate != null)
            {
                var regionTypeIds = await GetLocationTypeIdsAsync(updatedProgram.RegionIds);
                this.logger.Trace("Retrieved region type by region ids [{0}].", String.Join(", ", updatedProgram.RegionIds));

                var owner = GetOrganizationById(updatedProgram.OwnerOrganizationId);
                this.logger.Trace("Retrieved owner organization by id [{0}].", updatedProgram.OwnerOrganizationId);

                var ownerOfficeSettings = await officeService.GetOfficeSettingsAsync(owner.OrganizationId);
                this.logger.Trace("Retrieved office settings for owner organization with id [{0}].", owner.OrganizationId);

                var parentProgramId = updatedProgram.ParentProgramId;
                Program parentProgram = parentProgramId.HasValue ? await GetProgramEntityByIdAsync(updatedProgram.ParentProgramId.Value) : null;

                List<OrganizationProgramDTO> parentProgramParentPrograms = new List<OrganizationProgramDTO>();
                if (parentProgram != null)
                {
                    parentProgramParentPrograms = await GetParentProgramsAsync(parentProgram.ProgramId);
                }

                var inactiveRegionIds = await GetNewInactiveProgramLocations(updatedProgram.Id, updatedProgram.RegionIds).Select(x => x.LocationId).ToListAsync();

                DoUpdate(programToUpdate, updatedProgram, GetValidationEntity(
                    program: updatedProgram,
                    owner: owner,
                    ownerOfficeSettings: ownerOfficeSettings,
                    parentProgram: parentProgram,
                    regionTypesIds: regionTypeIds,
                    inactiveRegionIds: inactiveRegionIds,
                    parentProgramParentPrograms: parentProgramParentPrograms));
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
            programToUpdate.Description = updatedProgram.Description;
            programToUpdate.EndDate = updatedProgram.EndDate;
            programToUpdate.Name = updatedProgram.Name;
            programToUpdate.OwnerId = updatedProgram.OwnerOrganizationId;
            programToUpdate.ParentProgramId = updatedProgram.ParentProgramId;
            programToUpdate.ProgramStatusId = updatedProgram.ProgramStatusId;
            programToUpdate.RowVersion = updatedProgram.RowVersion;
            programToUpdate.StartDate = updatedProgram.StartDate;

            updatedProgram.Audit.SetHistory(programToUpdate);

            SetGoals(updatedProgram.GoalIds, programToUpdate);
            SetPointOfContacts(updatedProgram.ContactIds, programToUpdate);
            SetThemes(updatedProgram.ThemeIds, programToUpdate);
            SetRegions(updatedProgram.RegionIds, programToUpdate);
            SetCategories(updatedProgram.FocusCategoryIds, programToUpdate);
            SetObjectives(updatedProgram.JustificationObjectiveIds, programToUpdate);
            SetWebsitesToUpdate(updatedProgram, programToUpdate);
        }

        private void SetWebsitesToUpdate(EcaProgram updatedProgram, Program program)
        {
            var websiteIds = updatedProgram.Websites.Select(x => x.Id).ToList();
            var websitesToRemove = program.Websites.Where(x => !websiteIds.Where(y => y != null).Contains(x.WebsiteId)).ToList();
            foreach (Website website in websitesToRemove)
            {
                program.Websites.Remove(website);
                Context.Websites.Remove(website);
            }
            var websitesToAdd = updatedProgram.Websites.Where(x => x.Id == null).ToList();
            foreach (WebsiteDTO website in websitesToAdd)
            {
                var newWebsite = new Website { WebsiteValue = website.Value };
                updatedProgram.Audit.SetHistory(newWebsite);
                program.Websites.Add(newWebsite);
            }
            var websitesToUpdate = program.Websites.Where(x => websiteIds.Contains(x.WebsiteId)).ToList();
            foreach (Website website in websitesToUpdate)
            {
                var updatedWebsite = updatedProgram.Websites.Where(x => x.Id == website.WebsiteId).FirstOrDefault();
                website.WebsiteValue = updatedWebsite.Value;
                updatedProgram.Audit.SetHistory(website);
            }
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

        private ProgramServiceValidationEntity GetValidationEntity(
            EcaProgram program,
            Organization owner,
            OfficeSettings ownerOfficeSettings,
            Program parentProgram,
            List<int> regionTypesIds,
            List<int> inactiveRegionIds,
            List<OrganizationProgramDTO> parentProgramParentPrograms)
        {
            return new ProgramServiceValidationEntity(
                programId: program.Id,
                name: program.Name,
                description: program.Description,
                regionLocationTypeIds: regionTypesIds,
                inactiveRegionIds: inactiveRegionIds,
                contactIds: program.ContactIds,
                owner: owner,
                themeIds: program.ThemeIds,
                goalIds: program.GoalIds,
                regionIds: program.RegionIds,
                categoryIds: program.FocusCategoryIds,
                objectiveIds: program.JustificationObjectiveIds,
                parentProgramId: program.ParentProgramId,
                parentProgram: parentProgram,
                ownerOfficeSettings: ownerOfficeSettings,
                parentProgramParentPrograms: parentProgramParentPrograms
                );
        }

        private DbRawSqlQuery<OrganizationProgramDTO> CreateGetProgramsHierarchySqlQuery()
        {
            return this.Context.Database.SqlQuery<OrganizationProgramDTO>(OfficeService.GET_PROGRAMS_SPROC_NAME);
        }
    }
}
