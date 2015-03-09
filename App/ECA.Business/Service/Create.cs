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
    /// A Create audit is an audit record for when a business entity is being created.
    /// </summary>
    public class Create : Audit
    {
        /// <summary>
        /// Creates a new audit business entity with the user performing the change.
        /// </summary>
        /// <param name="creator"></param>
        public Create(User creator) : base(creator)
        {
            Contract.Requires(creator != null, "The creator must not be null.");
        }

        /// <summary>
        /// Sets the history on a data model with this audit's information.
        /// </summary>
        /// <param name="historicalEntity">The historical entity.</param>
        public override void SetHistory(IHistorical historicalEntity)
        {
            Contract.Requires(historicalEntity != null, "The historical entity must not be null.");
            if (historicalEntity.History == null)
            {
                historicalEntity.History = new History();
            }
            historicalEntity.History.CreatedBy = this.User.Id;
            historicalEntity.History.RevisedBy = this.User.Id;
            historicalEntity.History.CreatedOn = this.Date;
            historicalEntity.History.RevisedOn = this.Date;
        }
    }
}
