﻿using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.Logging;
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

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Person service
    /// </summary>
    public class PersonService : EcaService, IPersonService
    {
        private static readonly string COMPONENT_NAME = typeof(PersonService).FullName;
        private readonly ILogger logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="logger">The logger to use</param>
        public PersonService(EcaContext context, ILogger logger) : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
        }

        /// <summary>
        /// Returns personally identifiable information for a user 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Personally identifiable information for person</returns>
        public PiiDTO GetPiiById(int personId)
        {
            var stopwatch = Stopwatch.StartNew();
            var pii = PersonQueries.CreateGetPiiByIdQuery(this.Context, personId).SingleOrDefault();
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "personId", personId } });
            return pii;
        }

        /// <summary>
        /// Returns personally identifiable information for a user asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Personally identifiable information for person</returns>
        public Task<PiiDTO> GetPiiByIdAsync(int personId)
        {
            var stopwatch = Stopwatch.StartNew();
            var pii = PersonQueries.CreateGetPiiByIdQuery(this.Context, personId).SingleOrDefaultAsync();
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "personId", personId } });
            return pii;
        }

        /// <summary>
        /// Returns contact info related to a person
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Contact info related to person</returns>
        public ContactInfoDTO GetContactInfoById(int personId)
        {
            var stopwatch = Stopwatch.StartNew();
            var contactInfo = PersonQueries.CreateGetContactInfoByIdQuery(this.Context, personId).SingleOrDefault();
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "personId", personId } });
            return contactInfo;
        }

        /// <summary>
        /// Returns contact info related to a person asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Contact info related to person</returns>
        public Task<ContactInfoDTO> GetContactInfoByIdAsync(int personId)
        {
            var stopwatch = Stopwatch.StartNew();
            var contactInfo = PersonQueries.CreateGetContactInfoByIdQuery(this.Context, personId).SingleOrDefaultAsync();
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "personId", personId } });
            return contactInfo;
        }

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
            var participant = CreateParticipant(person, project);
            return person;
        }

        /// <summary>
        /// Gets the project by id asyncronously
        /// </summary>
        /// <param name="projectId">The project id to lookup</param>
        /// <returns>A project</returns>
        protected async Task<Project> GetProjectByIdAsync(int projectId)
        {
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

            return participant;
        }

    }
}
