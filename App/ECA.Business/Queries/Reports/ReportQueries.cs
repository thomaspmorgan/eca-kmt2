using ECA.Business.Queries.Models.Reports;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Programs
{
    public static class ReportQueries
    {
        /// <summary>
        /// Creates a query to return a list of projects and the dollar award.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to return filtered and sorted simple program dtos.</returns>
        public static IQueryable<ProjectAwardDTO> CreateGetProjectAward(EcaContext context, int programId, int countryId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = context.Projects.Where(p => p.ProgramId == programId  && p.Locations.FirstOrDefault().CountryId == countryId).Select(x => new ProjectAwardDTO
            {
                Title = x.Name,
                Summary = x.Description,
                Year = x.StartDate.Year,
                Award = x.RecipientProjectMoneyFlows.Sum(p => p.Value)
            }).OrderByDescending(p => p.Year).ThenBy(p => p.Title);
            return query;
        }

        public static string CreateGetProgramName(EcaContext context, int programId)
        {
            return context.Programs.Find(programId).Name;
        }

        public static string CreateGetCountryName(EcaContext context, int countryId)
        {
            return context.Locations.Find(countryId).LocationName;
        }

        public static IQueryable<RegionAwardDTO> CreateGetRegionAwards(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var result = (from m in context.MoneyFlows where m.RecipientProject.ProgramId == programId
                             group m by new {m.RecipientProject.Locations.FirstOrDefault().Region.LocationName}
                             into grp
                             select new RegionAwardDTO
                             {
                                 Region = grp.Key.LocationName,
                                 Projects = grp.Count(),
                                 ProgramValue = grp.Where(m => m.SourceProgramId == programId).Sum(m => m.Value),
                                 OtherValue = grp.Where(m => m.SourceProgramId == null || m.SourceProgramId != programId).Sum(m => m.Value),
                                 Average = grp.Average(m => m.Value),
                             });
            return result;            
        }
        
    }
}
