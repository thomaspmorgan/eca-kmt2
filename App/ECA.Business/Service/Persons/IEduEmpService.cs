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
    public interface IEduEmpService : ISaveable
    {

        ProfessionEducation Create(NewPersonEduEmp eduemp);

        Task<ProfessionEducation> CreateAsync(NewPersonEduEmp eduemp);

        void Update(UpdatedPersonEduEmp updatededuemp);

        Task UpdateAsync(UpdatedPersonEduEmp updatededuemp);
        
        PagedQueryResults<EducationEmploymentDTO> Get(QueryableOperator<EducationEmploymentDTO> queryOperator);
        
        Task<PagedQueryResults<EducationEmploymentDTO>> GetAsync(QueryableOperator<EducationEmploymentDTO> queryOperator);
        
        EducationEmploymentDTO GetById(int id);
        
        Task<EducationEmploymentDTO> GetByIdAsync(int id);

        void Delete(int eduempId);

        Task DeleteAsync(int eduempId);
    }

    [ContractClassFor(typeof(IEduEmpService))]
    public abstract class EduEmpServiceContract : IEduEmpService
    {
        public ProfessionEducation Create(NewPersonEduEmp eduemp)
        {
            Contract.Requires(eduemp != null, "The education/profession entity must not be null.");
            Contract.Ensures(Contract.Result<ProfessionEducation>() != null, "The education/profession entity returned must not be null.");
            return null;
        }

        public Task<ProfessionEducation> CreateAsync(NewPersonEduEmp eduemp)
        {
            Contract.Requires(eduemp != null, "The education/professional entity must not be null.");
            Contract.Ensures(Contract.Result<Task<ProfessionEducation>>() != null, "The education/profession entity returned must not be null.");
            return Task.FromResult<ProfessionEducation>(null);
        }

        public PagedQueryResults<EducationEmploymentDTO> Get(QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            return null;
        }

        public Task<PagedQueryResults<EducationEmploymentDTO>> GetAsync(QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            return Task.FromResult<PagedQueryResults<EducationEmploymentDTO>>(null);
        }

        public EducationEmploymentDTO GetById(int id)
        {
            return null;
        }

        public Task<EducationEmploymentDTO> GetByIdAsync(int id)
        {
            return Task.FromResult<EducationEmploymentDTO>(null);
        }

        public int SaveChanges()
        {
            return 1;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult<int>(1);
        }

        public void Update(UpdatedPersonEduEmp updatededuemp)
        {
            Contract.Requires(updatededuemp != null, "The updated education/profession must not be null.");
        }

        public Task UpdateAsync(UpdatedPersonEduEmp updatededuemp)
        {
            Contract.Requires(updatededuemp != null, "The updated education/profession must not be null.");
            return Task.FromResult<object>(null);
        }

        public void Delete(int eduempId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int eduempId)
        {
            throw new NotImplementedException();
        }

    }
}
