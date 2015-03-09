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
    /// An Audit business entity is an entity that tracks the user making a change and the date of the change.
    /// </summary>
    public abstract class Audit
    {
        public Audit(User user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            this.User = user;
            this.Date = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// Gets the date of the audit.
        /// </summary>
        public DateTimeOffset Date { get; private set; }

        /// <summary>
        /// Updates the history information on the given entity with this history.
        /// </summary>
        /// <param name="historicalEntity">The historical entity.</param>
        public abstract void SetHistory(IHistorical historicalEntity);
    }
}
