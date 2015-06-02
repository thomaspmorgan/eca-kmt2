using ECA.Data;
using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq;
using System.Diagnostics.Contracts;

namespace ECA.Business.Queries.Admin
{
    /// <summary>
    /// Contains queries for moneyflows against a db context
    /// </summary>
    class MoneyFlowQueries
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
                            Type = (moneyflows.SourceProjectId == projectId ? moneyflows.RecipientType.TypeName : moneyflows.SourceType.TypeName),
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
                        };
            query = query.Apply(queryOperator);
            return query;
        }

    }
}
