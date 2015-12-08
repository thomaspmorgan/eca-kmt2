using System.Collections.Generic;
using System.Threading.Tasks;
using ECA.Business.Queries.Itineraries;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryService is used to perform crud operations for project itineraries.
    /// </summary>
    public interface IItineraryService
    {
        /// <summary>
        /// Returns the itineraries for the given project by project id.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The itineraries of the project.</returns>
        List<ItineraryDTO> GetItinerariesByProjectId(int projectId);

        /// <summary>
        /// Returns the itineraries for the given project by project id.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The itineraries of the project.</returns>
        Task<List<ItineraryDTO>> GetItinerariesByProjectIdAsync(int projectId);
    }
}