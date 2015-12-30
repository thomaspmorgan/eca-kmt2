using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// A ParticipantPersonSevisDTO is used to represent a person participant in the ECA system and their associated Sevis related information.
    /// </summary>
    public class ParticipantPersonSevisDTO
    {
        public ParticipantPersonSevisDTO()
        {
            SevisCommStatuses = new List<ParticipantPersonSevisCommStatusDTO>();
        }
        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the participant's sevis id
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's Host Institution (organization)
        /// </summary>
        public InstitutionDTO HostInstitution { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's Home Institution (organization)
        /// </summary>
        public InstitutionDTO HomeInstitution { get; set; }

        /// <summary>
        /// Gets or sets the date revised on.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId of this participant.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Get or sets the participant type
        /// </summary>
        public string ParticipantType { get; set; }

        /// <summary>
        /// Gets or sets the participant status
        /// </summary>
        public string ParticipantStatus { get; set; }

        /// <summary>
        /// has the participant been sent to Sevis via RTI (manual web interface)
        /// </summary>
        public bool IsSentToSevisViaRTI { get; set; }

        /// <summary>
        /// has the participant been validated via RTI (manual web interface)
        /// </summary>
        public bool IsValidatedViaRTI { get; set; }

        /// <summary>
        /// has the participant been cancelled
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// has the DS2019 been printed
        /// </summary>
        public bool IsDS2019Printed { get; set; }

        /// <summary>
        /// does the participant need updating in Sevis (previous Sevis data sent has been changed)
        /// </summary>
        public bool IsNeedsUpdate { get; set; }

        /// <summary>
        /// has the DS2019 been sent to the traveler
        /// </summary>
        public bool IsDS2019SentToTraveler { get; set; }

        /// <summary>
        /// the start date of the visit
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        /// The end date of the visit
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// List of Sevis Communication Statuses for this participant
        /// </summary>
        public IEnumerable<ParticipantPersonSevisCommStatusDTO> SevisCommStatuses { get; set; }

        /// <summary>
        /// the most recent date of a batch status record being written for this participant
        /// </summary>
        public DateTimeOffset? LastBatchDate { get; set; }

    }
}
