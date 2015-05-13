using CAM.Business.Queries.Models;
using CAM.Data;
using ECA.Core.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Queries
{
    /// <summary>
    /// UserQueries contains queries for retrieving users from CAM.
    /// </summary>
    public static class UserQueries
    {
        /// <summary>
        /// Returns a query to retrieve user dto's from the cam model.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to retrieve the users.</returns>
        public static IQueryable<UserDTO> CreateGetUsersQuery(CamModel context, QueryableOperator<UserDTO> queryOperator)
        {
            var query = context.UserAccounts.Select(x => new UserDTO
            {
                AdGuid = x.AdGuid,
                DisplayName = x.DisplayName,
                FirstName = x.FirstName,
                PrincipalId = x.PrincipalId,
                LastName = x.LastName,
                Email = x.EmailAddress,
            });
            query = query.Apply(queryOperator);
            return query;
        }
    }
}
