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
    public static class JustificationObjectiveQueries
    {
        /// <summary>
        /// Returns a query to select justification objectives dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="officeId">The office id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query.</returns>
        public static IQueryable<JustificationObjectiveDTO> CreateGetJustificationObjectiveDTOQuery(EcaContext context, int officeId, QueryableOperator<JustificationObjectiveDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = context.Objectives
                .Where(x => x.Justification.OfficeId == officeId)
                .Select(j => new JustificationObjectiveDTO
                {
                    Id = j.ObjectiveId,
                    Name = j.ObjectiveName,
                    JustificationName = j.Justification.JustificationName,
                });
            query = query.Apply(queryOperator);
            return query;
        }
    }
}
