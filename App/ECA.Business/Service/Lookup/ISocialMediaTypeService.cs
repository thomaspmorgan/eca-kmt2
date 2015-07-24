using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An ISocialMediaTypeService is capable of performing crud operations on social media types.
    /// </summary>
    [ContractClass(typeof(SocialMediaTypeServiceContract))]
    public interface ISocialMediaTypeService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted social media types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The social media types in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.SocialMediaTypeDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.SocialMediaTypeDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted social media types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The social media types in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.SocialMediaTypeDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.SocialMediaTypeDTO> queryOperator);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(ISocialMediaTypeService))]
    public abstract class SocialMediaTypeServiceContract : ISocialMediaTypeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Core.Query.PagedQueryResults<SocialMediaTypeDTO> Get(Core.DynamicLinq.QueryableOperator<SocialMediaTypeDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<Core.Query.PagedQueryResults<SocialMediaTypeDTO>> GetAsync(Core.DynamicLinq.QueryableOperator<SocialMediaTypeDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<ECA.Core.Query.PagedQueryResults<SocialMediaTypeDTO>>(null);
        }
    }
}
