using ECA.Data;
using ECA.Core.Service;
using ECA.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Business.Queries.Admin;
using System.Diagnostics;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// A service to perform crud operations on moneyflows
    /// </summary>
    public class MoneyFlowService : DbContextService<EcaContext>, IMoneyFlowService
    {
        private static readonly string COMPONENT_NAME = typeof(MoneyFlowService).FullName;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="logger">The logger to inject</param>
        public MoneyFlowService(EcaContext context, ILogger logger) : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
        }

        /// <summary>
        /// Gets moneyflows by the project id 
        /// </summary>
        /// <param name="projectId">The project id to find associated moneyflows</param>
        /// <param name="queryOperator"></param>
        /// <returns>List of moneyflows that are paged, sorted, and filtered</returns>
        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByProjectId(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var stopwatch = Stopwatch.StartNew();
            var moneyFlows = MoneyFlowQueries.CreateGetMoneyFlowsByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "queryOperator", queryOperator } });
            return moneyFlows;
        }

        /// <summary>
        /// Gets moneyflows by the project id asynchronously
        /// </summary>
        /// <param name="projectId">The project id to find associated moneyflows</param>
        /// <param name="queryOperator"></param>
        /// <returns>List of moneyflows that are paged, sorted, and filtered</returns>
        public async Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByProjectIdAsync(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var stopwatch = Stopwatch.StartNew();
            var moneyFlows = await MoneyFlowQueries.CreateGetMoneyFlowsByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "queryOperator", queryOperator } });
            return moneyFlows;
        }
    }
}
