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

        public static IQueryable<RegionAwardDTO> CreateGetRegionAward(EcaContext context, int programId)
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

        public static IQueryable<CountryAwardDTO> CreateGetCountryAward(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var result = (from p in context.Locations
                          from m in context.MoneyFlows
                          where p.CountryId == m.RecipientProject.Locations.FirstOrDefault().CountryId
                          && m.RecipientProject.ProgramId == programId
                          && p.LocationTypeId == LocationType.Post.Id
                          group new { p, m } by new { Country = p.Country.LocationName, Post = p.LocationName, Region = p.Region.LocationIso }
                              into grp
                              orderby grp.Key.Country
                              select new CountryAwardDTO
                              {
                                  Country = grp.Key.Country,
                                  Post = grp.Key.Post,
                                  Region = grp.Key.Region,
                                  Projects = grp.Count(),
                                  ProgramValue = grp.Where(g => g.m.SourceProgramId == programId).Sum(g => g.m.Value),
                                  OtherValue = grp.Where(g => g.m.SourceProgramId == null || g.m.SourceProgramId != programId).Sum(g => g.m.Value),
                                  Average = grp.Average(g => g.m.Value)
                              });
            return result;
        }


        public static IQueryable<PostAwardDTO> CreateGetPostAward(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var result = (from p in context.Locations 
                          from m in context.MoneyFlows 
                          where p.CountryId == m.RecipientProject.Locations.FirstOrDefault().CountryId 
                          && m.RecipientProject.ProgramId == programId
                          && p.LocationTypeId == LocationType.Post.Id
                          group new {p, m} by new { Post = p.LocationName, Region = p.Region.LocationName}
                              into grp
                              orderby grp.Key.Post
                              select new PostAwardDTO
                              {
                                  Post = grp.Key.Post,
                                  Region = grp.Key.Region,
                                  Projects = grp.Count(),
                                  ProgramValue = grp.Where(g => g.m.SourceProgramId == programId).Sum(g => g.m.Value),
                                  OtherValue = grp.Where(g => g.m.SourceProgramId == null || g.m.SourceProgramId != programId).Sum(g => g.m.Value),
                                  Average = grp.Average(g => g.m.Value)
                              });
            return result;
        }


        public static IQueryable<FocusAwardDTO> CreateGetFocusAward(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var result = (from m in context.MoneyFlows
                          where m.RecipientProject.ProgramId == programId
                          group m by new { Focus = m.RecipientProject.Categories.FirstOrDefault().Focus.FocusName}
                              into grp
                              orderby grp.Key.Focus
                              select new FocusAwardDTO
                              {
                                  Focus = grp.Key.Focus,
                                  Projects = grp.Count(),
                                  ProgramValue = grp.Where(m => m.SourceProgramId == programId).Sum(m => m.Value),
                                  OtherValue = grp.Where(m => m.SourceProgramId == null || m.SourceProgramId != programId).Sum(m => m.Value),
                                  Average = grp.Average(m => m.Value)
                              });
            return result;
        }


        public static IQueryable<FocusCategoryAwardDTO> CreateGetFocusCategoryAward(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var result = (from m in context.MoneyFlows
                          where m.RecipientProject.ProgramId == programId
                          group m by new { Focus = m.RecipientProject.Categories.FirstOrDefault().Focus.FocusName, Category = m.RecipientProject.Categories.FirstOrDefault().CategoryName }
                              into grp
                              orderby grp.Key.Focus, grp.Key.Category
                              select new FocusCategoryAwardDTO
                              {
                                  Focus = grp.Key.Focus,
                                  Category = grp.Key.Category,
                                  Projects = grp.Count(),
                                  ProgramValue = grp.Where(m => m.SourceProgramId == programId).Sum(m => m.Value),
                                  OtherValue = grp.Where(m => m.SourceProgramId == null || m.SourceProgramId != programId).Sum(m => m.Value),
                                  
                              });
            return result;
        }

        public static IQueryable<ObjectiveAwardDTO> CreateGetObjectiveAward(EcaContext context, int programId, int objectiveId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var result = (from m in context.MoneyFlows
                          from o in context.Objectives
                          where m.RecipientProject.ProgramId == programId && m.RecipientProject.Objectives.Any(p => p.ObjectiveId == objectiveId)
                          group m by new { Project = m.RecipientProject.Name, 
                                           Objective = m.RecipientProject.Objectives.Where(p => p.ObjectiveId == objectiveId).FirstOrDefault().ObjectiveName,
                                           Country = m.RecipientProject.Locations.FirstOrDefault().Country.LocationName}
                                  into grp
                                  orderby grp.Key.Objective, grp.Key.Country, grp.Key.Project
                              select new ObjectiveAwardDTO
                              {
                                  Objective = grp.Key.Objective,
                                  Country = grp.Key.Country,
                                  Project =grp.Key.Project,
                                  ProgramValue = grp.Where(m => m.SourceProgramId == programId).Sum(m => m.Value),
                                  OtherValue = grp.Where(m => m.SourceProgramId == null || m.SourceProgramId != programId).Sum(m => m.Value),
                                  Year = grp.FirstOrDefault().RecipientProject.StartDate.Year
                              });
            return result;
        }


        public static IQueryable<YearAwardDTO> CreateGetYearAward(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var result = (from m in context.MoneyFlows
                          where m.RecipientProject.ProgramId == programId
                          group m by new
                          {
                              Year = m.RecipientProject.StartDate.Year,
                          }
                              into grp
                              orderby grp.Key.Year
                              select new YearAwardDTO
                              {
                                  Year = grp.Key.Year,
                                  ProgramValue = grp.Where(m => m.SourceProgramId == programId).Sum(m => m.Value),
                                  OtherValue = grp.Where(m => m.SourceProgramId == null || m.SourceProgramId != programId).Sum(m => m.Value),
                                  Projects = grp.Count(),
                                  Average = grp.Average(m => m.Value)
                              });
            return result;
        }
    }
}
