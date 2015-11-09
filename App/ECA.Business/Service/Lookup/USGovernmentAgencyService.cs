using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;


namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// The USGovernmentAgencyService service is used to perform crud operations on SEVIS USGovernmentAgencies.
    /// </summary>
    public class USGovernmentAgencyService : LookupService<SimpleSevisLookupDTO>, IUSGovernmentAgencyService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to query.</param>
        public USGovernmentAgencyService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Gets a set of Sevis USGovernmentAgencies dtos
        /// </summary>
        /// <returns>SimpleSevisLookupDTO</returns>
        protected override IQueryable<SimpleSevisLookupDTO> GetSelectDTOQuery()
        {
            return this.Context.USGovernmentAgencies.Select(x => new SimpleSevisLookupDTO
            {
                Id = x.AgencyId,
                Code = x.AgencyCode,
                Description = x.Description
            });
        }
    }
}
