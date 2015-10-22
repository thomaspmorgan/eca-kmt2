using ECA.Business.Exceptions;
using ECA.Business.Service;
using ECA.Business.Validation;
using ECA.Core.Data;
using ECA.Core.Exceptions;
using ECA.Core.Generation;
using ECA.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace ECA.Business.Models.Fundings
{
    /// <summary>
    /// An AdditionalMoneyFlow is used to add a new money flow to the eca system.
    /// </summary>
    public class AdditionalMoneyFlow : IAuditable
    {
        /// <summary>
        /// Creates a new AdditionalMoneyFlow object that can be used to add Money Flow to the ECA system.
        /// </summary>
        /// <param name="createdBy">The creator.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        /// <param name="moneyFlowStatusId">The money flow status id.</param>
        /// <param name="transactionDate">The transaction date.</param>
        /// <param name="fiscalYear">The fiscal year.</param>
        /// <param name="parentMoneyFlowId">The parent money flow id.  This is the line item that this money flow is getting money from.</param>
        /// <param name="sourceEntityId">The source entity id.</param>
        /// <param name="recipientEntityId">The recipient entity id.</param>
        /// <param name="sourceEntityTypeId">The source entity type id.</param>
        /// <param name="recipientEntityTypeId">The recipient entity type id.</param>
        public AdditionalMoneyFlow(
            User createdBy,
            int? parentMoneyFlowId,
            string description,
            decimal value,
            int moneyFlowStatusId,
            DateTimeOffset transactionDate,
            int fiscalYear,
            int? sourceEntityId,
            int? recipientEntityId,
            int sourceEntityTypeId,
            int recipientEntityTypeId)
        {
            Contract.Requires(createdBy != null, "The created by user must not be null.");
            var moneyFlowStatus = MoneyFlowStatus.GetStaticLookup(moneyFlowStatusId);
            if (moneyFlowStatus == null)
            {
                throw new UnknownStaticLookupException(String.Format("The money flow status [{0}] is not supported.", moneyFlowStatusId));
            }
            var recipientEntityType = MoneyFlowSourceRecipientType.GetStaticLookup(recipientEntityTypeId);
            if (recipientEntityType == null)
            {
                throw new UnknownStaticLookupException(String.Format("The recipient type [{0}] is not supported.", recipientEntityTypeId));
            }
            var sourceEntityType = MoneyFlowSourceRecipientType.GetStaticLookup(sourceEntityTypeId);
            if (sourceEntityType == null)
            {
                throw new UnknownStaticLookupException(String.Format("The source type [{0}] is not supported.", sourceEntityTypeId));
            }

            this.Audit = new Create(createdBy);
            this.Description = description;
            this.ParentMoneyFlowId = parentMoneyFlowId;
            this.Value = value;
            this.MoneyFlowStatusId = moneyFlowStatusId;
            this.TransactionDate = transactionDate;
            this.FiscalYear = fiscalYear;
            this.SourceEntityId = sourceEntityId;
            this.SourceEntityTypeId = sourceEntityTypeId;
            this.RecipientEntityId = recipientEntityId;
            this.RecipientEntityTypeId = recipientEntityTypeId;
            this.MoneyFlowTypeId = MoneyFlowType.Incoming.Id;
        }

        /// <summary>
        /// Gets the money flow type id.
        /// </summary>
        public int MoneyFlowTypeId { get; private set; }

        /// <summary>
        /// Gets the create audit details.
        /// </summary>
        public Audit Audit { get; private set; }

        /// <summary>
        /// Gets or sets the parent money flow id.
        /// </summary>
        public int? ParentMoneyFlowId { get; private set; }

        /// <summary>
        /// Gets the money flow description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the money flow value.
        /// </summary>
        public decimal Value { get; private set; }

        /// <summary>
        /// Gets the money flow status id.
        /// </summary>
        public int MoneyFlowStatusId { get; private set; }

        /// <summary>
        /// Gets the transaction date.
        /// </summary>
        public DateTimeOffset TransactionDate { get; private set; }

        /// <summary>
        /// Gets the fiscal year.
        /// </summary>
        public int FiscalYear { get; private set; }

        /// <summary>
        /// Gets the money flow source entity id.
        /// </summary>
        public int? SourceEntityId { get; private set; }

        /// <summary>
        /// Gets the money flow source entity type id.
        /// </summary>
        public int SourceEntityTypeId { get; private set; }

        /// <summary>
        /// Gets the money flow recipient entity id.
        /// </summary>
        public int? RecipientEntityId { get; private set; }

        /// <summary>
        /// Gets the money flow recipient entity type id.
        /// </summary>
        public int RecipientEntityTypeId { get; private set; }

        /// <summary>
        /// Returns a property instantiated money flow object that can be inserted into the eca context.
        /// </summary>
        /// <returns>The new money flow object.</returns>
        public MoneyFlow GetMoneyFlow()
        {
            Contract.Assert(this.SourceEntityTypeId != MoneyFlowSourceRecipientType.Accomodation.Id, "The source entity type id must not be an Accomodation.");
            Contract.Assert(this.SourceEntityTypeId != MoneyFlowSourceRecipientType.Expense.Id, "The source entity type id must not be an Expense.");

            var moneyFlow = new MoneyFlow();
            moneyFlow.Description = this.Description;
            moneyFlow.FiscalYear = this.FiscalYear;
            moneyFlow.MoneyFlowStatusId = this.MoneyFlowStatusId;
            moneyFlow.MoneyFlowTypeId = this.MoneyFlowTypeId;
            moneyFlow.TransactionDate = this.TransactionDate;
            moneyFlow.Value = this.Value;
            moneyFlow.SourceTypeId = this.SourceEntityTypeId;
            moneyFlow.RecipientTypeId = this.RecipientEntityTypeId;
            moneyFlow.ParentMoneyFlowId = this.ParentMoneyFlowId;

            var sourcePropertyDictionary = new Dictionary<int, Expression<Func<MoneyFlow, int?>>>();
            sourcePropertyDictionary.Add(MoneyFlowSourceRecipientType.ItineraryStop.Id, x => x.SourceItineraryStopId);
            sourcePropertyDictionary.Add(MoneyFlowSourceRecipientType.Office.Id, x => x.SourceOrganizationId);
            sourcePropertyDictionary.Add(MoneyFlowSourceRecipientType.Organization.Id, x => x.SourceOrganizationId);
            sourcePropertyDictionary.Add(MoneyFlowSourceRecipientType.Participant.Id, x => x.SourceParticipantId);
            sourcePropertyDictionary.Add(MoneyFlowSourceRecipientType.Post.Id, x => x.SourceOrganizationId);
            sourcePropertyDictionary.Add(MoneyFlowSourceRecipientType.Program.Id, x => x.SourceProgramId);
            sourcePropertyDictionary.Add(MoneyFlowSourceRecipientType.Project.Id, x => x.SourceProjectId);
            Contract.Assert(sourcePropertyDictionary.ContainsKey(this.SourceEntityTypeId), String.Format("The entity source type [{0}] is not recognized.", this.SourceEntityTypeId));            

            var recipientPropertyDictionary = new Dictionary<int, Expression<Func<MoneyFlow, int?>>>();
            recipientPropertyDictionary.Add(MoneyFlowSourceRecipientType.Accomodation.Id, x => x.RecipientAccommodationId);
            recipientPropertyDictionary.Add(MoneyFlowSourceRecipientType.Transportation.Id, x => x.RecipientTransportationId);
            recipientPropertyDictionary.Add(MoneyFlowSourceRecipientType.ItineraryStop.Id, x => x.RecipientItineraryStopId);
            recipientPropertyDictionary.Add(MoneyFlowSourceRecipientType.Organization.Id, x => x.RecipientOrganizationId);
            recipientPropertyDictionary.Add(MoneyFlowSourceRecipientType.Office.Id, x => x.RecipientOrganizationId);
            recipientPropertyDictionary.Add(MoneyFlowSourceRecipientType.Participant.Id, x => x.RecipientParticipantId);
            recipientPropertyDictionary.Add(MoneyFlowSourceRecipientType.Post.Id, x => x.RecipientOrganizationId);
            recipientPropertyDictionary.Add(MoneyFlowSourceRecipientType.Program.Id, x => x.RecipientProgramId);
            recipientPropertyDictionary.Add(MoneyFlowSourceRecipientType.Project.Id, x => x.RecipientProjectId);

            SetPropertyValue(moneyFlow, sourcePropertyDictionary[this.SourceEntityTypeId], this.SourceEntityId);
            if (recipientPropertyDictionary.ContainsKey(this.RecipientEntityTypeId))
            {
                SetPropertyValue(moneyFlow, recipientPropertyDictionary[this.RecipientEntityTypeId], this.RecipientEntityId);
            }
            this.Audit.SetHistory(moneyFlow);
            return moneyFlow;
        }

        private void SetPropertyValue(MoneyFlow moneyFlow, Expression<Func<MoneyFlow, int?>> propertySelector, int? value)
        {
            var memberSelectorExpression = propertySelector.Body as MemberExpression;
            Contract.Assert(memberSelectorExpression != null, "The member must not be null.");
            var property = memberSelectorExpression.Member as PropertyInfo;
            Contract.Assert(property != null, "The property must not be null.");
            property.SetValue(moneyFlow, value, null);
        }
    }
}
