using ECA.Business.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ECA.Business.Models.Admin;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class DraftMoneyFlow : EcaMoneyFlow
    {
        public DraftMoneyFlow( User createdBy,
            int id, 
            string description,
            int moneyFlowTypeId,
            float value,
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
                :base(
            updatedBy: createdBy,
            id: 0,
            description: description,
            moneyFlowTypeId: moneyFlowTypeId,
            moneyFlowStatusId: moneyFlowStatusId,
            value: value,
            transactionDate: transactionDate,
            fiscalYear: fiscalYear,
            sourceTypeId: sourceTypeId,
            recipientTypeId: recipientTypeId,
            sourceOrganizationId: sourceOrganizationId,
            recipientOrganizationId: recipientOrganizationId,
            sourceProgramId:sourceProgramId,
            recipientProgramId: recipientProgramId,
            sourceProjectId: sourceProjectId,
            recipientProjectId: recipientProjectId,
            sourceParticipantId:sourceParticipantId,
            recipientParticipantId:recipientParticipantId,
            sourceItineraryStopId: sourceItineraryStopId,
            recipientItineraryStopId:recipientItineraryStopId,
            recipientTransportationId:recipientTransportationId,
            recipientAccommodationId: recipientAccommodationId)
        {
            Contract.Requires(createdBy != null, "The created by user must not be null.");
            this.Audit = new Create(createdBy);
        }

        public int Id { get; private set; }

        public int MoneyFlowTypeId { get; private set; }

        public float Value { get; private set; }

        public int MoneyFlowStatusId { get; private set; }

        public DateTimeOffset TransactionDate { get; private set; }

        public int FiscalYear { get; private set; }

        public int SourceTypeId { get; private set; }

        public int RecipientTypeId { get; private set; }

        public string Description { get; private set; }

        public int ParentMoneyFlowId { get; private set; }

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
    }
}
