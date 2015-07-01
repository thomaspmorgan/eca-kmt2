using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    public class MoneyFlowDTO
    {
        public int Id { get; set; }
        public DateTimeOffset TransactionDate { get; set; }

        public string SourceType { get; set; }
        public string Type { get; set; }
        public string FromTo { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public int FiscalYear { get; set; }
    }
}
