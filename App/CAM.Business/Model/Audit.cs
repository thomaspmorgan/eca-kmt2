using CAM.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Model
{
    /// <summary>
    /// An Audit business entity is an entity that tracks the user making a change and the date of the change.
    /// </summary>
    public class Audit
    {
        /// <summary>
        /// Creates a new Audit entity with the given user making the changes.
        /// </summary>
        /// <param name="user">The user performing changes.</param>
        public Audit(int userId)
        {
            this.UserId = userId;
            this.Date = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Gets the date of the audit.
        /// </summary>
        public DateTimeOffset Date { get; private set; }
    }
}