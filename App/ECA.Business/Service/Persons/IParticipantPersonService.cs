using System;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System.Diagnostics.Contracts;
using ECA.Core.Service;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IParticipantService is capable of performing crud operations on participants.
    /// </summary>
    [ContractClass(typeof(ParticipantPersonServiceContract))]
    public interface IParticipantPersonService : ISaveable
    {
        /// <summary>
        /// Returns the participantPersons for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersons.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO> GetParticipantPersonsByProjectId(int projectId, ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersons for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersons.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO>> GetParticipantPersonsByProjectIdAsync(int projectId, ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO> queryOperator);

        /// <summary>
        /// Returns the participantPerson by id
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The participantPerson</returns>
        ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO GetParticipantPersonById(int projectId, int participantId);

        /// <summary>
        /// Returns the participantPerson by id asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The participantPerson</returns>
        System.Threading.Tasks.Task<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO> GetParticipantPersonByIdAsync(int projectId, int participantId);

        /// <summary>
        /// Returns the participantPerson by id
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>The participantPerson</returns>
        SimpleParticipantPersonDTO GetParticipantPersonById(int personId);

        /// <summary>
        /// Returns a participantPerson asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>The participantPerson</returns>
        Task<SimpleParticipantPersonDTO> GetParticipantPersonByIdAsync(int personId);

        /// <summary>
        /// Updates a participant person with given updated participant information.
        /// </summary>
        /// <param name="updatedPerson">The updated participant person.</param>
        void CreateOrUpdate(UpdatedParticipantPerson updatedPerson);

        /// <summary>
        /// Updates a participant person with given updated participant information.
        /// </summary>
        /// <param name="updatedPerson">The updated participant person.</param>
        /// <returns>The task.</returns>
        System.Threading.Tasks.Task CreateOrUpdateAsync(UpdatedParticipantPerson updatedPerson);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IParticipantPersonService))]
    public abstract class ParticipantPersonServiceContract : IParticipantPersonService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="participantId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public SimpleParticipantPersonDTO GetParticipantPersonById(int projectId, int participantId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="participantId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Task<SimpleParticipantPersonDTO> GetParticipantPersonByIdAsync(int projectId, int participantId)
        {
            return Task.FromResult<SimpleParticipantPersonDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public SimpleParticipantPersonDTO GetParticipantPersonById(int personId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public Task<SimpleParticipantPersonDTO> GetParticipantPersonByIdAsync(int personId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<SimpleParticipantPersonDTO> GetParticipantPersonsByProjectId(int projectId, QueryableOperator<SimpleParticipantPersonDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<SimpleParticipantPersonDTO>> GetParticipantPersonsByProjectIdAsync(int projectId, QueryableOperator<SimpleParticipantPersonDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<SimpleParticipantPersonDTO>>(null);
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
        /// <param name="updatedPerson"></param>
        public void CreateOrUpdate(UpdatedParticipantPerson updatedPerson)
        {
            Contract.Requires(updatedPerson != null, "The updated person must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedPerson"></param>
        /// <returns></returns>
        public Task CreateOrUpdateAsync(UpdatedParticipantPerson updatedPerson)
        {
            Contract.Requires(updatedPerson != null, "The updated person must not be null.");
            return Task.FromResult<object>(null);
        }
    }
}


