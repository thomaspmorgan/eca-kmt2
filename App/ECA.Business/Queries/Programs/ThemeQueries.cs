using ECA.Business.Queries.Models.Programs;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Programs
{
    /// <summary>
    /// Contains queries for themes.
    /// </summary>
    public static class ThemeQueries
    {
        /// <summary>
        /// Creates a query to retrieved filtered and sorted themes.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to retrieve filtered and sorted themes.</returns>
        public static IQueryable<ThemeDTO> CreateGetThemesQuery(EcaContext context, QueryableOperator<ThemeDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = context.Themes.Select(x => new ThemeDTO
            {
                Id = x.ThemeId,
                Name = x.ThemeName
            });
            query = query.Apply(queryOperator);
            return query;
        }
    }
}
