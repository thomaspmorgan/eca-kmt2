using System.Threading.Tasks;
using ECA.Data;
using System.Collections.Generic;
using ECA.Business.Queries.Models.Itineraries;
using ECA.Core.Service;
using System;
using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An IItineraryStopService is used to perform crud operations on itinerary stops.
    /// </summary>
    [ContractClass(typeof(ItineraryStopServiceContract))]
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

        /// <summary>
        /// Sets the participants on the itinerary stop.
        /// </summary>
        /// <param name="itineraryStopParticipants">The business entity containing the participants by id that should be set on the itinerary stop.</param>
        void SetParticipants(ItineraryStopParticipants itineraryStopParticipants);

        /// <summary>
        /// Sets the participants on the itinerary top.
        /// </summary>
        /// <param name="itineraryStopParticipants">The business entity containing the participants by id that should be set on the itinerary stop.</param>
        /// <returns>The task.</returns>
        Task SetParticipantsAsync(ItineraryStopParticipants itineraryStopParticipants);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IItineraryStopService))]
    public abstract class ItineraryStopServiceContract : IItineraryStopService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addedStop"></param>
        /// <returns></returns>
        public ItineraryStop Create(AddedEcaItineraryStop addedStop)
        {
            Contract.Requires(addedStop != null, "The stop must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addedStop"></param>
        /// <returns></returns>
        public Task<ItineraryStop> CreateAsync(AddedEcaItineraryStop addedStop)
        {
            Contract.Requires(addedStop != null, "The stop must not be null.");
            return Task.FromResult<ItineraryStop>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itineraryStopId"></param>
        /// <returns></returns>
        public ItineraryStopDTO GetItineraryStopById(int itineraryStopId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itineraryStopId"></param>
        /// <returns></returns>
        public Task<ItineraryStopDTO> GetItineraryStopByIdAsync(int itineraryStopId)
        {
            return Task.FromResult<ItineraryStopDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="itineraryId"></param>
        /// <returns></returns>
        public List<ItineraryStopDTO> GetItineraryStopsByItineraryId(int projectId, int itineraryId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="itineraryId"></param>
        /// <returns></returns>
        public Task<List<ItineraryStopDTO>> GetItineraryStopsByItineraryIdAsync(int projectId, int itineraryId)
        {
            return Task.FromResult<List<ItineraryStopDTO>>(null);
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
        /// <param name="itineraryStopParticipants"></param>
        public void SetParticipants(ItineraryStopParticipants itineraryStopParticipants)
        {
            Contract.Requires(itineraryStopParticipants != null, "The itineraryStopParticipants must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itineraryStopParticipants"></param>
        /// <returns></returns>
        public Task SetParticipantsAsync(ItineraryStopParticipants itineraryStopParticipants)
        {
            Contract.Requires(itineraryStopParticipants != null, "The itineraryStopParticipants must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedStop"></param>
        public void Update(UpdatedEcaItineraryStop updatedStop)
        {
            Contract.Requires(updatedStop != null, "The updated stop must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedStop"></param>
        /// <returns></returns>
        public Task UpdateAsync(UpdatedEcaItineraryStop updatedStop)
        {
            Contract.Requires(updatedStop != null, "The updated stop must not be null.");
            return Task.FromResult<object>(null);
        }
    }
}