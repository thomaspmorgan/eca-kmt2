using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Fundings
{
    /// <summary>
    /// The UpdatedMoneyFlow allows a business layer client to update a money flow entry in the ECA system.
    /// </summary>
    public class UpdatedMoneyFlow : IAuditable
    {
        /// <summary>
        /// Creates a new UpdatedMoneyFlow object that can be used to update a Money Flow to the ECA system.
        /// </summary>
        /// <param name="updator">The updator.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        /// <param name="moneyFlowStatusId">The money flow status id.</param>
        /// <param name="transactionDate">The transaction date.</param>
        /// <param name="fiscalYear">The fiscal year.</param>
        /// <param name="id">The id of the money flow.</param>
        /// <param name="sourceEntityId">The source entity id.</param>
        public UpdatedMoneyFlow(
            User updator,
            int id,
            int sourceEntityId,
            string description,
            decimal value,
            int moneyFlowStatusId,
            DateTimeOffset transactionDate,
            int fiscalYear)
        {
            Contract.Requires(updator != null, "The updated by user must not be null.");
            var moneyFlowStatus = MoneyFlowStatus.GetStaticLookup(moneyFlowStatusId);
            if (moneyFlowStatus == null)
            {
                throw new UnknownStaticLookupException(String.Format("The money flow status [{0}] is not supported.", moneyFlowStatusId));
            }
            this.SourceEntityId = sourceEntityId;
            this.Id = id;
            this.Audit = new Update(updator);
            this.Description = description;
            this.Value = value;
            this.MoneyFlowStatusId = moneyFlowStatusId;
            this.TransactionDate = transactionDate;
            this.FiscalYear = fiscalYear;
            this.MoneyFlowTypeId = MoneyFlowType.Incoming.Id;
        }

        /// <summary>
        /// Gets the source entity id.
        /// </summary>
        public int SourceEntityId { get; private set; }

        /// <summary>
        /// Gets the audit.
        /// </summary>
        public Audit Audit { get; private set; }

        /// <summary>
        /// Gets the id of the money flow.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the transaction date.
        /// </summary>
        public DateTimeOffset TransactionDate { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public decimal Value { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the fiscal year.
        /// </summary>
        public int FiscalYear { get; private set; }

        /// <summary>
        /// Gets the money flow type id.
        /// </summary>
        public int MoneyFlowTypeId { get; private set; }

        /// <summary>
        /// Gets the money flow status id.
        /// </summary>
        public int MoneyFlowStatusId { get; private set; }
    }
}
