using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Programs;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Exceptions;
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
        private ILocationService locationService;

        /// <summary>
        /// Creates a new ProgramService with the given context to operator against.
        /// </summary>
        /// <param name="context">The context to operate on.</param>
        public ProgramService(EcaContext context, ILocationService locationService)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(locationService != null, "The location service must not be null.");
            this.locationService = locationService;
        }

        #region Get

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        public PagedQueryResults<SimpleProgramDTO> GetPrograms(QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            return ProgramQueries.CreateGetSimpleProgramDTOsQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        public Task<PagedQueryResults<SimpleProgramDTO>> GetProgramsAsync(QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            return ProgramQueries.CreateGetSimpleProgramDTOsQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the program with the given id, or null if it does not exist.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The program, or null if it doesn't exist.</returns>
        public ProgramDTO GetProgramById(int programId)
        {
            return ProgramQueries.CreateGetPublishedProgramByIdQuery(this.Context, programId).FirstOrDefault();
        }

        /// <summary>
        /// Returns the program with the given id, or null if it does not exist.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The program, or null if it doesn't exist.</returns>
        public Task<ProgramDTO> GetProgramByIdAsync(int programId)
        {
            return ProgramQueries.CreateGetPublishedProgramByIdQuery(this.Context, programId).FirstOrDefaultAsync();
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
            return DoCreate(regionTypeIds, draftProgram);
        }

        /// <summary>
        /// Creates a new program in the ECA system with a status of draft.
        /// </summary>
        /// <param name="draftProgram">The draft program.</param>
        /// <returns>The saved program.</returns>
        public async Task<Program> CreateAsync(DraftProgram draftProgram)
        {
            var regionTypeIds = await GetLocationTypeIdsAsync(draftProgram.RegionIds);
            return DoCreate(regionTypeIds, draftProgram);
        }

        private Program DoCreate(List<int> regionTypeIds, DraftProgram draftProgram)
        {
            Debug.Assert(draftProgram != null, "The draft program must not be null.");
            ValidateAllLocationsAreRegions(regionTypeIds);
            var owner = AttachOrganization(draftProgram.OwnerOrganizationId);
            var program = new Program
            {
                Description = draftProgram.Description,
                EndDate = draftProgram.EndDate,
                Focus = draftProgram.Focus,
                Name = draftProgram.Name,
                ParentProgram = draftProgram.ParentProgramId.HasValue ? AttachProgram(draftProgram.ParentProgramId.Value) : null,
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
            var programToUpdate = CreateGetProgramByIdQuery(updatedProgram.Id).FirstOrDefault();
            var regionLocationTypeIds = GetLocationTypeIds(updatedProgram.RegionIds);
            if (programToUpdate != null)
            {
                DoUpdate(programToUpdate, updatedProgram, regionLocationTypeIds);
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
            var programToUpdate = await CreateGetProgramByIdQuery(updatedProgram.Id).FirstOrDefaultAsync();
            var regionLocationTypeIds = await GetLocationTypeIdsAsync(updatedProgram.RegionIds);
            if (programToUpdate != null)
            {
                DoUpdate(programToUpdate, updatedProgram, regionLocationTypeIds);
            }
            else
            {
                throw new ModelNotFoundException(String.Format("The program with the given Id [{0}] was not found.", updatedProgram.Id));
            }
        }

        private void DoUpdate(Program programToUpdate, EcaProgram updatedProgram, List<int> locationTypeIds)
        {
            Debug.Assert(updatedProgram != null, "The updated program must not be null.");
            Debug.Assert(updatedProgram.Audit != null, "The audit must not be null.");
            ValidateAllLocationsAreRegions(locationTypeIds);

            var owner = AttachOrganization(updatedProgram.OwnerOrganizationId);
            programToUpdate.Description = updatedProgram.Description;
            programToUpdate.EndDate = updatedProgram.EndDate;
            programToUpdate.Focus = updatedProgram.Focus;
            programToUpdate.Name = updatedProgram.Name;
            programToUpdate.Owner = owner;
            programToUpdate.OwnerId = owner.OrganizationId;
            programToUpdate.ParentProgram = updatedProgram.ParentProgramId.HasValue ? AttachProgram(updatedProgram.ParentProgramId.Value) : null;
            programToUpdate.ProgramStatusId = updatedProgram.ProgramStatusId;
            programToUpdate.StartDate = updatedProgram.StartDate;
            programToUpdate.Website = updatedProgram.Website;

            updatedProgram.Audit.SetHistory(programToUpdate);

            SetGoals(updatedProgram.GoalIds, programToUpdate);
            SetPointOfContacts(updatedProgram.ContactIds, programToUpdate);
            SetThemes(updatedProgram.ThemeIds, programToUpdate);
            SetRegions(updatedProgram.RegionIds, programToUpdate);
        }
        #endregion

        private Organization AttachOrganization(int organization)
        {
            var org = new Organization { OrganizationId = organization };
            this.Context.Organizations.Attach(org);
            return org;
        }

        private Program AttachProgram(int programId)
        {
            var program = new Program { ProgramId = programId };
            this.Context.Programs.Attach(program);
            return program;
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

        /// <summary>
        /// Validates the given location types are all regions.
        /// </summary>
        /// <param name="locationTypeIds">The list of location types by id.</param>
        public void ValidateAllLocationsAreRegions(List<int> locationTypeIds)
        {
            List<int> typeIds;
            if (locationTypeIds == null)
            {
                typeIds = new List<int>();
            }
            else
            {
                typeIds = locationTypeIds.Distinct().ToList();
            }
            if (typeIds.Count != 1 || typeIds.First() != LocationType.Region.Id)
            {
                throw new ValidationException("The locations are not all regions.");
            }
        }

        /// <summary>
        /// Returns the location types for the given location ids.
        /// </summary>
        /// <param name="locationIds">The list of location ids.</param>
        /// <returns>The list of location type ids.</returns>
        public List<int> GetLocationTypeIds(List<int> locationIds)
        {
            return this.locationService.GetLocationTypeIds(locationIds);
        }

        /// <summary>
        /// Returns the location types for the given location ids.
        /// </summary>
        /// <param name="locationIds">The list of location ids.</param>
        /// <returns>The list of location type ids.</returns>
        public async Task<List<int>> GetLocationTypeIdsAsync(List<int> locationIds)
        {
            return await this.locationService.GetLocationTypeIdsAsync(locationIds);
        }

    }
}
