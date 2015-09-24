using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ECA.Data
{
    public class ParticipantPerson
    {

        /// <summary>
        /// Gets the max length of the SEVIS Id.
        /// </summary>
        private const int SEVIS_ID_MAX_LENGTH = 11;

        /// <summary>
        /// Gets the max length of a Study Project
        /// </summary>
        private const int STUDY_PROJECT_MAX_LENGTH = 250;

        /// <summary>
        /// constructor to initialize history for a ParticipantPerson
        /// </summary>
        public ParticipantPerson()
        {
            this.History = new History();
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
        /// The study project for the student (if student)
        /// </summary>
        [MaxLength(STUDY_PROJECT_MAX_LENGTH)]
        public string StudyProject {get; set;}

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
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// The end date of the visit
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// Funding coming from the sponsor
        /// </summary>
        public decimal FundingSponsor { get; set; }

        /// <summary>
        /// Funding coming from the visitor
        /// </summary>
        public decimal FundingPersonal { get; set; }

        /// <summary>
        /// Funding from the visiting participant's government
        /// </summary>
        public decimal FundingVisGovt { get; set; }

        /// <summary>
        /// Funding from the visiting participant's BNC
        /// </summary>
        public decimal FundingVisBNC { get; set; }

        /// <summary>
        /// Funding from another U.S. government agency
        /// </summary>
        public decimal FundingGovtAgency1 { get; set; }

        /// <summary>
        /// Funding from another U.S. government agency
        /// </summary>
        public decimal FundingGovtAgency2 { get; set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal FundingIntlOrg1 { get; set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal FundingIntlOrg2 { get; set; }

        /// <summary>
        /// Funding from other source
        /// </summary>
        public decimal FundingOther { get; set; }

        /// <summary>
        /// Total funding
        /// </summary>
        public decimal FundingTotal { get; set; }

        //Relationships

        /// <summary>
        /// Foreign key to Field of Study
        /// </summary>
        public int? FieldOfStudyId { get; set; }

        /// <summary>
        /// Navigation property for Field of Study
        /// </summary>
        public FieldOfStudy FieldOfStudy { get; set; }

        /// <summary>
        /// Foreign Key for ProgramCategory
        /// </summary>
        public int? ProgramCategoryId { get; set; }

        /// <summary>
        /// Foreign Key For 
        /// </summary>
        public ProgramCategory ProgramCategory { get; set; }

        /// <summary>
        /// Foreign Key for Position
        /// </summary>
        public int? PositionId { get; set; }

        /// <summary>
        /// Navigation Property for Position
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// navigation property for HostInstitution
        /// </summary>
        public Organization HostInstitution {get; set;}

        /// <summary>
        /// Foreign Key for HostInstitution
        /// </summary>
        public int? HostInstitutionId {get; set;}

        /// <summary>
        /// navigation property for HomeInstitution
        /// </summary>
        public Organization HomeInstitution {get; set;}

        /// <summary>
        /// Foreign Key for HomeInstitution
        /// </summary>
        public int? HomeInstitutionId {get; set;}

        /// <summary>
        /// Reference to the participant record
        /// </summary>
        public Participant Participant { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public ICollection<SevisCommStatus> SevisCommStatuses { get; set; }

        /// <summary>
        /// create/update time and user
        /// </summary>
        public History History { get; set; }
    }
}
