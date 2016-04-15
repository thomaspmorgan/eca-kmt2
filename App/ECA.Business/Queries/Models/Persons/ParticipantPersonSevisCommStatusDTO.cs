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
    public class ParticipantPersonSevisCommStatusDTO
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; } 
        
        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the project id the participant belongs to.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the SevisCommStatus id.
        /// </summary>
        public int SevisCommStatusId { get; set; }

        /// <summary>
        /// Gets or sets the SEVIS Communication Status name.
        /// </summary>
        public string SevisCommStatusName { get; set; }

        /// <summary>
        /// Gets or sets the dated added.
        /// </summary>
        public DateTimeOffset AddedOn { get; set; }

        /// <summary>
        /// Gets or sets the batch id.
        /// </summary>
        public string BatchId { get; set; }

        /// <summary>
        /// Gets or sets the sevis org id for this status.
        /// </summary>
        public string SevisOrgId { get; set; }

        /// <summary>
        /// Gets or sets the principal id of the principal that created the status.
        /// </summary>
        public int? PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the principal that created the status.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the principal that created the status.
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
