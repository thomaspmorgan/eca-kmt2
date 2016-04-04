using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
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
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new LookupService with theg vien context and logger.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name = "saveActions" > The save actions.</param>
        public LookupService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of dtos.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted dtos.</returns>
        public PagedQueryResults<DTOType> Get(QueryableOperator<DTOType> queryOperator)
        {
            var results = GetDTOQuery(queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded lookups of type [{0}] with query operator = [{1}].", typeof(DTOType).FullName, queryOperator);
            return results;
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of dtos.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted dtos.</returns>
        public async Task<PagedQueryResults<DTOType>> GetAsync(QueryableOperator<DTOType> queryOperator)
        {
            var results = await GetDTOQuery(queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded lookups of type [{0}] with query operator = [{1}].", typeof(DTOType).FullName, queryOperator);
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
        public LookupServiceContract(EcaContext context) : base(context) { }
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
