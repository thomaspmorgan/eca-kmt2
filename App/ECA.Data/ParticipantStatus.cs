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
        public string Status { get; set; }

        public History History { get; set; }

        /// <summary>
        /// collection of Participants that share this status
        /// </summary>
        public ICollection<Participant> Participants { get; set; }
    }
}
