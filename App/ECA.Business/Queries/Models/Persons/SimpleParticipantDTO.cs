using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// A SimpleParticipantDTO is used to represent a participant in the ECA system, whether that participant is an organization, or an individual.
    /// </summary>
    public class SimpleParticipantDTO
    {
        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the Person Id.
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or sets the organization id.
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantTypeId { get; set; }

        /// <summary>
        /// Gets or sets the participant type.
        /// </summary>
        public string ParticipantType { get; set; }

        /// <summary>
        /// Gets or sets the name of the participant.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the participant's sevis id
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Gets or sets the participant's status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the participant's last status date
        /// </summary>
        public DateTimeOffset? StatusDate { get; set; }

        /// <summary>
        /// Gets or sets the date revised on.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }
    }
}
