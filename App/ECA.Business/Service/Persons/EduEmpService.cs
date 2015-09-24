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
        /// Retrieves the profession/education dto with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EducationEmploymentDTO GetProfessionEducationById(int id)
        {
            var dto = ProfessionEducationQueries.CreateGetProfessionEducationDTOByIdQuery(this.Context, id).FirstOrDefault();
            logger.Info("Retrieved the education/employment dto with given id [{0}]", id);
            return dto;
        }

        /// <summary>
        /// Retrieves the profession/education dto with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EducationEmploymentDTO> GetProfessionEducationByIdAsync(int id)
        {
            var dto = await ProfessionEducationQueries.CreateGetProfessionEducationDTOByIdQuery(this.Context, id).FirstOrDefaultAsync();
            logger.Info("Retrieved the education/employment dto with given id [{0}]", id);
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
        /// Creates a new profession/education in the ECA system.
        /// </summary>
        /// <param name="personEduEmp"></param>
        /// <returns></returns>
        public ProfessionEducation CreateProfessionEducation(NewPersonEduEmp personEduEmp)
        {
            var person = this.Context.People.Find(personEduEmp.PersonId);
            return DoCreateProfessionEducation(personEduEmp, person);
        }

        /// <summary>
        /// Creates a new profession/education in the ECA system.
        /// </summary>
        /// <param name="personEduEmp"></param>
        /// <returns></returns>
        public async Task<ProfessionEducation> CreateProfessionEducationAsync(NewPersonEduEmp personEduEmp)
        {
            var person = await this.Context.People.FindAsync(personEduEmp.PersonId);
            return DoCreateProfessionEducation(personEduEmp, person);
        }
        
        private ProfessionEducation DoCreateProfessionEducation(NewPersonEduEmp personEduEmp, Person person)
        {
            throwIfPersonEntityNotFound(person, personEduEmp.PersonId);
            return personEduEmp.AddProfessionEducation(person);
        }
        
        #endregion

        #region Update

        /// <summary>
        /// Updates the ECA system's profession/education data with the given updated profession/education.
        /// </summary>
        /// <param name="updatedEduEmp"></param>
        public void UpdateProfessionEducation(UpdatedPersonEduEmp updatedEduEmp)
        {
            var eduemp = Context.ProfessionEducations.Find(updatedEduEmp.ProfessionEducationId);
            DoUpdateProfessionEducation(updatedEduEmp, eduemp);
        }

        /// <summary>
        /// Updates the ECA system's education data with the given updated education.
        /// </summary>
        /// <param name="updatedEduEmp"></param>
        /// <returns></returns>
        public async Task UpdateProfessionEducationAsync(UpdatedPersonEduEmp updatedEduEmp)
        {
            int id = updatedEduEmp.ProfessionEducationId;
            var eduemp = await Context.ProfessionEducations.FindAsync(id);
            DoUpdateProfessionEducation(updatedEduEmp, eduemp);
        }
        
        private void DoUpdateProfessionEducation(UpdatedPersonEduEmp updatedEduEmp, ProfessionEducation modelToUpdate)
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
