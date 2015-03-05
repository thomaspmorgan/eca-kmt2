using ECA.Data;
using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq;
using System.Diagnostics.Contracts;

namespace ECA.Business.Queries.Admin
{
    class MoneyFlowQueries
    {

        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowsByProjectIdQuery(EcaContext context, int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");

            var query = from moneyflows in context.MoneyFlows
                        where moneyflows.RecipientProjectId == projectId ||
                              moneyflows.SourceProjectId == projectId
                        select new MoneyFlowDTO
                        {
                            Id = moneyflows.MoneyFlowId,
                            TransactionDate = moneyflows.TransactionDate,
                            SourceType = moneyflows.SourceType.TypeName,
                            SourceName = "source name",
                            RecipientType = moneyflows.RecipientType.TypeName,
                            RecipientName = "recipient name",
                            Description = moneyflows.Description
                        };
            query = query.Apply(queryOperator);
            return query;
        }

    }
}
