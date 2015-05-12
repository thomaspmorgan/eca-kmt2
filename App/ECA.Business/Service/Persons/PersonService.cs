using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Diagnostics;
using ECA.Business.Queries.Admin;
using NLog;
using ECA.Business.Exceptions;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Person service
    /// </summary>
    public class PersonService : EcaService, IPersonService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The context to query</param>
        public PersonService(EcaContext context) : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Returns personally identifiable information for a user 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Personally identifiable information for person</returns>
        public PiiDTO GetPiiById(int personId)
        {
            var pii = PersonQueries.CreateGetPiiByIdQuery(this.Context, personId).SingleOrDefault();
            this.logger.Trace("Retrieved person by id {0}.", personId);
            return pii;
        }

        /// <summary>
        /// Returns personally identifiable information for a user asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Personally identifiable information for person</returns>
        public Task<PiiDTO> GetPiiByIdAsync(int personId)
        {
            var pii = PersonQueries.CreateGetPiiByIdQuery(this.Context, personId).SingleOrDefaultAsync();
            this.logger.Trace("Retrieved person by id {0}.", personId);
            return pii;
        }

        /// <summary>
        /// Returns contact info related to a person
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Contact info related to person</returns>
        public ContactInfoDTO GetContactInfoById(int personId)
        {
            var contactInfo = PersonQueries.CreateGetContactInfoByIdQuery(this.Context, personId).SingleOrDefault();
            this.logger.Trace("Retrieved contact info by id {0}.", personId);            
            return contactInfo;
        }

        /// <summary>
        /// Returns contact info related to a person asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Contact info related to person</returns>
        public Task<ContactInfoDTO> GetContactInfoByIdAsync(int personId)
        {
            var contactInfo = PersonQueries.CreateGetContactInfoByIdQuery(this.Context, personId).SingleOrDefaultAsync();
            this.logger.Trace("Retrieved contact info by id {0}.", personId);             
            return contactInfo;
        }

        /// <summary>
        /// Create a person
        /// </summary>
        /// <param name="newPerson">The person to create</param>
        /// <returns>The person created</returns>
        public async Task<Person> CreateAsync(NewPerson newPerson)
        {

            var existingPerson = await GetExistingPerson(newPerson);
            if (existingPerson != null)
            {
                this.logger.Trace("Found existing person {0}.", existingPerson);
                throw new EcaBusinessException("The person already exists.");
            }
            var project = await GetProjectByIdAsync(newPerson.ProjectId);
            var countriesOfCitizenship = await GetLocationsByIdAsync(newPerson.CountriesOfCitizenship);
            var person = CreatePerson(newPerson, countriesOfCitizenship);
            var participant = CreateParticipant(person, project);
            this.logger.Trace("Created participant {0}.", newPerson); 
            return person;
        }

        /// <summary>
        /// Get existing person
        /// </summary>
        /// <param name="newPerson">The person to lookup</param>
        /// <returns>The person found</returns>
        public async Task<Person> GetExistingPerson(NewPerson newPerson)
        {
            this.logger.Trace("Retrieving person {0}.", newPerson);
            return await CreateGetPerson(newPerson.FirstName, newPerson.LastName, newPerson.Gender, newPerson.DateOfBirth, newPerson.CityOfBirth).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get existing person 
        /// </summary>
        /// <param name="pii">The pii to lookup</param>
        /// <returns>The person found</returns>
        public async Task<Person> GetExistingPerson(UpdatePii pii)
        {
            this.logger.Trace("Retrieving person {0}.", pii);
            return await CreateGetPerson(pii.FirstName, pii.LastName, pii.GenderId, pii.DateOfBirth, pii.CityOfBirthId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates get person query
        /// </summary>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="genderId">The gender id</param>
        /// <param name="dateOfBirth">The date of birth</param>
        /// <param name="cityOfBirthId">The city of birth id</param>
        /// <returns>Queryable person object</returns>
        private IQueryable<Person> CreateGetPerson(string firstName, string lastName, int genderId, DateTime dateOfBirth, int cityOfBirthId)
        {
            return Context.People.Where(
                    x => x.FirstName.ToLower().Trim() == firstName.ToLower().Trim() &&
                         x.LastName.ToLower().Trim() == lastName.ToLower().Trim() &&
                         x.GenderId == genderId &&
                         x.DateOfBirth.HasValue &&
                         x.DateOfBirth.Value.Day == dateOfBirth.Day &&
                         x.DateOfBirth.Value.Month == dateOfBirth.Month &&
                         x.DateOfBirth.Value.Year == dateOfBirth.Year &&
                         x.PlaceOfBirthId == cityOfBirthId
                    );
        }

        /// <summary>
        /// Gets the project by id asyncronously
        /// </summary>
        /// <param name="projectId">The project id to lookup</param>
        /// <returns>A project</returns>
        public async Task<Project> GetProjectByIdAsync(int projectId)
        {
            this.logger.Trace("Retrieving project with id {0}.", projectId); 
            return await CreateGetProjectById(projectId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates query to get project by id
        /// </summary>
        /// <param name="projectId">The project id to lookup</param>
        /// <returns>Queryable list of projects</returns>
        private IQueryable<Project> CreateGetProjectById(int projectId)
        {
            return Context.Projects.Where(x => x.ProjectId == projectId);
        }

       /// <summary>
       /// Creates a person
       /// </summary>
       /// <param name="newPerson">The person to create</param>
       /// <param name="countriesOfCitizenship">The countries of citizenship</param>
       /// <returns>The person created</returns>
        private Person CreatePerson(NewPerson newPerson, List<Location> countriesOfCitizenship)
        {
            var person = new Person
            {
                FirstName = newPerson.FirstName,
                LastName = newPerson.LastName,
                GenderId = newPerson.Gender,
                DateOfBirth = newPerson.DateOfBirth,
                PlaceOfBirthId = newPerson.CityOfBirth,
                CountriesOfCitizenship = countriesOfCitizenship
            };

            newPerson.Audit.SetHistory(person);
            this.Context.People.Add(person);
            this.logger.Trace("Creating new person {0}.", newPerson); 
            return person;
        }

        /// <summary>
        /// Create a participant
        /// </summary>
        /// <param name="person">Person to associate with participant</param>
        /// <param name="project">Project to assocate with participant</param>
        /// <returns></returns>
        private Participant CreateParticipant(Person person, Project project)
        {
            var participant = new Participant
            {
                PersonId = person.PersonId,
                ParticipantTypeId = ParticipantType.Individual.Id
            };

            participant.Projects.Add(project);
            this.Context.Participants.Add(participant);
            this.logger.Trace("Creating new participant {0}.", person); 
            return participant;
        }

        /// <summary>
        /// Update pii
        /// </summary>
        /// <param name="pii">The pii business model</param>
        /// <returns>The person updated</returns>
        public async Task<Person> UpdatePiiAsync(UpdatePii pii) {
            var existingPerson = await GetExistingPerson(pii);
            if (existingPerson != null && pii.PersonId != existingPerson.PersonId)
            {
                this.logger.Trace("Found existing person {0}.", existingPerson);
                throw new EcaBusinessException("The person already exists.");
            }
            var personToUpdate = await GetPersonByIdAsync(pii.PersonId);
            var participantToUpdate = await GetParticipantByIdAsync(pii.ParticipantId);
            var countriesOfCitizenship = await GetLocationsByIdAsync(pii.CountriesOfCitizenship);
            DoUpdate(pii, personToUpdate, participantToUpdate, countriesOfCitizenship);
            return personToUpdate;
        }

        /// <summary>
        /// Get the person by id 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>The person</returns>
        public async Task<Person> GetPersonByIdAsync(int personId)
        {
            this.logger.Trace("Retrieving person with id {0}.", personId);
            return await CreateGetPersonById(personId).FirstOrDefaultAsync();
        }

        private IQueryable<Person> CreateGetPersonById(int personId)
        {
            return Context.People.Where(x => x.PersonId == personId).Include(x => x.CountriesOfCitizenship);
        }

        private void DoUpdate(UpdatePii updatePii, Person person, Participant participant, List<Location> countriesOfCitizenship) {
            Contract.Requires(updatePii != null, "The update pii must not be null.");
            Contract.Requires(person != null, "The person to update must not be null.");
            Contract.Requires(participant != null, "The participant to update must not be null.");
            person.FirstName = updatePii.FirstName;
            person.LastName = updatePii.LastName;
            person.NamePrefix = updatePii.NamePrefix;
            person.NameSuffix = updatePii.NameSuffix;
            person.GivenName = updatePii.GivenName;
            person.FamilyName = updatePii.FamilyName;
            person.MiddleName = updatePii.MiddleName;
            person.Patronym = updatePii.Patronym;
            person.Alias = updatePii.Alias;
            person.GenderId = updatePii.GenderId;
            person.Ethnicity = updatePii.Ethnicity;
            person.PlaceOfBirthId = updatePii.CityOfBirthId;
            person.DateOfBirth = updatePii.DateOfBirth;
            person.MedicalConditions = updatePii.MedicalConditions;
            person.MaritalStatusId = updatePii.MaritalStatusId;
            participant.SevisId = updatePii.SevisId;

            SetCountriesOfCitizenship(countriesOfCitizenship, person);
        }

        private void SetCountriesOfCitizenship(List<Location> countriesOfCitizenship, Person person)
        {
            Contract.Requires(countriesOfCitizenship != null, "The country ids must not be null.");
            Contract.Requires(person != null, "The person entity must not be null.");
            person.CountriesOfCitizenship.Clear();
            countriesOfCitizenship.ForEach(x =>
            {
                person.CountriesOfCitizenship.Add(x);
            });
        }

        /// <summary>
        /// Get participant by id 
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participant</returns>
        public async Task<Participant> GetParticipantByIdAsync(int participantId)
        {
            this.logger.Trace("Retrieving participant with id {0}.", participantId);
            return await CreateGetParticipantById(participantId).FirstOrDefaultAsync();
        }

        private IQueryable<Participant> CreateGetParticipantById(int participantId)
        {
            return Context.Participants.Where(x => x.ParticipantId == participantId);
        }
    }
}
