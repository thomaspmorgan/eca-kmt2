using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;


namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// The SevisCommStatusService service is used to perform crud operations on SevisCommStatuses.
    /// </summary>
    public class FieldOfStudyService : LookupService<SimpleSevisLookupDTO>, IFieldOfStudyService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to query.</param>
        public FieldOfStudyService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Gets a set of Sevis Comm Status dtos
        /// </summary>
        /// <returns>SevisCommStatusDTOs</returns>
        protected override IQueryable<SimpleSevisLookupDTO> GetSelectDTOQuery()
        {
            return this.Context.FieldOfStudies.Select(x => new SimpleSevisLookupDTO
            {
                Id = x.FieldOfStudyId,
                Code = x.FieldOfStudyCode,
                Description = x.Description
            });
        }
    }
}
