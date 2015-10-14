using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// Provies a lookup service for address types using the EcaContext.
    /// </summary>
    public class AddressTypeService : LookupService<AddressTypeDTO>, IAddressTypeService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="saveActions">The save actions.</param>
        public AddressTypeService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        protected override IQueryable<AddressTypeDTO> GetSelectDTOQuery()
        {
            return this.Context.AddressTypes.Select(x => new AddressTypeDTO
            {
                Id = x.AddressTypeId,
                Name = x.AddressName
            });
        }
    }
}
