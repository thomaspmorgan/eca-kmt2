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
    [ContractClass(typeof(EmailAddressTypeServiceContract))]
    public interface IEmailAddressTypeService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted social media types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The social media types in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.EmailAddressTypeDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.EmailAddressTypeDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted social media types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The social media types in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.EmailAddressTypeDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.EmailAddressTypeDTO> queryOperator);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IEmailAddressTypeService))]
    public abstract class EmailAddressTypeServiceContract : IEmailAddressTypeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Core.Query.PagedQueryResults<EmailAddressTypeDTO> Get(Core.DynamicLinq.QueryableOperator<EmailAddressTypeDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<Core.Query.PagedQueryResults<EmailAddressTypeDTO>> GetAsync(Core.DynamicLinq.QueryableOperator<EmailAddressTypeDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<ECA.Core.Query.PagedQueryResults<EmailAddressTypeDTO>>(null);
        }
    }
}
