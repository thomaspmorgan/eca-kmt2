using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Business.Service.Lookup;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An ISocialMediaTypeService is capable of performing crud operations on languages.
    /// </summary>
    [ContractClass(typeof(LanguageServiceContract))]
    public interface ILanguageService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted languages in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The languages in the system.</returns>
        PagedQueryResults<LanguageDTO> Get(QueryableOperator<LanguageDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted languages in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The languages in the system.</returns>
        Task<PagedQueryResults<LanguageDTO>> GetAsync(QueryableOperator<LanguageDTO> queryOperator);
    }

    /// <summary>
    /// Contract for ILanguageService
    /// </summary>
    [ContractClassFor(typeof(ILanguageService))]
    public abstract class LanguageServiceContract : ILanguageService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted languages in the system.
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<LanguageDTO> Get(QueryableOperator<LanguageDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// Returns paged, filtered, and sorted languages in the system.
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<LanguageDTO>> GetAsync(QueryableOperator<LanguageDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<LanguageDTO>>(null);
        }
    }
}
