﻿using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Interface for person service
    /// </summary>
    public interface IPersonService : ISaveable
    {
        /// <summary>
        /// Returns personally identifiable information for a user 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Personally identifiable information for person</returns>
        PiiDTO GetPiiById(int personId);

        /// <summary>
        /// Returns personally identifiable information for a user asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Personally identifiable information for person</returns>
        Task<PiiDTO> GetPiiByIdAsync(int personId);

        /// <summary>
        /// Returns contact info related to a person
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Contact info related to person</returns>
        ContactInfoDTO GetContactInfoById(int personId);

        /// <summary>
        /// Returns contact info related to a person asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Contact info related to person</returns>
        Task<ContactInfoDTO> GetContactInfoByIdAsync(int personId);

        /// <summary>
        /// Returns general information for a user 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>General information for person</returns>
        GeneralDTO GetGeneralById(int personId);

        /// <summary>
        /// Returns general information for a user asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>General information for person</returns>
        Task<GeneralDTO> GetGeneralByIdAsync(int personId);

        /// <summary>
        /// Returns educations information for a user 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Educations information for person</returns>
        IList<EducationEmploymentDTO> GetEducationsByPersonId(int personId);

        /// <summary>
        /// Returns educations information for a user asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Educations information for person</returns>
        Task<IList<EducationEmploymentDTO>> GetEducationsByPersonIdAsync(int personId);

        /// <summary>
        /// Returns professional employments information for a user 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Professional employments information for person</returns>
        IList<EducationEmploymentDTO> GetEmploymentsByPersonId(int personId);

        /// <summary>
        /// Returns professional employments information for a user asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Professional employment information for person</returns>
        Task<IList<EducationEmploymentDTO>> GetEmploymentsByPersonIdAsync(int personId);
        
        /// <summary>
        /// Returns evaluation-notes information for a user 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Evaluation-notes information for person</returns>
        IList<EvaluationNoteDTO> GetEvaluationNotesByPersonId(int personId);

        /// <summary>
        /// Returns evaluation-notes information for a user asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Evaluation-notes information for person</returns>
        Task<IList<EvaluationNoteDTO>> GetEvaluationNotesByPersonIdAsync(int personId);

        /// <summary>
        /// Creates a new person
        /// </summary>
        /// <returns>The person that was created</returns>
        Task<Person> CreateAsync(NewPerson person);

        /// <summary>
        /// Creates a new person dependent
        /// </summary>
        /// <returns>The person dependent that was created</returns>
        Task<PersonDependent> CreateDependentAsync(NewPersonDependent newDependent);
        
        /// <summary>
        /// Updates the PII section of a person
        /// </summary>
        /// <param name="pii"></param>
        /// <returns></returns>
        Task<Person> UpdatePiiAsync(UpdatePii pii);
        
        /// <summary>
        /// Update general section of a person
        /// </summary>
        /// <param name="general"></param>
        /// <returns></returns>
        Task<Person> UpdateGeneralAsync(UpdateGeneral general);

        /// <summary>
        /// Update contact info section of a person
        /// </summary>
        /// <param name="contactInfo"></param>
        /// <returns></returns>
        Task<Person> UpdateContactInfoAsync(UpdateContactInfo contactInfo);

        /// <summary>
        /// Returns the paged, sorted, and filtered people in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, sorted, and filtered people in the system.</returns>
        PagedQueryResults<SimplePersonDTO> GetPeople(QueryableOperator<SimplePersonDTO> queryOperator);

        /// <summary>
        /// Returns the paged, sorted, and filtered people in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, sorted, and filtered people in the system.</returns>
        Task<PagedQueryResults<SimplePersonDTO>> GetPeopleAsync(QueryableOperator<SimplePersonDTO> queryOperator);

        /// <summary>
        /// Gets a person as a simple DTO record
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<SimplePersonDTO> GetPersonByIdAsync(int personId);

        /// <summary>
        /// Get a person dependent
        /// </summary>
        /// <param name="dependentId">The dependent Id</param>
        /// <returns>The person dependent</returns>
        Task<SimplePersonDependentDTO> GetPersonDependentByIdAsync(int dependentId);
        
        /// <summary>
        /// Update a person dependent
        /// </summary>
        /// <param name="person">The dependent to update</param>
        /// <returns></returns>
        Task<PersonDependent> UpdatePersonDependentAsync(UpdatedPersonDependent person);

        /// <summary>
        /// Delete a dependent
        /// </summary>
        /// <param name="updatedDependent">The dependent to delete/update</param>
        /// <returns></returns>
        Task DeleteDependentAsync(UpdatedPersonDependent updatedDependent);

        /// <summary>
        /// Gets DS2019 file name for the dependent.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="dependentId">The dependent id.</param>
        /// <returns>The DS2019 file name</returns>
        Task<string> GetDS2019FileNameAsync(User user, int dependentId);

        /// <summary>
        /// Gets DS2019 file name for the dependent.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="dependentId">The dependent id.</param>
        /// <returns>The DS2019 file name</returns>
        string GetDS2019FileName(User user, int dependentId);
    }
}
