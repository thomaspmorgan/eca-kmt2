using ECA.Business.Queries.Models.Fundings;
using ECA.Business.Queries.Persons;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

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

        #region Money Flow DTOs

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

                        let accomodation = moneyFlow.RecipientAccommodationId.HasValue ? moneyFlow.RecipientAccommodation : null
                        let transportation = moneyFlow.RecipientTransportationId.HasValue ? moneyFlow.RecipientTransportation : null
                        let organization = moneyFlow.RecipientOrganizationId.HasValue ? moneyFlow.RecipientOrganization : null
                        let itinerary = moneyFlow.RecipientItineraryStopId.HasValue ? ITINERARY_NAME : null
                        let project = moneyFlow.RecipientProjectId.HasValue ? moneyFlow.RecipientProject : null
                        let program = moneyFlow.RecipientProgramId.HasValue ? moneyFlow.RecipientProgram : null
                        let participant = moneyFlow.RecipientParticipantId.HasValue ? moneyFlow.RecipientParticipant : null
                        let participantType = participant == null ? null : participant.ParticipantType
                        let isOrgParticipant = participant != null && participant.OrganizationId.HasValue
                        let isPersonParticipant = participant != null && participant.PersonId.HasValue
                        let orgParticipant = isOrgParticipant ? participant.Organization : null
                        let personParticipant = isPersonParticipant ? participant.Person : null

                        let recipientEntityName = moneyFlow.RecipientAccommodationId.HasValue ? accomodation.Host.Name
                           : moneyFlow.RecipientItineraryStopId.HasValue ? itinerary
                           : moneyFlow.RecipientOrganizationId.HasValue ? organization.Name
                           : moneyFlow.RecipientProgramId.HasValue ? program.Name
                           : moneyFlow.RecipientProjectId.HasValue ? project.Name
                           : moneyFlow.RecipientTransportationId.HasValue ? transportation.Carrier.Name
                           : moneyFlow.RecipientParticipantId.HasValue ?
                               (isOrgParticipant ? orgParticipant.Name :
                                   (isPersonParticipant ?
                                   personParticipant.FullName
                                   : UNKNOWN_PARTICIPANT_NAME))
                           : UNKNOWN_OUTGOING_ENTITY_NAME

                        select new MoneyFlowDTO
                        {
                            Amount = -moneyFlow.Value,
                            Description = moneyFlow.Description,
                            EntityId = sourceEntityId,
                            EntityTypeId = sourceEntityTypeId,
                            FiscalYear = moneyFlow.FiscalYear,
                            Id = moneyFlow.MoneyFlowId,
                            MoneyFlowStatus = status,
                            MoneyFlowStatusId = statusId,
                            MoneyFlowType = outgoingMoneyFlowType,
                            ParticipantTypeId = participantType == null ? default(int?) : participantType.ParticipantTypeId,
                            ParticipantTypeName = participantType == null ? null : participantType.Name,
                            ParentMoneyFlowId = moneyFlow.ParentMoneyFlowId,
                            SourceRecipientTypeName = recipientTypeName,
                            SourceRecipientEntityTypeId = recipientTypeId,
                            SourceRecipientEntityId = isExpense ? default(int?) : recipientEntityId,
                            SourceRecipientName = isExpense ? null : recipientEntityName,
                            TransactionDate = moneyFlow.TransactionDate,
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

                        let organization = moneyFlow.SourceOrganizationId.HasValue ? moneyFlow.SourceOrganization : null
                        let itinerary = moneyFlow.SourceItineraryStopId.HasValue ? ITINERARY_NAME : null
                        let project = moneyFlow.SourceProjectId.HasValue ? moneyFlow.SourceProject : null
                        let program = moneyFlow.SourceProgramId.HasValue ? moneyFlow.SourceProgram : null
                        let participant = moneyFlow.SourceParticipantId.HasValue ? moneyFlow.SourceParticipant : null
                        let isOrgParticipant = participant != null && participant.OrganizationId.HasValue
                        let isPersonParticipant = participant != null && participant.PersonId.HasValue
                        let participantType = participant == null ? null : participant.ParticipantType
                        let orgParticipant = isOrgParticipant ? participant.Organization : null
                        let personParticipant = isPersonParticipant ? participant.Person : null

                        let sourceEntityName = moneyFlow.SourceItineraryStopId.HasValue ? itinerary
                            : moneyFlow.SourceOrganizationId.HasValue ? organization.Name
                            : moneyFlow.SourceProgramId.HasValue ? program.Name
                            : moneyFlow.SourceProjectId.HasValue ? project.Name
                            : moneyFlow.SourceParticipantId.HasValue ?
                                (isOrgParticipant ? orgParticipant.Name :
                                    (isPersonParticipant ?
                                        personParticipant.FullName
                                        : UNKNOWN_PARTICIPANT_NAME))
                            : UNKNOWN_INCOMING_ENTITY_NAME

                        select new MoneyFlowDTO
                        {
                            Amount = moneyFlow.Value,
                            Description = moneyFlow.Description,
                            EntityId = recipientEntityId,
                            EntityTypeId = recipientEntityTypeId,
                            FiscalYear = moneyFlow.FiscalYear,
                            Id = moneyFlow.MoneyFlowId,
                            MoneyFlowStatus = status,
                            MoneyFlowStatusId = statusId,
                            MoneyFlowType = incomingMoneyFlowType,
                            ParticipantTypeId = participantType == null ? default(int?) : participantType.ParticipantTypeId,
                            ParticipantTypeName = participantType == null ? null : participantType.Name,
                            ParentMoneyFlowId = moneyFlow.ParentMoneyFlowId,
                            SourceRecipientTypeName = sourceTypeName,
                            SourceRecipientEntityTypeId = sourceTypeId,
                            SourceRecipientEntityId = sourceEntityId,
                            SourceRecipientName = sourceEntityName,
                            TransactionDate = moneyFlow.TransactionDate,
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

                        join moneyFlow in moneyFlows
                        on new { ParticipantId = participant.ParticipantId, TypeId = participantTypeEntityTypeId }
                        equals new { ParticipantId = moneyFlow.EntityId, TypeId = moneyFlow.EntityTypeId }

                        where participant.PersonId.HasValue && participant.PersonId.Value == personId
                        select moneyFlow;

            query = query.Distinct().Apply(queryOperator);
            return query;
        }

        #endregion

        #region Summaries

        /// <summary>
        /// Returns a query to retrieve fiscal year summaries from the money flows in the context.  The fiscal
        /// year summaries will detail the entity, incoming and outgoing amounts, fiscal year, and status.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to return fiscal year summaries from the money flow.</returns>
        public static IQueryable<FiscalYearSummaryDTO> CreateGetFiscalYearSummaryDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from moneyFlow in CreateGetSimpleMoneyFlowDTOsQuery(context)

                        group moneyFlow by new
                        {
                            FiscalYear = moneyFlow.FiscalYear,
                            EntityId = moneyFlow.EntityId,
                            EntityTypeId = moneyFlow.EntityTypeId,
                            StatusId = moneyFlow.MoneyFlowStatusId,
                        } into g

                        let incomingAmount = g.Where(x => x.Amount >= 0).Sum(x => (decimal?)x.Amount) ?? 0.0m
                        let outgoingAmount = -1 * g.Where(x => x.Amount < 0).Sum(x => (decimal?)x.Amount) ?? 0.0m

                        join status in context.MoneyFlowStatuses
                        on g.Key.StatusId equals status.MoneyFlowStatusId

                        select new FiscalYearSummaryDTO
                        {
                            EntityId = g.Key.EntityId,
                            EntityTypeId = g.Key.EntityTypeId,
                            FiscalYear = g.Key.FiscalYear,
                            IncomingAmount = incomingAmount,
                            OutgoingAmount = outgoingAmount,
                            RemainingAmount = incomingAmount - outgoingAmount,
                            Status = status.MoneyFlowStatusName,
                            StatusId = g.Key.StatusId
                        };
            query = query.OrderByDescending(x => x.FiscalYear).ThenBy(x => x.StatusId);
            return query;
        }

        /// <summary>
        /// Returns a query to retrieve fiscal year summaries from the money flows in the context for the given entity type and entity id.  The fiscal
        /// year summaries will detail the entity, incoming and outgoing amounts, fiscal year, and status.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to return fiscal year summaries from the money flow.</returns>
        public static IQueryable<FiscalYearSummaryDTO> CreateGetFiscalYearSummaryDTOByEntityIdAndEntityTypeIdQuery(EcaContext context, int entityId, int moneyFlowSourceRecipientTypeId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetFiscalYearSummaryDTOQuery(context).Where(x => x.EntityId == entityId && x.EntityTypeId == moneyFlowSourceRecipientTypeId);
        }

        /// <summary>
        /// Returns a query to retrieve fiscal year summaries from the money flows in the context for the given project by id.  The fiscal
        /// year summaries will detail the entity, incoming and outgoing amounts, fiscal year, and status.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to return fiscal year summaries from the money flow.</returns>
        public static IQueryable<FiscalYearSummaryDTO> CreateGetFiscalYearSummaryDTOByProjectId(EcaContext context, int projectId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetFiscalYearSummaryDTOByEntityIdAndEntityTypeIdQuery(context, projectId, MoneyFlowSourceRecipientType.Project.Id);
        }

        /// <summary>
        /// Returns a query to retrieve fiscal year summaries from the money flows in the context for the given program by id.  The fiscal
        /// year summaries will detail the entity, incoming and outgoing amounts, fiscal year, and status.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to return fiscal year summaries from the money flow.</returns>
        public static IQueryable<FiscalYearSummaryDTO> CreateGetFiscalYearSummaryDTOByProgramId(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetFiscalYearSummaryDTOByEntityIdAndEntityTypeIdQuery(context, programId, MoneyFlowSourceRecipientType.Program.Id);
        }

        /// <summary>
        /// Returns a query to retrieve fiscal year summaries from the money flows in the context for the given office by id.  The fiscal
        /// year summaries will detail the entity, incoming and outgoing amounts, fiscal year, and status.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to return fiscal year summaries from the money flow.</returns>
        public static IQueryable<FiscalYearSummaryDTO> CreateGetFiscalYearSummaryDTOByOfficeId(EcaContext context, int officeId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetFiscalYearSummaryDTOByEntityIdAndEntityTypeIdQuery(context, officeId, MoneyFlowSourceRecipientType.Office.Id);
        }

        /// <summary>
        /// Returns a query to retrieve fiscal year summaries from the money flows in the context for the given org by id.  The fiscal
        /// year summaries will detail the entity, incoming and outgoing amounts, fiscal year, and status.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to return fiscal year summaries from the money flow.</returns>
        public static IQueryable<FiscalYearSummaryDTO> CreateGetFiscalYearSummaryDTOByOrganizationId(EcaContext context, int organizationId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetFiscalYearSummaryDTOByEntityIdAndEntityTypeIdQuery(context, organizationId, MoneyFlowSourceRecipientType.Organization.Id);
        }

        /// <summary>
        /// Returns a query to retrieve fiscal year summaries from the money flows in the context for the given person by id.  The fiscal
        /// year summaries will detail the entity, incoming and outgoing amounts, fiscal year, and status.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to return fiscal year summaries from the money flow.</returns>
        public static IQueryable<FiscalYearSummaryDTO> CreateGetFiscalYearSummaryDTOByPersonId(EcaContext context, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var participantTypeEntityTypeId = MoneyFlowSourceRecipientType.Participant.Id;
            var summaryQuery = CreateGetFiscalYearSummaryDTOQuery(context);

            var query = from participant in context.Participants

                        join summary in summaryQuery
                        on new { ParticipantId = participant.ParticipantId, TypeId = participantTypeEntityTypeId }
                        equals new { ParticipantId = summary.EntityId, TypeId = summary.EntityTypeId }

                        where participant.PersonId.HasValue && participant.PersonId.Value == personId
                        select summary;

            query = query.Distinct();
            return query;
        }

        /// <summary>
        /// Returns a query to get incoming and outgoing money flow dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The incoming and outgoing money flows.</returns>
        private static IQueryable<SimpleMoneyFlowDTO> CreateGetSimpleMoneyFlowDTOsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetIncomingSimpleMoneyFlowDTOsQuery(context).Union(CreateGetOutgoingSimpleMoneyFlowDTOsQuery(context));
        }

        /// <summary>
        /// Creates a query to get incoming simple money flow dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The incoming simple money flows.</returns>
        private static IQueryable<SimpleMoneyFlowDTO> CreateGetIncomingSimpleMoneyFlowDTOsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from moneyFlow in context.MoneyFlows
                        let sourceTypeName = moneyFlow.SourceType.TypeName
                        let sourceTypeId = moneyFlow.SourceTypeId
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
                            
                        select new SimpleMoneyFlowDTO
                        {
                            Amount = moneyFlow.Value,
                            EntityId = recipientEntityId,
                            EntityTypeId = recipientEntityTypeId,
                            FiscalYear = moneyFlow.FiscalYear,
                            Id = moneyFlow.MoneyFlowId,
                            MoneyFlowStatusId = statusId,
                            ParentMoneyFlowId = moneyFlow.ParentMoneyFlowId,
                            SourceRecipientEntityTypeId = sourceTypeId,
                            SourceRecipientEntityId = sourceEntityId,
                            TransactionDate = moneyFlow.TransactionDate,
                        };
            return query;
        }

        /// <summary>
        /// Creates a query to get outgoing simple money flow dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The outgoing simple money flows.</returns>
        private static IQueryable<SimpleMoneyFlowDTO> CreateGetOutgoingSimpleMoneyFlowDTOsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var expenseMoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Expense.Id;

            var query = from moneyFlow in context.MoneyFlows
                        let isExpense = moneyFlow.RecipientTypeId == expenseMoneyFlowSourceRecipientTypeId
                        let recipientTypeId = isExpense ? expenseMoneyFlowSourceRecipientTypeId : moneyFlow.RecipientTypeId
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

                        select new SimpleMoneyFlowDTO
                        {
                            Amount = -moneyFlow.Value,
                            EntityId = sourceEntityId,
                            EntityTypeId = sourceEntityTypeId,
                            FiscalYear = moneyFlow.FiscalYear,
                            Id = moneyFlow.MoneyFlowId,
                            MoneyFlowStatusId = statusId,
                            ParentMoneyFlowId = moneyFlow.ParentMoneyFlowId,
                            SourceRecipientEntityTypeId = recipientTypeId,
                            SourceRecipientEntityId = isExpense ? default(int?) : recipientEntityId,
                            TransactionDate = moneyFlow.TransactionDate,
                        };
            return query;
        }

        #endregion

        #region Source Money Flow DTOs

        /// <summary>
        /// Creates a query to retrieve incoming money flow dtos and return the original amount for the incoming money flow
        /// minus all child money flows.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query returning source money flow dtos detailing the original money flow amount, the remaining amount, and source entity information.</returns>
        public static IQueryable<SourceMoneyFlowDTO> CreateGetSourceMoneyFlowDTOsQuery(EcaContext context)
        {
            var query = (from incomingMoneyFlow in CreateIncomingMoneyFlowDTOsQuery(context)
                         let childMoneyFlows = context.MoneyFlows.Where(x => x.ParentMoneyFlowId == incomingMoneyFlow.Id)
                         orderby incomingMoneyFlow.SourceRecipientName
                         select new SourceMoneyFlowDTO
                         {
                             Amount = incomingMoneyFlow.Amount,
                             ChildMoneyFlowIds = childMoneyFlows.Select(x => x.MoneyFlowId),
                             EntityId = incomingMoneyFlow.EntityId,
                             EntityTypeId = incomingMoneyFlow.EntityTypeId,
                             FiscalYear = incomingMoneyFlow.FiscalYear,
                             Id = incomingMoneyFlow.Id,
                             RemainingAmount = incomingMoneyFlow.Amount - childMoneyFlows.Select(x => x.Value).DefaultIfEmpty().Sum(),
                             SourceName = incomingMoneyFlow.SourceRecipientName,
                             SourceEntityId = incomingMoneyFlow.SourceRecipientEntityId,
                             SourceEntityTypeId = incomingMoneyFlow.SourceRecipientEntityTypeId,
                             SourceEntityTypeName = incomingMoneyFlow.SourceRecipientTypeName
                         });
            return query;
        }

        /// <summary>
        /// Returns a query to retrieve source money flow dtos by entity type id and entity id.  The query looks for incoming moneyflows by entity type and id
        /// and details the original money flow amount and the remaining amount.  The query also returns source entity information.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="entityId">The entity id.</param>
        /// <param name="entityTypeId">The entity type id.</param>
        /// <returns>The query.</returns>
        public static IQueryable<SourceMoneyFlowDTO> CreateGetSourceMoneyFlowDTOsByEntityIdAndEntityTypeId(EcaContext context, int entityId, int entityTypeId)
        {
            return CreateGetSourceMoneyFlowDTOsQuery(context).Where(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);
        }

        /// <summary>
        /// Returns a query to retrieve source money flows for the project with the given project id.  The source money flows
        /// will details the original amount of money and the remaining amounts of money from that money flow, i.e. other money
        /// flows that have withdrawn from that source.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="projectId">The id of the project to get source money flows.</param>
        /// <returns>The source money flows.</returns>
        public static IQueryable<SourceMoneyFlowDTO> CreateGetSourceMoneyFlowDTOsByProjectId(EcaContext context, int projectId)
        {
            return CreateGetSourceMoneyFlowDTOsByEntityIdAndEntityTypeId(context, projectId, MoneyFlowSourceRecipientType.Project.Id);
        }

        /// <summary>
        /// Returns a query to retrieve source money flows for the program with the given program id.  The source money flows
        /// will details the original amount of money and the remaining amounts of money from that money flow, i.e. other money
        /// flows that have withdrawn from that source.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="projectId">The id of the program to get source money flows.</param>
        /// <returns>The source money flows.</returns>
        public static IQueryable<SourceMoneyFlowDTO> CreateGetSourceMoneyFlowDTOsByProgramId(EcaContext context, int programId)
        {
            return CreateGetSourceMoneyFlowDTOsByEntityIdAndEntityTypeId(context, programId, MoneyFlowSourceRecipientType.Program.Id);
        }

        /// <summary>
        /// Returns a query to retrieve source money flows for the organization with the given organization id.  The source money flows
        /// will details the original amount of money and the remaining amounts of money from that money flow, i.e. other money
        /// flows that have withdrawn from that source.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="projectId">The id of the organization to get source money flows.</param>
        /// <returns>The source money flows.</returns>
        public static IQueryable<SourceMoneyFlowDTO> CreateGetSourceMoneyFlowDTOsByOrganizationId(EcaContext context, int organizationId)
        {
            return CreateGetSourceMoneyFlowDTOsByEntityIdAndEntityTypeId(context, organizationId, MoneyFlowSourceRecipientType.Organization.Id);
        }

        /// <summary>
        /// Returns a query to retrieve source money flows for the office with the given office id.  The source money flows
        /// will details the original amount of money and the remaining amounts of money from that money flow, i.e. other money
        /// flows that have withdrawn from that source.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="projectId">The id of the office to get source money flows.</param>
        /// <returns>The source money flows.</returns>
        public static IQueryable<SourceMoneyFlowDTO> CreateGetSourceMoneyFlowDTOsByOfficeId(EcaContext context, int officeId)
        {
            return CreateGetSourceMoneyFlowDTOsByEntityIdAndEntityTypeId(context, officeId, MoneyFlowSourceRecipientType.Office.Id);
        }
        #endregion
    }
}
