using ECA.Business.Queries.Models.Itineraries;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryGroupService is a service capable of performing crud operations on itinerary groups.
    /// </summary>
    public interface IItineraryGroupService : ISaveable
    {
        /// <summary>
        /// Returns the itinerary groups for the given itinerary by id.
        /// </summary>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <param name="projectId">The project id.  Used for security purposes.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, sorted itinerary groups.</returns>
        PagedQueryResults<ItineraryGroupDTO> GetItineraryGroupsByItineraryId(int projectId, int itineraryId, QueryableOperator<ItineraryGroupDTO> queryOperator);

        /// <summary>
        /// Returns the itinerary groups for the given itinerary by id.
        /// </summary>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <param name="projectId">The project id.  Used for security purposes.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, sorted itinerary groups.</returns>
        Task<PagedQueryResults<ItineraryGroupDTO>> GetItineraryGroupsByItineraryIdAsync(int projectId, int itineraryId, QueryableOperator<ItineraryGroupDTO> queryOperator);
    }
}