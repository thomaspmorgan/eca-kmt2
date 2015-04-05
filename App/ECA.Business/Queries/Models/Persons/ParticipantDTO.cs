using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons
{
    public class ParticipantDTO : SimpleParticipantDTO
    {
        public string SevisId { get; set; }
        public bool ContactAgreement { get; set; }
    }
}
