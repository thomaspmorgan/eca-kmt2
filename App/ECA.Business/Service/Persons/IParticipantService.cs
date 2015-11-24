using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IParticipantService is capable of performing crud operations on participants.
    /// </summary>
    [ContractClass(typeof(ParticipantServiceContract))]
    public interface IParticipantService : ISaveable
    {
        /// <summary>
        /// Returns the participants in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> GetParticipants(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> queryOperator);

        /// <summary>
        /// Returns the participants in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO>> GetParticipantsAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> queryOperator);

        /// <summary>
        /// Returns the participants for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participants.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> GetParticipantsByProjectId(int projectId, ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> queryOperator);

        /// <summary>
        /// Returns the participants for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participants.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO>> GetParticipantsByProjectIdAsync(int projectId, ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> queryOperator);

        /// <summary>
        /// Returns the participant by id
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participant</returns>
        ECA.Business.Queries.Models.Persons.ParticipantDTO GetParticipantById(int participantId);

        /// <summary>
        /// Returns the participant by id asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participant</returns>
        System.Threading.Tasks.Task<ECA.Business.Queries.Models.Persons.ParticipantDTO> GetParticipantByIdAsync(int participantId);

        /// <summary>
        /// Deletes the participant from the datastore given the DeletedParticipant business entity.
        /// </summary>
        /// <param name="deletedParticipant">The business entity.</param>
        void Delete(DeletedParticipant deletedParticipant);

        /// <summary>
        /// Deletes the participant from the datastore given the DeletedParticipant business entity.
        /// </summary>
        /// <param name="deletedParticipant">The business entity.</param>
        System.Threading.Tasks.Task DeleteAsync(DeletedParticipant deletedParticipant);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IParticipantService))]
    public abstract class ParticipantServiceContract : IParticipantService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deletedParticipant"></param>
        public void Delete(DeletedParticipant deletedParticipant)
        {
            Contract.Requires(deletedParticipant != null, "The deleted participant must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deletedParticipant"></param>
        /// <returns></returns>
        public Task DeleteAsync(DeletedParticipant deletedParticipant)
        {
            Contract.Requires(deletedParticipant != null, "The deleted participant must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="participantId"></param>
        /// <returns></returns>
        public ParticipantDTO GetParticipantById(int participantId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="participantId"></param>
        /// <returns></returns>
        public Task<ParticipantDTO> GetParticipantByIdAsync(int participantId)
        {
            return Task.FromResult<ParticipantDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<SimpleParticipantDTO> GetParticipants(QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<SimpleParticipantDTO>> GetParticipantsAsync(QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<SimpleParticipantDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<SimpleParticipantDTO> GetParticipantsByProjectId(int projectId, QueryableOperator<SimpleParticipantDTO> queryOperator)
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
        public Task<PagedQueryResults<SimpleParticipantDTO>> GetParticipantsByProjectIdAsync(int projectId, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            return Task.FromResult<PagedQueryResults<SimpleParticipantDTO>>(null);
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
