using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using ECA.Business.Service.Admin;
using ECA.Business.Service;
using ECA.Business.Queries.Admin;

namespace ECA.Business.Queries.Programs
{
    /// <summary>
    /// Contains queries for retrieving program dto's from the eca context.
    /// </summary>
    public static class ProgramQueries
    {
        /// <summary>
        /// Returns a query to locate a program by id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="programId">The program id.</param>
        /// <returns>The program.</returns>
        public static IQueryable<ProgramDTO> CreateGetPublishedProgramByIdQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetPublishedProgramsQuery(context).Where(x => x.Id == programId);
        }

        public static IQueryable<OrganizationProgramDTO> CreateGetSubprogramsQuery(EcaContext context, int programId, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetOrganizationProgramDTOQuery(context, queryOperator).Where(x => x.ParentProgram_ProgramId == programId);
        }

        /// <summary>
        /// Returns a query for retrieving organization program dto's in non-hierarchy collection.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query.</returns>
        public static IQueryable<OrganizationProgramDTO> CreateGetOrganizationProgramDTOQuery(EcaContext context, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = from program in context.Programs
                        let owner = program.Owner
                        let parentProgram = program.ParentProgram
                        select new OrganizationProgramDTO
                        {
                            CreatedByUserId = program.History.CreatedBy,
                            Description = program.Description,
                            Name = program.Name,
                            NumChildren = 0,
                            OfficeSymbol = owner.OfficeSymbol,
                            Status = program.ProgramStatus.Status,
                            OrgName = owner.Name,
                            Owner_OrganizationId = owner.OrganizationId,
                            ParentProgram_ProgramId = parentProgram == null ? default(int?) : parentProgram.ProgramId,
                            ProgramId = program.ProgramId,
                            ProgramLevel = 0,
                            ProgramStatusId = program.ProgramStatusId,
                            SortOrder = 0
                        };
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns an EcaProgram query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The EcaProgram query.</returns>
        public static IQueryable<ProgramDTO> CreateGetPublishedProgramsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var regionTypeId = LocationType.Region.Id;
            var countryTypeId = LocationType.Country.Id;
            var allLocations = LocationQueries.CreateGetLocationsQuery(context);

            var countryQuery = from country in context.Locations
                               where country.LocationTypeId == LocationType.Country.Id
                               select country;

            var query = from program in context.Programs
                        let owner = program.Owner
                        let themes = program.Themes                        
                        let parentProgram = program.ParentProgram
                        let goals = program.Goals
                        let contacts = program.Contacts
                        let categories = program.Categories
                        let objectives = program.Objectives
                        let status = program.ProgramStatus
                        let websites = program.Websites

                        let regions = from location in allLocations
                                      join programRegion in program.Regions
                                      on location.Id equals programRegion.LocationId
                                      select location

                        let countries = from country in allLocations
                                        join region in regions
                                        on country.RegionId equals region.Id
                                        where country.LocationTypeId == countryTypeId
                                        select country

                        join officeSetting in context.OfficeSettings
                        on owner equals officeSetting.Office into focusSetting 
                        from tempFocusSetting in focusSetting.Where(x => x.Name == OfficeSetting.FOCUS_SETTING_KEY).DefaultIfEmpty()

                        join officeSetting in context.OfficeSettings
                        on owner equals officeSetting.Office into objectiveSetting
                        from tempObjectiveSetting in objectiveSetting.Where(x => x.Name == OfficeSetting.OBJECTIVE_SETTING_KEY).DefaultIfEmpty()

                        select new ProgramDTO
                        {
                            Contacts = contacts.Select(x => new SimpleLookupDTO { Id = x.ContactId, Value = x.FullName + " (" + x.Position + ")" }),
                            CountryIsos = countries.Select(x => new SimpleLookupDTO { Id = x.Id, Value = x.LocationIso }),
                            Categories = categories.Select(c => new FocusCategoryDTO { Id = c.CategoryId, Name = c.CategoryName, FocusName = c.Focus.FocusName }),
                            Description = program.Description,
                            EndDate = program.EndDate,                            
                            Goals = goals.Select(x => new SimpleLookupDTO { Id = x.GoalId, Value = x.GoalName }),
                            Id = program.ProgramId,
                            Objectives = objectives.Select(o => new JustificationObjectiveDTO { Id = o.ObjectiveId, Name = o.ObjectiveName, JustificationName = o.Justification.JustificationName }),
                            Name = program.Name,
                            OwnerDescription = owner.Description,
                            OwnerName = owner.Name,
                            OwnerOfficeSymbol = owner.OfficeSymbol,
                            OwnerOrganizationId = owner.OrganizationId,
                            OwnerOrganizationCategoryLabel = tempFocusSetting == null ? OfficeSettings.CATEGORY_DEFAULT_LABEL : tempFocusSetting.Value,
                            OwnerOrganizationObjectiveLabel = tempObjectiveSetting == null ? OfficeSettings.OBJECTIVE_DEFAULT_LABEL : tempObjectiveSetting.Value,
                            ParentProgramId = parentProgram == null ? default(int?) : parentProgram.ProgramId,
                            ParentProgramName = parentProgram == null ? null : parentProgram.Name,
                            RevisedOn = program.History.RevisedOn,
                            RegionIsos = regions.Select(x => new SimpleLookupDTO { Id = x.Id, Value = x.LocationIso }),
                            Regions = regions,
                            RowVersion = program.RowVersion,
                            StartDate = program.StartDate,
                            Themes = themes.Select(x => new SimpleLookupDTO { Id = x.ThemeId, Value = x.ThemeName }),
                            ProgramStatusId = program.ProgramStatusId,
                            Websites = websites.Select(x => new SimpleLookupDTO { Id = x.WebsiteId, Value = x.WebsiteValue }),
                            ProgramStatusName = status.Status
                        };
            return query;
        }


    }
}
