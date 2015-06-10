using ECA.Business.Queries.Models.Lookup;
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
    /// An OrganizationTypeService is a service handling a lookup on an organization type.
    /// </summary>
    public class OrganizationTypeService : LookupService<OrganizationTypeDTO>, IOrganizationTypeService
    {
        /// <summary>
        /// Creates a new OrganizationTypeService.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public OrganizationTypeService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get
        /// <summary>
        /// Returns a query to get dtos.
        /// </summary>
        /// <returns>The query to get organization type dtos.</returns>
        protected override IQueryable<OrganizationTypeDTO> GetSelectDTOQuery()
        {
            return this.Context.OrganizationTypes.Select(x => new OrganizationTypeDTO
            {
                Id = x.OrganizationTypeId,
                Name = x.OrganizationTypeName
            });
        }
        #endregion
    }
}
