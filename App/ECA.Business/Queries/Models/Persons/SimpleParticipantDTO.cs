using System;

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
        /// Gets or sets the is person participant type flag.
        /// </summary>
        public bool IsPersonParticipantType { get; set; }

        /// <summary>
        /// Gets or sets the name of the participant.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the date revised on.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the country the participant is located.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the city the participant is located.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the participant status
        /// </summary>
        public string ParticipantStatus { get; set; }

        /// <summary>
        /// Gets or sets the participant status id.
        /// </summary>
        public int? StatusId { get; set; }

        /// <summary>
        /// Gets or sets the participant Sevis Status
        /// </summary>
        public string SevisStatus { get; set; }

        /// <summary>
        /// Gets or sets the participant Sevis Status id
        /// </summary>
        public int? SevisStatusId { get; set; }
    }
}
