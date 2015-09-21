using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Fundings
{
    /// <summary>
    /// The validation entity used in the money flow service for creating a new money flow.
    /// </summary>
    public class MoneyFlowServiceCreateValidationEntity
    {
        /// <summary>
        /// Creates a new instance with the given validaiton values.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        /// <param name="sourceEntityTypeId">The MoneyFlowSourceRecipientType id for the source entity.</param>
        /// <param name="sourceEntityId">The id of the source entity.</param>
        /// <param name="recipientEntityId">The id of the recipient entity.</param>
        /// <param name="recipientEntityTypeId">The MoneyFlowSourceRecipientType id for the recipient entity.</param>
        /// <param name="hasSourceEntityType">True, if the source entity has a mapped eca data entity.</param>
        /// <param name="hasRecipientEntityType">True, if the recipient entity has a mapped eca data entity.</param>
        /// <param name="transactionDate">The transaction date.</param>
        /// <param name="fiscalYear">The fiscal year.</param>
        /// <param name="allowedRecipientEntityTypeIds">The collection of MoneyFlowSourceRecipientEntityTypes by id that are allowed by the source and settings.</param>
        public MoneyFlowServiceCreateValidationEntity(
            string description,
            decimal value,
            int sourceEntityTypeId,
            int recipientEntityTypeId,
            IEnumerable<int> allowedRecipientEntityTypeIds,
            int? sourceEntityId,
            int? recipientEntityId,
            bool hasSourceEntityType,
            bool hasRecipientEntityType,
            DateTimeOffset transactionDate,
            int fiscalYear)
        {
            this.Description = description;
            this.Value = value;
            this.TransactionDate = transactionDate;
            this.HasRecipientEntityType = hasRecipientEntityType;
            this.HasSourceEntityType = hasSourceEntityType;
            this.SourceEntityId = sourceEntityId;
            this.RecipientEntityId = recipientEntityId;
            this.RecipientEntityTypeId = recipientEntityTypeId;
            this.SourceEntityTypeId = sourceEntityTypeId;
            this.FiscalYear = fiscalYear;
            this.AllowedRecipientEntityTypeIds = allowedRecipientEntityTypeIds.Distinct().ToList() ?? new List<int>();
        }

        /// <summary>
        /// Gets the fiscal year.
        /// </summary>
        public int FiscalYear { get; private set; }

        /// <summary>
        /// Gets the flag indicating the source entity has a mapped ECA.Data entity type.
        /// </summary>
        public bool HasSourceEntityType { get; private set; }

        /// <summary>
        /// Gets the flag indicating the recipient entity has a mapped ECA.Data entity type.
        /// </summary>
        public bool HasRecipientEntityType { get; private set; }

        /// <summary>
        /// Gets the id of the source entity.
        /// </summary>
        public int? SourceEntityId { get; private set; }

        /// <summary>
        /// Gets the allowed recipient entity type ids.
        /// </summary>
        public IEnumerable<int> AllowedRecipientEntityTypeIds { get; private set; }

        /// <summary>
        /// Gets the id of the recipient entity.
        /// </summary>
        public int? RecipientEntityId { get; private set; }

        /// <summary>
        /// Gets the source entity type (MoneyFlowSourceRecipientType) id.
        /// </summary>
        public int SourceEntityTypeId { get; private set; }

        /// <summary>
        /// Gets the recipient entity type (MoneyFlowSourceRecipientType) id.
        /// </summary>
        public int RecipientEntityTypeId { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public decimal Value { get; private set; }

        /// <summary>
        /// Gets the transaction date.
        /// </summary>
        public DateTimeOffset TransactionDate { get; private set; }
    }

    /// <summary>
    /// The MoneyFlowServiceUpdateValidationEntity is used to perform validation when updating a money flow entity.
    /// </summary>
    public class MoneyFlowServiceUpdateValidationEntity
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="description">The description of the money flow.</param>
        /// <param name="value">The value of the money flow.</param>
        /// <param name="fiscalYear">The fiscal year.</param>
        public MoneyFlowServiceUpdateValidationEntity(
            string description, 
            decimal value,
            int fiscalYear)
        {
            this.Description = description;
            this.Value = value;
            this.FiscalYear = fiscalYear;
        }

        /// <summary>
        /// Gets the fiscal year.
        /// </summary>
        public int FiscalYear { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public decimal Value { get; private set; }
    }
}

