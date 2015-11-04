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
    /// The ParticipantStatusService is a lookup service for handling participant statii.
    /// </summary>
    public class ParticipantStatusService : LookupService<ParticipantStatusDTO>, IParticipantStatusService
    {
        /// <summary>
        /// Creates a new ParticipantTypeService instance with the context to query.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to query.</param>
        public ParticipantStatusService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        protected override IQueryable<ParticipantStatusDTO> GetSelectDTOQuery()
        {
            return Context.ParticipantStatuses.Select(x => new ParticipantStatusDTO
            {
                Id = x.ParticipantStatusId,
                Name = x.Status
            });
        }
    }
}
