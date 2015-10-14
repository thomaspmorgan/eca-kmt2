using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;
using ECA.Business.Service.Projects;
using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// The ProgramStatusService is used to retrieve program stati from the db context.
    /// </summary>
    public class ProgramStatusService : LookupService<ProgramStatusDTO>, IProgramStatusService
    {
        /// <summary>
        /// Creates a new ProgramStatusService with the context.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ProgramStatusService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Returns a query to retrieve program status dtos.
        /// </summary>
        /// <returns>A query to get program status dtos.</returns>
        protected override IQueryable<ProgramStatusDTO> GetSelectDTOQuery()
        {
            return this.Context.ProgramStatuses.Select(x => new ProgramStatusDTO
            {
                Id = x.ProgramStatusId,
                Name = x.Status
            });
        }
    }
}
