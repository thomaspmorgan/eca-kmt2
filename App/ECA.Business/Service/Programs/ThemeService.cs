using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// The ThemeService is capable of performing crud operations on Themes with the system using entity framework.
    /// </summary>
    public class ThemeService : DbContextService<EcaContext>, ECA.Business.Service.Programs.IThemeService
    {
        /// <summary>
        /// Creates a new ThemeService.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public ThemeService(EcaContext context) : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns paged, filtered, and sorted themes in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The themes in the system.</returns>
        public PagedQueryResults<ThemeDTO> GetThemes(QueryableOperator<ThemeDTO> queryOperator)
        {
            return ThemeQueries.CreateGetThemesQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns paged, filtered, and sorted themes in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The themes in the system.</returns>
        public Task<PagedQueryResults<ThemeDTO>> GetThemesAsync(QueryableOperator<ThemeDTO> queryOperator)
        {
            return ThemeQueries.CreateGetThemesQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
        }

        #endregion
    }
}
