using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// A discrete transfer of money to or from a program, project or participant. 
    /// Money Flows can be subdivided into children which can be allocated to projects, programs or participants. 
    /// For example, an allocation to the Fullbright program may be subdivided into the Fullbright sub-programs and again into those program's projects.
    /// </summary>
    public class MoneyFlow : IHistorical
    {
        /// <summary>
        /// The max length of the money flow description.
        /// </summary>
        public const int DESCRIPTION_MAX_LENGTH = 255;

        /// <summary>
        /// The max length of the money flow grant number.
        /// </summary>
        public const int GRANT_NUMBER_MAX_LENGTH = 25;

        /// <summary>
        /// Creates a new money flow instance.
        /// </summary>
        public MoneyFlow()
        {
            this.History = new History();
            this.Children = new HashSet<MoneyFlow>();
        }

        /// <summary>
        /// Gets or sets the money flow id.
        /// </summary>
        [Key]
        public int MoneyFlowId { get; set; }

        /// <summary>
        /// Gets or sets the money flow type.
        /// </summary>
        public MoneyFlowType MoneyFlowType { get; set; }

        /// <summary>
        /// Gets or sets whether the money flow is considered direct, if false, it's considered in-kind.
        /// </summary>
        public bool IsDirect { get; set; }

        /// <summary>
        /// Gets or sets the money flow type id.
        /// </summary>
        [Required]
        public int MoneyFlowTypeId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Required]
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the money flow status.
        /// </summary>
        public MoneyFlowStatus MoneyFlowStatus { get; set; }

        /// <summary>
        /// Gets or sets the money flow status id.
        /// </summary>
        [Required]
        public int MoneyFlowStatusId { get; set; }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        [Required]
        public DateTimeOffset TransactionDate { get; set; }

        /// <summary>
        /// Gets or sets the fiscal year.
        /// </summary>
        [Required]
        public int FiscalYear { get; set; }

        /// <summary>
        /// Gets or sets the source type.
        /// </summary>
        [InverseProperty("SourceTypes")]
        [ForeignKey("SourceTypeId")]
        public MoneyFlowSourceRecipientType SourceType { get; set; }

        /// <summary>
        /// Gets or sets the source type id.
        /// </summary>
        public int SourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the recipient type.
        /// </summary>
        [InverseProperty("RecipientTypes")]
        [ForeignKey("RecipientTypeId")]
        public MoneyFlowSourceRecipientType RecipientType { get; set; }

        /// <summary>
        /// Gets or sets the recipient type id.
        /// </summary>
        public int RecipientTypeId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [MaxLength(DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the grant number.
        /// </summary>
        [MaxLength(GRANT_NUMBER_MAX_LENGTH)]
        public string GrantNumber { get; set; }

        /// <summary>
        /// Gets or sets the parent money flow id.
        /// </summary>
        [Column("Parent_MoneyFlowId")]
        public int? ParentMoneyFlowId { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        [ForeignKey("ParentMoneyFlowId")]
        public MoneyFlow Parent { get; set; }

        /// <summary>
        /// Gets or sets the source organization.
        /// </summary>
        [InverseProperty("MoneyFlowSources")]
        [ForeignKey("SourceOrganizationId")]
        public Organization SourceOrganization { get; set; }

        /// <summary>
        /// Gets or sets the source organization id.
        /// </summary>
        public int? SourceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the recipient organization.
        /// </summary>
        [InverseProperty("MoneyFlowRecipients")]
        [ForeignKey("RecipientOrganizationId")]
        public Organization RecipientOrganization { get; set; }

        /// <summary>
        /// Gets or sets the recipient organization id.
        /// </summary>
        public int? RecipientOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the source program.
        /// </summary>
        [InverseProperty("SourceProgramMoneyFlows")]
        [ForeignKey("SourceProgramId")]
        public Program SourceProgram { get; set; }

        /// <summary>
        /// Gets or sets the source program id.
        /// </summary>
        public int? SourceProgramId { get; set; }

        /// <summary>
        /// Gets or sets the recipient program.
        /// </summary>
        [InverseProperty("RecipientProgramMoneyFlows")]
        [ForeignKey("RecipientProgramId")]
        public Program RecipientProgram { get; set; }

        /// <summary>
        /// Gets or sets the recipient program id.
        /// </summary>
        public int? RecipientProgramId { get; set; }

        /// <summary>
        /// Gets or sets the source project.
        /// </summary>
        [InverseProperty("SourceProjectMoneyFlows")]
        [ForeignKey("SourceProjectId")]
        public Project SourceProject { get; set; }

        /// <summary>
        /// Gets or sets the source project id.
        /// </summary>
        public int? SourceProjectId { get; set; }

        /// <summary>
        /// Gets or sets the recipient project.
        /// </summary>
        [InverseProperty("RecipientProjectMoneyFlows")]
        [ForeignKey("RecipientProjectId")]
        public Project RecipientProject { get; set; }

        /// <summary>
        /// Gets or sets the recipient project id.
        /// </summary>
        public int? RecipientProjectId { get; set; }

        /// <summary>
        /// Gets or sets the source participant.
        /// </summary>
        [InverseProperty("SourceParticipantMoneyFlows")]
        [ForeignKey("SourceParticipantId")]
        public Participant SourceParticipant { get; set; }

        /// <summary>
        /// Gets or sets the source participant id.
        /// </summary>
        public int? SourceParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the recipient participant.
        /// </summary>
        [InverseProperty("RecipientParticipantMoneyFlows")]
        [ForeignKey("RecipientParticipantId")]
        public Participant RecipientParticipant { get; set; }

        /// <summary>
        /// Gets or sets the recipient participant id.
        /// </summary>
        public int? RecipientParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the source itinerary stop.
        /// </summary>
        [InverseProperty("SourceItineraryStopMoneyFlows")]
        [ForeignKey("SourceItineraryStopId")]
        public ItineraryStop SourceItineraryStop { get; set; }

        /// <summary>
        /// Gets or sets the source itinerary stop id.
        /// </summary>
        public int? SourceItineraryStopId { get; set; }

        /// <summary>
        /// Gets or sets the recipient itinerary stop.
        /// </summary>
        [InverseProperty("RecipientItineraryStopMoneyFlows")]
        [ForeignKey("RecipientItineraryStopId")]
        public ItineraryStop RecipientItineraryStop { get; set; }

        /// <summary>
        /// Gets or sets the recipient itinerary stop id.
        /// </summary>
        public int? RecipientItineraryStopId { get; set; }

        [InverseProperty("RecipientTransportationExpenses")]
        [ForeignKey("RecipientTransportationId")]
        public Transportation RecipientTransportation { get; set; }

        /// <summary>
        /// Gets or sets the recipient transportation id.
        /// </summary>
        public int? RecipientTransportationId { get; set; }

        /// <summary>
        /// Gets or sets recipient accomdation.
        /// </summary>
        [InverseProperty("RecipientAccommodationExpenses")]
        [ForeignKey("RecipientAccommodationId")]
        public Accommodation RecipientAccommodation { get; set; }

        /// <summary>
        /// Get or sets recipient accomdation id.
        /// </summary>
        public int? RecipientAccommodationId { get; set; }

        /// <summary>
        /// Gets or sets the children money flows.
        /// </summary>
        [ForeignKey("ParentMoneyFlowId")]
        public virtual ICollection<MoneyFlow> Children { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }
    }
}
