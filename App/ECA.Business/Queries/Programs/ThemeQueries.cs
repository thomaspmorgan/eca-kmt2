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
        /// Creates a query to retrieved theme dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query.</returns>
        public static IQueryable<ThemeDTO> CreateGetThemesQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = context.Themes.Select(x => new ThemeDTO
            {
                Id = x.ThemeId,
                Name = x.ThemeName
            });
            return query;
        }
    }
}
