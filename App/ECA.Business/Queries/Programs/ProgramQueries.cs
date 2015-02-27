using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Programs
{
    public static class ProgramQueries
    {
        /// <summary>
        /// Creates a query to return filtered and sorted simple program dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to return filtered and sorted simple program dtos.</returns>
        public static IQueryable<SimpleProgramDTO> CreateGetSimpleProgramDTOsQuery(EcaContext context, QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = context.Programs.Select(x => new SimpleProgramDTO
            {
                Description = x.Description,
                Name = x.Name,
                OwnerId = x.Owner.OrganizationId,
                ProgramId = x.ProgramId
            });
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a query to locate programs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public static IQueryable<EcaProgram> CreateGetPublishedProgramByIdQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetPublishedProgramsQuery(context).Where(x => x.Id == programId);
        }

        private static IQueryable<EcaProgram> CreateGetPublishedProgramsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var countryQuery = from country in context.Locations
                               where country.LocationTypeId == LocationType.Country.Id
                               select country;

            var query = from program in context.Programs
                        let themes = program.Themes
                        let regions = program.Regions                        
                        let parentProgram = program.ParentProgram
                        let goals = program.Goals
                        let contacts = program.Contacts
                        let countries = countryQuery.Where(x => regions.Select(y => y.LocationId).Contains(x.Region.LocationId))

                        select new EcaProgram
                        {
                            ContactIds = contacts.Select(x => x.ContactId),
                            CountryIsos = countries.Select(x => x.LocationIso),
                            CountryIds = countries.Select(x => x.LocationId),
                            Description = program.Description,
                            GoalIds = goals.Select(x => x.GoalId),
                            Id = program.ProgramId,
                            Name = program.Name,
                            ParentProgramId = parentProgram == null ? default(int?) : parentProgram.ProgramId,
                            RevisedOn = program.History.RevisedOn,
                            StartDate = program.StartDate,
                            ThemeIds = themes.Select(x => x.ThemeId)
                        };
            return query;
        }


    }
}
