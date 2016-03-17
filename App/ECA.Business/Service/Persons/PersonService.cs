using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using NLog;
using ECA.Business.Exceptions;
using ECA.Business.Validation;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Core.Exceptions;
using ECA.Business.Service.Lookup;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Person service
    /// </summary>
    public class PersonService : EcaService, IPersonService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IBusinessValidator<PersonServiceValidationEntity, PersonServiceValidationEntity> validator;
        private Action<Location, int> throwIfLocationNotFound;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to query</param>
        public PersonService(EcaContext context, IBusinessValidator<PersonServiceValidationEntity, PersonServiceValidationEntity> validator, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(validator != null, "The validator must not be null.");
            this.validator = validator;

            throwIfLocationNotFound = (location, id) =>
            {
                if (location == null)
                {
                    throw new ModelNotFoundException(String.Format("The location entity with id [{0}] was not found.", id));
                }
            };
        }

        #region Pii

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
        /// Update pii
        /// </summary>
        /// <param name="pii">The pii business model</param>
        /// <returns>The person updated</returns>
        public async Task<Person> UpdatePiiAsync(UpdatePii pii)
        {
            var existingPerson = await GetExistingPersonAsync(pii);
            if (existingPerson != null && pii.PersonId != existingPerson.PersonId)
            {
                this.logger.Trace("Found existing person {0}.", existingPerson);
                throw new EcaBusinessException("The person already exists.");
            }
            var personToUpdate = await GetPersonModelByIdAsync(pii.PersonId);
            Location cityOfBirth = null;
            if (pii.CityOfBirthId.HasValue)
            {
                cityOfBirth = await GetLocationByIdAsync(pii.CityOfBirthId.Value);
                throwIfLocationNotFound(cityOfBirth, pii.CityOfBirthId.Value);
            }
            var countriesOfCitizenship = await GetLocationsByIdAsync(pii.CountriesOfCitizenship);
            DoUpdate(pii, personToUpdate, cityOfBirth, countriesOfCitizenship);
            return personToUpdate;
        }

        private void DoUpdate(UpdatePii updatePii, Person person, Location cityOfBirth, List<Location> countriesOfCitizenship)
        {
            validator.ValidateUpdate(GetPersonServiceValidationEntity(
                person: person,
                dateOfBirth: updatePii.DateOfBirth,
                genderId: updatePii.GenderId,
                countriesOfCitizenship: countriesOfCitizenship,
                placeOfBirthId: cityOfBirth != null ? cityOfBirth.LocationId : default(int?),
                isDateOfBirthUnknown: updatePii.IsDateOfBirthUnknown,
                isDateOfBirthEstimated: updatePii.IsDateOfBirthEstimated,
                isPlaceOfBirthUnknown: updatePii.IsPlaceOfBirthUnknown
                ));
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
            person.PlaceOfBirth = cityOfBirth;
            person.PlaceOfBirthId = updatePii.CityOfBirthId;
            person.IsPlaceOfBirthUnknown = updatePii.IsPlaceOfBirthUnknown;
            person.DateOfBirth = updatePii.DateOfBirth;
            person.IsDateOfBirthUnknown = updatePii.IsDateOfBirthUnknown;
            person.MedicalConditions = updatePii.MedicalConditions;
            person.MaritalStatusId = updatePii.MaritalStatusId;
            person.IsDateOfBirthEstimated = updatePii.IsDateOfBirthEstimated;
            updatePii.Audit.SetHistory(person);
            SetCountriesOfCitizenship(countriesOfCitizenship, person);
        }

        #endregion

        #region General

        /// <summary>
        /// Returns general information for a user 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Personally identifiable information for person</returns>
        public GeneralDTO GetGeneralById(int personId)
        {
            var general = PersonQueries.CreateGetGeneralByIdQuery(this.Context, personId).SingleOrDefault();
            this.logger.Trace("Retrieved general person info by id {0}.", personId);
            return general;
        }

        /// <summary>
        /// Returns general information for a user asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Personally identifiable information for person</returns>
        public Task<GeneralDTO> GetGeneralByIdAsync(int personId)
        {
            var general = PersonQueries.CreateGetGeneralByIdQuery(this.Context, personId).SingleOrDefaultAsync();
            this.logger.Trace("Retrieved general person info by id {0}.", personId);
            return general;
        }

        private void DoDelete(Person personToDelete)
        {
            if (personToDelete != null)
            {
                Context.People.Remove(personToDelete);
            }
        }

        /// <summary>
        /// Update general
        /// </summary>
        /// <param name="general">The general business model</param>
        /// <returns>The person updated</returns>
        public async Task<Person> UpdateGeneralAsync(UpdateGeneral general)
        {
            var personToUpdate = await GetPersonWithProminentCategoriesByIdAsync(general.PersonId);
            var prominentCategories = await GetProminentCategoriesByIdAsync(general.ProminentCategories);
            DoUpdate(general, personToUpdate, prominentCategories);

            return personToUpdate;
        }

        /// <summary>
        /// Update General list of prominent categories
        /// </summary>
        /// <param name="general"></param>
        /// <param name="person"></param>
        /// <param name="prominentCategories"></param>
        private void DoUpdate(UpdateGeneral general, Person person, List<ProminentCategory> prominentCategories)
        {
            SetProminentCategories(person, prominentCategories);
        }

        #endregion

        #region Dependent
        
        /// <summary>
        /// Get a person dependent
        /// </summary>
        /// <param name="personId">The person Id</param>
        /// <returns>The person dependent</returns>
        public async Task<SimplePersonDependentDTO> GetPersonDependentByIdAsync(int dependentId)
        {
            this.logger.Trace("Retrieving person with id {0}.", dependentId);
            return await CreateGetSimplePersonDependent(dependentId).FirstOrDefaultAsync();
        }

        private IQueryable<SimplePersonDependentDTO> CreateGetSimplePersonDependent(int dependentId)
        {
            var query = PersonQueries.CreateGetSimplePersonDependentDTOsQuery(this.Context);
            return query.Where(p => p.PersonId == dependentId);
        }
        
        /// <summary>
        /// Create a person dependent
        /// </summary>
        /// <param name="newPersonDependent">The person dependent to create</param>
        /// <returns>The person create</returns>
        public Task<Person> CreateDependentAsync(NewPersonDependent newPersonDependent)
        {
            return Task.Run(() => CreatePersonDependent(newPersonDependent, newPersonDependent.CountriesOfCitizenship));
        }
        
        /// <summary>
        /// Creates a person dependent
        /// </summary>
        /// <param name="newPerson"></param>
        /// <param name="countriesOfCitizenship"></param>
        /// <returns></returns>
        private Person CreatePersonDependent(NewPersonDependent newPerson, List<SimpleLookupDTO> countriesOfCitizenship)
        {
            HashSet<EmailAddress> emails = new HashSet<EmailAddress>();
            EmailAddress email = new EmailAddress { Address = newPerson.EmailAddress };
            var locationsOfCitizenship = GetLocationsByIdAsync(countriesOfCitizenship.Select(x => x.Id).ToList());
            emails.Add(email);

            var person = new Person
            {
                FirstName = newPerson.FullName.FirstName,
                LastName = newPerson.FullName.LastName,
                GenderId = newPerson.Gender,
                DateOfBirth = newPerson.DateOfBirth,
                PlaceOfBirthId = newPerson.CityOfBirth,
                CountriesOfCitizenship = locationsOfCitizenship.Result,
                EmailAddresses = emails,
                PersonTypeId = newPerson.PersonTypeId
            };

            newPerson.Audit.SetHistory(person);
            this.Context.People.Add(person);
            this.logger.Trace("Creating new person dependent {0}.", newPerson);
            return person;
        }

        /// <summary>
        /// Update a person dependent
        /// </summary>
        /// <param name="person">The dependent to update</param>
        /// <returns>The updated dependent</returns>
        public async Task<Person> UpdatePersonDependentAsync(UpdatedPersonDependent person)
        {
            var personToUpdate = await GetPersonModelByIdAsync(person.PersonId);
            Location cityOfBirth = null;
            if (person.CityOfBirth > 0)
            {
                cityOfBirth = await GetLocationByIdAsync(person.CityOfBirth);
                throwIfLocationNotFound(cityOfBirth, person.CityOfBirth);
            }
            var countriesOfCitizenship = await GetLocationsByIdAsync(person.CountriesOfCitizenship);
            DoDependentUpdate(person, personToUpdate, cityOfBirth, countriesOfCitizenship);
            return personToUpdate;
        }

        private void DoDependentUpdate(UpdatedPersonDependent updateDependent, Person person, Location cityOfBirth, List<Location> countriesOfCitizenship)
        {
            HashSet<EmailAddress> emails = new HashSet<EmailAddress>();
            EmailAddress email = new EmailAddress { Address = updateDependent.EmailAddress, EmailAddressTypeId = EmailAddressType.Personal.Id, IsPrimary = true };
            emails.Add(email);
            HashSet<Address> addresses = new HashSet<Address>();
            Address address = new Address { LocationId = updateDependent.PermanentResidenceCountryCode, PersonId = updateDependent.PersonId, AddressTypeId = AddressType.Home.Id };
            addresses.Add(address);
            person.FirstName = updateDependent.FullName.FirstName;
            person.LastName = updateDependent.FullName.LastName;
            person.NameSuffix = updateDependent.FullName.Suffix;
            person.DateOfBirth = updateDependent.DateOfBirth;
            person.GenderId = updateDependent.GenderId;
            person.PlaceOfBirthId = cityOfBirth.LocationId;
            person.Addresses = addresses;
            person.EmailAddresses = emails;
            person.PersonTypeId = updateDependent.PersonTypeId;
            //person.BirthCountryReason = updateDependent.BirthCountryReason;
            updateDependent.Audit.SetHistory(person);
            SetCountriesOfCitizenship(countriesOfCitizenship, person);
        }

        /// <summary>
        /// Deletes a dependent from a person family
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="dependentId"></param>
        /// <returns></returns>
        public async Task DeletePersonDependentByIdAsync(int personId, int dependentId)
        {
            var person = await Context.People.FindAsync(personId);
            var dependent = await Context.People.FindAsync(dependentId);
            person.Family.Remove(dependent);
            DoDelete(dependent);
        }
        
        #endregion

        #region Contact
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
        /// Update contact Info
        /// </summary>
        /// <param name="contactInfo">The general business model</param>
        /// <returns>The person updated</returns>
        public async Task<Person> UpdateContactInfoAsync(UpdateContactInfo contactInfo)
        {
            var personToUpdate = await GetPersonModelByIdAsync(contactInfo.PersonId);
            DoUpdate(contactInfo, personToUpdate);
            return personToUpdate;
        }

        // Update contact info HasContactAgreement

        private void DoUpdate(UpdateContactInfo contactInfo, Person person)
        {
            person.HasContactAgreement = contactInfo.HasContactAgreement;
        }

        #endregion

        #region Employment
        /// <summary>
        /// Returns education and professional employment information for a user 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Education and professional employment information for person</returns>
        public IList<EducationEmploymentDTO> GetEmploymentsByPersonId(int personId)
        {
            var employments = PersonQueries.CreateGetEmploymentsByPersonIdQuery(this.Context, personId).ToList();
            this.logger.Trace("Retrieved employments for person info by id {0}.", personId);
            return employments;
        }

        /// <summary>
        /// Returns education and professional employment information for a user asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Education and professional employment information for person</returns>
        public async Task<IList<EducationEmploymentDTO>> GetEmploymentsByPersonIdAsync(int personId)
        {
            var employments = await PersonQueries.CreateGetEmploymentsByPersonIdQuery(this.Context, personId).ToListAsync();
            this.logger.Trace("Retrieved employments for person info by id {0}.", personId);
            return employments;
        }
        #endregion

        #region Education
        /// <summary>
        /// Returns professional employments information for a user 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Professional employments information for person</returns>
        public IList<EducationEmploymentDTO> GetEducationsByPersonId(int personId)
        {
            var educations = PersonQueries.CreateGetEducationsByPersonIdQuery(this.Context, personId).ToList();
            this.logger.Trace("Retrieved educations for person info by id {0}.", personId);
            return educations;
        }

        /// <summary>
        /// Returns educations information for a user asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Educations information for person</returns>
        public async Task<IList<EducationEmploymentDTO>> GetEducationsByPersonIdAsync(int personId)
        {
            var educations = await PersonQueries.CreateGetEducationsByPersonIdQuery(this.Context, personId).ToListAsync();
            this.logger.Trace("Retrieved educations for person info by id {0}.", personId);
            return educations;
        }
        #endregion

        #region Evaluation Notes
        /// <summary>
        /// Returns evaluationNotes information for a user 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>EvaluationNotes information for person</returns>
        public IList<EvaluationNoteDTO> GetEvaluationNotesByPersonId(int personId)
        {
            var evaluationNotes = PersonQueries.CreateGetEvaluationNotesByPersonIdQuery(this.Context, personId).ToList();
            this.logger.Trace("Retrieved evaluationNotes for person info by id {0}.", personId);
            return evaluationNotes;
        }

        /// <summary>
        /// Returns educations information for a user asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Educations information for person</returns>
        public async Task<IList<EvaluationNoteDTO>> GetEvaluationNotesByPersonIdAsync(int personId)
        {
            var evaluationNotes = await PersonQueries.CreateGetEvaluationNotesByPersonIdQuery(this.Context, personId).ToListAsync();
            this.logger.Trace("Retrieved evaluationNotes for person info by id {0}.", personId);
            return evaluationNotes;
        }
        #endregion

        #region ProminentCategories

        /// <summary>
        /// Get the person with prominent categories by id 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>The person</returns>
        public async Task<Person> GetPersonWithProminentCategoriesByIdAsync(int personId)
        {
            this.logger.Trace("Retrieving person with prominent categories id {0}.", personId);
            return await CreateGetPersonWithProminentCategoriesById(personId).FirstOrDefaultAsync();
        }

        private IQueryable<Person> CreateGetPersonWithProminentCategoriesById(int personId)
        {
            return Context.People.Where(x => x.PersonId == personId).Include(x => x.ProminentCategories);
        }

        /// <summary>
        /// Get a list of prominent categories
        /// </summary>
        /// <param name="prominentCategoryIds">Ids to lookup</param>
        /// <returns>A list of prominent categories</returns>
        protected async Task<List<ProminentCategory>> GetProminentCategoriesByIdAsync(List<int> prominentCategoryIds)
        {
            var prominentCategories = await CreateGetProminentCategoriesById(prominentCategoryIds).ToListAsync();
            logger.Trace("Retrieved prominent categories by ids {0}.", String.Join(", ", prominentCategoryIds));
            return prominentCategories;
        }

        /// <summary>
        /// Creates query for looking up a list of prominent categories
        /// </summary>
        /// <param name="prominentCategoryIds">Ids to lookup</param>
        /// <returns>Queryable list of prominent categories</returns>
        private IQueryable<ProminentCategory> CreateGetProminentCategoriesById(List<int> prominentCategoryIds)
        {
            var prominentCategories = Context.ProminentCategories.Where(x => prominentCategoryIds.Contains(x.ProminentCategoryId));
            logger.Trace("Retrieved prominent categories by ids {0}.", String.Join(", ", prominentCategoryIds));
            return prominentCategories;
        }

        private void SetProminentCategories(Person person, List<ProminentCategory> prominentCategories)
        {
            Contract.Requires(prominentCategories != null, "The promiment ids must not be null.");
            Contract.Requires(person != null, "The person entity must not be null.");
            person.ProminentCategories = prominentCategories;
        }

        #endregion ProminentCategories

        #region Create
        /// <summary>
        /// Create a person
        /// </summary>
        /// <param name="newPerson">The person to create</param>
        /// <returns>The person created</returns>
        public async Task<Person> CreateAsync(NewPerson newPerson)
        {
            var project = await GetProjectByIdAsync(newPerson.ProjectId);
            var countriesOfCitizenship = await GetLocationsByIdAsync(newPerson.CountriesOfCitizenship);
            var person = CreatePerson(newPerson, countriesOfCitizenship);
            var participant = CreateParticipant(person, newPerson.ParticipantTypeId, project);
            this.validator.ValidateCreate(GetPersonServiceValidationEntity(
                person: person,
                dateOfBirth: newPerson.DateOfBirth,
                genderId: newPerson.Gender,
                countriesOfCitizenship: countriesOfCitizenship,
                placeOfBirthId: newPerson.CityOfBirth,
                isDateOfBirthUnknown: newPerson.IsDateOfBirthUnknown,
                isDateOfBirthEstimated: newPerson.IsDateOfBirthEstimated,
                isPlaceOfBirthUnknown: newPerson.IsPlaceOfBirthUnknown
                ));
            this.logger.Trace("Created participant {0}.", newPerson);
            return person;
        }

        /// <summary>
        /// Get existing person
        /// </summary>
        /// <param name="newPerson">The person to lookup</param>
        /// <returns>The person found</returns>
        public async Task<Person> GetExistingPersonAsync(NewPerson newPerson)
        {
            this.logger.Trace("Retrieving person {0}.", newPerson);
            return await CreateGetPerson(newPerson.FirstName, newPerson.LastName, newPerson.Gender, newPerson.DateOfBirth, newPerson.CityOfBirth).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get existing person 
        /// </summary>
        /// <param name="pii">The pii to lookup</param>
        /// <returns>The person found</returns>
        public async Task<Person> GetExistingPersonAsync(UpdatePii pii)
        {
            this.logger.Trace("Retrieving person {0}.", pii);
            return await CreateGetPerson(pii.FirstName, pii.LastName, pii.GenderId, pii.DateOfBirth, pii.CityOfBirthId).FirstOrDefaultAsync();
        }

        private PersonServiceValidationEntity GetPersonServiceValidationEntity(
            Person person, 
            DateTime? dateOfBirth,
            int genderId, 
            List<Location> countriesOfCitizenship, 
            int? placeOfBirthId, 
            bool? isDateOfBirthUnknown,
            bool? isDateOfBirthEstimated,
            bool? isPlaceOfBirthUnknown)
        {
            return new PersonServiceValidationEntity(
                person: person, 
                dateOfBirth: dateOfBirth,
                genderId: genderId, 
                countriesOfCitizenship: countriesOfCitizenship, 
                placeOfBirthId: placeOfBirthId, 
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown);
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
        private IQueryable<Person> CreateGetPerson(string firstName, string lastName, int genderId, DateTime? dateOfBirth, int? cityOfBirthId)
        {
            return Context.People.Where(
                    x => x.FirstName.ToLower().Trim() == firstName.ToLower().Trim() &&
                         x.LastName.ToLower().Trim() == lastName.ToLower().Trim() &&
                         x.GenderId == genderId &&
                         x.DateOfBirth.HasValue &&
                         x.DateOfBirth.Value.Day == dateOfBirth.Value.Day &&
                         x.DateOfBirth.Value.Month == dateOfBirth.Value.Month &&
                         x.DateOfBirth.Value.Year == dateOfBirth.Value.Year &&
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
                CountriesOfCitizenship = countriesOfCitizenship,
                IsDateOfBirthEstimated = newPerson.IsDateOfBirthEstimated,
                IsDateOfBirthUnknown = newPerson.IsDateOfBirthUnknown,
                IsPlaceOfBirthUnknown = newPerson.IsPlaceOfBirthUnknown,
                PersonTypeId = newPerson.PersonTypeId
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
        /// <param name="participantTypeId">Participant type id</param>
        /// <param name="project">Project to assocate with participant</param>
        /// <returns></returns>
        private Participant CreateParticipant(Person person, int participantTypeId, Project project)
        {
            var participant = new Participant
            {
                PersonId = person.PersonId,
                ParticipantTypeId = participantTypeId
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant
            };
            participant.ParticipantPerson = participantPerson;
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant
            };
            participant.ParticipantExchangeVisitor = participantExchangeVisitor;
            participant.Project = project;
            person.Participations.Add(participant);
            this.Context.Participants.Add(participant);
            this.Context.ParticipantPersons.Add(participantPerson);
            this.Context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            this.logger.Trace("Creating new participant {0}.", person);
            return participant;
        }

        /// <summary>
        /// Get the person by id 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>The person model</returns>
        private async Task<Person> GetPersonModelByIdAsync(int personId)
        {
            this.logger.Trace("Retrieving person with id {0}.", personId);
            return await CreateGetPersonById(personId).FirstOrDefaultAsync();
        }

        private IQueryable<Person> CreateGetPersonById(int personId)
        {
            return Context.People.Where(x => x.PersonId == personId).Include(x => x.CountriesOfCitizenship);
        }

        /// <summary>
        /// Get the person by id 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>The person as SimplePersonDTO</returns>
        public async Task<SimplePersonDTO> GetPersonByIdAsync(int personId)
        {
            this.logger.Trace("Retrieving person with id {0}.", personId);
            return await CreateGetSimplePerson(personId).FirstOrDefaultAsync();
        }

        private IQueryable<SimplePersonDTO> CreateGetSimplePerson(int personId)
        {
            var query = PersonQueries.CreateGetSimplePersonDTOsQuery(this.Context);
            return query.Where(p => p.PersonId == personId);
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

        #endregion

        #region Get People

        /// <summary>
        /// Returns the paged, sorted, and filtered people in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, sorted, and filtered people in the system.</returns>
        public PagedQueryResults<SimplePersonDTO> GetPeople(QueryableOperator<SimplePersonDTO> queryOperator)
        {
            var people = PersonQueries.CreateGetSimplePersonDTOsQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Retrieved people using the query operator [{0}]", queryOperator);
            return people;
        }

        /// <summary>
        /// Returns the paged, sorted, and filtered people in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, sorted, and filtered people in the system.</returns>
        public async Task<PagedQueryResults<SimplePersonDTO>> GetPeopleAsync(QueryableOperator<SimplePersonDTO> queryOperator)
        {
            var people = await PersonQueries.CreateGetSimplePersonDTOsQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Retrieved people using the query operator [{0}]", queryOperator);
            return people;
        }
        #endregion

    }
}
