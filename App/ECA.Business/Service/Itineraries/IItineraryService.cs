using ECA.Business.Queries.Models.Itineraries;
using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryService is used to perform crud operations for project itineraries.
    /// </summary>
    [ContractClass(typeof(ItineraryServiceContract))]
    public interface IItineraryService : ISaveable
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
        /// Returns the itinerary with the given id and project id.
        /// </summary>
        /// <param name="id">The id of the itinerary.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The itinerary.</returns>
        ItineraryDTO GetItineraryById(int projectId, int id);

        /// <summary>
        /// Returns the itinerary with the given id and project id.
        /// </summary>
        /// <param name="id">The id of the itinerary.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The itinerary.</returns>
        Task<ItineraryDTO> GetItineraryByIdAsync(int projectId, int id);

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
        /// <param name="projectId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ItineraryDTO GetItineraryById(int projectId, int id)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ItineraryDTO> GetItineraryByIdAsync(int projectId, int id)
        {
            return Task.FromResult<ItineraryDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult<int>(1);
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