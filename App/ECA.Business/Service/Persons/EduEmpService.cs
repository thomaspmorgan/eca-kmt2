using ECA.Business.Queries.Persons;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Persons;
using NLog;

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

            throwIfPersonEntityNotFound = (person, id) =>
            {
                if (person == null)
                {
                    throw new ModelNotFoundException(String.Format("The person with id[{0}] was not found.", id));
                }
            };
        }

        #region Get

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of education dtos.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<EducationEmploymentDTO> GetEducations(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            var results = GetEducationDTOQuery(personId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded educations query operator = [{0}]", queryOperator);
            return results;
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of education dtos.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public async Task<PagedQueryResults<EducationEmploymentDTO>> GetEducationsAsync(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            var results = await GetEducationDTOQuery(personId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded educations query operator = [{0}]", queryOperator);
            return results;
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of employment dtos.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<EducationEmploymentDTO> GetEmployments(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            var results = GetEmploymentDTOQuery(personId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded professions query operator = [{0}]", queryOperator);
            return results;
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of employment dtos.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public async Task<PagedQueryResults<EducationEmploymentDTO>> GetEmploymentsAsync(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            var results = await GetEmploymentDTOQuery(personId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded professions query operator = [{0}]", queryOperator);
            return results;
        }

        /// <summary>
        /// Retrieves the education dto with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EducationEmploymentDTO GetEducationById(int id)
        {
            var dto = PersonQueries.CreateGetEducationsByPersonIdQuery(this.Context, id).FirstOrDefault();
            logger.Info("Retrieved the education dto with given id [{0}]", id);
            return dto;
        }

        /// <summary>
        /// Retrieves the education dto with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EducationEmploymentDTO> GetEducationByIdAsync(int id)
        {
            var dto = await PersonQueries.CreateGetEducationsByPersonIdQuery(this.Context, id).FirstOrDefaultAsync();
            logger.Info("Retrieved the education dto with given id [{0}]", id);
            return dto;
        }

        /// <summary>
        /// Retrieves the employment dto with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EducationEmploymentDTO GetEmploymentById(int id)
        {
            var dto = PersonQueries.CreateGetEmploymentsByPersonIdQuery(this.Context, id).FirstOrDefault();
            logger.Info("Retrieved the employment dto with given id [{0}]", id);
            return dto;
        }

        /// <summary>
        /// Retrieves the employment dto with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EducationEmploymentDTO> GetEmploymentByIdAsync(int id)
        {
            var dto = await PersonQueries.CreateGetEmploymentsByPersonIdQuery(this.Context, id).FirstOrDefaultAsync();
            logger.Info("Retrieved the employment dto with given id [{0}]", id);
            return dto;
        }
        
        private IQueryable<EducationEmploymentDTO> GetEducationDTOQuery(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            var query = GetSelectDTOQuery().Where(x => x.PersonOfEducation_PersonId == personId);
            query = query.Apply(queryOperator);
            return query;
        }

        private IQueryable<EducationEmploymentDTO> GetEmploymentDTOQuery(int personId, QueryableOperator<EducationEmploymentDTO> queryOperator)
        {
            var query = GetSelectDTOQuery().Where(x => x.PersonOfProfession_PersonId == personId);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Retrieves the EducationEmploymentDTO.
        /// </summary>
        /// <returns></returns>
        protected IQueryable<EducationEmploymentDTO> GetSelectDTOQuery()
        {
            //var allOrganizations = OrganizationQueries.CreateGetSimpleOrganizationsDTOQuery(this.Context);

            return Context.ProfessionEducations.Select(x => new EducationEmploymentDTO
            {
                ProfessionEducationId = x.ProfessionEducationId,
                Title = x.Title,
                Role = x.Role,
                StartDate = x.DateFrom,
                EndDate = x.DateTo,
                OrganizationId = null,
                PersonOfEducation_PersonId = x.PersonOfEducation_PersonId,
                PersonOfProfession_PersonId = x.PersonOfProfession_PersonId
            });
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates a new education in the ECA system.
        /// </summary>
        /// <param name="personEduEmp"></param>
        /// <returns></returns>
        public ProfessionEducation CreateEducation(NewPersonEduEmp personEduEmp)
        {
            var person = this.Context.People.Find(personEduEmp.PersonId);
            return DoCreateEducation(personEduEmp, person);
        }

        /// <summary>
        /// Creates a new education in the ECA system.
        /// </summary>
        /// <param name="personEduEmp"></param>
        /// <returns></returns>
        public async Task<ProfessionEducation> CreateEducationAsync(NewPersonEduEmp personEduEmp)
        {
            var person = await this.Context.People.FindAsync(personEduEmp.PersonId);
            return DoCreateEducation(personEduEmp, person);
        }
        
        private ProfessionEducation DoCreateEducation(NewPersonEduEmp personEduEmp, Person person)
        {
            throwIfPersonEntityNotFound(person, personEduEmp.PersonId);
            return personEduEmp.AddPersonEducation(person);
        }

        /// <summary>
        /// Creates a new employment in the ECA system.
        /// </summary>
        /// <param name="personEduEmp"></param>
        /// <returns></returns>
        public ProfessionEducation CreateEmployment(NewPersonEduEmp personEduEmp)
        {
            var person = this.Context.People.Find(personEduEmp.PersonId);
            return DoCreateEmployment(personEduEmp, person);
        }

        /// <summary>
        /// Creates a new employment in the ECA system.
        /// </summary>
        /// <param name="personEduEmp"></param>
        /// <returns></returns>
        public async Task<ProfessionEducation> CreateEmploymentAsync(NewPersonEduEmp personEduEmp)
        {
            var person = await this.Context.People.FindAsync(personEduEmp.PersonId);
            return DoCreateEmployment(personEduEmp, person);
        }

        private ProfessionEducation DoCreateEmployment(NewPersonEduEmp personEduEmp, Person person)
        {
            throwIfPersonEntityNotFound(person, personEduEmp.PersonId);
            return personEduEmp.AddPersonProfession(person);
        }
        #endregion

        #region Update

        /// <summary>
        /// Updates the ECA system's education data with the given updated education.
        /// </summary>
        /// <param name="updatedEduEmp"></param>
        public void UpdateEducation(UpdatedPersonEduEmp updatedEduEmp)
        {
            var eduemp = Context.ProfessionEducations.Find(updatedEduEmp.ProfessionEducationId);
            DoUpdateEducation(updatedEduEmp, eduemp);
        }

        /// <summary>
        /// Updates the ECA system's education data with the given updated education.
        /// </summary>
        /// <param name="updatedEduEmp"></param>
        /// <returns></returns>
        public async Task UpdateEducationAsync(UpdatedPersonEduEmp updatedEduEmp)
        {
            int id = updatedEduEmp.ProfessionEducationId;
            var eduemp = await Context.ProfessionEducations.FindAsync(id);
            DoUpdateEducation(updatedEduEmp, eduemp);
        }

        /// <summary>
        /// Updates the ECA system's employment data with the given updated employment.
        /// </summary>
        /// <param name="updatedEduEmp"></param>
        public void UpdateEmployment(UpdatedPersonEduEmp updatedEduEmp)
        {
            var eduemp = Context.ProfessionEducations.Find(updatedEduEmp.ProfessionEducationId);
            DoUpdateEmployment(updatedEduEmp, eduemp);
        }

        /// <summary>
        /// Updates the ECA system's employment data with the given updated employment.
        /// </summary>
        /// <param name="updatedEduEmp"></param>
        /// <returns></returns>
        public async Task UpdateEmploymentAsync(UpdatedPersonEduEmp updatedEduEmp)
        {
            var eduemp = await Context.ProfessionEducations.FindAsync(updatedEduEmp.ProfessionEducationId);
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
            modelToUpdate.PersonOfEducation_PersonId = updatedEduEmp.PersonOfEducation_PersonId;
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
            modelToUpdate.PersonOfProfession_PersonId = updatedEduEmp.PersonOfProfession_PersonId;
            updatedEduEmp.Update.SetHistory(modelToUpdate);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the ProfessionEducation entry with the given id.
        /// </summary>
        /// <param name="eduempId"></param>
        public void Delete(int eduempId)
        {
            var eduemp = Context.ProfessionEducations.Find(eduempId);
            DoDelete(eduemp);
        }

        /// <summary>
        /// Deletes the ProfessionEducation entry with the given id.
        /// </summary>
        /// <param name="eduempId"></param>
        /// <returns></returns>
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
