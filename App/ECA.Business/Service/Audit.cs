using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service
{
    public abstract class Audit
    {
        public Audit(User user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            this.User = user;
            this.Date = DateTimeOffset.UtcNow;
        }

        public User User { get; set; }

        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Updates the history information on the given entity with this history.
        /// </summary>
        /// <param name="historicalEntity">The historical entity.</param>
        public abstract void SetHistory(IHistorical historicalEntity);
    }
}
