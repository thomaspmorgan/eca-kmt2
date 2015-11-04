using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// The ParticipantQueries are used to query a DbContext for Participant information.
    /// </summary>
    public static class ParticipantPersonsSevisCommStatusQueries
    {
        /// <summary>
        /// Query to get a list of participant people with SEVIS information
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>List of participant people with SEVIS</returns>
        public static IQueryable<ParticipantPersonSevisCommStatusDTO> CreateGetParticipantPersonsSevisCommStatusDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = (from p in context.ParticipantPersonSevisCommStatuses
                         select new ParticipantPersonSevisCommStatusDTO
                         {
                             Id = p.Id,
                             ParticipantId = p.ParticipantId,
                             SevisCommStatusId = p.SevisCommStatusId,
                             SevisCommStatusName = p.SevisCommStatus.SevisCommStatusName,
                             AddedOn = p.AddedOn
                         });
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantPersonSevises in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The filtered and sorted query to retrieve participantPersonSevises.</returns>
        public static IQueryable<ParticipantPersonSevisCommStatusDTO> CreateGetParticipantPersonsSevisCommStatusDTOQuery(EcaContext context, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetParticipantPersonsSevisCommStatusDTOQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns the participantPersonSevis by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public static IQueryable<ParticipantPersonSevisCommStatusDTO> CreateGetParticipantPersonsSevisCommStatusDTOByIdQuery(EcaContext context, int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetParticipantPersonsSevisCommStatusDTOQuery(context).
                Where(p => p.ParticipantId == participantId);
            query = query.Apply(queryOperator);
            return query;
        }

        public static IQueryable<ParticipantPersonSevisCommStatusDTO> CreateParticipantPersonsSevisCommStatusesDTOsByParticipantIdsQuery(EcaContext context, int[] participantIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetParticipantPersonsSevisCommStatusDTOQuery(context).Where(p => participantIds.Contains(p.ParticipantId));
            return query;
        }
    }
}
