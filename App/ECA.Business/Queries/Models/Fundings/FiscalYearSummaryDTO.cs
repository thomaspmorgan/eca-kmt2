using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Fundings
{
    public class SimpleFiscalYearSummaryDTO
    {
        public int FiscalYear { get; set; }

        public decimal Amount { get; set; }

        public int StatusId { get; set; }

        public int EntityId { get; set; }

        public int EntityTypeId { get; set; }
    }

    public class FiscalYearSummaryDTO
    {
        public int FiscalYear { get; set; }

        public int EntityId { get; set; }

        public int EntityTypeId { get; set; }

        public decimal IncomingAmount { get; set; }

        public decimal OutgoingAmount { get; set; }

        public decimal RemainingAmount { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }
    }
}
