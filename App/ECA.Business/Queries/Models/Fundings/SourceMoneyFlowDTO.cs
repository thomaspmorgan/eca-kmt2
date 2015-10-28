using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Fundings
{
    /// <summary>
    /// A SourceMoneyFlowDTO is a dto representing an incoming money flow to an entity (EntityId and EntityTypeId)
    /// from a source entity.
    /// </summary>
    public class SourceMoneyFlowDTO
    {
        /// <summary>
        /// Gets or sets the id of the money flow.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the entity id of the entity that is the recipient of this money flow.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity type id of the entity that is the recipient of this money flow.
        /// </summary>
        public int EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the source name.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// Gets or sets the source entity type id.
        /// </summary>
        public int SourceEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the source entity type name.
        /// </summary>
        public string SourceEntityTypeName { get; set; }

        /// <summary>
        /// Gets or sets the source entity id.
        /// </summary>
        public int? SourceEntityId { get; set; }

        /// <summary>
        /// Gets or sets the remaining amount.
        /// </summary>
        public decimal RemainingAmount { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the fiscal year.
        /// </summary>
        public int FiscalYear { get; set; }

        /// <summary>
        /// Gets or sets the child money flow ids i.e. the money flows whose source is the money flow equal to this dto's id.
        /// </summary>
        public IEnumerable<int> ChildMoneyFlowIds { get; set; }
    }
}
