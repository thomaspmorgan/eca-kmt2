using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
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
    public class GoalService : IDisposable, IGoalService
    {
        private EcaContext context;

        /// <summary>
        /// Creates a new GoalService with the context to operate against.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public GoalService(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            this.context = context;
        }


        #region Get
        /// <summary>
        /// Returns the goals currently in the system.
        /// </summary>
        /// <param name="queryOperator">The query oprator.</param>
        /// <returns>The goals in the system.</returns>
        public PagedQueryResults<GoalDTO> GetGoals(QueryableOperator<GoalDTO> queryOperator)
        {
            return GoalQueries.CreateGetGoalsQuery(this.context, queryOperator).ToPagedQueryResults<GoalDTO>(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the goals currently in the system.
        /// </summary>
        /// <param name="queryOperator">The query oprator.</param>
        /// <returns>The goals in the system.</returns>
        public Task<PagedQueryResults<GoalDTO>> GetGoalsAsync(QueryableOperator<GoalDTO> queryOperator)
        {
            return GoalQueries.CreateGetGoalsQuery(this.context, queryOperator).ToPagedQueryResultsAsync<GoalDTO>(queryOperator.Start, queryOperator.Limit);
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
