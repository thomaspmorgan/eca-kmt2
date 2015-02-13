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
    /// Represents the type of source or recipient of the money flow (Organization, Program, Project, Participant, ItineraryStop, Accomodation, Transportation)
    /// </summary>
    public class MoneyFlowSourceRecipientType
    {
        [Key]
        public int MoneyFlowSourceRecipientTypeId { get; set; }

        [Required]
        [MaxLength(20)]
        public string TypeName { get; set; }

        // relationships
        public ICollection<MoneyFlow> SourceTypes { get; set; }
        public ICollection<MoneyFlow> RecipientTypes { get; set; }

        public History History { get; set; }
    }
}
