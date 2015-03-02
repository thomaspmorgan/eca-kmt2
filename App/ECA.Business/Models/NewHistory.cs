using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Models
{
    /// <summary>
    /// A NewHistory model is a model used when a historical entity is being created for the first time.
    /// </summary>
    public class NewHistory
    {
        /// <summary>
        /// Initializes a NewHistory instance with the given user id and the current utc time.
        /// </summary>
        /// <param name="creatorUserId">The user creating the history.</param>
        public NewHistory(int creatorUserId)
        {
            this.CreatedAndRevisedOn = DateTimeOffset.UtcNow;
            this.CreatorUserId = creatorUserId;
        }

        /// <summary>
        /// Gets the creating user id.
        /// </summary>
        public int CreatorUserId { get; private set; }

        /// <summary>
        /// Gets the date the history item is created on.
        /// </summary>
        public DateTimeOffset CreatedAndRevisedOn { get; private set; }

        /// <summary>
        /// Updates the history information on the given entity with this history.
        /// </summary>
        /// <param name="historicalEntity">The historical entity.</param>
        public virtual void SetHistory(IHistorical historicalEntity)
        {
            Contract.Requires(historicalEntity != null, "The historical entity must not be null.");
            if(historicalEntity.History == null)
            {
                historicalEntity.History = new History();
            }
            historicalEntity.History.CreatedBy = this.CreatorUserId;
            historicalEntity.History.RevisedBy = this.CreatorUserId;
            historicalEntity.History.CreatedOn = this.CreatedAndRevisedOn;
            historicalEntity.History.RevisedOn = this.CreatedAndRevisedOn;
        }
    }
}
