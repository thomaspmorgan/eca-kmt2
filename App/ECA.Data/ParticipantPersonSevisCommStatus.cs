using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Data
{
    /// <summary>
    /// The ParticipantPersonSevisCommStatus is used to indicate a step in the participant's sevis registration.
    /// </summary>
    public class ParticipantPersonSevisCommStatus
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Participant Id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the participant person.
        /// </summary>
        public ParticipantPerson ParticipantPerson { get; set; }

        /// <summary>
        /// Gets or sets the sevis comm status id.
        /// </summary>
        public int SevisCommStatusId { get; set; }

        /// <summary>
        /// Gets or sets the sevis comm status.
        /// </summary>
        public SevisCommStatus SevisCommStatus { get; set; }

        /// <summary>
        /// Gets or sets the added on date.
        /// </summary>
        public DateTimeOffset AddedOn { get; set; }

        /// <summary>
        /// Gets or sets the batch id associated with this comm status.
        /// </summary>
        public string BatchId { get; set; }
    }
}
