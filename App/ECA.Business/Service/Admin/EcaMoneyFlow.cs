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

namespace ECA.Business.Models.Admin
{
    public class EcaMoneyFlow : IAuditable
    {
        public EcaMoneyFlow(MoneyFlow moneyFlow)
        {
            this.Description = moneyFlow.Description;
            this.MoneyFlowTypeId = moneyFlow.MoneyFlowTypeId;
            this.Value = moneyFlow.Value;
            this.MoneyFlowStatusId = moneyFlow.MoneyFlowStatusId;
            this.TransactionDate = moneyFlow.TransactionDate;
            this.FiscalYear = moneyFlow.FiscalYear;
            this.SourceTypeId = moneyFlow.SourceTypeId;
            this.RecipientTypeId = moneyFlow.RecipientTypeId;
            this.SourceOrganizationId = moneyFlow.SourceOrganizationId;
            int? RecipientOrganizationId = moneyFlow.RecipientOrganizationId;
            this.SourceProgramId = moneyFlow.SourceProgramId;
            this.RecipientProgramId = moneyFlow.RecipientProgramId;
            this.SourceProjectId = moneyFlow.SourceProjectId;
            this.RecipientProjectId = moneyFlow.RecipientProjectId;
            this.SourceParticipantId = moneyFlow.SourceParticipantId;
            this.RecipientParticipantId = moneyFlow.RecipientParticipantId;
            this.SourceItineraryStopId = moneyFlow.SourceItineraryStopId;
            this.RecipientItineraryStopId = moneyFlow.RecipientItineraryStopId;
            this.RecipientTransportationId = moneyFlow.RecipientTransportationId;
            this.RecipientAccommodationId = moneyFlow.RecipientAccommodationId;
        }

        public EcaMoneyFlow(
            User updatedBy,
            int id, 
            string description,
            int moneyFlowTypeId,
            decimal value,
            int moneyFlowStatusId,
            DateTimeOffset transactionDate,
            int fiscalYear,
            int sourceTypeId,
            int recipientTypeId,
            int? sourceOrganizationId,
            int? recipientOrganizationId,
            int? sourceProgramId,
            int? recipientProgramId,
            int? sourceProjectId,
            int? recipientProjectId,
            int? sourceParticipantId,
            int? recipientParticipantId,
            int? sourceItineraryStopId,
            int? recipientItineraryStopId,
            int? recipientTransportationId,
            int? recipientAccommodationId)
        {
            Contract.Requires(updatedBy != null, "The updated by user must not be null.");
            var moneyFlowStatus = MoneyFlowStatus.GetStaticLookup(moneyFlowStatusId);
            if (moneyFlowStatus == null)
            {
                throw new UnknownStaticLookupException(String.Format("The money flow status [{0}] is not supported.", moneyFlowStatusId));
            }
            var moneyFlowType = MoneyFlowType.GetStaticLookup(moneyFlowTypeId);
            if (moneyFlowStatus == null)
            {
                throw new UnknownStaticLookupException(String.Format("The money flow type [{0}] is not supported.", moneyFlowTypeId));
            }

            this.Id = id;
            this.Description = description;
            this.MoneyFlowTypeId = moneyFlowTypeId;
            this.Value = value;
            this.MoneyFlowStatusId = moneyFlowStatusId;
            this.TransactionDate = transactionDate;
            this.FiscalYear = fiscalYear;
            this.SourceTypeId = sourceTypeId;
            this.RecipientTypeId = recipientTypeId;
            this.SourceOrganizationId = sourceOrganizationId;
            int? RecipientOrganizationId = recipientOrganizationId;
            this.SourceProgramId = sourceProgramId;
            this.RecipientProgramId = recipientProgramId;
            this.SourceProjectId = sourceProjectId;
            this.RecipientProjectId = recipientProjectId;
            this.SourceParticipantId = sourceParticipantId;
            this.RecipientParticipantId = recipientParticipantId;
            this.SourceItineraryStopId = sourceItineraryStopId;
            this.RecipientItineraryStopId = recipientItineraryStopId;
            this.RecipientTransportationId = recipientTransportationId;
            this.RecipientAccommodationId = recipientAccommodationId;

        }


        public int Id { get; private set; }

        public int MoneyFlowTypeId { get; private set; }

        public decimal Value { get; private set; }

        public int MoneyFlowStatusId { get; private set; }

        public DateTimeOffset TransactionDate { get; private set; }

        public int FiscalYear { get; private set; }

        public int SourceTypeId { get; private set; }

        public int RecipientTypeId { get; private set; }

        public string Description { get; private set; }

        public int? SourceOrganizationId { get; private set; }

        public int? RecipientOrganizationId { get; private set; }

        public int? SourceProgramId { get; private set; }

        public int? RecipientProgramId { get; private set; }

        public int? SourceProjectId { get; private set; }

        public int? RecipientProjectId { get; private set; }

        public int? SourceParticipantId { get; private set; }

        public int? RecipientParticipantId { get; private set; }

        public int? SourceItineraryStopId { get; private set; }

        public int? RecipientItineraryStopId { get; private set; }

        public int? RecipientTransportationId { get; private set; }

        public int? RecipientAccommodationId { get; private set; }

        public Audit Audit { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        public byte[] RowVersion { get; set; }

    }
}
