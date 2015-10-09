using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Data
{
    public class ParticipantPersonSevisCommStatus
    {
        [Key]
        public int Id { get; set; }

        public int ParticipantId { get; set; }

        public ParticipantPerson ParticipantPerson { get; set; }

        public int SevisCommStatusId { get; set; }

        public SevisCommStatus SevisCommStatus { get; set; }

        public DateTimeOffset AddedOn { get; set; }
    }
}
