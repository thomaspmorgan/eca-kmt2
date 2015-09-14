using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IPhoneNumberTypeService is capable of performing crud operations on phone number types.
    /// </summary>
    [ContractClass(typeof(PhoneNumberTypeServiceContract))]
    public interface IPhoneNumberTypeService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted phone number types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The social media types in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.PhoneNumberTypeDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.PhoneNumberTypeDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted phone number  types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The phone number types in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.PhoneNumberTypeDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.PhoneNumberTypeDTO> queryOperator);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IPhoneNumberTypeService))]
    public abstract class PhoneNumberTypeServiceContract : IPhoneNumberTypeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Core.Query.PagedQueryResults<PhoneNumberTypeDTO> Get(Core.DynamicLinq.QueryableOperator<PhoneNumberTypeDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<Core.Query.PagedQueryResults<PhoneNumberTypeDTO>> GetAsync(Core.DynamicLinq.QueryableOperator<PhoneNumberTypeDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<ECA.Core.Query.PagedQueryResults<PhoneNumberTypeDTO>>(null);
        }
    }
}
