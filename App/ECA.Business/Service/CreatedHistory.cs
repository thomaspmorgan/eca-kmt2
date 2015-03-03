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
    /// A NewHistory model is a model used when a historical entity is being created for the first time.
    /// </summary>
    public class CreatedHistory
    {
        /// <summary>
        /// Initializes a NewHistory instance with the given user id and the current utc time.
        /// </summary>
        /// <param name="creatorUserId">The user creating the history.</param>
        public CreatedHistory(User user)
        {
            this.CreatedAndRevisedOn = DateTimeOffset.UtcNow;
            this.CreatedBy = user;
        }

        /// <summary>
        /// Gets the creating user id.
        /// </summary>
        public User CreatedBy { get; private set; }

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
            historicalEntity.History.CreatedBy = this.CreatedBy.Id;
            historicalEntity.History.RevisedBy = this.CreatedBy.Id;
            historicalEntity.History.CreatedOn = this.CreatedAndRevisedOn;
            historicalEntity.History.RevisedOn = this.CreatedAndRevisedOn;
        }
    }
}
