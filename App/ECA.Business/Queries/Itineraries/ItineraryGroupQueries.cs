using ECA.Business.Models.Itineraries;
using ECA.Core.DynamicLinq;
using ECA.Data;
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
            return CreateGetItineraryGroupDTOQuery(context)
                .Where(x => x.ItineraryId == itineraryId)
                .Where(x => x.ProjectId == projectId)
                .Apply(queryOperator);
        }
    }
}
