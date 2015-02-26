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
    }
}
