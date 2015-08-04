using ECA.Business.Service;
using ECA.Business.Service.Fundings;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Fundings
{
    /// <summary>
    /// An UpdatedMoneyFlowBindingModel is used to update the system's money flow with the given updated values.
    /// </summary>
    public class UpdatedMoneyFlowBindingModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the money flow status id.
        /// </summary>
        public int MoneyFlowStatusId { get; set; }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        public DateTimeOffset TransactionDate { get; set; }

        /// <summary>
        /// Gets or sets the fiscal year.
        /// </summary>
        public int FiscalYear { get; set; }

        /// <summary>
        /// Returns a business instance of the UpdatedMoneyFlow.
        /// </summary>
        /// <param name="user">The user doing the update.</param>
        /// <param name="sourceEntityId">The source entity id.</param>
        /// <returns>The business entity to update a money flow.</returns>
        public UpdatedMoneyFlow ToUpdatedMoneyFlow(User user, int sourceEntityId)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return new UpdatedMoneyFlow(
                updator: user, 
                id: this.Id, 
                sourceEntityId: sourceEntityId, 
                description: this.Description, 
                value: this.Value, 
                moneyFlowStatusId: this.MoneyFlowStatusId, 
                transactionDate: this.TransactionDate, 
                fiscalYear: this.FiscalYear);
        }
    }
}