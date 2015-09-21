using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// A MoneyFlowSourceRecipientTypeSetting is a configuration detailing a money flow source recipient type
    /// and it's peer money flow source type that can either be a source or a recipient.
    /// </summary>
    [Table("MoneyFlowSourceRecipientTypeSettings")]
    public class MoneyFlowSourceRecipientTypeSetting
    {
        /// <summary>
        /// The name of the MoneyFlowSourceRecipientTypeId foreign key.
        /// </summary>
        public const string MONEY_FLOW_SOURCE_RECIPIENT_TYPE_ID_FOREIGN_KEY_NAME = "MoneyFlowSourceRecipientTypeId";

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the money flow source recipient type id.
        /// </summary>
        public int MoneyFlowSourceRecipientTypeId { get; set; }

        /// <summary>
        /// Gets or sets the peer money flow source recipient type id.
        /// </summary>
        public int PeerMoneyFlowSourceRecipientTypeId { get; set; }

        /// <summary>
        /// Gets or sets whether the peer money flow source type is a source to the money flow source type.
        /// </summary>
        public bool IsSource { get; set; }

        /// <summary>
        /// Gets or sets whether the peer money flow source type is a recipient to the money flow source type.
        /// </summary>
        public bool IsRecipient { get; set; }

        /// <summary>
        /// Gets or sets the money flow source type being configured.
        /// </summary>
        [ForeignKey(MONEY_FLOW_SOURCE_RECIPIENT_TYPE_ID_FOREIGN_KEY_NAME)]
        public virtual MoneyFlowSourceRecipientType MoneyFlowSourceRecipientType { get; set; }

        /// <summary>
        /// Gets or sets the peer money flow source type that is a source or recipient to the money flow source type.
        /// </summary>
        [ForeignKey("PeerMoneyFlowSourceRecipientTypeId")]
        public virtual MoneyFlowSourceRecipientType PeerMoneyFlowSourceRecipientType { get; set; }
    }
}
