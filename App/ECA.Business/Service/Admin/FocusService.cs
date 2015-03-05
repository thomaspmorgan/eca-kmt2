using ECA.Business.Queries.Admin;
using System.Data.Entity;
using ECA.Business.Queries.Models.Admin;
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
using ECA.Core.Logging;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The FocusService is capable of performing crud operations on ECA Foci.
    /// </summary>
    public class FocusService : DbContextService<EcaContext>, ECA.Business.Service.Admin.IFocusService
    {
        /// <summary>
        /// Creates a new FocusSerivce with the given context.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public FocusService(EcaContext context, ILogger logger)
            : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
        }

        #region Get
        /// <summary>
        /// Returns the foci currently in the system.
        /// </summary>
        /// <param name="queryOperator">The query oprator.</param>
        /// <returns>The foci in the system.</returns>
        public PagedQueryResults<FocusDTO> GetFoci(QueryableOperator<FocusDTO> queryOperator)
        {
            return FocusQueries.CreateFociDTOsQuery(this.Context, queryOperator).ToPagedQueryResults<FocusDTO>(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the foci currently in the system.
        /// </summary>
        /// <param name="queryOperator">The query oprator.</param>
        /// <returns>The foci in the system.</returns>
        public Task<PagedQueryResults<FocusDTO>> GetFociAsync(QueryableOperator<FocusDTO> queryOperator)
        {
            return FocusQueries.CreateFociDTOsQuery(this.Context, queryOperator).ToPagedQueryResultsAsync<FocusDTO>(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the focus with the given id or null if not found.
        /// </summary>
        /// <param name="id">The id of the focus.</param>
        /// <returns>The focus with the given id, or null if not found.</returns>
        public FocusDTO GetFocusById(int id)
        {
            return FocusQueries.CreateGetFocusByIdQuery(this.Context, id).FirstOrDefault();
        }

        /// <summary>
        /// Returns the focus with the given id or null if not found.
        /// </summary>
        /// <param name="id">The id of the focus.</param>
        /// <returns>The focus with the given id, or null if not found.</returns>
        public Task<FocusDTO> GetFocusByIdAsync(int id)
        {
            return FocusQueries.CreateGetFocusByIdQuery(this.Context, id).FirstOrDefaultAsync();
        }
        #endregion
    }
}
