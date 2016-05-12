using ECA.Business.Exceptions;
using ECA.Data;
using System;
using System.Linq;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// A ParticipantPersonSevisDTO is used to represent a person participant in the ECA system and their associated Sevis related information.
    /// </summary>
    public class ParticipantPersonSevisDTO
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
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Get or sets the participant type
        /// </summary>
        public string ParticipantType { get; set; }

        /// <summary>
        /// Gets or sets the participant type id
        /// </summary>
        public int? ParticipantTypeId { get; set; }

        /// <summary>
        /// Gets or sets the participant status
        /// </summary>
        public string ParticipantStatus { get; set; }

        /// <summary>
        /// Gets or sets the participant status id.
        /// </summary>
        public int? ParticipantStatusId { get; set; }

        /// <summary>
        /// has the participant been sent to Sevis via RTI (manual web interface)
        /// </summary>
        public bool IsSentToSevisViaRTI { get; set; }

        /// <summary>
        /// has the participant been validated via RTI (manual web interface)
        /// </summary>
        public bool IsValidatedViaRTI { get; set; }

        /// <summary>
        /// Gets or sets the flag
        /// </summary>
        public bool IsCreatedViaBatch { get; set; }

        /// <summary>
        /// Gets or sets the flag
        /// </summary>
        public bool IsValidatedViaBatch { get; set; }

        /// <summary>
        /// has the participant been cancelled
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// has the DS2019 been printed
        /// </summary>
        public bool IsDS2019Printed { get; set; }
        
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
        /// The most recent date of a batch status record being written for this participant
        /// </summary>
        public DateTimeOffset? LastBatchDate { get; set; }

        /// <summary>
        /// The most recent participant Sevis valiation result
        /// </summary>
        public string SevisValidationResult { get; set; }

        /// <summary>
        /// The most recent participant Sevis batch submission result
        /// </summary>
        public string SevisBatchResult { get; set; }

        /// <summary>
        /// Gets or sets the participant Sevis Status
        /// </summary>
        public string SevisStatus { get; set; }

        /// <summary>
        /// Gets or sets the participant Sevis Status id
        /// </summary>
        public int? SevisStatusId { get; set; }

        /// <summary>
        /// Gets or sets flag for ds 2019
        /// </summary>
        public bool HasDS2019 { get; set; }
    }
    
    public static class Validation
    {
        public static void ValidateSevisLock(this ParticipantPersonSevisDTO participant)
        {
            if (participant != null && participant.SevisStatusId.HasValue)
            {
                if (participant != null && Participant.LOCKED_SEVIS_COMM_STATUSES.Contains((int)participant.SevisStatusId))
                {
                    var msg = String.Format("An update was attempted on participant with id [{0}] but should have failed validation.",
                            participant.ParticipantId);

                    throw new EcaBusinessException(msg);
                }
            }
        }
    }


}
