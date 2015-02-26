using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Admin
{
    public static class ProjectQueries
    {
        /// <summary>
        /// Returns a query to retrieve filtered and sorted simple project dtos for the given program id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="programId">The project's parent program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to retrieved filtered and sorted simple project dtos for the given program id.</returns>
        public static IQueryable<SimpleProjectDTO> CreateGetProjectsByProgramQuery(EcaContext context, int programId, QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");

            var query = from project in context.Projects
                        let parentProgram = project.ParentProgram
                        let locations = project.Locations
                        let status = project.Status
                        let startDate = project.StartDate
                        where project.ProgramId == programId
                        select new SimpleProjectDTO
                        {
                            ProgramId = parentProgram.ProgramId,
                            ProjectId = project.ProjectId,
                            ProjectName = project.Name,
                            LocationNames = locations.Select(x => x.LocationName),
                            ProjectStatusId = status.ProjectStatusId,
                            ProjectStatusName = status.Status,
                            StartDate = startDate,
                            StartYear = startDate.Year,
                            StartYearAsString = startDate.Year.ToString()
                        };

            query = query.Apply(queryOperator);
            return query;
        }
    }
}
