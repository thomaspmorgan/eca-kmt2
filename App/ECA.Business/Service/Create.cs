using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service
{
    public class Create : Audit
    {
        public Create(User creator) : base(creator)
        {
            Contract.Requires(creator != null, "The creator must not be null.");
        }

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
