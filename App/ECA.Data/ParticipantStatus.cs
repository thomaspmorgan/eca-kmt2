using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECA.Core.Generation;

namespace ECA.Data
{
    /// <summary>
    /// Participant Status describes the state of a person's journey through an itinerary, project, phase, etc.
    /// </summary>
    public partial class ParticipantStatus
    {
        /// <summary>
        /// The list of participant statuses that pertain to exchange visitor validation i.e. if the participant's status
        /// is in this list, then the participant's exchange visitor validation should be executed.
        /// </summary>
        public static readonly List<StaticLookup> EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES = new List<StaticLookup>
            {
                ParticipantStatus.Active,
                ParticipantStatus.Nominee,
                ParticipantStatus.Applicant,
                ParticipantStatus.Alternate,
                ParticipantStatus.Approved,
                ParticipantStatus.Intention,
                ParticipantStatus.Pending,
            }.OrderBy(x => x.Value).ToList();

        /// <summary>
        /// Creates a new ParticipantStatus and initializes the participants and history properties.
        /// </summary>
        public ParticipantStatus()
        {
            this.Participants = new HashSet<Participant>();
            this.History = new History();
        }

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
