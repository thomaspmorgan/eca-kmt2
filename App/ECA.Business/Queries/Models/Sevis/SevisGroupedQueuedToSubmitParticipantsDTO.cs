using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Sevis
{
    public class SevisGroupedQueuedToSubmitParticipantsDTO
    {
        public SevisGroupedQueuedToSubmitParticipantsDTO()
        {
            this.Participants = new List<QueuedToSubmitParticipantDTO>();
        }

        public string SevisOrgId { get; set; }

        public string SevisUsername { get; set; }

        public IEnumerable<QueuedToSubmitParticipantDTO> Participants { get; set; }
    }
}
