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
        public MoneyFlowType MoneyFlowType {get; set;}
        public virtual MoneyFlow Parent {get; set;}
        [Required]
        [InverseProperty("MoneyFlowSources")]
        public virtual Organization Source {get; set;}
        [Required]
        [InverseProperty("MoneyFlowRecipients")]
        public virtual Organization Recipient {get; set;}
        [Required]
        public float Value {get; set;}
        [Required]
        public MoneyFlowStatus MoneyFlowStatus { get; set;}

        //relations
        public int ProgramId { get; set; }
        public virtual ECA.Data.Program Program { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public int PersonId { get; set; }
        public virtual Person Person {get; set;}

        public int ItineraryStopId { get; set; }
        public virtual ItineraryStop ItineraryStop { get; set; }

        public int AccommodationId { get; set; }
        public virtual Accommodation Accommodation { get; set; }

        public History History { get; set; }

    }
}
