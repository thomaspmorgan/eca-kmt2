﻿using ECA.Core.Exceptions;
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
    public class UpdatedMoneyFlow : EditedMoneyFlow, IAuditable
    {
        /// <summary>
        /// Creates a new UpdatedMoneyFlow object that can be used to update a Money Flow to the ECA system.
        /// </summary>
        /// <param name="updator">The updator.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        /// <param name="grantNumber">The grant number.</param>
        /// <param name="moneyFlowStatusId">The money flow status id.</param>
        /// <param name="transactionDate">The transaction date.</param>
        /// <param name="fiscalYear">The fiscal year.</param>
        /// <param name="id">The id of the money flow.</param>
        /// <param name="sourceOrRecipientEntityId">The source or recipient entity id, useful for security.</param>
        /// <param name="sourceOrRecipientEntityTypeId">The source or recipient entity type id, useful for security.</param>
        public UpdatedMoneyFlow(
            User updator,
            int id,
            int sourceOrRecipientEntityId,
            int sourceOrRecipientEntityTypeId,
            string description,
            string grantNumber,
            decimal value,
            int moneyFlowStatusId,
            DateTimeOffset transactionDate,
            int fiscalYear,
            bool isDirect)
            : base(id, sourceOrRecipientEntityId, sourceOrRecipientEntityTypeId)
        {
            Contract.Requires(updator != null, "The updated by user must not be null.");
            var moneyFlowStatus = MoneyFlowStatus.GetStaticLookup(moneyFlowStatusId);
            if (moneyFlowStatus == null)
            {
                throw new UnknownStaticLookupException(String.Format("The money flow status [{0}] is not supported.", moneyFlowStatusId));
            }
            this.Audit = new Update(updator);
            this.Description = description;
            this.Value = value;
            this.MoneyFlowStatusId = moneyFlowStatusId;
            this.TransactionDate = transactionDate;
            this.FiscalYear = fiscalYear;
            this.MoneyFlowTypeId = MoneyFlowType.Incoming.Id;
            this.GrantNumber = grantNumber;
            this.IsDirect = isDirect;
        }

        /// <summary>
        /// Gets the grant number.
        /// </summary>
        public string GrantNumber { get; private set; }

        /// <summary>
        /// Gets the audit.
        /// </summary>
        public Audit Audit { get; private set; }

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

        /// <summary>
        /// Gets or sets whether the money flow is direct, if false it's considered in-kind.
        /// </summary>
        public bool IsDirect { get; private set; }
    }
}
