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
    public class JustificationObjectiveService : DbContextService<EcaContext>, 
        ECA.Business.Service.Admin.IJustificationObjectiveService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public JustificationObjectiveService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }


        #region Get

        /// <summary>
        /// Returns the justification objectives for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office by id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The justification objectives.</returns>
        public PagedQueryResults<JustificationObjectiveDTO> GetJustificationObjectivesByOfficeId(int officeId, QueryableOperator<JustificationObjectiveDTO> queryOperator)
        {
            var results = JustificationObjectiveQueries.CreateGetJustificationObjectiveDTOQuery(this.Context, officeId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Retrieved justification objectives for office with id [{0}].", officeId);
            return results;
        }

        /// <summary>
        /// Returns the justification objectives for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office by id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The justification objectives.</returns>
        public async Task<PagedQueryResults<JustificationObjectiveDTO>> GetJustificationObjectivesByOfficeIdAsync(int officeId, QueryableOperator<JustificationObjectiveDTO> queryOperator)
        {
            var results = await JustificationObjectiveQueries.CreateGetJustificationObjectiveDTOQuery(this.Context, officeId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Retrieved justification objectives for office with id [{0}].", officeId);
            return results;
        }
        #endregion
        
    }
}
