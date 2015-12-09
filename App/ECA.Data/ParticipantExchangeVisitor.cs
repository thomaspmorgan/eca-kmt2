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
    /// <summary>
    /// An exchange visitor
    /// </summary>
    public class ParticipantExchangeVisitor : IHistorical
    {

        /// <summary>
        /// Gets the max length of a Study Project
        /// </summary>
        private const int AGENCY_ORG_NAME_MAX_LENGTH = 60;

        /// <summary>
        /// constructor to initialize history for a ParticipantStudentVisitor
        /// </summary>
        public ParticipantExchangeVisitor()
        {
            this.History = new History();
        }

        /// <summary>
        /// The key, and foreign key to the participant
        /// </summary>
        [Key]
        public int ParticipantId { get; set; }

        /// <summary>
        /// Funding coming from the sponsor
        /// </summary>
        public decimal? FundingSponsor { get; set; }

        /// <summary>
        /// Funding coming from the visitor
        /// </summary>
        public decimal? FundingPersonal { get; set; }

        /// <summary>
        /// Funding from the visiting participant's government
        /// </summary>
        public decimal? FundingVisGovt { get; set; }

        /// <summary>
        /// Funding from the visiting participant's BNC
        /// </summary>
        public decimal? FundingVisBNC { get; set; }

        /// <summary>
        /// Funding from another U.S. government agency
        /// </summary>
        public decimal? FundingGovtAgency1 { get; set; }

        /// <summary>
        /// A U.S. government agency
        /// </summary>
        public USGovernmentAgency GovtAgency1 { get; set; }

        /// <summary>
        /// Id of another U.S. government agency
        /// </summary>
        public int? GovtAgency1Id { get; set; }

        /// <summary>
        /// Other Name of another U.S. government agency
        /// </summary>
        [MaxLength(AGENCY_ORG_NAME_MAX_LENGTH)]
        public string GovtAgency1OtherName { get; set; }

        /// <summary>
        /// Funding from another U.S. government agency
        /// </summary>
        public decimal? FundingGovtAgency2 { get; set; }

        /// <summary>
        /// Another U.S. government agency
        /// </summary>
        public USGovernmentAgency GovtAgency2 { get; set; }

        /// <summary>
        /// Id of  another U.S. government agency
        /// </summary>
        public int? GovtAgency2Id { get; set; }

        /// <summary>
        /// Other Name of another U.S. government agency
        /// </summary>
        [MaxLength(AGENCY_ORG_NAME_MAX_LENGTH)]
        public string GovtAgency2OtherName { get; set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal? FundingIntlOrg1 { get; set; }

        /// <summary>
        /// A International Organization (SEVIS)
        /// </summary>
        public InternationalOrganization IntlOrg1 { get; set; }

        /// <summary>
        /// id of another International org
        /// </summary>
        public int? IntlOrg1Id { get; set; }

        /// <summary>
        /// Other Name of another International org
        /// </summary>
        [MaxLength(AGENCY_ORG_NAME_MAX_LENGTH)]
        public string IntlOrg1OtherName { get; set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal? FundingIntlOrg2 { get; set; }

        /// <summary>
        /// A International Organization (SEVIS)
        /// </summary>
        public InternationalOrganization IntlOrg2 { get; set; }

        /// <summary>
        /// Id of another International org
        /// </summary>
        public int? IntlOrg2Id { get; set; }

        /// <summary>
        /// Other Name of another International org
        /// </summary>
        [MaxLength(AGENCY_ORG_NAME_MAX_LENGTH)]
        public string IntlOrg2OtherName { get; set; }

        /// <summary>
        /// Funding from other source
        /// </summary>
        public decimal? FundingOther { get; set; }

        /// <summary>
        /// Name of other source
        /// </summary>
        [MaxLength(AGENCY_ORG_NAME_MAX_LENGTH)]
        public string OtherName { get; set; }

        /// <summary>
        /// Total funding
        /// </summary>
        public decimal? FundingTotal { get; set; }

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
        /// create/update time and user
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// The associated participant record
        /// </summary>
        public Participant Participant { get; set; }

        /// <summary>
        /// Gets or sets the participant person.
        /// </summary>
        [ForeignKey("ParticipantId")]
        public virtual ParticipantPerson ParticipantPerson { get; set; }

    }
}
