using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Gets or sets the participantPerson's ProgramSubject
        /// </summary>
        public string ProgramSubject { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's Host Institution (organization)
        /// </summary>
        public SimpleOrganizationDTO HostInstitution { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's Home Institution (organization)
        /// </summary>
        public SimpleOrganizationDTO HomeInstitution { get; set; }

        /// <summary>
        /// Gets or sets the date revised on.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }
    }
}
