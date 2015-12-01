using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service
{
    public class Delete : Audit
    {
        /// <summary>
        /// Creates a new audit business entity with the user performing the change.
        /// </summary>
        /// <param name="user">The user deleting the auditable entity.</param>
        public Delete(User user)
            : base(user)
        {
            Contract.Requires(user != null, "The must not be null.");
        }

        /// <summary>
        /// Sets the history on a data model with this audit's information.
        /// </summary>
        /// <param name="historicalEntity">The historical entity.</param>
        public override void SetHistory(IHistorical historicalEntity)
        {
            if(historicalEntity.History == null)
            {
                historicalEntity.History = new History();
            }
            historicalEntity.History.RevisedBy = this.User.Id;
            historicalEntity.History.RevisedOn = this.Date;
        }
    }
}
