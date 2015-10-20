using System;
using ECA.Business.Queries.Models.Programs;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using ECA.Business.Queries.Fundings;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Programs;
using ECA.Business.Service.Programs;

namespace ECA.Business.Queries.Admin
{
    public static class SnapshotQueries
    {
        private static ProgramService service;

        public static async Task<SnapshotDTO> CreateGetProgramCountryCountQuery(EcaContext context, int programId)
        {
            var allChildPrograms = await service.GetAllChildProgramsAsync(programId);
            var allLocations = await Task.Factory.StartNew(() => LocationQueries.CreateGetLocationsQuery(context));

            var query = from parent in allChildPrograms
                        from program in context.Programs

                        let regions = from location in allLocations
                                      join programRegion in program.Regions
                                      on location.Id equals programRegion.LocationId
                                      select location

                        let countries = from country in allLocations
                                        join region in regions
                                        on country.RegionId equals region.Id
                                        where country.LocationTypeId == LocationType.Country.Id
                                        select country

                        where program.ProgramId == programId
                        select new SnapshotDTO()
                        {
                            DataLabel = "COUNTRIES",
                            DataValue = countries.Count()
                        };

            return query.FirstOrDefault();
        }






        /// <summary>
        /// Get program snapshot
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="programId">Program ID</param>
        /// <returns>Program snapshot data</returns>
        public static IQueryable<ProgramSnapshotDTO> CreateGetProgramSnapshotDTOQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            DateTime oldestDate = DateTime.UtcNow.AddYears(-5);
            var ranges = new[] { 10, 20, 30, 40, 100 };
            
            var allLocations = LocationQueries.CreateGetLocationsQuery(context);

            var prominentCategory = from pc in context.Projects
                                    where pc.ProgramId == programId && pc.EndDate.Value.Year >= oldestDate.Year
                                    orderby pc.Categories.GroupBy(x => x.CategoryId).Count() descending
                                    select pc.Categories.GroupBy(x => x.CategoryId).Count();

            var allThemes = ThemeQueries.CreateGetThemesQuery(context);
            var programMoneyFlows = MoneyFlowQueries.CreateGetMoneyFlowDTOsByProgramId(context, programId);
            var projectThemes = allThemes.GroupBy(theme => theme.Name).OrderByDescending(group => group.Count()).Select(group => group.Key);
            var participantLocation = context.Participants.Where(p => p.Project.ProgramId == programId).GroupBy(x => x.Project.Locations.Select(l => l.LocationId).FirstOrDefault()).Select(r => r).OrderByDescending(p => p.Key).ToList();

            var locationGroups = ranges.Select(r => new SnapshotDTO()
            {
                DataLabel = r.ToString(),
                DataValue = participantLocation.Where(g => g.Key > r || g.Key == 0).Sum(g => g.Count())
            });
            
            var query = from program in context.Programs

                        let regions = from location in allLocations
                                      join programRegion in program.Regions
                                      on location.Id equals programRegion.LocationId
                                      select location

                        let countries = from country in allLocations
                                        join region in regions
                                        on country.RegionId equals region.Id
                                        where country.LocationTypeId == LocationType.Country.Id
                                        select country
                                        
                        let budgetByYear = (from mf in context.MoneyFlows
                                            where mf.RecipientProgramId == programId && mf.FiscalYear >= oldestDate.Year
                                            group mf by mf.FiscalYear into g
                                            select new SnapshotDTO()
                                            {
                                                DataLabel = g.Key.ToString(),
                                                DataValue = g.ToList().Sum(m => (int)m.Value)
                                            }).OrderByDescending(m => m.DataLabel).ToList()
                        
                        where program.ProgramId == programId
                        select new ProgramSnapshotDTO
                        {
                            ProgramId = program.ProgramId,
                            Countries = countries.Count(),
                            RelatedProjects = context.Projects.Count(x => x.ProgramId == program.ProgramId && x.EndDate.Value.Year >= oldestDate.Year),
                            Participants = context.Participants.Join(context.Projects, part => part.ProjectId, proj => proj.ProjectId, (part, proj) => new { Participant = part, Project = proj }).Count(partProj => partProj.Project.ProgramId == programId && partProj.Participant.StatusDate.Value.Year >= oldestDate.Year),
                            FundingSources = context.MoneyFlows.Join(context.Projects, mf => mf.SourceProjectId.Value, proj => proj.ProjectId, (mf, proj) => new { MoneyFlow = mf, Project = proj }).Count(mfProj => mfProj.Project.ProgramId == programId && mfProj.MoneyFlow.TransactionDate.Year >= oldestDate.Year),
                            Alumni = context.Participants.Join(context.Projects, part => part.ProjectId, proj => proj.ProjectId, (part, proj) => new { Participant = part, Project = proj }).Count(partProj => partProj.Project.ProgramId == programId && partProj.Participant.ParticipantStatusId == ParticipantStatus.Alumnus.Id && partProj.Participant.StatusDate.Value.Year >= oldestDate.Year),
                            Budget = context.MoneyFlows.Join(context.Projects, mf => mf.SourceProjectId.Value, proj => proj.ProjectId, (mf, proj) => new { MoneyFlow = mf, Project = proj }).Where(mfProj => mfProj.Project.ProgramId == programId && mfProj.MoneyFlow.TransactionDate.Year >= oldestDate.Year).Select(b => b.MoneyFlow.Value).DefaultIfEmpty(0).Sum(),
                            ImpactStories = context.Impacts.Count(i => i.ProgramId == program.ProgramId && i.Project.EndDate.Value.Year >= oldestDate.Year),
                            Beneficiaries = 0,
                            Prominence = prominentCategory.FirstOrDefault(),
                            TopThemes = projectThemes.ToList().Take(3),
                            BudgetByYear = budgetByYear,
                            ParticipantsByYear = context.Participants.Where(p => p.Project.ProgramId == programId && p.Project.StartDate.Year >= oldestDate.Year).GroupBy(x => x.Project.StartDate.Year).Select(r => new SnapshotDTO() { DataLabel = r.Key.ToString(), DataValue = r.Count() }).OrderByDescending(p => p.DataLabel).ToList().Take(5),
                            ParticipantLocations = locationGroups.OrderByDescending(m => m.DataLabel).ToList()
                        };

            return query;
        }

    }
}
