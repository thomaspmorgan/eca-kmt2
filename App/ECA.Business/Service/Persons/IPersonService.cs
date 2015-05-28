﻿using ECA.Business.Queries.Models.Persons;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Creates a new person
        /// </summary>
        /// <returns>The person that was created</returns>
        Task<Person> CreateAsync(NewPerson person);

        Task<Person> UpdatePiiAsync(UpdatePii pii);
    }
}
