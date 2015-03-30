using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Logging;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// The ThemeService is capable of performing crud operations on Themes with the system using entity framework.
    /// </summary>
    public class ThemeService : LookupService<ThemeDTO>, ECA.Business.Service.Lookup.IThemeService
    {
        /// <summary>
        /// Creates a new ThemeService.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="logger">The logger.</param>
        public ThemeService(EcaContext context, ILogger logger) : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
        }

        #region Get
        /// <summary>
        /// Returns a query to get dtos.
        /// </summary>
        /// <returns>The query to get theme dtos.</returns>
        protected override IQueryable<ThemeDTO> GetSelectDTOQuery()
        {
            return ThemeQueries.CreateGetThemesQuery(this.Context);
        }
        #endregion

        
    }
}
