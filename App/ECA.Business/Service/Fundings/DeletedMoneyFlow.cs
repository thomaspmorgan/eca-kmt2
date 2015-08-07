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
    public class DeletedMoneyFlow : IAuditable
    {
        /// <summary>
        /// Creats a new DeletedMoneyFlow instance.
        /// </summary>
        /// <param name="user">The user performing the delete.</param>
        /// <param name="id">The id of the money flow to delete.</param>
        /// <param name="sourceOrRecipientEntityId">The id of the source or recipient entity id, used for security.</param>
        public DeletedMoneyFlow(User user, int id, int sourceOrRecipientEntityId)
        {
            Contract.Requires(user != null, "The user must not be null.");
            this.Id = id;
            this.SourceOrRecipientEntityId = sourceOrRecipientEntityId;
            this.Audit = new Delete(user);
        }

        /// <summary>
        /// Gets the audit of this deleted money flow.
        /// </summary>
        public Audit Audit { get; private set; }

        /// <summary>
        /// Gets the id of the money flow.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the source or recipient entity id.
        /// </summary>
        public int SourceOrRecipientEntityId { get; private set; }
    }
}
