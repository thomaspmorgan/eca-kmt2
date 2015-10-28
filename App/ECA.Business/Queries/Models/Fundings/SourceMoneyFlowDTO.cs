using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Fundings
{
    public class SourceMoneyFlowDTO
    {
        public int Id { get; set; }

        public int EntityId { get; set; }

        public int EntityTypeId { get; set; }

        public string SourceName { get; set; }

        public int SourceEntityTypeId { get; set; }

        public string SourceEntityTypeName { get; set; }

        public int? SourceEntityId { get; set; }

        public decimal RemainingAmount { get; set; }

        public decimal Amount { get; set; }

        public int FiscalYear { get; set; }

        public IEnumerable<int> ChildMoneyFlowIds { get; set; }
    }
}
