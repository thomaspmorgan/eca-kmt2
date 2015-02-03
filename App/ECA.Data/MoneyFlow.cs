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
    public class MoneyFlow
    {
        [Key]
        public int MoneyFlowId {get; set;}
        [Required]
        public MoneyFlowType MoneyFlowType {get; set;}
        public MoneyFlow Parent {get; set;}
        [InverseProperty("MoneyFlowSources")]
        public Organization Source {get; set;}
        [InverseProperty("MoneyFlowRecipients")]
        public Organization Recipient {get; set;}
        [Required]
        public float Value {get; set;}
        [Required]
        public MoneyFlowStatus MoneyFlowStatus { get; set;}
        [Required]
        public DateTimeOffset TransactionDate { get; set; }
        [Required]
        public int FiscalYear { get; set; }

        //relations
        public int? ProgramId { get; set; }
        public ECA.Data.Program Program { get; set; }

        public int? ProjectId { get; set; }
        public Project Project { get; set; }

        public int? PersonId { get; set; }
        public Person Person {get; set;}

        public int? ItineraryStopId { get; set; }
        public ItineraryStop ItineraryStop { get; set; }

        public int? AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }

        public History History { get; set; }

    }
}
