using ECA.Data;
using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq;
using System.Diagnostics.Contracts;

namespace ECA.Business.Queries.Fundings
{
    /// <summary>
    /// Contains queries for moneyflows against a db context
    /// </summary>
    public static class MoneyFlowQueries
    {
        /// <summary>
        /// The placeholder for an itinerary stop.
        /// </summary>
        public const string ITINERARY_NAME = "Itinerary Stop";


        /// <summary>
        /// Returns a query to get all outgoing money flows currently in the system.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to get outgoing money flows in the system.</returns>
        public static IQueryable<MoneyFlowDTO> CreateOutgoingMoneyFlowDTOsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var outgoingMoneyFlowType = MoneyFlowType.Outgoing.Value;
            var expenseMoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Expense.Id;
            var expenseMoneyFlowSourceRecipientTypeValue = MoneyFlowSourceRecipientType.Expense.Value;

            var query = from moneyFlow in context.MoneyFlows
                        let isExpense = moneyFlow.RecipientTypeId == expenseMoneyFlowSourceRecipientTypeId
                        let sourceType = isExpense ? expenseMoneyFlowSourceRecipientTypeValue : moneyFlow.SourceType.TypeName
                        let status = moneyFlow.MoneyFlowStatus.MoneyFlowStatusName
                        let amount = moneyFlow.Value
                        
                        let entityTypeId = moneyFlow.SourceTypeId

                        let sourceEntityId = moneyFlow.SourceItineraryStopId.HasValue ? moneyFlow.SourceItineraryStopId.Value
                            : moneyFlow.SourceOrganizationId.HasValue ? moneyFlow.SourceOrganizationId.Value
                            : moneyFlow.SourceParticipantId.HasValue ? moneyFlow.SourceParticipantId.Value
                            : moneyFlow.SourceProgramId.HasValue ? moneyFlow.SourceProgramId.Value
                            : moneyFlow.SourceProjectId.HasValue ? moneyFlow.SourceProjectId.Value
                            : -1

                        let recipientEntityId = moneyFlow.RecipientAccommodationId.HasValue ? moneyFlow.RecipientAccommodationId.Value
                            : moneyFlow.RecipientItineraryStopId.HasValue ? moneyFlow.RecipientItineraryStopId.Value
                            : moneyFlow.RecipientOrganizationId.HasValue ? moneyFlow.RecipientOrganizationId.Value
                            : moneyFlow.RecipientParticipantId.HasValue ? moneyFlow.RecipientParticipantId.Value
                            : moneyFlow.RecipientProgramId.HasValue ? moneyFlow.RecipientProgramId.Value
                            : moneyFlow.RecipientProjectId.HasValue ? moneyFlow.RecipientProjectId.Value
                            : moneyFlow.RecipientTransportationId.HasValue ? moneyFlow.RecipientTransportationId.Value
                            : -1

                        let accomodation = context.Accommodations.Where(x => x.AccommodationId == recipientEntityId).FirstOrDefault()
                        let transportation = context.Transportations.Where(x => x.TransportationId == recipientEntityId).FirstOrDefault()
                        let organization = context.Organizations.Where(x => x.OrganizationId == recipientEntityId).FirstOrDefault()
                        let itinerary = ITINERARY_NAME
                        let project = context.Projects.Where(x => x.ProjectId == recipientEntityId).FirstOrDefault()
                        let program = context.Programs.Where(x => x.ProgramId == recipientEntityId).FirstOrDefault()
                        let participant = context.Participants.Where(x => x.ParticipantId == recipientEntityId).FirstOrDefault()
                        let isOrgParticipant = participant == null ? false : participant.OrganizationId.HasValue
                        let isPersonParticipant = participant == null ? false : participant.PersonId.HasValue
                        let orgParticipant = isOrgParticipant ? participant.Organization : null
                        let personParticipant = isPersonParticipant ?  participant.Person : null                        

                        let recipientEntityName = moneyFlow.RecipientAccommodationId.HasValue ? accomodation.Host.Name
                           : moneyFlow.RecipientItineraryStopId.HasValue ? itinerary
                           : moneyFlow.RecipientOrganizationId.HasValue ? organization.Name
                           : moneyFlow.RecipientParticipantId.HasValue ?
                               (isOrgParticipant ? orgParticipant.Name :
                                   (isPersonParticipant ? personParticipant.FirstName + " " + personParticipant.LastName : "UNKNOWN PARTICIPANT NAME"))
                           : moneyFlow.RecipientProgramId.HasValue ? program.Name
                           : moneyFlow.RecipientProjectId.HasValue ? project.Name
                           : moneyFlow.RecipientTransportationId.HasValue ? transportation.Carrier.Name
                           : "UNKNOWN OUTGOING ENTITY NAME"


                        select new MoneyFlowDTO
                        {
                            Id = moneyFlow.MoneyFlowId,
                            EntityId = sourceEntityId,
                            EntityTypeId = entityTypeId,
                            Amount = -moneyFlow.Value,
                            Description = moneyFlow.Description,
                            FiscalYear = moneyFlow.FiscalYear,
                            SourceType = sourceType,
                            Status = status,
                            TransactionDate = moneyFlow.TransactionDate,
                            Type = outgoingMoneyFlowType,
                            FromTo = isExpense ? null : recipientEntityName
                        };
            return query;

        }

        /// <summary>
        /// Returns a query to get all incoming money flows as a dto.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to get all incoming money flows.</returns>
        public static IQueryable<MoneyFlowDTO> CreateIncomingMoneyFlowDTOsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var incomingMoneyFlowType = MoneyFlowType.Incoming.Value;
            var query = from moneyFlow in context.MoneyFlows
                        let sourceType = moneyFlow.SourceType.TypeName
                        let status = moneyFlow.MoneyFlowStatus.MoneyFlowStatusName
                        let amount = moneyFlow.Value

                        let entityTypeId = moneyFlow.RecipientTypeId

                        let sourceEntityId = moneyFlow.RecipientAccommodationId.HasValue ? moneyFlow.RecipientAccommodationId.Value
                            : moneyFlow.RecipientItineraryStopId.HasValue ? moneyFlow.RecipientItineraryStopId.Value
                            : moneyFlow.RecipientOrganizationId.HasValue ? moneyFlow.RecipientOrganizationId.Value
                            : moneyFlow.RecipientParticipantId.HasValue ? moneyFlow.RecipientParticipantId.Value
                            : moneyFlow.RecipientProgramId.HasValue ? moneyFlow.RecipientProgramId.Value
                            : moneyFlow.RecipientProjectId.HasValue ? moneyFlow.RecipientProjectId.Value
                            : moneyFlow.RecipientTransportationId.HasValue ? moneyFlow.RecipientTransportationId.Value
                            : -1

                        let recipientEntityId = moneyFlow.SourceItineraryStopId.HasValue ? moneyFlow.SourceItineraryStopId.Value
                            : moneyFlow.SourceOrganizationId.HasValue ? moneyFlow.SourceOrganizationId.Value
                            : moneyFlow.SourceParticipantId.HasValue ? moneyFlow.SourceParticipantId.Value
                            : moneyFlow.SourceProgramId.HasValue ? moneyFlow.SourceProgramId.Value
                            : moneyFlow.SourceProjectId.HasValue ? moneyFlow.SourceProjectId.Value
                            : -1

                        let organization = context.Organizations.Where(x => x.OrganizationId == recipientEntityId).FirstOrDefault()
                        let itinerary = ITINERARY_NAME
                        let project = context.Projects.Where(x => x.ProjectId == recipientEntityId).FirstOrDefault()
                        let program = context.Programs.Where(x => x.ProgramId == recipientEntityId).FirstOrDefault()
                        let participant = context.Participants.Where(x => x.ParticipantId == recipientEntityId).FirstOrDefault()
                        let isOrgParticipant = participant == null ? false : participant.OrganizationId.HasValue
                        let isPersonParticipant = participant == null ? false : participant.PersonId.HasValue
                        let orgParticipant = isOrgParticipant ? participant.Organization : null
                        let personParticipant = isPersonParticipant ? participant.Person : null

                        let fromEntityName = moneyFlow.SourceItineraryStopId.HasValue ? itinerary
                            : moneyFlow.SourceOrganizationId.HasValue ? organization.Name
                            : moneyFlow.SourceParticipantId.HasValue ?
                                (isOrgParticipant ? orgParticipant.Name :
                                    (isPersonParticipant ? personParticipant.FirstName + " " + personParticipant.LastName : "UNKNOWN PARTICIPANT NAME"))
                            : moneyFlow.SourceProgramId.HasValue ? program.Name
                            : moneyFlow.SourceProjectId.HasValue ? project.Name
                            : "UNKNOWN INCOMING ENTITY NAME"

                        select new MoneyFlowDTO
                        {
                            Id = moneyFlow.MoneyFlowId,
                            EntityId = sourceEntityId,
                            EntityTypeId = entityTypeId,
                            Amount = moneyFlow.Value,
                            Description = moneyFlow.Description,
                            FiscalYear = moneyFlow.FiscalYear,
                            SourceType = sourceType,
                            Status = status,
                            TransactionDate = moneyFlow.TransactionDate,
                            Type = incomingMoneyFlowType,
                            FromTo = fromEntityName
                        };
            return query;

        }

        /// <summary>
        /// Returns a query to get all incoming and outgoing money flows in the system.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to get all incoming and outgoing money flows.</returns>
        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowDTOsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateIncomingMoneyFlowDTOsQuery(context).Union(CreateOutgoingMoneyFlowDTOsQuery(context));
            return query;
        }

        /// <summary>
        /// Returns a query to get all incoming and outgoing money flows for an entity with the given type and id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="entityId">The entity id, i.e. the project id, program id, etc.</param>
        /// <param name="entityTypeId">The MoneyFlowsSourceRecipientType id.</param>
        /// <returns>The query to get all incoming and outgoing moneys for an entity.</returns>
        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowsDTOsByEntityType(EcaContext context, int entityId, int entityTypeId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetMoneyFlowDTOsQuery(context).Where(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);
        }

        /// <summary>
        /// Returns a query to get all money flows for the project with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to get all project money flows.</returns>
        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowDTOsByProjectId(EcaContext context, int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetMoneyFlowsDTOsByEntityType(context, projectId, MoneyFlowSourceRecipientType.Project.Id);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a query to get all money flows for the program with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to get all program money flows.</returns>
        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowDTOsByProgramId(EcaContext context, int programId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetMoneyFlowsDTOsByEntityType(context, programId, MoneyFlowSourceRecipientType.Program.Id);
            query = query.Apply(queryOperator);
            return query;
        }
    }
}
