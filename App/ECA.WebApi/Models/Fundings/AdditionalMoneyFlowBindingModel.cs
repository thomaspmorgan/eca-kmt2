using ECA.Business.Models.Fundings;
using ECA.Business.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Fundings
{
    /// <summary>
    /// An AdditionalMoneyFlowBindingModel is used to represent a client's request
    /// to add a new money flow to the eca system.
    /// </summary>
    /// <typeparam name="T">The money flow entity type.</typeparam>
    public abstract class AdditionalMoneyFlowBindingModel<T> where T : class
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required]
        [MaxLength(MoneyFlow.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        public DateTimeOffset TransactionDate { get; set; }

        /// <summary>
        /// Gets or sets the fiscal year.
        /// </summary>
        public int FiscalYear { get; set; } 

        /// <summary>
        /// Gets or sets the recipient type by id, i.e. Post, Expense, etc.
        /// </summary>
        public int RecipientTypeId { get; set; }

        /// <summary>
        /// Gets or sets the money flow status id.
        /// </summary>
        public int MoneyFlowStatusId { get; set; }

        /// <summary>
        /// Gets or sets the recipient entity id, i.e. the project id, program id, etc.
        /// </summary>
        public int? RecipientEntityId { get; set; }
        
        /// <summary>
        /// Returns the source entity type id.
        /// </summary>
        /// <returns>The source entity type id, i.e. Project, Post, Organization, etc.</returns>
        public abstract int GetSourceTypeId();

        /// <summary>
        /// Returns the source entity id, i.e. the project id, program id, etc.
        /// </summary>
        /// <returns>The source entity id, i.e. the project id, program id, etc.</returns>
        public abstract int GetSourceEntityId();

        /// <summary>
        /// Returns an AdditionalMoneyFlow business object instance to add a new money flow.
        /// </summary>
        /// <param name="user">The user creating the money flow.</param>
        /// <returns>The additional money flow business instance.</returns>
        public AdditionalMoneyFlow ToAdditionalMoneyFlow(User user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return new AdditionalMoneyFlow(
                createdBy: user,
                description: this.Description,
                value: this.Value,
                moneyFlowStatusId: this.MoneyFlowStatusId,
                transactionDate: this.TransactionDate,
                fiscalYear: this.FiscalYear,
                sourceEntityId: this.GetSourceEntityId(),
                recipientEntityId: this.RecipientEntityId,
                sourceEntityTypeId: this.GetSourceTypeId(),
                recipientEntityTypeId: this.RecipientTypeId
                );
        }
    }
}