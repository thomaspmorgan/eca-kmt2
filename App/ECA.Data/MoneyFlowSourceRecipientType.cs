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
    public partial class MoneyFlowSourceRecipientType
    {
        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public MoneyFlowSourceRecipientType()
        {
            this.History = new History();
            this.SourceTypes = new HashSet<MoneyFlow>();
            this.RecipientTypes = new HashSet<MoneyFlow>();
            this.MoneyFlowSourceRecipientTypeSettings = new HashSet<MoneyFlowSourceRecipientTypeSetting>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        public int MoneyFlowSourceRecipientTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type name.
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the source types.
        /// </summary>
        public virtual ICollection<MoneyFlow> SourceTypes { get; set; }

        /// <summary>
        /// Gets or sets the recipient types.
        /// </summary>
        public virtual ICollection<MoneyFlow> RecipientTypes { get; set; }

        /// <summary>
        /// Gets or sets the money flow source recipient type configurations.
        /// </summary>
        [ForeignKey(MoneyFlowSourceRecipientTypeSetting.MONEY_FLOW_SOURCE_RECIPIENT_TYPE_ID_FOREIGN_KEY_NAME)]
        public virtual ICollection<MoneyFlowSourceRecipientTypeSetting> MoneyFlowSourceRecipientTypeSettings { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }
    }
}
