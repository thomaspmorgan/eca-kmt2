using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using ECA.Data;
using System.Linq;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq.Filter;
using ECA.Business.Queries.Fundings;

namespace ECA.Business.Queries.Admin
{
    public static class SnapshotQueries
    {
        static DateTime oldestDate = DateTime.UtcNow.AddYears(-5);

        public static SnapshotDTO CreateGetProgramRelatedProjectsCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "RELATED PROJECTS",
                DataValue = context.Programs.Include("ChildPrograms")
                                .Where(x => x.ProgramId == programId || x.ParentProgramId == programId)
                                .Sum(p => p.Projects.Where(d => d.EndDate.Value.Year >= oldestDate.Year)
                                                                .Select(r => r.RelatedProjects).Count())
            };
        }

        /// <summary>
        /// Count of participants in all program projects
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static SnapshotDTO CreateGetProgramParticipantsCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "PARTICIPANTS",
                DataValue = context.Programs.Include("ChildPrograms")
                                .Where(x => x.ProgramId == programId || x.ParentProgramId == programId)
                                .Select(p => p.Projects.Where(d => d.EndDate.Value.Year >= oldestDate.Year))
                                .Sum(c => c.Select(t => t.Participants.Count).Sum())
            };
        }

        /// <summary>
        /// Total budget of all program projects
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static SnapshotDTO CreateGetProgramBudgetTotalQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "BUDGET",
                DataValue = (int)context.Programs.Include("ChildPrograms")
                                .Where(x => x.ProgramId == programId || x.ParentProgramId == programId)
                                .Sum(p => p.Projects.Select(r => r.RecipientProjectMoneyFlows
                                                                .Where(m => m.MoneyFlowTypeId == MoneyFlowType.Incoming.Id
                                                                        && m.TransactionDate.Year >= oldestDate.Year)
                                                                        .Select(m => m.Value).Sum()).Sum())
            };
        }

        /// <summary>
        /// Count of funding sources of all program projects
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static SnapshotDTO CreateGetProgramFundingSourceCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = MoneyFlowQueries.CreateGetSourceMoneyFlowDTOsByProgramId(context, programId);
  
            return new SnapshotDTO()
            {
                DataLabel = "FUNDING SOURCES",
                DataValue = query.Count(x => x.FiscalYear >= oldestDate.Year - 1)
            };
        }
        
        /// <summary>
        /// Count of location countries in all program projects
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static async Task<SnapshotDTO> CreateGetProgramCountryCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var allLocations = LocationQueries.CreateGetLocationsQuery(context);

            var query = from program in context.Programs.Include("ChildPrograms")

                        let regions = from location in allLocations
                                      join programRegion in program.Regions
                                      on location.Id equals programRegion.LocationId
                                      select location

                        let countries = from country in allLocations
                                        join region in regions
                                        on country.RegionId equals region.Id
                                        where country.LocationTypeId == LocationType.Country.Id
                                        select country

                        let countryCount = countries.Count()

                        where program.ProgramId == programId || program.ParentProgramId == programId
                        select new SnapshotDTO()
                        {
                            DataLabel = "COUNTRIES",
                            DataValue = countryCount
                        };

            return await query.FirstOrDefaultAsync();
        }

        public static SnapshotDTO CreateGetProgramBeneficiaryCountQuery(EcaContext context, int programId)
        {
            // TODO: get beneficiary count
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "BENEFICIARIES",
                DataValue = 0
            };
        }

        /// <summary>
        /// Count of impacts in a program
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static SnapshotDTO CreateGetProgramImpactStoryCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "IMPACT STORIES",
                DataValue = context.Programs.Include("ChildPrograms")
                                .Where(x => x.ProgramId == programId || x.ParentProgramId == programId)
                                .Select(i => i.Impacts).Count()
            };
        }

        /// <summary>
        /// Count of program alumni
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static SnapshotDTO CreateGetProgramAlumniCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "ALUMNI",
                DataValue = context.Programs.Include("ChildPrograms")
                                .Where(x => x.ProgramId == programId || x.ParentProgramId == programId)
                                .Select(p => p.Projects.Where(d => d.EndDate.Value.Year >= oldestDate.Year))
                                .Sum(c => c.Select(t => t.Participants.Count(s => s.ParticipantStatusId == ParticipantStatus.Alumnus.Id
                                                                                && s.StatusDate.Value.Year >= oldestDate.Year)).Sum())
            };
        }

        /// <summary>
        /// Count of most prominent category in program projects
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static SnapshotDTO CreateGetProgramProminenceCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var prominentCategory = from pc in context.Programs.Include("ChildPrograms")
                                    where pc.ProgramId == programId || pc.ParentProgramId == programId
                                    from project in pc.Projects
                                    where project.EndDate.Value.Year >= oldestDate.Year
                                    from category in project.Categories
                                    group category by category.CategoryName into g
                                    orderby g.Count() descending
                                    select g.Count();

            return new SnapshotDTO()
            {
                DataLabel = "PROMINENCE",
                DataValue = prominentCategory.FirstOrDefault()
            };
        }

        /// <summary>
        /// Count of funding sources for all program projects
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static async Task<SnapshotGraphDTO> CreateGetProgramBudgetByYearQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var budgetByYear = await (from pc in context.Programs.Include("ChildPrograms")
                                      where pc.ProgramId == programId || pc.ParentProgramId == programId
                                      from project in pc.Projects
                                      where project.EndDate.Value.Year >= oldestDate.Year
                                      from mf in project.RecipientProjectMoneyFlows
                                      where mf.FiscalYear >= oldestDate.Year && mf.MoneyFlowTypeId == MoneyFlowType.Incoming.Id
                                      group mf by mf.FiscalYear into g
                                      select g).ToListAsync();

            SnapshotGraphDTO graphValues = new SnapshotGraphDTO
            {
                key = "Budget",
                values = budgetByYear.Select(g => new KeyValuePair<int, int>(g.Key, g.ToList()
                                .Sum(m => (int)m.Value)))
                                .OrderBy(o => o.Key).ToList()
            };

            return graphValues;
        }

        public static IEnumerable<SnapshotDTO> CreateGetProgramMostFundedCountriesQuery(EcaContext context, int programId)
        {
            // TODO: get most funded countries
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get top five themes used by program projects
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> CreateGetProgramTopThemesQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var programThemes = await context.Programs.Include("ChildPrograms")
                                        .Where(p => p.ProgramId == programId || p.ParentProgramId == programId)
                                        .Select(t => t.Themes.GroupBy(n => n.ThemeId)).ToListAsync();
            List<string> allThemes = new List<string>();

            foreach (var themes in programThemes)
            {
                foreach (var themeGroup in themes)
                {
                    foreach (var group in themeGroup)
                    {
                        allThemes.Add(group.ThemeName);
                    }
                }
            }

            var topThemes = allThemes.GroupBy(theme => theme)
                                    .Select(g => new
                                    {
                                        themeName = g.Key,
                                        themeCount = g.Count()
                                    }).OrderByDescending(x => x.themeCount);

            return topThemes.Select(x => x.themeName).Take(5);
        }

        public static async Task<IEnumerable<SnapshotDTO>> CreateGetProgramParticipantsByLocationQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var programProjects = await context.Projects.Include("ChildPrograms").Where(p => p.ProgramId == programId).ToListAsync();

            List<SnapshotDTO> graphValues = new List<SnapshotDTO>();
            foreach (var project in programProjects)
            {
                foreach (var loc in project.Locations)
                {
                    graphValues.Add(new SnapshotDTO
                    {
                        DataLabel = loc.LocationIso,
                        DataValue = programProjects.Select(p => p.Participants.Select(x => x.ParticipantPerson.HomeInstitution.Addresses.Select(a => a.LocationId == loc.LocationId))).Count()
                    });
                }
            }

            return graphValues;
        }

        /// <summary>
        /// Count of participants by year for graph
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static async Task<SnapshotGraphDTO> CreateGetProgramParticipantsByYearQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var pby = await context.Participants.Where(p => p.Project.ProgramId == programId 
                                                        && p.Project.StartDate.Year >= oldestDate.Year)
                                                        .GroupBy(x => x.Project.StartDate.Year).ToListAsync();
            SnapshotGraphDTO graphValues = new SnapshotGraphDTO
            {
                key = "Participants",
                values = pby.Select(r => new KeyValuePair<int, int>(r.Key, r.Count())).OrderBy(p => p.Key).ToList()
            };

            return graphValues;
        }

        public static SnapshotGraphDTO CreateGetProgramParticipantGenderQuery(EcaContext context, int programId)
        {
            // TODO: get participant genders
            throw new NotImplementedException();
        }

        public static SnapshotGraphDTO CreateGetProgramParticipantAgeQuery(EcaContext context, int programId)
        {
            // TODO: get participant ages
            var ranges = new[] { 16, 25, 35, 50, 51 };

            throw new NotImplementedException();
        }

        public static SnapshotGraphDTO CreateGetProgramParticipantEducationQuery(EcaContext context, int programId)
        {
            // TODO: get participant education
            throw new NotImplementedException();
        }


    }
    
}
