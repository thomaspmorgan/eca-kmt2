using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service
{
    public class Update : Audit
    {
        public Update(User updater)
            : base(updater)
        {
            Contract.Requires(updater != null, "The updater must not be null.");
        }

        public override void SetHistory(IHistorical historicalEntity)
        {
            Contract.Requires(historicalEntity != null, "The historical entity must not be null.");
            Contract.Requires(historicalEntity.History != null, "The history entity must already have a history item associated with it.");
            historicalEntity.History.RevisedBy = this.User.Id;
            historicalEntity.History.RevisedOn = this.Date;
        }
    }
}
