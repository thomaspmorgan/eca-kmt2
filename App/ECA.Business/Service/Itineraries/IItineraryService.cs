using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECA.Business.Queries.Itineraries;
using ECA.Data;
using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryService is used to perform crud operations for project itineraries.
    /// </summary>
    [ContractClass(typeof(ItineraryServiceContract))]
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

        /// <summary>
        /// Creates a new itinerary in the eca datastore.
        /// </summary>
        /// <param name="itinerary">The new itinerary.</param>
        /// <returns>The itinerary that was added to the eca context.</returns>
        Itinerary Create(AddedEcaItinerary itinerary);

        /// <summary>
        /// Creates a new itinerary in the eca datastore.
        /// </summary>
        /// <param name="itinerary">The new itinerary.</param>
        /// <returns>The itinerary that was added to the eca context.</returns>
        Task<Itinerary> CreateAsync(AddedEcaItinerary itinerary);

        /// <summary>
        /// Updates the datastore's itinerary with the given itinerary.
        /// </summary>
        /// <param name="itinerary">The updated itinerary.</param>
        void Update(UpdatedEcaItinerary itinerary);

        /// <summary>
        /// Updates the datastore's itinerary with the given itinerary.
        /// </summary>
        /// <param name="itinerary">The updated itinerary.</param>
        Task UpdateAsync(UpdatedEcaItinerary itinerary);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IItineraryService))]
    public abstract class ItineraryServiceContract : IItineraryService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itinerary"></param>
        /// <returns></returns>
        public Itinerary Create(AddedEcaItinerary itinerary)
        {
            Contract.Requires(itinerary != null, "The itinerary must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itinerary"></param>
        /// <returns></returns>
        public Task<Itinerary> CreateAsync(AddedEcaItinerary itinerary)
        {
            Contract.Requires(itinerary != null, "The itinerary must not be null.");
            return Task.FromResult<Itinerary>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<ItineraryDTO> GetItinerariesByProjectId(int projectId)
        {
            return new List<ItineraryDTO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Task<List<ItineraryDTO>> GetItinerariesByProjectIdAsync(int projectId)
        {
            return Task.FromResult<List<ItineraryDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itinerary"></param>
        public void Update(UpdatedEcaItinerary itinerary)
        {
            Contract.Requires(itinerary != null, "The itinerary must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itinerary"></param>
        /// <returns></returns>
        public Task UpdateAsync(UpdatedEcaItinerary itinerary)
        {
            Contract.Requires(itinerary != null, "The itinerary must not be null.");
            return Task.FromResult<object>(null);
        }
    }
}