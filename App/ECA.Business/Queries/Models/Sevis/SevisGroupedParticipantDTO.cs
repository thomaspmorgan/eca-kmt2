﻿namespace ECA.Business.Queries.Models.Sevis
{
    /// <summary>
    /// A QueuedToSubmitParticipantDTO is used to find participants that should be used to create exchange visitors
    /// and be sent to sevis.
    /// </summary>
    public class SevisGroupedParticipantDTO
    {
        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the sevis id.
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Gets or sets the sevis comm status id.  Used to determine the action when staging.
        /// </summary>
        public int SevisCommStatusId { get; set; }
    }
}
