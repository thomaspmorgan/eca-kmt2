using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Class for new person
    /// </summary>
    public class NewPerson : IAuditable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="createdBy">User that created the person</param>
        /// <param name="projectId">The project id</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="gender">The gender</param>
        /// <param name="dateOfBirth">The date of birth</param>
        /// <param name="cityOfBirth">The city of birth</param>
        /// <param name="countriesOfCitizenship">The countries of citizenship</param>
        public NewPerson(User createdBy, int projectId, string firstName, string lastName, int gender, DateTime dateOfBirth,
                         int cityOfBirth, List<int> countriesOfCitizenship)
        {
            Contract.Requires(createdBy != null, "The created by user must not be null.");

            this.ProjectId = projectId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
            this.DateOfBirth = dateOfBirth;
            this.CityOfBirth = cityOfBirth;
            this.CountriesOfCitizenship = countriesOfCitizenship;
            this.Audit = new Create(createdBy);
        }

        /// <summary>
        /// Gets and sets the project id
        /// </summary>
        public int ProjectId { get; private set; }

        /// <summary>
        /// Gets and sets the first name
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets and sets the last name
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets and sets the gender
        /// </summary>
        public int Gender { get; private set; }

        /// <summary>
        /// Gets and sets the date of birth
        /// </summary>
        public DateTime DateOfBirth { get; private set; }
        
        /// <summary>
        /// Gets and sets the city of birth
        /// </summary>
        public int CityOfBirth { get; private set; }

        /// <summary>
        /// Gets and sets the countries of citizenship
        /// </summary>
        public List<int> CountriesOfCitizenship { get; private set; }

        /// <summary>
        /// Gets and sets the audit record
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
