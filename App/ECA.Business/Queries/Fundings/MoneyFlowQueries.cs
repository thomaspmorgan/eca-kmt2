using ECA.Data;
using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq;
using System.Diagnostics.Contracts;
using NLog;
using ECA.Business.Queries.Persons;

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
        /// The name of the participant when the participant (person/org) can not be found.
        /// </summary>
        public const string UNKNOWN_PARTICIPANT_NAME = "UNKNOWN PARTICIPANT NAME";

        /// <summary>
        /// The default outgoing money flow entity name.
        /// </summary>
        public const string UNKNOWN_OUTGOING_ENTITY_NAME = "UNKNOWN OUTGOING ENTITY NAME";

        /// <summary>
        /// The default incoming money flow entity name.
        /// </summary>
        public const string UNKNOWN_INCOMING_ENTITY_NAME = "UNKNOWN INCOMING ENTITY NAME";

        /// <summary>
        /// Returns a query to get all outgoing money flows currently in the system.  This would be
        /// money flow's with the entity from the source perspective.  In other words, supposed I'm interested
        /// in outgoing money flows from a project, then I need to find money flows whose sourceProjectId and source type id
        /// belong to a project.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to get outgoing money flows in the system.</returns>
        public static IQueryable<MoneyFlowDTO> CreateOutgoingMoneyFlowDTOsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var outgoingMoneyFlowType = MoneyFlowType.Outgoing.Value;
            var expenseMoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Expense.Id;
            var expenseMoneyFlowSourceRecipientTypeValue = MoneyFlowSourceRecipientType.Expense.Value;

            var people = PersonQueries.CreateGetSimplePersonDTOsQuery(context);

            var query = from moneyFlow in context.MoneyFlows
                        let isExpense = moneyFlow.RecipientTypeId == expenseMoneyFlowSourceRecipientTypeId
                        let recipientTypeName = isExpense ? expenseMoneyFlowSourceRecipientTypeValue : moneyFlow.RecipientType.TypeName
                        let recipientTypeId = isExpense ? expenseMoneyFlowSourceRecipientTypeId : moneyFlow.RecipientTypeId
                        let status = moneyFlow.MoneyFlowStatus.MoneyFlowStatusName
                        let statusId = moneyFlow.MoneyFlowStatusId
                        let amount = moneyFlow.Value

                        let sourceEntityTypeId = moneyFlow.SourceTypeId

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
                        let participantType = participant == null ? null : participant.ParticipantType
                        let isOrgParticipant = participant != null && participant.OrganizationId.HasValue
                        let isPersonParticipant = participant != null && participant.PersonId.HasValue
                        let orgParticipant = isOrgParticipant ? participant.Organization : null
                        let personParticipant = isPersonParticipant ? participant.Person : null

                        let recipientEntityName = moneyFlow.RecipientAccommodationId.HasValue ? accomodation.Host.Name
                           : moneyFlow.RecipientItineraryStopId.HasValue ? itinerary
                           : moneyFlow.RecipientOrganizationId.HasValue ? organization.Name
                           : moneyFlow.RecipientParticipantId.HasValue ?
                               (isOrgParticipant ? orgParticipant.Name :
                                   (isPersonParticipant ?
                                   people.Where(x => x.PersonId == personParticipant.PersonId).FirstOrDefault().FullName
                                   : UNKNOWN_PARTICIPANT_NAME))
                           : moneyFlow.RecipientProgramId.HasValue ? program.Name
                           : moneyFlow.RecipientProjectId.HasValue ? project.Name
                           : moneyFlow.RecipientTransportationId.HasValue ? transportation.Carrier.Name
                           : UNKNOWN_OUTGOING_ENTITY_NAME


                        select new MoneyFlowDTO
                        {
                            Id = moneyFlow.MoneyFlowId,
                            EntityId = sourceEntityId,
                            EntityTypeId = sourceEntityTypeId,
                            Amount = -moneyFlow.Value,
                            Description = moneyFlow.Description,
                            FiscalYear = moneyFlow.FiscalYear,
                            MoneyFlowStatus = status,
                            MoneyFlowStatusId = statusId,
                            TransactionDate = moneyFlow.TransactionDate,
                            MoneyFlowType = outgoingMoneyFlowType,
                            ParticipantTypeId = participantType == null ? default(int?) : participantType.ParticipantTypeId,
                            ParticipantTypeName = participantType == null ? null : participantType.Name,
                            SourceRecipientTypeName = recipientTypeName,
                            SourceRecipientEntityTypeId = recipientTypeId,
                            SourceRecipientEntityId = isExpense ? default(int?) : recipientEntityId,
                            SourceRecipientName = isExpense ? null : recipientEntityName
                        };
            return query;

        }

        /// <summary>
        /// Returns a query to get all incoming money flows as a dto.  This would be money flows from the perspective
        /// of the entity as the recipient.  For example, supposed I am concerned about incoming money to a project, then
        /// I need to find all money flows whose recipient project id and recipient type id belong to a project.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to get all incoming money flows.</returns>
        public static IQueryable<MoneyFlowDTO> CreateIncomingMoneyFlowDTOsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var incomingMoneyFlowType = MoneyFlowType.Incoming.Value;

            var people = PersonQueries.CreateGetSimplePersonDTOsQuery(context);

            var query = from moneyFlow in context.MoneyFlows
                        let sourceTypeName = moneyFlow.SourceType.TypeName
                        let sourceTypeId = moneyFlow.SourceTypeId
                        let status = moneyFlow.MoneyFlowStatus.MoneyFlowStatusName
                        let statusId = moneyFlow.MoneyFlowStatusId
                        let amount = moneyFlow.Value

                        let recipientEntityTypeId = moneyFlow.RecipientTypeId

                        let recipientEntityId = moneyFlow.RecipientAccommodationId.HasValue ? moneyFlow.RecipientAccommodationId.Value
                            : moneyFlow.RecipientItineraryStopId.HasValue ? moneyFlow.RecipientItineraryStopId.Value
                            : moneyFlow.RecipientOrganizationId.HasValue ? moneyFlow.RecipientOrganizationId.Value
                            : moneyFlow.RecipientParticipantId.HasValue ? moneyFlow.RecipientParticipantId.Value
                            : moneyFlow.RecipientProgramId.HasValue ? moneyFlow.RecipientProgramId.Value
                            : moneyFlow.RecipientProjectId.HasValue ? moneyFlow.RecipientProjectId.Value
                            : moneyFlow.RecipientTransportationId.HasValue ? moneyFlow.RecipientTransportationId.Value
                            : -1

                        let sourceEntityId = moneyFlow.SourceItineraryStopId.HasValue ? moneyFlow.SourceItineraryStopId.Value
                            : moneyFlow.SourceOrganizationId.HasValue ? moneyFlow.SourceOrganizationId.Value
                            : moneyFlow.SourceParticipantId.HasValue ? moneyFlow.SourceParticipantId.Value
                            : moneyFlow.SourceProgramId.HasValue ? moneyFlow.SourceProgramId.Value
                            : moneyFlow.SourceProjectId.HasValue ? moneyFlow.SourceProjectId.Value
                            : -1

                        let organization = context.Organizations.Where(x => x.OrganizationId == sourceEntityId).FirstOrDefault()
                        let itinerary = ITINERARY_NAME
                        let project = context.Projects.Where(x => x.ProjectId == sourceEntityId).FirstOrDefault()
                        let program = context.Programs.Where(x => x.ProgramId == sourceEntityId).FirstOrDefault()
                        let participant = context.Participants.Where(x => x.ParticipantId == sourceEntityId).FirstOrDefault()
                        let isOrgParticipant = participant != null && participant.OrganizationId.HasValue
                        let isPersonParticipant = participant != null && participant.PersonId.HasValue
                        let participantType = participant == null ? null : participant.ParticipantType
                        let orgParticipant = isOrgParticipant ? participant.Organization : null
                        let personParticipant = isPersonParticipant ? participant.Person : null
                        

                        let sourceEntityName = moneyFlow.SourceItineraryStopId.HasValue ? itinerary
                            : moneyFlow.SourceOrganizationId.HasValue ? organization.Name
                            : moneyFlow.SourceParticipantId.HasValue ?
                                (isOrgParticipant ? orgParticipant.Name :
                                    (isPersonParticipant ?
                                        people.Where(x => x.PersonId == personParticipant.PersonId).FirstOrDefault().FullName
                                        : UNKNOWN_PARTICIPANT_NAME))
                            : moneyFlow.SourceProgramId.HasValue ? program.Name
                            : moneyFlow.SourceProjectId.HasValue ? project.Name
                            : UNKNOWN_INCOMING_ENTITY_NAME

                        select new MoneyFlowDTO
                        {
                            Id = moneyFlow.MoneyFlowId,
                            EntityId = recipientEntityId,
                            EntityTypeId = recipientEntityTypeId,
                            Amount = moneyFlow.Value,
                            Description = moneyFlow.Description,
                            FiscalYear = moneyFlow.FiscalYear,
                            MoneyFlowStatus = status,
                            MoneyFlowStatusId = statusId,
                            TransactionDate = moneyFlow.TransactionDate,
                            MoneyFlowType = incomingMoneyFlowType,
                            ParticipantTypeId = participantType == null ? default(int?) : participantType.ParticipantTypeId,
                            ParticipantTypeName = participantType == null ? null : participantType.Name,
                            SourceRecipientTypeName = sourceTypeName,
                            SourceRecipientEntityTypeId = sourceTypeId,
                            SourceRecipientEntityId = sourceEntityId,
                            SourceRecipientName = sourceEntityName
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

        /// <summary>
        /// Returns a query to get all money flows for the program with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="programId">The program id.</param>
        /// <returns>The query to get all program money flows.</returns>
        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowDTOsByProgramId(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetMoneyFlowsDTOsByEntityType(context, programId, MoneyFlowSourceRecipientType.Program.Id);
            return query;
        }

        /// <summary>
        /// Returns a query to get all money flows for the organization with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to get all organization money flows.</returns>
        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowDTOsByOrganizationId(EcaContext context, int organizationId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetMoneyFlowsDTOsByEntityType(context, organizationId, MoneyFlowSourceRecipientType.Organization.Id);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a query to get all money flows for the office with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="officeId">The office id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to get all office money flows.</returns>
        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowDTOsByOfficeId(EcaContext context, int officeId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetMoneyFlowsDTOsByEntityType(context, officeId, MoneyFlowSourceRecipientType.Office.Id);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a query to get all money flows for the person with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="personId">The person id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to get all person money flows.</returns>
        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowDTOsByPersonId(EcaContext context, int personId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var participantTypeEntityTypeId = MoneyFlowSourceRecipientType.Participant.Id;
            var moneyFlows = CreateGetMoneyFlowDTOsQuery(context);

            var query = from participant in context.Participants

                        join person in context.Participants
                        on participant.PersonId equals person.PersonId

                        join moneyFlow in moneyFlows
                        on new { ParticipantId = participant.ParticipantId, TypeId = participantTypeEntityTypeId }
                        equals new { ParticipantId = moneyFlow.EntityId, TypeId = moneyFlow.EntityTypeId }

                        where person.PersonId == personId
                        select moneyFlow;

            query = query.Distinct().Apply(queryOperator);
            return query;
        }
    }
}
