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
        [Key]
        public int MoneyFlowId {get; set;}

        public MoneyFlowType MoneyFlowType {get; set;}
        [Required]
        public int MoneyFlowTypeId { get; set; }

        [Required]
        public decimal Value {get; set;}
       
        public MoneyFlowStatus MoneyFlowStatus { get; set;}
        [Required]
        public int MoneyFlowStatusId { get; set; }

        [Required]
        public DateTimeOffset TransactionDate { get; set; }
        [Required]
        public int FiscalYear { get; set; }

        [InverseProperty("SourceTypes")]
        [ForeignKey("SourceTypeId")]
        public MoneyFlowSourceRecipientType SourceType { get; set; }
        public int SourceTypeId { get; set; }
                
        [InverseProperty("RecipientTypes")]
        [ForeignKey("RecipientTypeId")]
        public MoneyFlowSourceRecipientType RecipientType { get; set; }
        public int RecipientTypeId { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        //relations

        public MoneyFlow Parent { get; set; }
        [InverseProperty("MoneyFlowSources")]
        [ForeignKey("SourceOrganizationId")]
        public Organization SourceOrganization { get; set; }
        public int? SourceOrganizationId { get; set; }
        [InverseProperty("MoneyFlowRecipients")]
        [ForeignKey("RecipientOrganizationId")]
        public Organization RecipientOrganization { get; set; }
        public int? RecipientOrganizationId { get; set; }

        [InverseProperty("SourceProgramMoneyFlows")]
        [ForeignKey("SourceProgramId")]
        public Program SourceProgram { get; set; }
        public int? SourceProgramId { get; set; }
        [InverseProperty("RecipientProgramMoneyFlows")]
        [ForeignKey("RecipientProgramId")]
        public Program RecipientProgram { get; set; }
        public int? RecipientProgramId { get; set; }

        [InverseProperty("SourceProjectMoneyFlows")]
        [ForeignKey("SourceProjectId")]
        public Project SourceProject { get; set; }
        public int? SourceProjectId { get; set; }
        [InverseProperty("RecipientProjectMoneyFlows")]
        [ForeignKey("RecipientProjectId")]
        public Project RecipientProject { get; set; }
        public int? RecipientProjectId { get; set; }

        [InverseProperty("SourceParticipantMoneyFlows")]
        [ForeignKey("SourceParticipantId")]
        public Participant SourceParticipant { get; set; }
        public int? SourceParticipantId { get; set; }
        [InverseProperty("RecipientParticipantMoneyFlows")]
        [ForeignKey("RecipientParticipantId")]
        public Participant RecipientParticipant { get; set; }
        public int? RecipientParticipantId { get; set; }

        [InverseProperty("SourceItineraryStopMoneyFlows")]
        [ForeignKey("SourceItineraryStopId")]
        public ItineraryStop SourceItineraryStop { get; set; }
        public int? SourceItineraryStopId { get; set; }
        [InverseProperty("RecipientItineraryStopMoneyFlows")]
        [ForeignKey("RecipientItineraryStopId")]
        public ItineraryStop RecipientItineraryStop { get; set; }
        public int? RecipientItineraryStopId { get; set; }

        [InverseProperty("RecipientTransportationExpenses")]
        [ForeignKey("RecipientTransportationId")]
        public Transportation RecipientTransportation { get; set; }
        public int? RecipientTransportationId { get; set; }

        [InverseProperty("RecipientAccommodationExpenses")]
        [ForeignKey("RecipientAccommodationId")]
        public Accommodation RecipientAccommodation { get; set; }
        public int? RecipientAccommodationId { get; set; }

        public History History { get; set; }

    }
}
