using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service
{
    /// <summary>
    /// A RevisedHistory object is an object to track a revision to an entity.
    /// </summary>
    public class RevisedHistory
    {
        /// <summary>
        /// Initializes a new RevisedHistory object with the given user id.
        /// </summary>
        /// <param name="userId">The user id revising an entity.</param>
        public RevisedHistory(User user)
        {
            this.RevisedBy = user;
            this.RevisedOn = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Gets the user id of the user doing a revision.
        /// </summary>
        public User RevisedBy { get; private set; }

        /// <summary>
        /// Gets the date the revision is occuring on.
        /// </summary>
        public DateTimeOffset RevisedOn { get; private set; }

        /// <summary>
        /// Updates the history information on the given entity with this history.
        /// </summary>
        /// <param name="historicalEntity">The historical entity.</param>
        public virtual void SetHistory(IHistorical historicalEntity)
        {
            Contract.Requires(historicalEntity != null, "The historical entity must not be null.");
            Contract.Requires(historicalEntity.History != null, "The history entity must already have a history item associated with it.");
            historicalEntity.History.RevisedBy = this.RevisedBy.Id;
            historicalEntity.History.RevisedOn = this.RevisedOn;
        }
    }
}
