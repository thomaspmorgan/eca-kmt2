using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// The ExchangeVisitorHistory is an entity used to save the previously submitted exchange visitor to sevis.
    /// </summary>
    public class ExchangeVisitorHistory
    {
        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the exchange visitor model as a string.  This model represents the exchange visitor
        /// information that is currently saved in the SEVIS api.
        /// </summary>
        public string LastSuccessfulModel { get; set; }

        /// <summary>
        /// Gets or sets the pending exchange visitor model.  This model represents an exchange visitor update
        /// that is currently being processed.
        /// </summary>
        public string PendingModel { get; set; }

        /// <summary>
        /// Gets or sets the last revised on date.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }
    }
}
