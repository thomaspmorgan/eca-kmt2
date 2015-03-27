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
        /// Returns a query to select goal dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query.</returns>
        public static IQueryable<GoalDTO> CreateGetGoalDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = context.Goals.Select(x => new GoalDTO
            {
                Id = x.GoalId,
                Name = x.GoalName
            });
            return query;
        }
    }
}
