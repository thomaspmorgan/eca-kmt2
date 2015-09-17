using ECA.Business.Queries.Persons;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Data;
using NLog;
using System;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Admin;

namespace ECA.Business.Service.Persons
{
    public class EduEmpService : DbContextService<EcaContext>, ECA.Business.Service.Persons.IEduEmpService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<Person, int> throwIfPersonEntityNotFound;
        private readonly Action<ProfessionEducation, int> throwIfEduEmpNotFound;

        /// <summary>
        /// Creates a new instance and initializes the context.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public EduEmpService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfEduEmpNotFound = (eduemp, id) =>
            {
                if (eduemp == null)
                {
                    throw new ModelNotFoundException(String.Format("The education/profession with id [{0}] was not found.", id));
                }
            };

            throwIfEduEmpNotFound = (person, id) =>
            {
                if (person == null)
                {
                    throw new ModelNotFoundException(String.Format("The person with id[{0}] was not found.", id));
                }
            };
        }

        #region Get
        public PagedQueryResults<EducationEmploymentDTO> Get(QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            var results = GetEduEmpDTOQuery(queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded education/profession query operator = [{0}]", queryOperator);
            return results;
        }

        public async Task<PagedQueryResults<EducationEmploymentDTO>> GetAsync(QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            var results = await GetEduEmpDTOQuery(queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded education/profession query operator = [{0}]", queryOperator);
            return results;
        }

        public EducationEmploymentDTO GetEducationById(int id)
        {
            var dto = PersonQueries.CreateGetEducationsByPersonIdQuery(this.Context, id).FirstOrDefault();
            logger.Info("Retrieved the education dto with given id [{0}]", id);
            return dto;
        }

        public async Task<EducationEmploymentDTO> GetEducationByIdAsync(int id)
        {
            var dto = await PersonQueries.CreateGetEducationsByPersonIdQuery(this.Context, id).FirstOrDefaultAsync();
            logger.Info("Retrieved the education dto with given id [{0}]", id);
            return dto;
        }

        public EducationEmploymentDTO GetEmploymentById(int id)
        {
            var dto = PersonQueries.CreateGetEmploymentsByPersonIdQuery(this.Context, id).FirstOrDefault();
            logger.Info("Retrieved the employment dto with given id [{0}]", id);
            return dto;
        }

        public async Task<EducationEmploymentDTO> GetEmploymentByIdAsync(int id)
        {
            var dto = await PersonQueries.CreateGetEmploymentsByPersonIdQuery(this.Context, id).FirstOrDefaultAsync();
            logger.Info("Retrieved the employment dto with given id [{0}]", id);
            return dto;
        }
        
        private IQueryable<EducationEmploymentDTO> GetEduEmpDTOQuery(QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            var query = GetSelectDTOQuery();
            query = query.Apply(queryOperator);
            return query;
        }

        protected IQueryable<EducationEmploymentDTO> GetSelectDTOQuery()
        {
            var allOrganizations = OrganizationQueries.CreateGetSimpleOrganizationsDTOQuery(this.Context);

            return Context.ProfessionEducations.Select(x => new EducationEmploymentDTO
            {
                Id = x.ProfessionEducationId,
                Title = x.Title,
                Role = x.Role,
                StartDate = x.DateFrom,
                EndDate = x.DateTo,
                Organization = allOrganizations.FirstOrDefault(o => o.OrganizationId == x.OrganizationId),
                PersonOfEducation_PersonId = x.PersonOfEducation_PersonId,
                PersonOfProfession_PersonId = x.PersonOfProfession_PersonId
            });
        }

        #endregion

        #region Create
        
        public ProfessionEducation CreateEducation(NewPersonEduEmp personEduEmp)
        {
            var person = this.Context.People.Find(personEduEmp.PersonId);
            return DoCreateEducation(personEduEmp, person);
        }

        public async Task<ProfessionEducation> CreateEducationAsync(NewPersonEduEmp personEduEmp)
        {
            var person = this.Context.People.Find(personEduEmp.PersonId);
            return DoCreateEducation(personEduEmp, person);
        }
        
        private ProfessionEducation DoCreateEducation(NewPersonEduEmp personEduEmp, Person person)
        {
            throwIfPersonEntityNotFound(person, personEduEmp.PersonId);
            return personEduEmp.AddPersonEducation(person);
        }

        public ProfessionEducation CreateEmployment(NewPersonEduEmp personEduEmp)
        {
            var person = this.Context.People.Find(personEduEmp.PersonId);
            return DoCreateEmployment(personEduEmp, person);
        }

        public async Task<ProfessionEducation> CreateEmploymentAsync(NewPersonEduEmp personEduEmp)
        {
            var person = this.Context.People.Find(personEduEmp.PersonId);
            return DoCreateEmployment(personEduEmp, person);
        }

        private ProfessionEducation DoCreateEmployment(NewPersonEduEmp personEduEmp, Person person)
        {
            throwIfPersonEntityNotFound(person, personEduEmp.PersonId);
            return personEduEmp.AddPersonProfession(person);
        }
        #endregion

        #region Update

        public void UpdateEducation(UpdatedPersonEduEmp updatedEduEmp)
        {
            var eduemp = Context.ProfessionEducations.Find(updatedEduEmp);
            DoUpdateEducation(updatedEduEmp, eduemp);
        }

        public async Task UpdateEducationAsync(UpdatedPersonEduEmp updatedEduEmp)
        {
            var eduemp = await Context.ProfessionEducations.FindAsync(updatedEduEmp);
            DoUpdateEducation(updatedEduEmp, eduemp);
        }

        public void UpdateEmployment(UpdatedPersonEduEmp updatedEduEmp)
        {
            var eduemp = Context.ProfessionEducations.Find(updatedEduEmp);
            DoUpdateEmployment(updatedEduEmp, eduemp);
        }

        public async Task UpdateEmploymentAsync(UpdatedPersonEduEmp updatedEduEmp)
        {
            var eduemp = await Context.ProfessionEducations.FindAsync(updatedEduEmp);
            DoUpdateEmployment(updatedEduEmp, eduemp);
        }

        private void DoUpdateEducation(UpdatedPersonEduEmp updatedEduEmp, ProfessionEducation modelToUpdate)
        {
            Contract.Requires(updatedEduEmp != null, "The updatedEduEmp must not be null.");
            throwIfEduEmpNotFound(modelToUpdate, updatedEduEmp.ProfessionEducationId);

            modelToUpdate.ProfessionEducationId = updatedEduEmp.ProfessionEducationId;
            modelToUpdate.Title = updatedEduEmp.Title;
            modelToUpdate.Role = updatedEduEmp.Role;
            modelToUpdate.DateFrom = updatedEduEmp.StartDate;
            modelToUpdate.DateTo = updatedEduEmp.EndDate;
            modelToUpdate.OrganizationId = updatedEduEmp.OrganizationId;
            modelToUpdate.PersonOfEducation_PersonId = updatedEduEmp.PersonOfEducationId;
            updatedEduEmp.Update.SetHistory(modelToUpdate);
        }

        private void DoUpdateEmployment(UpdatedPersonEduEmp updatedEduEmp, ProfessionEducation modelToUpdate)
        {
            Contract.Requires(updatedEduEmp != null, "The updatedEduEmp must not be null.");
            throwIfEduEmpNotFound(modelToUpdate, updatedEduEmp.ProfessionEducationId);

            modelToUpdate.ProfessionEducationId = updatedEduEmp.ProfessionEducationId;
            modelToUpdate.Title = updatedEduEmp.Title;
            modelToUpdate.Role = updatedEduEmp.Role;
            modelToUpdate.DateFrom = updatedEduEmp.StartDate;
            modelToUpdate.DateTo = updatedEduEmp.EndDate;
            modelToUpdate.OrganizationId = updatedEduEmp.OrganizationId;
            modelToUpdate.PersonOfProfession_PersonId = updatedEduEmp.PersonOfProfessionId;
            updatedEduEmp.Update.SetHistory(modelToUpdate);
        }

        #endregion

        #region Delete

        public void Delete(int eduempId)
        {
            var eduemp = Context.ProfessionEducations.Find(eduempId);
            DoDelete(eduemp);
        }

        public async Task DeleteAsync(int eduempId)
        {
            var eduemp = await Context.ProfessionEducations.FindAsync(eduempId);
            DoDelete(eduemp);
        }

        private void DoDelete(ProfessionEducation eduempToDelete)
        {
            if (eduempToDelete != null)
            {
                Context.ProfessionEducations.Remove(eduempToDelete);
            }
        }

        #endregion

        

    }
}
