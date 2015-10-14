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
    /// The SocialMediaTypeService service is used to perform crud operations on a social media types.
    /// </summary>
    public class PhoneNumberTypeService : LookupService<PhoneNumberTypeDTO>, IPhoneNumberTypeService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to query.</param>
        public PhoneNumberTypeService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Gets a set of phone number type dtos
        /// </summary>
        /// <returns>PhoneNumberTypeDTOs</returns>
        protected override IQueryable<PhoneNumberTypeDTO> GetSelectDTOQuery()
        {
            return this.Context.PhoneNumberTypes.Select(x => new PhoneNumberTypeDTO
            {
                Id = x.PhoneNumberTypeId,
                Name = x.PhoneNumberTypeName
            });
        }
    }
}
