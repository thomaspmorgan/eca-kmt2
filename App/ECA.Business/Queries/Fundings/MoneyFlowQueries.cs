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
        /// Creates a query object to get moneyflows by project id
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="projectId">The project id to find associated moneyflows</param>
        /// <param name="queryOperator">The query to retrieve filtered and sorted moneyflows</param>
        /// <returns>List of moneyflows that are paged, sorted, and filtered</returns>
        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowsByProjectIdQuery(EcaContext context, int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");

            var query = from moneyflows in context.MoneyFlows
                        let sourceType = moneyflows.SourceType.TypeName
                        let recipientType = moneyflows.RecipientType.TypeName
                        let amount = moneyflows.Value
                        where moneyflows.RecipientProjectId == projectId ||
                              moneyflows.SourceProjectId == projectId
                        select new MoneyFlowDTO
                        {
                            Id = moneyflows.MoneyFlowId,
                            TransactionDate = moneyflows.TransactionDate,
                            SourceType = (moneyflows.SourceProjectId == projectId ? moneyflows.RecipientType.TypeName : moneyflows.SourceType.TypeName),
                            Status = moneyflows.MoneyFlowStatus.MoneyFlowStatusName,
                            FromTo = (
                                // If project is source
                                moneyflows.SourceProjectId == projectId && moneyflows.RecipientAccommodationId != null ? moneyflows.RecipientAccommodation.Host.Name :
                                moneyflows.SourceProjectId == projectId && moneyflows.RecipientOrganizationId  != null ? moneyflows.RecipientOrganization.Name :
                                moneyflows.SourceProjectId == projectId && moneyflows.RecipientProgramId != null ? moneyflows.RecipientProgram.Name : 
                                moneyflows.SourceProjectId == projectId && moneyflows.RecipientProjectId != null ? moneyflows.RecipientProject.Name : 
                                // Participant can be person or organization
                                moneyflows.SourceProjectId == projectId && moneyflows.RecipientParticipantId != null && moneyflows.RecipientParticipant.Person != null ? moneyflows.RecipientParticipant.Person.FirstName + " " +  moneyflows.RecipientParticipant.Person.LastName :
                                moneyflows.SourceProjectId == projectId && moneyflows.RecipientParticipantId != null && moneyflows.RecipientParticipant.Organization != null ? moneyflows.RecipientParticipant.Organization.Name :
                                moneyflows.SourceProjectId == projectId && moneyflows.RecipientItineraryStopId != null ? "Unknown" : // ItineraryStop needs name
                                moneyflows.SourceProjectId == projectId && moneyflows.RecipientTransportationId != null ? moneyflows.RecipientTransportation.Carrier.Name :

                                // If project is recipient
                                moneyflows.RecipientProjectId == projectId && moneyflows.SourceOrganizationId != null ? moneyflows.SourceOrganization.Name :
                                moneyflows.RecipientProjectId == projectId && moneyflows.SourceProgramId != null ? moneyflows.SourceProgram.Name :
                                moneyflows.RecipientProjectId == projectId && moneyflows.SourceProjectId != null ? moneyflows.SourceProject.Name :
                                moneyflows.RecipientProjectId == projectId && moneyflows.SourceParticipantId != null && moneyflows.SourceParticipant.Person != null ? moneyflows.SourceParticipant.Person.FirstName + " " + moneyflows.SourceParticipant.Person.LastName :
                                moneyflows.RecipientProjectId == projectId && moneyflows.SourceParticipantId != null && moneyflows.SourceParticipant.Organization != null ? moneyflows.SourceParticipant.Organization.Name :
                                moneyflows.RecipientProjectId == projectId && moneyflows.SourceItineraryStopId != null ? "Unknown" : // ItineraryStop needs name

                                // else case
                                "Unknown"
                            ),
                            Amount = (sourceType == "Project" ? -amount : amount),
                            Description = moneyflows.Description,
                            FiscalYear = moneyflows.FiscalYear,
                            Type = moneyflows.MoneyFlowType.MoneyFlowTypeName
                        };
            query = query.Apply(queryOperator);
            return query;
        }

        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowsByProgramIdQuery(EcaContext context, int programId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");

            var query = from moneyflows in context.MoneyFlows
                        let sourceType = moneyflows.SourceType.TypeName
                        let recipientType = moneyflows.RecipientType.TypeName
                        let amount = moneyflows.Value
                        where moneyflows.RecipientProgramId == programId ||
                              moneyflows.SourceProgramId == programId
                        select new MoneyFlowDTO
                        {
                            Id = moneyflows.MoneyFlowId,
                            TransactionDate = moneyflows.TransactionDate,
                            SourceType = (moneyflows.SourceProgramId == programId ? moneyflows.RecipientType.TypeName : moneyflows.SourceType.TypeName),
                            Status = moneyflows.MoneyFlowStatus.MoneyFlowStatusName,
                            FromTo = (
                                // If project is source
                                moneyflows.SourceProgramId == programId && moneyflows.RecipientAccommodationId != null ? moneyflows.RecipientAccommodation.Host.Name :
                                moneyflows.SourceProgramId == programId && moneyflows.RecipientOrganizationId != null ? moneyflows.RecipientOrganization.Name :
                                moneyflows.SourceProgramId == programId && moneyflows.RecipientProgramId != null ? moneyflows.RecipientProgram.Name :
                                moneyflows.SourceProgramId == programId && moneyflows.RecipientProjectId != null ? moneyflows.RecipientProject.Name :
                                // Participant can be person or organization
                                moneyflows.SourceProgramId == programId && moneyflows.RecipientParticipantId != null && moneyflows.RecipientParticipant.Person != null ? moneyflows.RecipientParticipant.Person.FirstName + " " + moneyflows.RecipientParticipant.Person.LastName :
                                moneyflows.SourceProgramId == programId && moneyflows.RecipientParticipantId != null && moneyflows.RecipientParticipant.Organization != null ? moneyflows.RecipientParticipant.Organization.Name :
                                moneyflows.SourceProgramId == programId && moneyflows.RecipientItineraryStopId != null ? "Unknown" : // ItineraryStop needs name
                                moneyflows.SourceProgramId == programId && moneyflows.RecipientTransportationId != null ? moneyflows.RecipientTransportation.Carrier.Name :

                                // If project is recipient
                                moneyflows.RecipientProgramId == programId && moneyflows.SourceOrganizationId != null ? moneyflows.SourceOrganization.Name :
                                moneyflows.RecipientProgramId == programId && moneyflows.SourceProgramId != null ? moneyflows.SourceProgram.Name :
                                moneyflows.RecipientProgramId == programId && moneyflows.SourceProjectId != null ? moneyflows.SourceProject.Name :
                                moneyflows.RecipientProgramId == programId && moneyflows.SourceParticipantId != null && moneyflows.SourceParticipant.Person != null ? moneyflows.SourceParticipant.Person.FirstName + " " + moneyflows.SourceParticipant.Person.LastName :
                                moneyflows.RecipientProgramId == programId && moneyflows.SourceParticipantId != null && moneyflows.SourceParticipant.Organization != null ? moneyflows.SourceParticipant.Organization.Name :
                                moneyflows.RecipientProgramId == programId && moneyflows.SourceItineraryStopId != null ? "Unknown" : // ItineraryStop needs name

                                // else case
                                "Unknown"
                            ),
                            Amount = (sourceType == "Program" ? -amount : amount),
                            Description = moneyflows.Description,
                            FiscalYear = moneyflows.FiscalYear,
                            Type = moneyflows.MoneyFlowType.MoneyFlowTypeName
                        };
            query = query.Apply(queryOperator);
            return query;
        }

        public static IQueryable<MoneyFlowDTO> CreateOutgoingMoneyFlowDTOsQuery(EcaContext context)
        {
            var outgoingMoneyFlowType = MoneyFlowType.Outgoing.Value;
            var expenseMoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Expense.Id;

            var query = from moneyFlow in context.MoneyFlows
                        let sourceType = moneyFlow.SourceType.TypeName
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
                        let itinerary = "Itinerary Name"
                        let project = context.Projects.Where(x => x.ProjectId == recipientEntityId).FirstOrDefault()
                        let program = context.Programs.Where(x => x.ProgramId == recipientEntityId).FirstOrDefault()
                        let participant = context.Participants.Where(x => x.ParticipantId == recipientEntityId).FirstOrDefault()
                        let isOrgParticipant = participant.OrganizationId.HasValue
                        let isPersonParticipant = participant.PersonId.HasValue
                        let orgParticipant = participant.Organization
                        let personParticipant = participant.Person

                        let isExpense = moneyFlow.RecipientTypeId == expenseMoneyFlowSourceRecipientTypeId

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

        public static IQueryable<MoneyFlowDTO> CreateIncomingMoneyFlowDTOsQuery(EcaContext context)
        {
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
                        let itinerary = "Itinerary Name"
                        let project = context.Projects.Where(x => x.ProjectId == recipientEntityId).FirstOrDefault()
                        let program = context.Programs.Where(x => x.ProgramId == recipientEntityId).FirstOrDefault()
                        let participant = context.Participants.Where(x => x.ParticipantId == recipientEntityId).FirstOrDefault()
                        let isOrgParticipant = participant.OrganizationId.HasValue
                        let isPersonParticipant = participant.PersonId.HasValue
                        let orgParticipant = participant.Organization
                        let personParticipant = participant.Person

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

        //public static IQueryable<SourcedMoneyFlow> CreateGetSourcedMoneyFlows(EcaContext context)
        //{
        //    var expenseMoneyFlowTypeId = MoneyFlowSourceRecipientType.Expense.Id;
        //    var accomodationMoneyFlowTypeId = MoneyFlowSourceRecipientType.Accomodation.Id;
        //    var query = from moneyFlow in context.MoneyFlows

        //                let isSource = (moneyFlow.SourceItineraryStopId.HasValue
        //                || moneyFlow.SourceOrganizationId.HasValue
        //                || moneyFlow.SourceParticipantId.HasValue
        //                || moneyFlow.SourceProgramId.HasValue
        //                || moneyFlow.SourceProjectId.HasValue)

        //                let isExpense = moneyFlow.RecipientTypeId == expenseMoneyFlowTypeId || moneyFlow.RecipientTypeId == accomodationMoneyFlowTypeId

        //                let isItineraryStopMoneyFlow = moneyFlow.SourceItineraryStopId.HasValue || moneyFlow.RecipientItineraryStopId.HasValue
        //                let isOrganizationMoneyFlow = moneyFlow.SourceOrganizationId.HasValue || moneyFlow.RecipientOrganizationId.HasValue
        //                let isParticipantMoneyFlow = moneyFlow.SourceParticipantId.HasValue || moneyFlow.RecipientParticipantId.HasValue
        //                let isProgramMoneyFlow = moneyFlow.SourceProgramId.HasValue || moneyFlow.RecipientProgramId.HasValue
        //                let isProjectMoneyFlow = moneyFlow.SourceProjectId.HasValue || moneyFlow.RecipientProjectId.HasValue
        //                let isTransportationMoneyFlow = moneyFlow.RecipientTransportationId.HasValue
        //                let isAccomodationMoneyFlow = moneyFlow.RecipientAccommodationId.HasValue

        //                let entityId = isItineraryStopMoneyFlow ? (isSource ? moneyFlow.SourceItineraryStopId.Value : moneyFlow.RecipientItineraryStopId.Value)
        //                                : isOrganizationMoneyFlow ? (isSource ? moneyFlow.SourceOrganizationId.Value : moneyFlow.RecipientOrganizationId.Value)
        //                                : isParticipantMoneyFlow ? (isSource ? moneyFlow.SourceParticipantId.Value : moneyFlow.RecipientParticipantId.Value)
        //                                : isProgramMoneyFlow ? (isSource ? moneyFlow.SourceProgramId.Value : moneyFlow.RecipientProgramId.Value)
        //                                : isProjectMoneyFlow ? (isSource ? moneyFlow.SourceProjectId.Value : moneyFlow.RecipientProjectId.Value)
        //                                : isTransportationMoneyFlow ? moneyFlow.RecipientTransportationId.Value
        //                                : isAccomodationMoneyFlow ? moneyFlow.RecipientAccommodationId.Value
        //                                : -1

        //                select new SourcedMoneyFlow
        //                {
        //                    Description = moneyFlow.Description,
        //                    EntityId = entityId,
        //                    FiscalYear = moneyFlow.FiscalYear,
        //                    History = moneyFlow.History,
        //                    IsExpense = isExpense,
        //                    IsItineraryStopMoneyFlow = isItineraryStopMoneyFlow,
        //                    IsOrganizationMoneyFlow = isOrganizationMoneyFlow,
        //                    IsParticipantMoneyFlow = isParticipantMoneyFlow,
        //                    IsProgramMoneyFlow = isProgramMoneyFlow,
        //                    IsProjectMoneyFlow = isProjectMoneyFlow,
        //                    IsSource = isSource,
        //                    MoneyFlowId = moneyFlow.MoneyFlowId,

        //                    MoneyFlowStatus = moneyFlow.MoneyFlowStatus,
        //                    MoneyFlowStatusId = moneyFlow.MoneyFlowStatusId,
        //                    MoneyFlowType = moneyFlow.MoneyFlowType,
        //                    MoneyFlowTypeId = moneyFlow.MoneyFlowTypeId,
        //                    Parent = moneyFlow.Parent,
        //                    RecipientAccommodation = moneyFlow.RecipientAccommodation,
        //                    RecipientAccommodationId = moneyFlow.RecipientAccommodationId,
        //                    RecipientItineraryStop = moneyFlow.RecipientItineraryStop,
        //                    RecipientItineraryStopId = moneyFlow.RecipientItineraryStopId,
        //                    RecipientOrganization = moneyFlow.RecipientOrganization,
        //                    RecipientOrganizationId = moneyFlow.RecipientOrganizationId,
        //                    RecipientParticipant = moneyFlow.RecipientParticipant,
        //                    RecipientParticipantId = moneyFlow.RecipientParticipantId,
        //                    RecipientProgram = moneyFlow.RecipientProgram,
        //                    RecipientProgramId = moneyFlow.RecipientProgramId,
        //                    RecipientProject = moneyFlow.RecipientProject,
        //                    RecipientProjectId = moneyFlow.RecipientProjectId,
        //                    RecipientTransportation = moneyFlow.RecipientTransportation,
        //                    RecipientTransportationId = moneyFlow.RecipientTransportationId,
        //                    RecipientType = moneyFlow.RecipientType,
        //                    RecipientTypeId = moneyFlow.RecipientTypeId,
        //                    SourceItineraryStop = moneyFlow.SourceItineraryStop,
        //                    SourceItineraryStopId = moneyFlow.SourceItineraryStopId,
        //                    SourceOrganization = moneyFlow.SourceOrganization,
        //                    SourceOrganizationId = moneyFlow.SourceOrganizationId,
        //                    SourceParticipant = moneyFlow.SourceParticipant,
        //                    SourceParticipantId = moneyFlow.SourceParticipantId,
        //                    SourceProgram = moneyFlow.SourceProgram,
        //                    SourceProgramId = moneyFlow.SourceProgramId,
        //                    SourceProject = moneyFlow.SourceProject,
        //                    SourceProjectId = moneyFlow.SourceProjectId,
        //                    SourceType = moneyFlow.SourceType,
        //                    SourceTypeId = moneyFlow.SourceTypeId,
        //                    TransactionDate = moneyFlow.TransactionDate,
        //                    Value = moneyFlow.Value
        //                };
        //    return query;
        //}

        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowDTOsQuery(EcaContext context)
        {
            var query = CreateIncomingMoneyFlowDTOsQuery(context).Union(CreateOutgoingMoneyFlowDTOsQuery(context));
            return query;
        }

        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlosDTOsByEntityType(EcaContext context, int entityId, int entityTypeId)
        {
            return CreateGetMoneyFlowDTOsQuery(context).Where(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);
        }

        public static IQueryable<MoneyFlowDTO> CreateGetMoneyFlowDTOsByProjectId(EcaContext context, int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var query = CreateGetMoneyFlosDTOsByEntityType(context, projectId, MoneyFlowSourceRecipientType.Project.Id);
            query = query.Apply(queryOperator);
            return query;
        }
    }

    /// <summary>
    /// A SourcedMoneyFlow is a money flow item that has been sized down to make it easier
    /// to query for money flow dtos by determining what type of entities the source and recipients are.
    /// 
    /// </summary>
    public class SourcedMoneyFlow
    {
        public MoneyFlow MoneyFlow { get; set; }

        public bool IsProgramMoneyFlow { get; set; }

        public bool IsProjectMoneyFlow { get; set; }

        public bool IsItineraryStopMoneyFlow { get; set; }

        public bool IsOrganizationMoneyFlow { get; set; }

        public bool IsParticipantMoneyFlow { get; set; }

        public int EntityId { get; set; }
    }
}
