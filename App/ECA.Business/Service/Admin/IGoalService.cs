using System;
namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An IGoalService is capable of performing crud operations with goals.
    /// </summary>
    public interface IGoalService
    {
        /// <summary>
        /// Returns the goals currently in the system.
        /// </summary>
        /// <param name="queryOperator">The query oprator.</param>
        /// <returns>The goals in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Admin.GoalDTO> GetGoals(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Admin.GoalDTO> queryOperator);

        /// <summary>
        /// Returns the goals currently in the system.
        /// </summary>
        /// <param name="queryOperator">The query oprator.</param>
        /// <returns>The goals in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Admin.GoalDTO>> GetGoalsAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Admin.GoalDTO> queryOperator);
    }
}
