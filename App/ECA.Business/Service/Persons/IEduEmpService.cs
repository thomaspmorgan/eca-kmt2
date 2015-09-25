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
        PagedQueryResults<EducationEmploymentDTO> GetEducations(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator);

        Task<PagedQueryResults<EducationEmploymentDTO>> GetEducationsAsync(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator);

        PagedQueryResults<EducationEmploymentDTO> GetEmployments(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator);

        Task<PagedQueryResults<EducationEmploymentDTO>> GetEmploymentsAsync(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator);

        EducationEmploymentDTO GetProfessionEducationById(int id);

        Task<EducationEmploymentDTO> GetProfessionEducationByIdAsync(int id);

        ProfessionEducation CreateProfessionEducation(NewPersonEduEmp eduemp);

        Task<ProfessionEducation> CreateProfessionEducationAsync(NewPersonEduEmp eduemp);

        void UpdateProfessionEducation(UpdatedPersonEduEmp updatededuempp);

        Task UpdateProfessionEducationAsync(UpdatedPersonEduEmp updatededuemp);

        void Delete(int eduempId);

        Task DeleteAsync(int eduempId);
    }

    [ContractClassFor(typeof(IEduEmpService))]
    public abstract class EduEmpServiceContract : IEduEmpService
    {

        public PagedQueryResults<EducationEmploymentDTO> GetEducations(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            return null;
        }

        public Task<PagedQueryResults<EducationEmploymentDTO>> GetEducationsAsync(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            return Task.FromResult<PagedQueryResults<EducationEmploymentDTO>>(null);
        }

        public PagedQueryResults<EducationEmploymentDTO> GetEmployments(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            return null;
        }

        public Task<PagedQueryResults<EducationEmploymentDTO>> GetEmploymentsAsync(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            return Task.FromResult<PagedQueryResults<EducationEmploymentDTO>>(null);
        }

        public EducationEmploymentDTO GetProfessionEducationById(int id)
        {
            return null;
        }

        public Task<EducationEmploymentDTO> GetProfessionEducationByIdAsync(int id)
        {
            return Task.FromResult<EducationEmploymentDTO>(null);
        }

        public ProfessionEducation CreateProfessionEducation(NewPersonEduEmp eduemp)
        {
            Contract.Requires(eduemp != null, "The education entity must not be null.");
            Contract.Ensures(Contract.Result<ProfessionEducation>() != null, "The education entity returned must not be null.");
            return null;
        }

        public Task<ProfessionEducation> CreateProfessionEducationAsync(NewPersonEduEmp eduemp)
        {
            Contract.Requires(eduemp != null, "The education entity must not be null.");
            Contract.Ensures(Contract.Result<Task<ProfessionEducation>>() != null, "The education entity returned must not be null.");
            return Task.FromResult<ProfessionEducation>(null);
        }

        public void UpdateProfessionEducation(UpdatedPersonEduEmp updatededuemp)
        {
            Contract.Requires(updatededuemp != null, "The updated education must not be null.");
        }

        public Task UpdateProfessionEducationAsync(UpdatedPersonEduEmp updatededuemp)
        {
            Contract.Requires(updatededuemp != null, "The updated education must not be null.");
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
