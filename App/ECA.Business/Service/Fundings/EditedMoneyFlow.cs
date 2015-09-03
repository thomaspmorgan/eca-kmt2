using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Fundings
{
    /// <summary>
    /// A EditedMoneyFlow is used to keep track of an existing money flow and it's source or recipient entity type id and
    /// source or recipient entity id when performing an update or delete on a money flow.
    /// </summary>
    public class EditedMoneyFlow
    {
        /// <summary>
        /// Creates a new SimpleMoneyFlow.
        /// </summary>
        /// <param name="id">The id of the money flow.</param>
        /// <param name="sourceOrRecipientEntityId">The id of the source or recipient entity.</param>
        /// <param name="sourceOrRecipientEntityTypeId">The type id of the source or recipient money flow.</param>
        public EditedMoneyFlow(int id, int sourceOrRecipientEntityId, int sourceOrRecipientEntityTypeId)
        {
            this.Id = id;
            this.SourceOrRecipientEntityId = sourceOrRecipientEntityId;
            if (MoneyFlowSourceRecipientType.GetStaticLookup(sourceOrRecipientEntityTypeId) == null)
            {
                throw new UnknownStaticLookupException(String.Format("The money flow source or recipient entity type id [{0}] is not recognized.", sourceOrRecipientEntityTypeId));
            }
            this.SourceOrRecipientEntityTypeId = sourceOrRecipientEntityTypeId;
        }

        /// <summary>
        /// Gets the id of the money flow.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the source or recipient entity id.
        /// </summary>
        public int SourceOrRecipientEntityId { get; private set; }

        /// <summary>
        /// Gets the source or recipient entity type id.
        /// </summary>
        public int SourceOrRecipientEntityTypeId { get; private set; }
    }
}
