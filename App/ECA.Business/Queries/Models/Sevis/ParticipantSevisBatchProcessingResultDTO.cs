
namespace ECA.Business.Queries.Models.Sevis
{
    public class ParticipantSevisBatchProcessingResultDTO
    {
        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId of this participant.
        /// </summary>
        public int ProjectId { get; set; }
        
        /// <summary>
        /// Sevis Communication Status for this participant
        /// </summary>
        public string SevisCommStatus { get; set; }
    }
}
