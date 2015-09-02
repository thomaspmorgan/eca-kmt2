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
    public class EmailAddressTypeService : LookupService<EmailAddressTypeDTO>, IEmailAddressTypeService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        public EmailAddressTypeService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Gets a set of Email Address Type dtos
        /// </summary>
        /// <returns>EmailAddressTypeDTOs</returns>
        protected override IQueryable<EmailAddressTypeDTO> GetSelectDTOQuery()
        {
            return this.Context.EmailAddressTypes.Select(x => new EmailAddressTypeDTO
            {
                Id = x.EmailAddressTypeId,
                Name = x.EmailAddressTypeName
            });
        }
    }
}
