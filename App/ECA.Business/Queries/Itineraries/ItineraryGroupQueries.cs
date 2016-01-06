using ECA.Business.Queries.Models.Itineraries;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Itineraries
{
    /// <summary>
    /// ItineraryGroupQueries can be used to query a given EcaContext for itinerary group objects.
    /// </summary>
    public static class ItineraryGroupQueries
    {
        /// <summary>
        /// Creates a query to return all itinerary groups.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to get itinerary groups.</returns>
        public static IQueryable<ItineraryGroupDTO> CreateGetItineraryGroupDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = context.ItineraryGroups.Select(x => new ItineraryGroupDTO
            {
                ItineraryGroupId = x.ItineraryGroupId,
                ItineraryId = x.ItineraryId,
                ItineraryName = x.Itinerary.Name,
                ItineraryGroupName = x.Name,
                ProjectId = x.Itinerary.ProjectId
            });
            return query;
        }

        /// <summary>
        /// Returns a query to get all filtered, sorted, and paged itinerary groups by itinerary id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <param name="projectId">The project id.  Used for security purposes.</param>
        /// <returns>The query to get all filtered, sorted, and paged itinerary groups.</returns>
        public static IQueryable<ItineraryGroupDTO> CreateGetItineraryGroupDTOByItineraryIdQuery(EcaContext context, int projectId, int itineraryId, QueryableOperator<ItineraryGroupDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return CreateGetItineraryGroupDTOQuery(context)
                .Where(x => x.ItineraryId == itineraryId)
                .Where(x => x.ProjectId == projectId)
                .Apply(queryOperator);
        }

        /// <summary>
        /// Returns a query that retrieves itinerary groups and their participant persons given the project and itinerary.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query that retrieves itinerary groups and their people.</returns>
        public static IQueryable<ItineraryGroupParticipantsDTO> CreateGetItineraryGroupParticipantsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from itineraryGroup in context.ItineraryGroups
                        let itinerary = itineraryGroup.Itinerary
                        let participants = itineraryGroup.Participants
                        

                        select new ItineraryGroupParticipantsDTO
                        {
                            ItineraryGroupId = itineraryGroup.ItineraryGroupId,
                            ItineraryGroupName = itineraryGroup.Name,
                            ItineraryId = itineraryGroup.ItineraryId,
                            ItineraryName = itinerary.Name,
                            People = participants.Where(x => x.PersonId.HasValue).Select(x => new ItineraryParticipantDTO
                            {
                                FullName = x.Person.FullName,
                                ParticipantId = x.ParticipantId,
                                PersonId = x.PersonId.Value,
                            }),
                            ProjectId = itinerary.ProjectId
                        };
            return query;          
        }

        /// <summary>
        /// Returns a query that retrieves itinerary groups and their participant persons given the project and itinerary.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="projectId">The project by id.  Used for security purposes.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>The query that retrieves itinerary groups and their people.</returns>
        public static IQueryable<ItineraryGroupParticipantsDTO> CreateGetItineraryGroupParticipantsQuery(EcaContext context, int projectId, int itineraryId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetItineraryGroupParticipantsQuery(context).Where(x => x.ProjectId == projectId && x.ItineraryId == itineraryId);
            return query;
        }

        /// <summary>
        /// Returns a query that retrieves itinerary groups that match the given criteria.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="participantIds">The group participants by id.</param>
        /// <returns>The query to find the matching itinerary groups.</returns>
        public static IQueryable<ItineraryGroupDTO> CreateGetEqualItineraryGroupsQuery(EcaContext context, int itineraryId, int projectId, IEnumerable<int> participantIds)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from itineraryGroup in context.ItineraryGroups
                        let itinerary = itineraryGroup.Itinerary
                        let project = itinerary.Project
                        let itineraryGroupParticipantIds = itineraryGroup.Participants.Select(x => x.ParticipantId)

                        where itineraryGroupParticipantIds.Distinct().OrderBy(x => x).All(x => participantIds.Distinct().OrderBy(y => y).Contains(x))
                        && itinerary.ItineraryId == itineraryId
                        && project.ProjectId == projectId

                        select new ItineraryGroupDTO
                        {
                            ItineraryGroupId = itineraryGroup.ItineraryGroupId,
                            ItineraryGroupName = itineraryGroup.Name,
                            ItineraryId = itineraryGroup.ItineraryId,
                            ItineraryName = itineraryGroup.Itinerary.Name,
                            ProjectId = itineraryGroup.Itinerary.ProjectId
                        };
            return query;
        }
    }
}
