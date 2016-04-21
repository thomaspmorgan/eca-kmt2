using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Sevis
{
    /// <summary>
    /// A SevisGroupedQueuedToSubmitParticipantsDTO represents the participants that queued to submit to sevis by sevis account.
    /// </summary>
    public class SevisGroupedQueuedToSubmitParticipantsDTO
    {
        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public SevisGroupedQueuedToSubmitParticipantsDTO()
        {
            this.Participants = new List<QueuedToSubmitParticipantDTO>();
        }

        /// <summary>
        /// Gets or sets the sevis org id.
        /// </summary>
        public string SevisOrgId { get; set; }

        /// <summary>
        /// Gets or sets the sevis username.
        /// </summary>
        public string SevisUsername { get; set; }

        /// <summary>
        /// Gets or sets the participants that were queued to submit by this sevis user.
        /// </summary>
        public IEnumerable<QueuedToSubmitParticipantDTO> Participants { get; set; }
    }
}
