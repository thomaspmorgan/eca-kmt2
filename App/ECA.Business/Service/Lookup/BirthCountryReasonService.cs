using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Service.Lookup
{
    public class BirthCountryReasonService : LookupService<BirthCountryReasonDTO>, IBirthCountryReasonService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="saveActions">The save actions.</param>
        public BirthCountryReasonService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        protected override IQueryable<BirthCountryReasonDTO> GetSelectDTOQuery()
        {
            return this.Context.BirthCountryReasons.Select(x => new BirthCountryReasonDTO
            {
                Id = x.BirthCountryReasonId,
                Name = x.Description,
                BirthReasonCode = x.BirthReasonCode
            });
        }
    }
}
