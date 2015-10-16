using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using ECA.Business.Queries.Models.Programs;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Reports;
using ECA.Business.Queries.Programs;

namespace ECA.Business.Queries.Admin
{
    public static class SnapshotQueries
    {
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
            // range grouping: stackoverflow.com/questions/13828216/group-by-range-using-linq

            var allLocations = LocationQueries.CreateGetLocationsQuery(context);

            var prominentCategory = from pc in context.Projects
                                    where pc.ProgramId == programId && pc.EndDate >= oldestDate
                                    orderby pc.Categories.GroupBy(x => x.CategoryId).Count() descending
                                    select pc.Categories.GroupBy(x => x.CategoryId).Count();

            var allThemes = ThemeQueries.CreateGetThemesQuery(context);

            var projectThemes = allThemes.GroupBy(theme => theme.Name).OrderByDescending(group => group.Count()).Select(group => group.Key);

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

                        where program.ProgramId == programId
                        select new ProgramSnapshotDTO
                        {
                            ProgramId = program.ProgramId,
                            Countries = countries.Count(),
                            RelatedProjects = context.Projects.Count(x => x.ProgramId == program.ProgramId && x.EndDate >= oldestDate),
                            Participants = context.Participants.Join(context.Projects, part => part.ProjectId, proj => proj.ProjectId, (part, proj) => new { Participant = part, Project = proj }).Count(partProj => partProj.Project.ProgramId == programId && partProj.Participant.StatusDate >= oldestDate),
                            FundingSources = context.MoneyFlows.Join(context.Projects, mf => mf.SourceProjectId.Value, proj => proj.ProjectId, (mf, proj) => new { MoneyFlow = mf, Project = proj }).Count(mfProj => mfProj.Project.ProgramId == programId && mfProj.MoneyFlow.TransactionDate >= oldestDate),
                            Alumni = context.Participants.Join(context.Projects, part => part.ProjectId, proj => proj.ProjectId, (part, proj) => new { Participant = part, Project = proj }).Count(partProj => partProj.Project.ProgramId == programId && partProj.Participant.ParticipantStatusId == ParticipantStatus.Alumnus.Id && partProj.Participant.StatusDate >= oldestDate),
                            Budget = context.MoneyFlows.Join(context.Projects, mf => mf.SourceProjectId.Value, proj => proj.ProjectId, (mf, proj) => new { MoneyFlow = mf, Project = proj }).Where(mfProj => mfProj.Project.ProgramId == programId && mfProj.MoneyFlow.TransactionDate >= oldestDate).Select(b => b.MoneyFlow.Value).DefaultIfEmpty(0).Sum(),
                            ImpactStories = context.Impacts.Count(i => i.ProgramId == program.ProgramId && i.Project.EndDate >= oldestDate),
                            Beneficiaries = 0,
                            Prominence = prominentCategory.FirstOrDefault(),
                            TopThemes = projectThemes.ToList().Take(3)
                        };
            return query;
        }

    }
}
