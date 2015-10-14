using ECA.Business.Service.Lookup;
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
    /// The LocationTypeService is used to perform crud operations on location types in the eca system.
    /// </summary>
    public class LocationTypeService : LookupService<SimpleLookupDTO>, ILocationTypeService
    {
        /// <summary>
        /// Creates a new LocationTypeService.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="saveActions">The save actions.</param>
        public LocationTypeService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        protected override IQueryable<SimpleLookupDTO> GetSelectDTOQuery()
        {
            return this.Context.LocationTypes.Select(x => new SimpleLookupDTO
            {
                Id = x.LocationTypeId,
                Value = x.LocationTypeName
            });
        }
    }
}
