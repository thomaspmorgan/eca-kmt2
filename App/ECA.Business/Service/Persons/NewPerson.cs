using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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
        /// <param name="participantTypeId">The participant type id</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="gender">The gender</param>
        /// <param name="dateOfBirth">The date of birth</param>
        /// <param name="isDateOfBirthUnknown">Denotes date of birth is unknown</param>
        /// <param name="isDateOfBirthEstimated">Denotes date of birth estimated.</param>
        /// <param name="isPlaceOfBirthUnknown">Denotes place of birth unknown.</param>
        /// <param name="cityOfBirth">The city of birth</param>
        /// <param name="countriesOfCitizenship">The countries of citizenship</param>
        public NewPerson(
            User createdBy,
            int projectId,
            int participantTypeId,
            bool isSingleName,
            string firstName,
            string lastName,
            int gender,
            DateTime? dateOfBirth,
            bool? isDateOfBirthUnknown,
            bool? isDateOfBirthEstimated,
            bool? isPlaceOfBirthUnknown,
            int? cityOfBirth,
            List<int> countriesOfCitizenship)
        {
            Contract.Requires(createdBy != null, "The created by user must not be null.");
            this.ProjectId = projectId;
            this.ParticipantTypeId = participantTypeId;
            this.IsSingleName = isSingleName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
            this.DateOfBirth = dateOfBirth;
            this.IsDateOfBirthUnknown = isDateOfBirthUnknown;
            this.CityOfBirth = cityOfBirth;
            this.CountriesOfCitizenship = countriesOfCitizenship;
            this.IsDateOfBirthEstimated = isDateOfBirthEstimated;
            this.IsPlaceOfBirthUnknown = isPlaceOfBirthUnknown;
            this.Audit = new Create(createdBy);
        }
        
        /// <summary>
        /// Gets and sets the project id
        /// </summary>
        public int ProjectId { get; private set; }

        /// <summary>
        /// Gets and sets the participant type id
        /// </summary>
        public int ParticipantTypeId { get; private set; }

        /// <summary>
        /// Denotes if participant has one name only
        /// </summary>
        public bool IsSingleName { get; private set; }

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
        public DateTime? DateOfBirth { get; private set; }

        /// <summary>
        /// Denotes if date of birth is unknown
        /// </summary>
        public bool? IsDateOfBirthUnknown { get; private set; }
        
        /// <summary>
        /// Denotes if date of birth is estimated.
        /// </summary>
        public bool? IsDateOfBirthEstimated { get; private set; }

        /// <summary>
        /// Gets whether the place of birth is unknown.
        /// </summary>
        public bool? IsPlaceOfBirthUnknown { get; private set; }

        /// <summary>
        /// Gets and sets the city of birth
        /// </summary>
        public int? CityOfBirth { get; private set; }

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
