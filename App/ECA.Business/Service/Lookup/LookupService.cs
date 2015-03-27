using ECA.Core.DynamicLinq;
using ECA.Core.Logging;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// A LookupService is a service that operates against an EcaContext to query for lookups.
    /// </summary>
    /// <typeparam name="DTOType">The dtop type representing the lookup.</typeparam>
    [ContractClass(typeof(LookupServiceContract<>))]
    public abstract class LookupService<DTOType> : DbContextService<EcaContext>, ECA.Business.Service.Lookup.ILookupService<DTOType> where DTOType : class
    {
        private static readonly string COMPONENT_NAME = typeof(LookupService<>).FullName;
        private readonly ILogger logger;

        /// <summary>
        /// Creates a new LookupService with theg vien context and logger.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="logger">The logger.</param>
        public LookupService(EcaContext context, ILogger logger)
            : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
        }

        #region Get

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of dtos.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted dtos.</returns>
        public PagedQueryResults<DTOType> Get(QueryableOperator<DTOType> queryOperator)
        {
            var stopWatch = Stopwatch.StartNew();
            var results = GetDTOQuery(queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed, new Dictionary<string, object> { { "queryOperator", queryOperator } });
            return results;
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of dtos.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted dtos.</returns>
        public async Task<PagedQueryResults<DTOType>> GetAsync(QueryableOperator<DTOType> queryOperator)
        {
            var stopWatch = Stopwatch.StartNew();
            var results = await GetDTOQuery(queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed, new Dictionary<string, object> { { "queryOperator", queryOperator } });
            return results;
        }

        private IQueryable<DTOType> GetDTOQuery(QueryableOperator<DTOType> queryOperator)
        {
            var query = GetSelectDTOQuery();
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a query to select dtos.
        /// </summary>
        /// <returns>The query to select dtos.</returns>
        protected abstract IQueryable<DTOType> GetSelectDTOQuery();
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="DTOType"></typeparam>
    [ContractClassFor(typeof(LookupService<>))]
    public abstract class LookupServiceContract<DTOType> : LookupService<DTOType> where DTOType : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public LookupServiceContract(EcaContext context, ILogger logger) : base(context, logger) { }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IQueryable<DTOType> GetSelectDTOQuery()
        {
            Contract.Ensures(Contract.Result<IQueryable<DTOType>>() != null, "The query must not be null.");
            return null;
        }
    }
}
