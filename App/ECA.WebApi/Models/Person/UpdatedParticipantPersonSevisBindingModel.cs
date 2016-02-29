using System;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for editing SEVIS info
    /// </summary>
    public class UpdatedParticipantPersonSevisBindingModel
    {
        /// <summary>
        /// Gets the participant id.
        /// </summary>
        public int ParticipantId { get;  set; }

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
        /// Convert binding model to business model 
        /// </summary>
        /// <param name="user">The user updating the membership</param>
        /// <returns>Update membership business model</returns>
        public UpdatedParticipantPersonSevis ToUpdatedParticipantPersonSevis(User user)
        {
            return new UpdatedParticipantPersonSevis(
                updater: user,
                participantId: this.ParticipantId,
                sevisId: this.SevisId,
                isSentToSevisViaRTI: this.IsSentToSevisViaRTI,
                isValidatedViaRTI: this.IsValidatedViaRTI,
                isCancelled: this.IsCancelled,
                isDS2019Printed: this.IsDS2019Printed,
                isNeedsUpdate: this.IsNeedsUpdate,
                isDS2019SentToTraveler: this.IsDS2019SentToTraveler,
                startDate: this.StartDate,
                endDate: this.EndDate
            );
        }
    }
}