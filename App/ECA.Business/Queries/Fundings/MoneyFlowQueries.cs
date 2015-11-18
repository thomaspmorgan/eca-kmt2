﻿using ECA.Business.Queries.Models.Fundings;
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

        #endregion

        #region Summaries

        public static IQueryable<SimpleFiscalYearSummaryDTO> CreateGetIncomingFiscalYearSummaryDTOQuery(EcaContext context)
        {
            var query = from moneyFlow in CreateIncomingMoneyFlowDTOsQuery(context)
                        where moneyFlow.Amount >= 0
                        group moneyFlow by new
                        {
                            FiscalYear = moneyFlow.FiscalYear,
                            EntityId = moneyFlow.EntityId,
                            EntityTypeId = moneyFlow.EntityTypeId,
                            StatusId = moneyFlow.MoneyFlowStatusId,
                        } into g
                        select new SimpleFiscalYearSummaryDTO
                        {
                            Amount = g.Sum(x => x.Amount),
                            EntityId = g.Key.EntityId,
                            EntityTypeId = g.Key.EntityTypeId,
                            FiscalYear = g.Key.FiscalYear,
                            StatusId = g.Key.StatusId
                        };
            return query;
        }

        public static IQueryable<SimpleFiscalYearSummaryDTO> CreateGetOutgoingFiscalYearSummaryDTOQuery(EcaContext context)
        {

            var query = from moneyFlow in CreateOutgoingMoneyFlowDTOsQuery(context)
                        where moneyFlow.Amount < 0
                        group moneyFlow by new
                        {
                            FiscalYear = moneyFlow.FiscalYear,
                            EntityId = moneyFlow.EntityId,
                            EntityTypeId = moneyFlow.EntityTypeId,
                            StatusId = moneyFlow.MoneyFlowStatusId,
                        } into g
                        select new SimpleFiscalYearSummaryDTO
                        {
                            Amount = g.Sum(x => x.Amount),
                            EntityId = g.Key.EntityId,
                            EntityTypeId = g.Key.EntityTypeId,
                            FiscalYear = g.Key.FiscalYear,
                            StatusId = g.Key.StatusId
                        };
            return query;
        }

        //public static IQueryable<FiscalYearSummaryDTO> CreateGetFiscalYearSummaryDTOQuery(EcaContext context)
        //{
        //    var query = from incomingSummary in CreateGetIncomingFiscalYearSummaryDTOQuery(context)
        //                from outgoingSummary in CreateGetOutgoingFiscalYearSummaryDTOQuery(context)
        //                let totalRemaining = 

        //                where incomingSummary.EntityId == outgoingSummary.EntityId
        //                && incomingSummary.EntityTypeId == outgoingSummary.EntityTypeId
        //                && incomingSummary.FiscalYear == outgoingSummary.FiscalYear
        //                && incomingSummary.StatusId == outgoingSummary.StatusId

        //                select new FiscalYearSummaryDTO
        //                {

        //                };


        //}

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
