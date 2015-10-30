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

        public static SnapshotDTO CreateGetProgramRelatedProjectsCountQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "RELATED PROJECTS",
                DataValue = context.Programs
                                .Where(x => programIds.Contains(x.ProgramId))
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
        public static SnapshotDTO CreateGetProgramParticipantsCountQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "PARTICIPANTS",
                DataValue = context.Programs
                                .Where(x => programIds.Contains(x.ProgramId))
                                .Select(p => p.Projects.Where(d => d.EndDate.Value.Year >= oldestDate.Year))
                                                        .Sum(c => (int?)c.Sum(t => (int?)t.Participants.Count ?? 0) ?? 0)
            };
        }

        /// <summary>
        /// Total budget of all program projects
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static SnapshotDTO CreateGetProgramBudgetTotalQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "BUDGET",
                DataValue = (int)context.Programs
                                .Where(x => programIds.Contains(x.ProgramId))
                                .Sum(p => (decimal?)p.Projects.Sum(r => (decimal?)r.RecipientProjectMoneyFlows
                                                                .Where(m => m.MoneyFlowTypeId == MoneyFlowType.Incoming.Id
                                                                        && m.TransactionDate.Year >= oldestDate.Year 
                                                                        && m.Value > 0)
                                                                        .Sum(m => (decimal?)m.Value ?? 0) ?? 0) ?? 0)
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
        public static IQueryable<int> CreateGetProgramCountriesByProgramIdsQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var allLocations = LocationQueries.CreateGetLocationsQuery(context);
            
            var query = (from program in context.Programs
                         let regions = program.Regions
                         let locations = program.Locations

                         let countryByRegions = regions.Select(x => x.Country)
                         let countryByLocation = locations.Where(x => x.LocationTypeId == LocationType.Country.Id)
                         let allCountries = countryByLocation.Union(countryByRegions)

                         where programIds.Contains(program.ProgramId)
                         select allCountries).SelectMany(x => x).Select(i => i.LocationId).Distinct();

            return query;
        }

        public static SnapshotDTO CreateGetProgramBeneficiaryCountQuery(EcaContext context, IEnumerable<int> programIds)
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
        public static IQueryable<int> CreateGetProgramImpactStoryCountQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var query = context.Programs
                                .Where(x => programIds.Contains(x.ProgramId))
                                .Select(i => i).SelectMany(x => x.Impacts).Select(m => m.ImpactId).Distinct();
            return query;
        }

        /// <summary>
        /// Count of program alumni
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static SnapshotDTO CreateGetProgramAlumniCountQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "ALUMNI",
                DataValue = context.Programs
                                .Where(x => programIds.Contains(x.ProgramId))
                                .Select(p => p.Projects.Where(d => d.EndDate.Value.Year >= oldestDate.Year))
                                .Sum(c => (int?)c.Sum(t => (int?)t.Participants.Where(d => d.StatusDate.Value.Year >= oldestDate.Year)
                                                        .Count(s => s.ParticipantStatusId == ParticipantStatus.Alumnus.Id) ?? 0) ?? 0)
            };
        }

        /// <summary>
        /// Count of most prominent category in program projects
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static IQueryable<int> CreateGetProgramProminenceCountQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = (from pc in context.Programs
                            where programIds.Contains(pc.ProgramId)
                            from project in pc.Projects
                            where project.EndDate.Value.Year >= oldestDate.Year
                            from category in project.Categories
                            group category by category.CategoryName into g
                            select g).SelectMany(x => x).Select(c => c.CategoryId).Distinct();

            return query;            
        }

        /// <summary>
        /// Count of funding sources for all program projects
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static async Task<SnapshotGraphDTO> CreateGetProgramBudgetByYearQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var budgetByYear = await (from pc in context.Programs
                                      where programIds.Contains(pc.ProgramId)
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

        public static IEnumerable<SnapshotDTO> CreateGetProgramMostFundedCountriesQuery(EcaContext context, IEnumerable<int> programIds)
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
        public static async Task<IEnumerable<string>> CreateGetProgramTopThemesQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var programThemes = await context.Programs
                                        .Where(p => programIds.Contains(p.ProgramId))
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

        // TODO: needs new query
        public static async Task<IEnumerable<SnapshotDTO>> CreateGetProgramParticipantsByLocationQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var programProjects = await context.Projects
                                            .Where(p => programIds.Contains(p.ProgramId)).ToListAsync();

            List<SnapshotDTO> graphValues = new List<SnapshotDTO>();
            foreach (var project in programProjects)
            {
                foreach (var loc in project.Locations)
                {
                    graphValues.Add(new SnapshotDTO
                    {
                        DataLabel = loc.LocationIso,
                        DataValue = programProjects.Select(p => p.Participants.Select(x => x.ParticipantPerson.HomeInstitution.Addresses
                                                                                        .Select(a => a.LocationId == loc.LocationId))).Count()
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
        public static async Task<SnapshotGraphDTO> CreateGetProgramParticipantsByYearQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var pby = await context.Participants.Where(p => programIds.Contains(p.Project.ProgramId)
                                                        && p.Project.StartDate.Year >= oldestDate.Year)
                                                        .GroupBy(x => x.Project.StartDate.Year).ToListAsync();

            SnapshotGraphDTO graphValues = new SnapshotGraphDTO
            {
                key = "Participants",
                values = pby.Select(r => new KeyValuePair<int, int>(r.Key, r.Count())).OrderBy(p => p.Key).ToList()
            };

            return graphValues;
        }

        public static SnapshotGraphDTO CreateGetProgramParticipantGenderQuery(EcaContext context, IEnumerable<int> programIds)
        {
            // TODO: get participant genders
            throw new NotImplementedException();
        }

        public static SnapshotGraphDTO CreateGetProgramParticipantAgeQuery(EcaContext context, IEnumerable<int> programIds)
        {
            // TODO: get participant ages
            var ranges = new[] { 16, 25, 35, 50, 51 };

            throw new NotImplementedException();
        }

        public static SnapshotGraphDTO CreateGetProgramParticipantEducationQuery(EcaContext context, IEnumerable<int> programIds)
        {
            // TODO: get participant education
            throw new NotImplementedException();
        }


    }
    
}
