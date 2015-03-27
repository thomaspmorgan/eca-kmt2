using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An ILookupService is a service that can return a pagedqueryresult set of dtos.
    /// </summary>
    /// <typeparam name="DTOType">The dto type that will be paged, filtered, and sorted.</typeparam>
    [ContractClass(typeof(ILookupServiceContract<>))]
    public interface ILookupService<DTOType>
     where DTOType : class
    {
        /// <summary>
        /// Returns a paged, filtered, and sorted instance of dtos.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted dtos.</returns>
        PagedQueryResults<DTOType> Get(QueryableOperator<DTOType> queryOperator);

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of dtos.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted dtos.</returns>
        Task<PagedQueryResults<DTOType>> GetAsync(QueryableOperator<DTOType> queryOperator);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="DTOType"></typeparam>
    [ContractClassFor(typeof(ILookupService<>))]
    public abstract class ILookupServiceContract<DTOType> : ILookupService<DTOType> where DTOType : class{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<DTOType> Get(QueryableOperator<DTOType> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<DTOType>> GetAsync(QueryableOperator<DTOType> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<DTOType>>(null);
        }
    }
}
