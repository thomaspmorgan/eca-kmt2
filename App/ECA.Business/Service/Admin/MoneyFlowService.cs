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

namespace ECA.Business.Service.Admin
{
    public class MoneyFlowService : DbContextService<EcaContext>, IMoneyFlowService
    {
        public MoneyFlowService(EcaContext context, ILogger logger) : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByProjectId(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            return MoneyFlowQueries.CreateGetMoneyFlowsByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        public Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByProjectIdAsync(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            return MoneyFlowQueries.CreateGetMoneyFlowsByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
        }
    }
}
