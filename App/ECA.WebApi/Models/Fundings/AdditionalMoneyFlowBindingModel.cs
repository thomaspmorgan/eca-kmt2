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
    /// to add a new money flow to the eca system.  The PeerEntityId and PeerEntityTypeId detail
    /// the primary key id and money flow source recipient type id for where the money flow is going
    /// to or coming from.  The GetEntityId() and GetEntityTypeId() methods must be overridden by a base class
    /// to details the entity for whom a user has permission to add a money flow, i.e. the edit project permission
    /// for a project with entity id x.
    /// </summary>
    public abstract class AdditionalMoneyFlowBindingModel
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// True if the funding is direct, of false if it is in-kind.
        /// </summary>
        public bool IsDirect { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [MaxLength(MoneyFlow.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the grant number.
        /// </summary>
        [MaxLength(MoneyFlow.GRANT_NUMBER_MAX_LENGTH)]
        public string GrantNumber { get; set; }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        public DateTimeOffset TransactionDate { get; set; }

        /// <summary>
        /// Gets or sets the fiscal year.
        /// </summary>
        public int FiscalYear { get; set; } 

        /// <summary>
        /// Gets or sets the money flow status id.
        /// </summary>
        public int MoneyFlowStatusId { get; set; }

        /// <summary>
        /// Gets or sets the peer entity type by id, i.e. Post, Expense, etc where the money flow is going to or coming from.
        /// </summary>
        public int PeerEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the peer entity id, i.e. the project id, program id, etc where the money flow is going to or coming from.
        /// </summary>
        public int? PeerEntityId { get; set; }

        /// <summary>
        /// If true, the money flow is outgoing from the entity to the peer entity, i.e. money is leaving from the permissable
        /// entity and going to the peer entity.  If false, money is outgoing from the peer entity and incoming to this entity.
        /// </summary>
        public bool IsOutgoing { get; set; }

        /// <summary>
        /// The parent money flow by id.
        /// </summary>
        public int? ParentMoneyFlowId { get; set; }

        /// <summary>
        /// Returns the permissable entity type id.
        /// </summary>
        /// <returns>The permissable entity type id, i.e. Project, Post, Organization, etc.</returns>
        public abstract int GetEntityTypeId();

        /// <summary>
        /// Returns the permissable entity id, i.e. the project id, program id, etc from where the money flow is coming from or to.
        /// </summary>
        /// <returns>The permissable entity id, i.e. the project id, program id, etc. from where the money flow is coming from or to.</returns>
        public abstract int GetEntityId();

        /// <summary>
        /// Returns an AdditionalMoneyFlow business object instance to add a new money flow.
        /// </summary>
        /// <param name="user">The user creating the money flow.</param>
        /// <returns>The additional money flow business instance.</returns>
        public AdditionalMoneyFlow ToAdditionalMoneyFlow(User user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            if (IsOutgoing)
            {
                return new AdditionalMoneyFlow(
                createdBy: user,
                parentMoneyFlowId: this.ParentMoneyFlowId,
                description: this.Description,
                grantNumber: this.GrantNumber,
                value: this.Value,
                moneyFlowStatusId: this.MoneyFlowStatusId,
                transactionDate: this.TransactionDate,
                fiscalYear: this.FiscalYear,
                sourceEntityId: this.GetEntityId(),
                recipientEntityId: this.PeerEntityId,
                sourceEntityTypeId: this.GetEntityTypeId(),
                recipientEntityTypeId: this.PeerEntityTypeId,
                isDirect: this.IsDirect
                );
            }
            else
            {
                return new AdditionalMoneyFlow(
                    createdBy: user,
                    description: this.Description,
                    grantNumber: this.GrantNumber,
                    parentMoneyFlowId: this.ParentMoneyFlowId,
                    value: this.Value,
                    moneyFlowStatusId: this.MoneyFlowStatusId,
                    transactionDate: this.TransactionDate,
                    fiscalYear: this.FiscalYear,
                    recipientEntityId: this.GetEntityId(),
                    sourceEntityId: this.PeerEntityId,
                    recipientEntityTypeId: this.GetEntityTypeId(),
                    sourceEntityTypeId: this.PeerEntityTypeId,
                isDirect: this.IsDirect
                    );
            }
        }
    }
}