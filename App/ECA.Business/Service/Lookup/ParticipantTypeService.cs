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
    /// The ParticipantTypeService is a lookup service for handling participant types.
    /// </summary>
    public class ParticipantTypeService : LookupService<ParticipantTypeDTO>, IParticipantTypeService
    {
        /// <summary>
        /// Creates a new ParticipantTypeService instance with the context to query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        public ParticipantTypeService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        protected override IQueryable<ParticipantTypeDTO> GetSelectDTOQuery()
        {
            return Context.ParticipantTypes.Select(x => new ParticipantTypeDTO
            {
                Id = x.ParticipantTypeId,
                Name = x.Name,
                IsPerson = x.IsPerson
            });
        }
    }
}
