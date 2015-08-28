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
    /// A DeletedMoneyFlow business entity is used to represent a business service client's request to delete
    /// a money flow.
    /// </summary>
    public class DeletedMoneyFlow : EditedMoneyFlow, IAuditable
    {
        /// <summary>
        /// Creats a new DeletedMoneyFlow instance.
        /// </summary>
        /// <param name="user">The user performing the delete.</param>
        /// <param name="id">The id of the money flow to delete.</param>
        /// <param name="sourceOrRecipientEntityId">The id of the source or recipient entity id, used for security.</param>
        /// <param name="sourceOrRecipientEntityTypeId">The money flow source or recipient entity type id.</param>
        public DeletedMoneyFlow(User user, int id, int sourceOrRecipientEntityId, int sourceOrRecipientEntityTypeId)
            :base(id, sourceOrRecipientEntityId, sourceOrRecipientEntityTypeId)
        {
            Contract.Requires(user != null, "The user must not be null.");            
            this.Audit = new Delete(user);
        }

        /// <summary>
        /// Gets the audit of this deleted money flow.
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
