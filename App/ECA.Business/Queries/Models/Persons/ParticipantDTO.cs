using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// The ParticipantDTO is a dto representing a single participant in the system.
    /// </summary>
    public class ParticipantDTO : SimpleParticipantDTO
    {
        /// <summary>
        /// Gets or sets the sevis id.
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Gets or sets the contact agreement.
        /// </summary>
        public bool ContactAgreement { get; set; }

        /// <summary>
        /// Gets or sets the participant's status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the participant's last status date
        /// </summary>
        public DateTimeOffset? StatusDate { get; set; }
    }
}
