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
    public class SevisCommStatusService : LookupService<SevisCommStatusDTO>, ISevisCommStatusService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to query.</param>
        public SevisCommStatusService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Gets a set of Sevis Comm Status dtos
        /// </summary>
        /// <returns>SevisCommStatusDTOs</returns>
        protected override IQueryable<SevisCommStatusDTO> GetSelectDTOQuery()
        {
            return this.Context.SevisCommStatuses.Select(x => new SevisCommStatusDTO
            {
                Id = x.SevisCommStatusId,
                Name = x.SevisCommStatusName
            });
        }
    }
}
