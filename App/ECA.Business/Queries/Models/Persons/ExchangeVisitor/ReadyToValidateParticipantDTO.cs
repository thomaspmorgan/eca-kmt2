using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons.ExchangeVisitor
{
    /// <summary>
    /// A ReadyToValidateParticipantDTO represents a participant that has a sevis id and has passed their start date.
    /// </summary>
    public class ReadyToValidateParticipantDTO
    {
        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the project id of the participant.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the sevis id.
        /// </summary>
        public string SevisId { get; set; }
    }
}
