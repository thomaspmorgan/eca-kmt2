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
    public class PersonTypeService : LookupService<PersonTypeDTO>, IPersonTypeService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="saveActions">The save actions.</param>
        public PersonTypeService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        protected override IQueryable<PersonTypeDTO> GetSelectDTOQuery()
        {
            return this.Context.PersonTypes.Select(x => new PersonTypeDTO
            {
                Id = x.PersonTypeId,
                IsDependentPersonType = x.IsDependentPersonType,
                Name = x.Name,
                SevisDependentTypeCode = x.SevisDependentTypeCode
            });
        }
    }
}
