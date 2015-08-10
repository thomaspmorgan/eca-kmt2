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
    /// The ProminentCategoryService is a lookup service for handling Prominent Categories.
    /// </summary>
    public class ProminentCategoryService : LookupService<ProminentCategoryDTO>, IProminentCategoryService
    {
        /// <summary>
        /// Creates a new ParticipantTypeService instance with the context to query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        public ProminentCategoryService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        protected override IQueryable<ProminentCategoryDTO> GetSelectDTOQuery()
        {
            return Context.ProminentCategories.Select(x => new ProminentCategoryDTO
            {
                Id = x.ProminentCategoryId,
                Name = x.Name
            });
        }
    }
}
