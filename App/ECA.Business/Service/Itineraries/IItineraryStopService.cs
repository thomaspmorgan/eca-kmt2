using System.Threading.Tasks;
using ECA.Data;
using System.Collections.Generic;
using ECA.Business.Queries.Models.Itineraries;
using ECA.Core.Service;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An IItineraryStopService is used to perform crud operations on itinerary stops.
    /// </summary>
    public interface IItineraryStopService : ISaveable
    {
        /// <summary>
        /// Creates a new itinerary stop.
        /// </summary>
        /// <param name="addedStop">The new itinerary stop.</param>
        /// <returns>The Eca.Data ItineraryStop instance.</returns>
        ItineraryStop Create(AddedEcaItineraryStop addedStop);

        /// <summary>
        /// Creates a new itinerary stop.
        /// </summary>
        /// <param name="addedStop">The new itinerary stop.</param>
        /// <returns>The Eca.Data ItineraryStop instance.</returns>
        Task<ItineraryStop> CreateAsync(AddedEcaItineraryStop addedStop);

        /// <summary>
        /// Updates the system's itinerary stop with the given updated stop.
        /// </summary>
        /// <param name="updatedStop">The updated itinerary stop.</param>
        void Update(UpdatedEcaItineraryStop updatedStop);

        /// <summary>
        /// Updates the system's itinerary stop with the given updated stop.
        /// </summary>
        /// <param name="updatedStop">The updated itinerary stop.</param>
        Task UpdateAsync(UpdatedEcaItineraryStop updatedStop);

        /// <summary>
        /// Returns a list of itinerary stops for the itinerary with the given id and project by id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>Returns the itinerary stops for the itinerary and project by ids.</returns>
        List<ItineraryStopDTO> GetItineraryStopsByItineraryId(int projectId, int itineraryId);

        /// <summary>
        /// Returns a list of itinerary stops for the itinerary with the given id and project by id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>Returns the itinerary stops for the itinerary and project by ids.</returns>
        Task<List<ItineraryStopDTO>> GetItineraryStopsByItineraryIdAsync(int projectId, int itineraryId);

        /// <summary>
        /// Returns the itinerary stopo with the given id.
        /// </summary>
        /// <param name="itineraryStopId">The itinerary stop id.</param>
        /// <returns>The itinerary stop dto.</returns>
        ItineraryStopDTO GetItineraryStopById(int itineraryStopId);

        /// <summary>
        /// Returns the itinerary stopo with the given id.
        /// </summary>
        /// <param name="itineraryStopId">The itinerary stop id.</param>
        /// <returns>The itinerary stop dto.</returns>
        Task<ItineraryStopDTO> GetItineraryStopByIdAsync(int itineraryStopId);
    }
}