using ECA.Business.Queries.Models.Programs;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

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
            var countryQuery = from country in context.Locations
                               where country.LocationTypeId == LocationType.Country.Id
                               select country;

            var query = from project in context.Projects
                        let regions = project.Regions
                        let countries = countryQuery.Where(x => regions.Select(y => y.LocationId).Contains(x.Region.LocationId))
                        where project.ProgramId == programId
                        select new ProgramSnapshotDTO
                        {
                            ProgramId = project.ProgramId,
                            Countries = countries.Distinct().Count()
                        };

            return query;
        }

    }
}
