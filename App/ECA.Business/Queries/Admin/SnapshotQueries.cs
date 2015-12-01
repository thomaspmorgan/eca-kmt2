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
        // Time frame in years used for data lookup filter
        static DateTime oldestDate = DateTime.UtcNow.AddYears(-5);

        /// <summary>
        /// Count of related projects in all program projects
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programIds"></param>
        /// <returns></returns>
        public static SnapshotDTO CreateGetProgramRelatedProjectsCountQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "RELATED PROJECTS",
                DataValue = context.Programs
                            .Where(x => programIds.Contains(x.ProgramId))
                            .Select(p => p.Projects.Where(d => d.EndDate.Value.Year >= oldestDate.Year && d.Status.ProjectStatusId == ProjectStatus.Active.Id))
                                                    .Sum(r => (int?)r.Sum(t => (int?)t.RelatedProjects.Count ?? 0) ?? 0)
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
                                .Select(p => p.Projects.Where(d => d.EndDate.Value.Year >= oldestDate.Year && d.ProjectStatusId == ProjectStatus.Active.Id))
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
                                                                .Where(m => m.TransactionDate.Year >= oldestDate.Year
                                                                        && m.Value > 0 
                                                                        && m.MoneyFlowStatusId == MoneyFlowStatus.Appropriated.Id)
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
        public static IEnumerable<Location> CreateGetProgramCountriesByProgramIdsQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = (from program in context.Programs
                         let regions = program.Regions
                         let locations = program.Locations

                         let countryByRegions = regions.Where(c => c.LocationTypeId == LocationType.Country.Id).Select(x => x.Country)
                         let countryByLocation = locations.Where(x => x.LocationTypeId == LocationType.Country.Id)
                         let allCountries = countryByLocation.Union(countryByRegions)

                         where programIds.Contains(program.ProgramId)
                         select allCountries).SelectMany(x => x);
                        
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
        public static IEnumerable<int> CreateGetProgramImpactStoryCountQuery(EcaContext context, IEnumerable<int> programIds)
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
                                .Select(p => p.Projects.Where(d => d.EndDate.Value.Year >= oldestDate.Year 
                                                                && d.ProjectStatusId == ProjectStatus.Active.Id))
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
        public static IEnumerable<int> CreateGetProgramProminenceCountQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = (from pc in context.Programs
                            where programIds.Contains(pc.ProgramId)
                            from project in pc.Projects
                            where project.EndDate.Value.Year >= oldestDate.Year
                                && project.ProjectStatusId == ProjectStatus.Active.Id
                         from category in project.Categories
                            group category by category.CategoryName into g
                            select g).SelectMany(x => x).Select(c => c.CategoryId).Distinct();

            return query;
        }

        #region Graph queries

        /// <summary>
        /// Count of funding sources for all program projects
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static async Task<List<KeyValuePair<int, int>>> CreateGetProgramBudgetByYearQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var budgetData = await (from pc in context.Programs
                                      where programIds.Contains(pc.ProgramId)
                                        && pc.ProgramStatusId == ProgramStatus.Active.Id
                                    from project in pc.Projects
                                      where project.EndDate.Value.Year >= oldestDate.Year
                                        && project.ProjectStatusId == ProjectStatus.Active.Id
                                      from mf in project.RecipientProjectMoneyFlows
                                      where mf.FiscalYear >= oldestDate.Year
                                       && mf.MoneyFlowStatusId == MoneyFlowStatus.Appropriated.Id
                                    select new {Key = mf.FiscalYear, Value = mf.Value}).ToListAsync();

            var budgetByYear = budgetData.GroupBy(g => g.Key, g => g.Value)
                                        .Select(y => new KeyValuePair<int, int>(y.Key, (int)y.Sum())).ToList();

            return budgetByYear;
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
                                        .Where(p => programIds.Contains(p.ProgramId) 
                                            && p.ProgramStatusId == ProgramStatus.Active.Id)
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

        /// <summary>
        /// Get participants by location for map graph
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programIds"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<SnapshotDTO>> CreateGetProgramParticipantsByLocationQuery(EcaContext context, IEnumerable<int> programIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var progPartic = await context.Participants.Where(p => programIds.Contains(p.Project.ProgramId)
                                                                && p.Project.StartDate.Year >= oldestDate.Year
                                                                && p.Project.ProjectStatusId == ProjectStatus.Active.Id)
                                                        .SelectMany(p => p.Person.Addresses.Where(a => a.IsPrimary == true)).ToListAsync();
            
            var graphValues = (from address in progPartic
                               group address by address.Location.Country.LocationIso into g
                                select new SnapshotDTO
                                {
                                    DataLabel = g.Key,
                                    DataValue = g.Count()
                                }).ToList();
            
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
                                                        && p.Project.StartDate.Year >= oldestDate.Year
                                                        && p.Project.ProjectStatusId == ProjectStatus.Active.Id)
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

        #endregion

    }

}
