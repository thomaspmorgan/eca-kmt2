using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
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
    public class ThemeService : IDisposable, ECA.Business.Service.Programs.IThemeService
    {
        private EcaContext context;

        /// <summary>
        /// Creates a new ThemeService.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public ThemeService(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            this.context = context;
        }

        #region Get

        /// <summary>
        /// Returns paged, filtered, and sorted themes in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The themes in the system.</returns>
        public PagedQueryResults<ThemeDTO> GetThemes(QueryableOperator<ThemeDTO> queryOperator)
        {
            return ThemeQueries.CreateGetThemesQuery(this.context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns paged, filtered, and sorted themes in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The themes in the system.</returns>
        public Task<PagedQueryResults<ThemeDTO>> GetThemesAsync(QueryableOperator<ThemeDTO> queryOperator)
        {
            return ThemeQueries.CreateGetThemesQuery(this.context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
        }

        #endregion

        #region IDispose

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context.Dispose();
                this.context = null;
            }
        }

        #endregion
    }
}
