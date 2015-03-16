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
    /// An Update audit instance is a class detaling the user and the date an update is made to a historical data entity.
    /// </summary>
    public class Update : Audit
    {
        /// <summary>
        /// Creates a new Update audit business entity with the given user performing the update.
        /// </summary>
        /// <param name="updater">The user performing the update.</param>
        public Update(User updater)
            : base(updater)
        {
            Contract.Requires(updater != null, "The updater must not be null.");
        }

        /// <summary>
        /// Sets the history on a data model with this audit's information.
        /// </summary>
        /// <param name="historicalEntity">The historical entity.</param>
        public override void SetHistory(IHistorical historicalEntity)
        {
            historicalEntity.History.RevisedBy = this.User.Id;
            historicalEntity.History.RevisedOn = this.Date;
        }
    }
}
