using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// The ParticipantExchangeVisitorQueries are used to query a DbContext for Participant Exchange Visitor information.
    /// </summary>
    public static class ParticipantExchangeVisitorQueries
    {
        /// <summary>
        /// Query to get a list of participant people with Exchange Visitor info
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>List of participant exchange visitors</returns>
        public static IQueryable<ParticipantExchangeVisitorDTO> CreateGetParticipantExchangeVisitorsDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = (from p in context.ParticipantExchangeVisitors
                         select new ParticipantExchangeVisitorDTO
                         {
                             ParticipantId = p.ParticipantId,
                             ProjectId = p.Participant.ProjectId,
                             FieldOfStudy = p.FieldOfStudy != null ? p.FieldOfStudy.Description : string.Empty,
                             ProgramCategory = p.ProgramCategory != null ? p.ProgramCategory.Description : string.Empty,
                             Position = p.Position != null ? p.Position.Description : string.Empty,
                             FieldOfStudyId = p.FieldOfStudyId,
                             ProgramCategoryId = p.ProgramCategoryId,
                             PositionId = p.PositionId,
                             FundingGovtAgency1 = p.FundingGovtAgency1 ?? 0,
                             GovtAgency1Id = p.GovtAgency1Id ?? 0,
                             GovtAgency1Name = p.GovtAgency1 != null ? p.GovtAgency1.Description : string.Empty,
                             GovtAgency1OtherName = p.GovtAgency1OtherName != null ? p.GovtAgency1OtherName : string.Empty,
                             FundingGovtAgency2 = p.FundingGovtAgency2 ?? 0,
                             GovtAgency2Id = p.GovtAgency2Id ?? 0,
                             GovtAgency2Name = p.GovtAgency2 != null ? p.GovtAgency2.Description : string.Empty,
                             GovtAgency2OtherName = p.GovtAgency2OtherName != null ? p.GovtAgency2OtherName : string.Empty,
                             FundingIntlOrg1 = p.FundingIntlOrg1 ?? 0,
                             IntlOrg1Id = p.IntlOrg1Id ?? 0,
                             IntlOrg1Name = p.IntlOrg1 != null ? p.IntlOrg1.Description : string.Empty,
                             IntlOrg1OtherName = p.IntlOrg1OtherName != null ? p.IntlOrg1OtherName : string.Empty,
                             FundingIntlOrg2 = p.FundingIntlOrg2 ?? 0,
                             IntlOrg2Id = p.IntlOrg2Id ?? 0,
                             IntlOrg2Name = p.IntlOrg2 != null ? p.IntlOrg2.Description : string.Empty,
                             IntlOrg2OtherName = p.IntlOrg2OtherName != null ? p.IntlOrg2OtherName : string.Empty,
                             FundingOther = p.FundingOther ?? 0,
                             OtherName = p.OtherName != null ? p.OtherName : string.Empty,
                             FundingPersonal = p.FundingPersonal ?? 0,
                             FundingSponsor = p.FundingSponsor ?? 0,
                             FundingTotal = p.FundingTotal ?? 0,
                             FundingVisBNC = p.FundingVisBNC ?? 0,
                             FundingVisGovt = p.FundingVisGovt ?? 0
                         });
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantExchangeVisitors in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The filtered and sorted query to retrieve participantExchangeVisitors.</returns>
        public static IQueryable<ParticipantExchangeVisitorDTO> CreateGetParticipantExchangeVisitorsDTOQuery(EcaContext context, QueryableOperator<ParticipantExchangeVisitorDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetParticipantExchangeVisitorsDTOQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantExchangeVisitors for the project with the given id in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The filtered and sorted query to retrieve participantExchangeVisitors.</returns>
        public static IQueryable<ParticipantExchangeVisitorDTO> CreateGetParticipantExchangeVisitorsDTOByProjectIdQuery(EcaContext context, int projectId, QueryableOperator<ParticipantExchangeVisitorDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetParticipantExchangeVisitorsDTOQuery(context).Where(x => x.ProjectId == projectId);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns the participantExchangeVisitors by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantExchangeVisitors</returns>
        public static IQueryable<ParticipantExchangeVisitorDTO> CreateGetParticipantExchangeVisitorDTOByIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetParticipantExchangeVisitorsDTOQuery(context).
                Where(p => p.ParticipantId == participantId);
            return query;
        }
    }
}
