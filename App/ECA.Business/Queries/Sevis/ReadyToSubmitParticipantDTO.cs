using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Sevis
{
    /// <summary>
    /// A ReadyToSubmitParticipantDTO is used to find participants that should be used to create exchange visitors
    /// and sent to sevis.
    /// </summary>
    public class ReadyToSubmitParticipantDTO
    {
        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the sevis id.
        /// </summary>
        public string SevisId { get; set; }

    }
}
