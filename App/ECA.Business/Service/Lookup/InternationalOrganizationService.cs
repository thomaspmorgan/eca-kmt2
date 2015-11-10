using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;


namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// The PositionService service is used to perform crud operations on SEVIS InternationalOrganizations.
    /// </summary>
    public class InternantionalOrganizationService : LookupService<SimpleSevisLookupDTO>, IProgramCategoryService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to query.</param>
        public InternantionalOrganizationService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Gets a set of Sevis InternationalOrganization dtos
        /// </summary>
        /// <returns>SimpleSevisLookupDTO</returns>
        protected override IQueryable<SimpleSevisLookupDTO> GetSelectDTOQuery()
        {
            return this.Context.InternationalOrganizations.Select(x => new SimpleSevisLookupDTO
            {
                Id = x.OrganizationId,
                Code = x.OrganizationCode,
                Description = x.Description
            });
        }
    }
}
