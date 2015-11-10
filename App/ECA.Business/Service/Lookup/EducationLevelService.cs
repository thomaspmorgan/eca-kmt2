using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;


namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// The EducationLevelService service is used to perform crud operations on SEVIS EducationLevels.
    /// </summary>
    public class EducationLevelService : LookupService<SimpleSevisLookupDTO>, IEducationLevelService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to query.</param>
        public EducationLevelService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Gets a set of Sevis EducationLevel dtos
        /// </summary>
        /// <returns>SimpleSevisLookupDTO</returns>
        protected override IQueryable<SimpleSevisLookupDTO> GetSelectDTOQuery()
        {
            return this.Context.EducationLevels.Select(x => new SimpleSevisLookupDTO
            {
                Id = x.EducationLevelId,
                Code = x.EducationLevelCode.ToString(),
                Description = x.Description
            });
        }
    }
}
