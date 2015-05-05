using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class MoneyFlowValidationEntity
    {

        public MoneyFlowValidationEntity(string description, float value, DateTimeOffset transactionDate)
        {
            this.Description = description;
        }

        public string Description { get; private set; }

        public float Value { get; private set; }

        public DateTimeOffset TransactionDate { get; set; }

    }
}

