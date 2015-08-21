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
    /// The LanguageService service is used to perform crud operations on a Language.
    /// </summary>
    public class LanguageService : LookupService<LanguageDTO>, ILanguageService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        public LanguageService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        protected override IQueryable<LanguageDTO> GetSelectDTOQuery()
        {
            return this.Context.Languages.Select(x => new LanguageDTO
            {
                Id = x.LanguageId,
                Name = x.LanguageName
            });
        }
    }
}
