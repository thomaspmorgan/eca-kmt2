using ECA.Business.Queries.Models.Persons;
using ECA.Core.Service;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System;

namespace ECA.Business.Service.Persons
{
    [ContractClass(typeof(EduEmpServiceContract))]
    public interface IEduEmpService : ISaveable
    {
        PagedQueryResults<EducationEmploymentDTO> Get(QueryableOperator<EducationEmploymentDTO> queryOperator);

        Task<PagedQueryResults<EducationEmploymentDTO>> GetAsync(QueryableOperator<EducationEmploymentDTO> queryOperator);

        EducationEmploymentDTO GetEducationById(int id);

        Task<EducationEmploymentDTO> GetEducationByIdAsync(int id);

        EducationEmploymentDTO GetEmploymentById(int id);

        Task<EducationEmploymentDTO> GetEmploymentByIdAsync(int id);

        ProfessionEducation CreateEducation(NewPersonEduEmp eduemp);

        Task<ProfessionEducation> CreateEducationAsync(NewPersonEduEmp eduemp);

        ProfessionEducation CreateEmployment(NewPersonEduEmp eduemp);

        Task<ProfessionEducation> CreateEmploymentAsync(NewPersonEduEmp eduemp);

        void UpdateEducation(UpdatedPersonEduEmp updatededuempp);

        Task UpdateEducationAsync(UpdatedPersonEduEmp updatededuemp);

        void UpdateEmployment(UpdatedPersonEduEmp updatededuemp);

        Task UpdateEmploymentAsync(UpdatedPersonEduEmp updatededuemp);

        void Delete(int eduempId);

        Task DeleteAsync(int eduempId);
    }

    [ContractClassFor(typeof(IEduEmpService))]
    public abstract class EduEmpServiceContract : IEduEmpService
    {

        public PagedQueryResults<EducationEmploymentDTO> Get(QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            return null;
        }

        public Task<PagedQueryResults<EducationEmploymentDTO>> GetAsync(QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            return Task.FromResult<PagedQueryResults<EducationEmploymentDTO>>(null);
        }

        public EducationEmploymentDTO GetEducationById(int id)
        {
            return null;
        }

        public Task<EducationEmploymentDTO> GetEducationByIdAsync(int id)
        {
            return Task.FromResult<EducationEmploymentDTO>(null);
        }

        public EducationEmploymentDTO GetEmploymentById(int id)
        {
            return null;
        }

        public Task<EducationEmploymentDTO> GetEmploymentByIdAsync(int id)
        {
            return Task.FromResult<EducationEmploymentDTO>(null);
        }

        public ProfessionEducation CreateEducation(NewPersonEduEmp eduemp)
        {
            Contract.Requires(eduemp != null, "The education entity must not be null.");
            Contract.Ensures(Contract.Result<ProfessionEducation>() != null, "The education entity returned must not be null.");
            return null;
        }

        public Task<ProfessionEducation> CreateEducationAsync(NewPersonEduEmp eduemp)
        {
            Contract.Requires(eduemp != null, "The education entity must not be null.");
            Contract.Ensures(Contract.Result<Task<ProfessionEducation>>() != null, "The education entity returned must not be null.");
            return Task.FromResult<ProfessionEducation>(null);
        }

        public ProfessionEducation CreateEmployment(NewPersonEduEmp eduemp)
        {
            Contract.Requires(eduemp != null, "The employment entity must not be null.");
            Contract.Ensures(Contract.Result<ProfessionEducation>() != null, "The employment entity returned must not be null.");
            return null;
        }

        public Task<ProfessionEducation> CreateEmploymentAsync(NewPersonEduEmp eduemp)
        {
            Contract.Requires(eduemp != null, "The employment entity must not be null.");
            Contract.Ensures(Contract.Result<Task<ProfessionEducation>>() != null, "The employment entity returned must not be null.");
            return Task.FromResult<ProfessionEducation>(null);
        }
        
        public void UpdateEducation(UpdatedPersonEduEmp updatededuemp)
        {
            Contract.Requires(updatededuemp != null, "The updated education must not be null.");
        }

        public Task UpdateEducationAsync(UpdatedPersonEduEmp updatededuemp)
        {
            Contract.Requires(updatededuemp != null, "The updated education must not be null.");
            return Task.FromResult<object>(null);
        }

        public void UpdateEmployment(UpdatedPersonEduEmp updatededuemp)
        {
            Contract.Requires(updatededuemp != null, "The updated employment must not be null.");
        }

        public Task UpdateEmploymentAsync(UpdatedPersonEduEmp updatededuemp)
        {
            Contract.Requires(updatededuemp != null, "The updated employment must not be null.");
            return Task.FromResult<object>(null);
        }

        public void Delete(int eduempId)
        {

        }

        public Task DeleteAsync(int eduempId)
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
