using ECA.Data;
using System;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An UpdatedParticipantPerson is used by a business layer client to update a person that is a project participant.
    /// </summary>
    public class UpdatedParticipantPersonSevis : IAuditable
    {
        /// <summary>
        /// A class to update a Participant Persons SEVIS info
        /// </summary>
        /// <param name="updater"></param>
        /// <param name="participantId"></param>
        /// <param name="sevisId"></param>
        /// <param name="isSentToSevisViaRTI"></param>
        /// <param name="isValidatedViaRTI"></param>
        /// <param name="isCancelled"></param>
        /// <param name="isDS2019Printed"></param>
        /// <param name="isNeedsUpdate"></param>
        /// <param name="isDS2019SentToTraveler"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public UpdatedParticipantPersonSevis(
            User updater, 
            int participantId, 
            string sevisId,
            bool isSentToSevisViaRTI,
            bool isValidatedViaRTI,
            bool isCancelled,
            bool isDS2019Printed,
            bool isNeedsUpdate,
            bool isDS2019SentToTraveler,
            DateTimeOffset? startDate,
            DateTimeOffset? endDate)
        {
            this.Audit = new Update(updater);
            this.ParticipantId = participantId;
            this.SevisId = sevisId;
            this.IsSentToSevisViaRTI = isSentToSevisViaRTI;
            this.IsValidatedViaRTI = isValidatedViaRTI;
            this.IsCancelled = isCancelled;
            this.IsDS2019Printed = isDS2019Printed;
            this.IsNeedsUpdate = isNeedsUpdate;
            this.IsDS2019SentToTraveler = isDS2019SentToTraveler;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        /// <summary>
        /// Gets the participant id.
        /// </summary>
        public int ParticipantId { get; private set; }

        /// <summary>
        /// Gets or sets the participant's sevis id
        /// </summary>
        public string SevisId { get; set; }

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
        /// Gets the update audit.
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
