using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models
{
    public class MoneyFlowDTO
    {

        public int MoneyFlowId { get; set; }

        public string MoneyFlowType { get; set; }

        public int MoneyFlowTypeId { get; set; }


        public float Value { get; set; }

        public string MoneyFlowStatus { get; set; }
        public int MoneyFlowStatusId { get; set; }

        public DateTimeOffset TransactionDate { get; set; }
       
        public int FiscalYear { get; set; }


        public string SourceType { get; set; }
        public int SourceTypeId { get; set; }

        public string RecipientType { get; set; }
        public int RecipientTypeId { get; set; }

        public string Description { get; set; }

        //relations

        public MoneyFlowDTO Parent { get; set; }

        public string SourceName { get; set; }
        public int? SourceId { get; set; }

        public string RecipientName { get; set; }
        public int? RecipientId { get; set; }


        public HistoryDTO History { get; set; }
    }
}