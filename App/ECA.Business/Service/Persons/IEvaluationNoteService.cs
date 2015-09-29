using ECA.Core.Service;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;

namespace ECA.Business.Service.Persons
{
    [ContractClass(typeof(EvaluationNoteContract))]
    public interface IEvaluationNoteService : ISaveable
    {
        PagedQueryResults<EvaluationNoteDTO> Get(QueryableOperator<EvaluationNoteDTO> queryOperator);

        Task<PagedQueryResults<EvaluationNoteDTO>> GetAsync(QueryableOperator<EvaluationNoteDTO> queryOperator);

        EvaluationNoteDTO GetById(int id);

        Task<EvaluationNoteDTO> GetByIdAsync(int id);

        PersonEvaluationNote Create(NewPersonEvaluationNote evalnote);

        Task<PersonEvaluationNote> CreateAsync(NewPersonEvaluationNote evalnote);

        void Update(UpdatedPersonEvaluationNote evalnote);

        Task UpdateAsync(UpdatedPersonEvaluationNote evalnote);

        void Delete(int evalnoteId);

        Task DeleteAsync(int evalnoteId);
    }

    [ContractClassFor(typeof(IEvaluationNoteService))]
    public abstract class EvaluationNoteContract : IEvaluationNoteService
    {

        public PagedQueryResults<EvaluationNoteDTO> Get(QueryableOperator<EvaluationNoteDTO> queryOperator)
        {
            return null;
        }

        public Task<PagedQueryResults<EvaluationNoteDTO>> GetAsync(QueryableOperator<EvaluationNoteDTO> queryOperator)
        {
            return Task.FromResult<PagedQueryResults<EvaluationNoteDTO>>(null);
        }
        
        public EvaluationNoteDTO GetById(int id)
        {
            return null;
        }

        public Task<EvaluationNoteDTO> GetByIdAsync(int id)
        {
            return Task.FromResult<EvaluationNoteDTO>(null);
        }

        public PersonEvaluationNote Create(NewPersonEvaluationNote evalnote)
        {
            return null;
        }

        public Task<PersonEvaluationNote> CreateAsync(NewPersonEvaluationNote evalnote)
        {

            return Task.FromResult<PersonEvaluationNote>(null);
        }

        public void Update(UpdatedPersonEvaluationNote updatedevalnote)
        {
            Contract.Requires(updatedevalnote != null, "The updated evaluation note must not be null.");
        }

        public Task UpdateAsync(UpdatedPersonEvaluationNote evalnote)
        {

            return Task.FromResult<object>(null);
        }

        public void Delete(int evalnote)
        { }

        public Task DeleteAsync(int evalnote)
        {
            return Task.FromResult<object>(null);
        }

        public int SaveChanges()
        {
            return 1;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult<int>(1);
        }

    }


}
