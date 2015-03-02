using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Admin
{
    /// <summary>
    /// GoalQueries contains queries to use against entity framework for goals.
    /// </summary>
    public static class GoalQueries
    {
        /// <summary>
        /// Returns a query to get filtered and sorted goals.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The filtered and sorted query.</returns>
        public static IQueryable<GoalDTO> CreateGetGoalsQuery(EcaContext context, QueryableOperator<GoalDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = context.Goals.Select(x => new GoalDTO
            {
                Id = x.GoalId,
                Name = x.GoalName
            });
            query = query.Apply(queryOperator);
            return query;
        }
    }
}
