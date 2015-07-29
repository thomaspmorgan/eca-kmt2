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
    public class SocialMediaTypeService : LookupService<SocialMediaTypeDTO>, ISocialMediaTypeService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        public SocialMediaTypeService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        protected override IQueryable<SocialMediaTypeDTO> GetSelectDTOQuery()
        {
            return this.Context.SocialMediaTypes.Select(x => new SocialMediaTypeDTO
            {
                Id = x.SocialMediaTypeId,
                Name = x.SocialMediaTypeName,
                Url = x.Url
            });
        }
    }
}
