using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
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

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The GoalService is capable of performing crud operations against and entity framework context.
    /// </summary>
    public class GoalService : DbContextService<EcaContext>, IGoalService
    {
        /// <summary>
        /// Creates a new GoalService with the context to operate against.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public GoalService(EcaContext context, ILogger logger) : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
        }


        #region Get
        /// <summary>
        /// Returns the goals currently in the system.
        /// </summary>
        /// <param name="queryOperator">The query oprator.</param>
        /// <returns>The goals in the system.</returns>
        public PagedQueryResults<GoalDTO> GetGoals(QueryableOperator<GoalDTO> queryOperator)
        {
            return GoalQueries.CreateGetGoalsQuery(this.Context, queryOperator).ToPagedQueryResults<GoalDTO>(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the goals currently in the system.
        /// </summary>
        /// <param name="queryOperator">The query oprator.</param>
        /// <returns>The goals in the system.</returns>
        public Task<PagedQueryResults<GoalDTO>> GetGoalsAsync(QueryableOperator<GoalDTO> queryOperator)
        {
            return GoalQueries.CreateGetGoalsQuery(this.Context, queryOperator).ToPagedQueryResultsAsync<GoalDTO>(queryOperator.Start, queryOperator.Limit);
        }
        #endregion

    }
}
