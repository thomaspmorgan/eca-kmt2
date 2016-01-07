
namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// A ParticipantStudentVisitorDTO is used to represent a participant that is a Student Visitor in the ECA system and their associated related information.
    /// </summary>
    public class ParticipantExchangeVisitorDTO
    {
        public ParticipantExchangeVisitorDTO()
        {

        }
        /// <summary>
        /// Gets or sets the students id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the project id,
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's Field of Study Id
        /// </summary>
        public int? FieldOfStudyId { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's Field of Study
        /// </summary>
        public string FieldOfStudy { get; set; }

        /// <summary>
        /// Gets or sets the participantPersons's Position Id
        /// </summary>
        public int? PositionId { get; set; }

        /// <summary>
        /// Gets or sets the participantPersons's Position Code
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// Gets or sets the participantPersons's Position
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's ProgramCategory Id
        /// </summary>
        public int? ProgramCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's ProgramCategory Code
        /// </summary>
        public string ProgramCategoryCode { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's ProgramCategory
        /// </summary>
        public string ProgramCategory { get; set; }

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
        /// Id of another U.S. government agency
        /// </summary>
        public int? GovtAgency1Id { get; set; }

        /// <summary>
        /// Name of another U.S. government agency
        /// </summary>
        public string GovtAgency1Name { get; set; }

        /// <summary>
        /// Other Name of another U.S. government agency
        /// </summary>
        public string GovtAgency1OtherName { get; set; }

        /// <summary>
        /// Funding from another U.S. government agency
        /// </summary>
        public decimal? FundingGovtAgency2 { get; set; }

        /// <summary>
        /// Id of  another U.S. government agency
        /// </summary>
        public int? GovtAgency2Id { get; set; }

        /// <summary>
        /// Name of another U.S. government agency
        /// </summary>
        public string GovtAgency2Name { get; set; }

        /// <summary>
        /// Other Name of another U.S. government agency
        /// </summary>
        public string GovtAgency2OtherName { get; set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal? FundingIntlOrg1 { get; set; }

        /// <summary>
        /// id of another International org
        /// </summary>
        public int? IntlOrg1Id { get; set; }

        /// <summary>
        /// Name of another International org
        /// </summary>
        public string IntlOrg1Name { get; set; }

        /// <summary>
        /// Other Name of another International org
        /// </summary>
        public string IntlOrg1OtherName { get; set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal? FundingIntlOrg2 { get; set; }

        /// <summary>
        /// Id of another International org
        /// </summary>
        public int? IntlOrg2Id { get; set; }

        /// <summary>
        /// Name of another International org
        /// </summary>
        public string IntlOrg2Name { get; set; }

        /// <summary>
        /// Other Name of another International org
        /// </summary>
        public string IntlOrg2OtherName { get; set; }

        /// <summary>
        /// Funding from other source
        /// </summary>
        public decimal? FundingOther { get; set; }

        /// <summary>
        /// Name of  other org
        /// </summary>
        public string OtherName { get; set; }

        /// <summary>
        /// Total funding
        /// </summary>
        public decimal? FundingTotal { get; set; }

    }
}
