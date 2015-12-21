using ECA.Business.Queries.Models.Itineraries;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryGroupService is a service capable of performing crud operations on itinerary groups.
    /// </summary>
    [ContractClass(typeof(ItineraryGroupServiceContract))]
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

        /// <summary>
        /// Returns the itinerary groups and participant persons given the project id and itinerary id.
        /// </summary>
        /// <param name="projectId">The id of the project.  Used for security purposes.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>The list of itinerary groups and participant persons per group.</returns>
        Task<List<ItineraryGroupParticipantsDTO>> GetItineraryGroupPersonsByItineraryIdAsync(int projectId, int itineraryId);

        /// <summary>
        /// Returns the itinerary groups and participant persons given the project id and itinerary id.
        /// </summary>
        /// <param name="projectId">The id of the project.  Used for security purposes.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>The list of itinerary groups and participant persons per group.</returns>
        List<ItineraryGroupParticipantsDTO> GetItineraryGroupPersonsByItineraryId(int projectId, int itineraryId);

        /// <summary>
        /// Creates a new itinerary group.
        /// </summary>
        /// <param name="addedGroup">The itinerary group.</param>
        /// <returns>The added eca.data itinerary group.</returns>
        ItineraryGroup Create(AddedEcaItineraryGroup addedGroup);

        /// <summary>
        /// Creates a new itinerary group.
        /// </summary>
        /// <param name="addedGroup">The itinerary group.</param>
        /// <returns>The added eca.data itinerary group.</returns>
        Task<ItineraryGroup> CreateAsync(AddedEcaItineraryGroup addedGroup);

        /// <summary>
        /// Returns the itinerary groups and participant persons given the project id and itinerary id.
        /// </summary>
        /// <param name="projectId">The id of the project.  Used for security purposes.</param>
        /// <param name="itineraryGroupId">The itinerary group id.</param>
        /// <returns>The itinerary group and its participant persons.</returns>
        Task<ItineraryGroupParticipantsDTO> GetItineraryGroupByIdAsync(int projectId, int itineraryGroupId);
        /// <summary>
        /// Returns the itinerary groups and participant persons given the project id and itinerary id.
        /// </summary>
        /// <param name="projectId">The id of the project.  Used for security purposes.</param>
        /// <param name="itineraryGroupId">The itinerary group id.</param>
        /// <returns>The itinerary group and its participant persons.</returns>
        ItineraryGroupParticipantsDTO GetItineraryGroupById(int projectId, int itineraryGroupId);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IItineraryGroupService))]
    public abstract class ItineraryGroupServiceContract : IItineraryGroupService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addedGroup"></param>
        /// <returns></returns>
        public ItineraryGroup Create(AddedEcaItineraryGroup addedGroup)
        {
            Contract.Requires(addedGroup != null, "The added group must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addedGroup"></param>
        /// <returns></returns>
        public Task<ItineraryGroup> CreateAsync(AddedEcaItineraryGroup addedGroup)
        {
            Contract.Requires(addedGroup != null, "The added group must not be null.");
            return Task.FromResult<ItineraryGroup>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="itineraryGroupId"></param>
        /// <returns></returns>
        public ItineraryGroupParticipantsDTO GetItineraryGroupById(int projectId, int itineraryGroupId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="itineraryGroupId"></param>
        /// <returns></returns>
        public Task<ItineraryGroupParticipantsDTO> GetItineraryGroupByIdAsync(int projectId, int itineraryGroupId)
        {
            return Task.FromResult<ItineraryGroupParticipantsDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="itineraryId"></param>
        /// <returns></returns>
        public List<ItineraryGroupParticipantsDTO> GetItineraryGroupPersonsByItineraryId(int projectId, int itineraryId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="itineraryId"></param>
        /// <returns></returns>
        public Task<List<ItineraryGroupParticipantsDTO>> GetItineraryGroupPersonsByItineraryIdAsync(int projectId, int itineraryId)
        {
            return Task.FromResult<List<ItineraryGroupParticipantsDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="itineraryId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<ItineraryGroupDTO> GetItineraryGroupsByItineraryId(int projectId, int itineraryId, QueryableOperator<ItineraryGroupDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="itineraryId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<ItineraryGroupDTO>> GetItineraryGroupsByItineraryIdAsync(int projectId, int itineraryId, QueryableOperator<ItineraryGroupDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<ItineraryGroupDTO>>(null);
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
    }
}