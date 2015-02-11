using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// Participant Status describes the state of a person's journey through an itinerary, project, phase, etc.
    /// </summary>
    public class ParticipantStatus
    {
        [Key]
        public int ParticipantStatusId { get; set; }
        [Required]
        public Participant Participant { get; set; }
        [Required]
        public ParticipantStatus Status { get; set; }

        public DateTimeOffset StatusDate { get; set; }

        public History History { get; set; }
    }
}
