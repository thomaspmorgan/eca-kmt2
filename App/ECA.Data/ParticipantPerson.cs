using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// A person participant on a project
    /// </summary>
    public class ParticipantPerson : IHistorical, IDS2019Fileable
    {
        /// <summary>
        /// The string to format for a participant's ds 2019 file name.
        /// </summary>
        public const string DS2019_FILE_NAME_FORMAT_STRING = "Participant_{0}_{1}.pdf";

        /// <summary>
        /// Gets the max length of the SEVIS Id.
        /// </summary>
        private const int SEVIS_ID_MAX_LENGTH = 11;

        /// <summary>
        /// constructor to initialize history for a ParticipantPerson
        /// </summary>
        public ParticipantPerson()
        {
            this.History = new History();
            ParticipantPersonSevisCommStatuses = new HashSet<ParticipantPersonSevisCommStatus>();
        }

        /// <summary>
        /// The key, and foreign key to the participant
        /// </summary>
        [Key]
        public int ParticipantId { get; set; }

        /// <summary>
        /// the SEVIS ID (assigned by SEVIS)
        /// </summary>
        [MaxLength(SEVIS_ID_MAX_LENGTH)]
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
        /// navigation property for HostInstitution
        /// </summary>
        public Organization HostInstitution { get; set; }

        /// <summary>
        /// Foreign Key for HostInstitution
        /// </summary>
        public int? HostInstitutionId { get; set; }

        /// <summary>
        /// navigation property for HomeInstitution
        /// </summary>
        public Organization HomeInstitution { get; set; }

        /// <summary>
        /// Foreign Key for HomeInstitution
        /// </summary>
        public int? HomeInstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the host institution address.
        /// </summary>
        [ForeignKey("HostInstitutionAddressId")]
        public virtual Address HostInstitutionAddress { get; set; }

        /// <summary>
        /// Gets or sets the host institution address id.
        /// </summary>
        public int? HostInstitutionAddressId { get; set; }

        /// <summary>
        /// Gets or sets the home institution address.
        /// </summary>
        [ForeignKey("HomeInstitutionAddressId")]
        public virtual Address HomeInstitutionAddress { get; set; }

        /// <summary>
        /// Gets or sets the home institution address id.
        /// </summary>
        public int? HomeInstitutionAddressId { get; set; }

        /// <summary>
        /// Reference to the participant record
        /// </summary>
        public Participant Participant { get; set; }

        /// <summary>
        /// The latest SEVIS participant verification results
        /// </summary>
        public string SevisValidationResult { get; set; }

        /// <summary>
        /// The most recent participant Sevis batch submission result
        /// </summary>
        public string SevisBatchResult { get; set; }

        /// <summary>
        /// Gets or sets the ds 2019 file url.
        /// </summary>
        public string DS2019FileName { get; set; }

        /// <summary>
        /// Collection of SEVIS communication statuses
        /// </summary>
        public ICollection<ParticipantPersonSevisCommStatus> ParticipantPersonSevisCommStatuses { get; set; }

        /// <summary>
        /// create/update time and user
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// Returns the DS2019 file name for the participant id and sevis id.
        /// </summary>
        /// <returns></returns>
        public string GetDS2019FileName()
        {
            return string.Format(DS2019_FILE_NAME_FORMAT_STRING, this.ParticipantId, this.SevisId);
        }
    }
}
