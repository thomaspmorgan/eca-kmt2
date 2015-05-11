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
using ECA.Business.Service.Lookup;
using System.Diagnostics;
using NLog;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The FocusService is capable of performing crud operations on ECA Foci.
    /// </summary>
    public class FocusCategoryService : DbContextService<EcaContext>, ECA.Business.Service.Admin.IFocusCategoryService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Creates a new FocusCategorySerivce with the given context.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public FocusCategoryService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns the focus categories for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office by id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The focus categories.</returns>
        public PagedQueryResults<FocusCategoryDTO> GetFocusCategoriesByOfficeId(int officeId, QueryableOperator<FocusCategoryDTO> queryOperator)
        {
            var results = FocusCategoryQueries.CreateGetFocusCategoryDTOQuery(this.Context, officeId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Retrieved focus categories for office with id [{0}].", officeId);
            return results;
        }

        /// <summary>
        /// Returns the focus categories for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office by id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The focus categories.</returns>
        public async Task<PagedQueryResults<FocusCategoryDTO>> GetFocusCategoriesByOfficeIdAsync(int officeId, QueryableOperator<FocusCategoryDTO> queryOperator)
        {
            var results = await FocusCategoryQueries.CreateGetFocusCategoryDTOQuery(this.Context, officeId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Retrieved focus categories for office with id [{0}].", officeId);
            return results;
        }
        #endregion
    }
}
