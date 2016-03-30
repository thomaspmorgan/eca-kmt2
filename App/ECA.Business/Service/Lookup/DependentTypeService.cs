using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// Provies a lookup service for address types using the EcaContext.
    /// </summary>
    public class DependentTypeService : LookupService<DependentTypeDTO>, IDependentTypeService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="saveActions">The save actions.</param>
        public DependentTypeService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        protected override IQueryable<DependentTypeDTO> GetSelectDTOQuery()
        {
            return this.Context.DependentTypes.Select(x => new DependentTypeDTO
            {
                Id = x.DependentTypeId,
                Name = x.Name,
                SevisDependentTypeCode = x.SevisDependentTypeCode
            });
        }
    }
}
