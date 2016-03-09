using System;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// A SimpleParticipantDTO is used to represent a participant in the ECA system, whether that participant is an organization, or an individual.
    /// </summary>
    public class SimpleParticipantPersonDTO
    {
        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the participant's sevis id
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's Field of Study
        /// </summary>
        public string FieldOfStudy { get; set; }

        /// <summary>
        /// Gets or sets the participantPersons's Position
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's StudyProject
        /// </summary>
        public string StudyProject { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's ContactAgreement
        /// </summary>
        public bool ContactAgreement { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's ProgramCategory
        /// </summary>
        public string ProgramCategory { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's Host Institution (organization)
        /// </summary>
        public OrganizationDTO HostInstitution { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's Home Institution (organization)
        /// </summary>
        public OrganizationDTO HomeInstitution { get; set; }

        /// <summary>
        /// Gets or sets the host institition address id for this participant.
        /// </summary>
        public int? HostInstitutionAddressId { get; set; }

        /// <summary>
        /// Gets or sets the home institition address id for this participant.
        /// </summary>
        public int? HomeInstitutionAddressId { get; set; }

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
        /// Gets or sets the participant type id.
        /// </summary>
        public int ParticipantTypeId { get; set; }

        /// <summary>
        /// Gets or sets the participant status
        /// </summary>
        public string ParticipantStatus { get; set; }

        /// <summary>
        /// Gets or sets the participant status id.
        /// </summary>
        public int? ParticipantStatusId { get; set; }

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
