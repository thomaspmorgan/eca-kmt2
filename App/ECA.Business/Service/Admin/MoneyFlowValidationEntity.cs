using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class MoneyFlowServiceCreateValidationEntity
    {

        public MoneyFlowServiceCreateValidationEntity(string description, float value, DateTimeOffset transactionDate)
        {
            this.Description = description;
            this.Value = value;
            this.TransactionDate = transactionDate;
        }

        public string Description { get; private set; }

        public float Value { get; private set; }

        public DateTimeOffset TransactionDate { get; private set; }

    }

        public class MoneyFlowServiceUpdateValidationEntity
    {

            public MoneyFlowServiceUpdateValidationEntity(string description, float value, DateTimeOffset transactionDate)
        {
            this.Description = description;
            this.Value = value;
            this.TransactionDate = transactionDate;
        }

        public string Description { get; private set; }

        public float Value { get; private set; }

        public DateTimeOffset TransactionDate { get; private set; }

    }
}

