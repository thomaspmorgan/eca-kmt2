using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using ECA.Data;
using System.Linq;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Programs;

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
                DataValue = context.Projects.Count(x => x.ProgramId == programId && x.EndDate.Value.Year >= oldestDate.Year)
            };
        }

        public static SnapshotDTO CreateGetProgramParticipantsCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "PARTICIPANTS",
                DataValue = context.Participants.Join(context.Projects, part => part.ProjectId, proj => proj.ProjectId, (part, proj) => new { Participant = part, Project = proj }).Count(partProj => partProj.Project.ProgramId == programId && partProj.Participant.StatusDate.Value.Year >= oldestDate.Year)
            };
        }

        public static SnapshotDTO CreateGetProgramBudgetTotalQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "BUDGET",
                DataValue = (int)context.MoneyFlows.Join(context.Projects, mf => mf.SourceProjectId.Value, proj => proj.ProjectId, (mf, proj) => new { MoneyFlow = mf, Project = proj }).Where(mfProj => mfProj.Project.ProgramId == programId && mfProj.MoneyFlow.TransactionDate.Year >= oldestDate.Year).Select(b => b.MoneyFlow.Value).DefaultIfEmpty(0).Sum()
            };
        }

        public static SnapshotDTO CreateGetProgramFundingSourceCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "FUNDING SOURCES",
                DataValue = context.MoneyFlows.Join(context.Projects, mf => mf.SourceProjectId.Value, proj => proj.ProjectId, (mf, proj) => new { MoneyFlow = mf, Project = proj }).Count(mfProj => mfProj.Project.ProgramId == programId && mfProj.MoneyFlow.TransactionDate.Year >= oldestDate.Year)
            };
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static async Task<SnapshotDTO> CreateGetProgramCountryCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var allLocations = LocationQueries.CreateGetLocationsQuery(context);

            var query = from parents in context.Programs
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

                        where parents.ProgramId == programId &&
                              (program.ProgramId == programId || program.ParentProgramId == programId)
                        select new SnapshotDTO()
                        {
                            DataLabel = "COUNTRIES",
                            DataValue = countries.Count()
                        };

            return await query.FirstOrDefaultAsync();
        }

        public static SnapshotDTO CreateGetProgramBeneficiaryCountQuery(EcaContext context, int programId)
        {
            // TODO: Calculate beneficiary count
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "BENEFICIARIES",
                DataValue = 0
            };
        }

        public static SnapshotDTO CreateGetProgramImpactStoryCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "IMPACT STORIES",
                DataValue = context.Impacts.Count(i => i.ProgramId == programId && i.Project.EndDate.Value.Year >= oldestDate.Year)
            };
        }

        public static SnapshotDTO CreateGetProgramAlumniCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return new SnapshotDTO()
            {
                DataLabel = "ALUMNI",
                DataValue = context.Participants.Join(context.Projects, part => part.ProjectId, proj => proj.ProjectId, (part, proj) => new { Participant = part, Project = proj }).Count(partProj => partProj.Project.ProgramId == programId && partProj.Participant.ParticipantStatusId == ParticipantStatus.Alumnus.Id && partProj.Participant.StatusDate.Value.Year >= oldestDate.Year)
            };
        }

        public static SnapshotDTO CreateGetProgramProminenceCountQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var prominentCategory = from pc in context.Projects
                                    where pc.ProgramId == programId && pc.EndDate.Value.Year >= oldestDate.Year
                                    orderby pc.Categories.GroupBy(x => x.CategoryId).Count() descending
                                    select pc.Categories.GroupBy(x => x.CategoryId).Count();

            return new SnapshotDTO()
            {
                DataLabel = "PROMINENCE",
                DataValue = prominentCategory.FirstOrDefault()
            };
        }

        public static async Task<List<SnapshotDTO>> CreateGetProgramBudgetByYearQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var budgetByYear = await (from mf in context.MoneyFlows
                                where mf.RecipientProgramId == programId && mf.FiscalYear >= oldestDate.Year
                                group mf by mf.FiscalYear into g
                                select new SnapshotDTO()
                                {
                                    DataLabel = g.Key.ToString(),
                                    DataValue = g.ToList().Sum(m => (int)m.Value)
                                }).OrderByDescending(m => m.DataLabel).ToListAsync();

            return budgetByYear;
        }

        public static IEnumerable<SnapshotDTO> CreateGetProgramMostFundedCountriesQuery(EcaContext context, int programId)
        {
            throw new NotImplementedException();
        }

        public static async Task<IEnumerable<string>> CreateGetProgramTopThemesQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var allThemes = ThemeQueries.CreateGetThemesQuery(context);
            var projectThemes = allThemes.GroupBy(theme => theme.Name).OrderByDescending(group => group.Count()).Select(group => group.Key);

            return await projectThemes.ToListAsync();
        }

        public static async Task<IEnumerable<SnapshotDTO>> CreateGetProgramParticipantLocationsQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var ranges = new[] { 10, 20, 30, 40, 100 };
            var participantLocation = await context.Participants.Where(p => p.Project.ProgramId == programId).GroupBy(x => x.Project.Locations.Select(l => l.LocationId).FirstOrDefault()).Select(r => r).OrderByDescending(p => p.Key).ToListAsync();

            var locationGroups = ranges.Select(r => new SnapshotDTO()
            {
                DataLabel = r.ToString(),
                DataValue = participantLocation.Where(g => g.Key > r || g.Key == 0).Sum(g => g.Count())
            });

            return locationGroups.OrderByDescending(m => m.DataLabel).ToList();
        }

        public static async Task<IEnumerable<SnapshotDTO>> CreateGetProgramParticipantsByYearQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var pby = await context.Participants.Where(p => p.Project.ProgramId == programId && p.Project.StartDate.Year >= oldestDate.Year).GroupBy(x => x.Project.StartDate.Year).Select(r => new SnapshotDTO() { DataLabel = r.Key.ToString(), DataValue = r.Count() }).OrderByDescending(p => p.DataLabel).ToListAsync();
            return pby;
        }

        public static IEnumerable<SnapshotDTO> CreateGetProgramParticipantGenderQuery(EcaContext context, int programId)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<SnapshotDTO> CreateGetProgramParticipantAgeQuery(EcaContext context, int programId)
        {
            var ranges = new[] { 16, 25, 35, 50, 51 };

            throw new NotImplementedException();
        }

        public static IEnumerable<SnapshotDTO> CreateGetProgramParticipantEducationQuery(EcaContext context, int programId)
        {
            throw new NotImplementedException();
        }
        
    }
}
