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
                this.logger.Trace("Found existing person {0}.");
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
        /// Query for an existing person
        /// </summary>
        /// <param name="newPerson">The person to query for</param>
        /// <returns>The existing person or null</returns>
        public async Task<Person> GetExistingPerson(NewPerson newPerson)
        {
            this.logger.Trace("Retrieving person with match to {0}.", newPerson);
            return await CreateGetPerson(newPerson).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates query for existing person
        /// </summary>
        /// <param name="newPerson">The person to query for</param>
        /// <returns>The queryable person or null</returns>
        private IQueryable<Person> CreateGetPerson(NewPerson newPerson)
        {
            return Context.People.Where(
                    x => x.FirstName.ToLower().Trim() == newPerson.FirstName.ToLower().Trim() &&
                         x.LastName.ToLower().Trim() == newPerson.LastName.ToLower().Trim() &&
                         x.GenderId == newPerson.Gender &&
                         x.DateOfBirth.Day == newPerson.DateOfBirth.Day &&
                         x.DateOfBirth.Month == newPerson.DateOfBirth.Month &&
                         x.DateOfBirth.Year == newPerson.DateOfBirth.Year &&
                         x.PlaceOfBirthId == newPerson.CityOfBirth
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

        public async Task<Person> UpdatePiiAsync(UpdatePii pii) {
            var personToUpdate = await GetPersonByIdAsync(pii.PersonId);
            var participantToUpdate = await GetParticipantByIdAsync(pii.ParticipantId);
            var countriesOfCitizenship = await GetLocationsByIdAsync(pii.CountriesOfCitizenship);
            DoUpdate(pii, personToUpdate, participantToUpdate, countriesOfCitizenship);
            return personToUpdate;
        }

        public async Task<Person> GetPersonByIdAsync(int personId)
        {
            this.logger.Trace("Retrieving person with id {0}.", personId);
            return await CreateGetPersonById(personId).FirstOrDefaultAsync();
        }

        private IQueryable<Person> CreateGetPersonById(int personId)
        {
            return Context.People.Where(x => x.PersonId == personId).Include("CountriesOfCitizenship");
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
