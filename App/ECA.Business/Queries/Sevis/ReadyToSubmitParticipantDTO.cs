using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Sevis
{
    public class ReadyToSubmitParticipantDTO
    {
        public int ParticipantId { get; set; }

        public int ProjectId { get; set; }

        public string SevisId { get; set; }

    }
}
