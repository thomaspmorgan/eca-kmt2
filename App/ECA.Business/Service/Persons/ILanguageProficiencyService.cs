using ECA.Business.Queries.Models.Persons;
using ECA.Core.Service;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An ILanguageProficiencyService is used to create or update languange proficiencys for person objects.
    /// </summary>
    [ContractClass(typeof(LanguageProficiencyServiceContract))]
    public interface ILanguageProficiencyService : ISaveable
    {
        /// <summary>
        /// Creates a new languageProficiency in the ECA system.
        /// </summary>
        /// <param name="newLanguageProficiency">The membership.</param>
        /// <returns>The created languageProficiency entity.</returns>
        PersonLanguageProficiency Create(NewPersonLanguageProficiency newLanguageProficiency);

        /// <summary>
        /// Creates a new languageProficiency in the ECA system.
        /// </summary>
        /// <param name="newLanguageProficiency">The membership.</param>
        /// <returns>The created languageProficiency entity.</returns>
        Task<PersonLanguageProficiency> CreateAsync(NewPersonLanguageProficiency newLanguageProficiency);

        /// <summary>
        /// Updates the ECA system's PersonLanguageProficiency data with the given updated PersonLanguageProficiency.
        /// </summary>
        /// <param name="updatedmembership">The updated languageProficiency.</param>
        void Update(UpdatedPersonLanguageProficiency updatedLanguageProficiency);

        /// <summary>
        /// Updates the ECA system's PersonLanguageProficiency data with the given updated PersonLanguageProficiency.
        /// </summary>
        /// <param name="updatedLanguageProficiency">The updated languageProficiency.</param>
        Task UpdateAsync(UpdatedPersonLanguageProficiency updatedLanguageProficiency);

        /// <summary>
        /// Returns paged, filtered, and sorted languageProficiencies in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The languageProficiencies in the system.</returns>
        PagedQueryResults<LanguageProficiencyDTO> Get(QueryableOperator<LanguageProficiencyDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted LanguageProficiencies in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The languageProficiencies in the system.</returns>
        Task<PagedQueryResults<LanguageProficiencyDTO>> GetAsync(QueryableOperator<LanguageProficiencyDTO> queryOperator);

        /// <summary>
        /// Retrieves the languageProficiency dto with the given id.
        /// </summary>
        /// <param name="id">The id of the languageProficiency.</param>
        /// <returns>The languageProficiency dto.</returns>
        LanguageProficiencyDTO GetById(int languageId, int personId);

        /// <summary>
        /// Retrieves the languageProficiency dto with the given id.
        /// </summary>
        /// <param name="languageId">The id of the language.</param>
        /// <param name="personId">The id of the person.</param>
        /// <returns>The languageProficiency dto.</returns>
        Task<LanguageProficiencyDTO> GetByIdAsync(int languageId, int personId);

        /// <summary>
        /// Deletes the languageProficiency entry with the given id.
        /// </summary>
        /// <param name="languageId">The id of the language.</param>
        /// <param name="personId">The id of the person.</param>
        void Delete(int languageId, int personId);

        /// <summary>
        /// Deletes the languageProficiency entry with the given id.
        /// </summary>
        /// <param name="languageId">The id of the language.</param>
        /// <param name="personId">The id of the person.</param>
        Task DeleteAsync(int languageId, int personId);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(ILanguageProficiencyService))]
    public abstract class LanguageProficiencyServiceContract : ILanguageProficiencyService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageProficiency"></param>
        /// <returns></returns>
        public PersonLanguageProficiency Create(NewPersonLanguageProficiency newPersonLanguageProficiency)
        {
            Contract.Requires(newPersonLanguageProficiency != null, "The newPersonLanguageProficiency entity must not be null.");
            Contract.Ensures(Contract.Result<PersonLanguageProficiency>() != null, "The PersonLanguageProficiency entity returned must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageProficiency"></param>
        /// <returns></returns>
        public Task<PersonLanguageProficiency> CreateAsync(NewPersonLanguageProficiency newPersonLanguageProficiency)
        {
            Contract.Requires(newPersonLanguageProficiency != null, "The newPersonLanguageProficiency entity must not be null.");
            Contract.Ensures(Contract.Result<Task<PersonLanguageProficiency>>() != null, "The PersonLanguageProficiency entity returned must not be null.");
            return Task.FromResult<PersonLanguageProficiency>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedLanguageProficiency"></param>
        public void Update(UpdatedPersonLanguageProficiency updatedLanguageProficiency)
        {
            Contract.Requires(updatedLanguageProficiency != null, "The updated updatedLanguageProficiency must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedmembership"></param>
        /// <returns></returns>
        public Task UpdateAsync(UpdatedPersonLanguageProficiency updatedLanguageProficiency)
        {
            Contract.Requires(updatedLanguageProficiency != null, "The updated updatedLanguageProficiency must not be null.");
            return Task.FromResult<object>(null);
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
        /// Returns paged, filtered, and sorted languageProficiencies in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The languageProficiencies in the system.</returns>
        public PagedQueryResults<LanguageProficiencyDTO> Get(QueryableOperator<LanguageProficiencyDTO> queryOperator)
        {
            return null;
        }

        /// <summary>
        /// Returns paged, filtered, and sorted languageProficiencies in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The languageProficiencies in the system.</returns>
        public Task<PagedQueryResults<LanguageProficiencyDTO>> GetAsync(QueryableOperator<LanguageProficiencyDTO> queryOperator)
        {
            return Task.FromResult<PagedQueryResults<LanguageProficiencyDTO>>(null);
        }

        /// <summary>
        /// Gets a languageProficiency by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>LanguageProficiencyDTO</returns>
        public LanguageProficiencyDTO GetById(int languageId, int personId)
        {
            return null;
        }

        /// <summary>
        /// Gets a languageProficiency by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returnsLanguageProficiencyDTOreturns>
        public Task<LanguageProficiencyDTO> GetByIdAsync(int languageId, int personId)
        {
            return Task.FromResult<LanguageProficiencyDTO>(null);
        }

        /// <summary>
        /// Deletes a LanguageProficiency by Id
        /// </summary>
        /// <param name="languageProficiencyId"></param>
        public void Delete(int languageId, int personId)
        {

        }

        /// <summary>
        /// Deletes a LanguageProficiency by Id
        /// </summary>
        /// <param name="languageProficiencyId"></param>
        /// <returns></returns>
        public Task DeleteAsync(int languageId, int personId)
        {
            return Task.FromResult<object>(null);
        }
    }
}
