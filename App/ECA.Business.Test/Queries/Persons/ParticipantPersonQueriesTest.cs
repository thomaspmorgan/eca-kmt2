using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Persons;
using ECA.Data;

namespace ECA.Business.Test.Queries.Persons
{
    [TestClass]
    public class ParticipantPersonQueriesTest
    {
        private TestEcaContext context;
        
        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestCreateGetSimpleParticipantPersonsDTOQuery_CheckProperties()
        {
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = "N0000000001",
                ContactAgreement = false,
                StudyProject = "studyProject",
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = participantPerson.ParticipantId,                
                ProjectId = project.ProjectId,
                Project = project
            };
            participantPerson.Participant = participant;
            project.Participants.Add(participant);

            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(participantPerson.SevisId, participantPersonResult.SevisId);
            Assert.AreEqual(participantPerson.ContactAgreement, participantPersonResult.ContactAgreement);
            Assert.AreEqual(participantPerson.StudyProject, participantPersonResult.StudyProject);
            Assert.AreEqual(project.ProjectId, participantPersonResult.ProjectId);

            Assert.IsNull(participantPersonResult.FieldOfStudy);
            Assert.IsNull(participantPersonResult.ProgramSubject);
            Assert.IsNull(participantPersonResult.Position);
            Assert.IsNull(participantPersonResult.HostInstitution);
            Assert.IsNull(participantPersonResult.HomeInstitution);
        }

        [TestMethod]
        public void TestCreateGetSimpleParticipantPersonsDTOQuery_CheckFieldOfStudy()
        {
            var fieldOfStudy = new FieldOfStudy
            {
                FieldOfStudyId = 1,
                FieldOfStudyCode = "123",
                Description = "description"
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                FieldOfStudy = fieldOfStudy
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = participantPerson.ParticipantId,
                ProjectId = project.ProjectId,
                Project = project
            };
            participantPerson.Participant = participant;
            project.Participants.Add(participant);

            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(fieldOfStudy.Description, participantPersonResult.FieldOfStudy);
        }

        [TestMethod]
        public void TestCreateGetSimpleParticipantPersonsDTOQuery_CheckProgramSubject()
        {
            var programSubject = new ProgramSubject
            {
                ProgramSubjectId = 1,
                ProgramSubjectCode = "123",
                Description = "description"
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                ProgramSubject = programSubject
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = participantPerson.ParticipantId,
                ProjectId = project.ProjectId,
                Project = project
            };
            participantPerson.Participant = participant;
            project.Participants.Add(participant);

            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            
            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(programSubject.Description, participantPersonResult.ProgramSubject);
        }

        [TestMethod]
        public void TestCreateGetSimpleParticipantPersonsDTOQuery_CheckPosition()
        {
            var position = new Position
            {
                PositionId = 1,
                PositionCode = "123",
                Description = "description"
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                Position = position
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = participantPerson.ParticipantId,
                ProjectId = project.ProjectId,
                Project = project
            };
            participantPerson.Participant = participant;
            project.Participants.Add(participant);

            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(position.Description, participantPersonResult.Position);
        }

        [TestMethod]
        public void TestCreateGetSimpleParticipantPersonsDTOQuery_HomeInstitution()
        {
            var country = new Location 
            {
                LocationId = 2,
                LocationTypeId = LocationType.Country.Id,
                LocationName = "country"
            };

            var city = new Location
            {
                LocationId = 3,
                LocationTypeId = LocationType.City.Id,
                LocationName = "city"
            };

            var location = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.Address.Id,
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
                Country = country, 
                City = city,
                PostalCode = "12345"
            };

            var address = new Address
            {
               AddressId = 1,
               Location = location
            };

            var homeInstitution = new Organization
            {
                OrganizationId = 1,
                Name = "homeInstitution"
            };

            homeInstitution.Addresses.Add(address);

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                HomeInstitution = homeInstitution
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = participantPerson.ParticipantId,
                ProjectId = project.ProjectId,
                Project = project
            };
            participantPerson.Participant = participant;
            project.Participants.Add(participant);

            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(homeInstitution.Name, participantPersonResult.HomeInstitution.Name);

            var addressResult = participantPersonResult.HomeInstitution.Addresses.FirstOrDefault();

            Assert.AreEqual(location.Street1, addressResult.Street1);
            Assert.AreEqual(location.Street2, addressResult.Street2);
            Assert.AreEqual(location.Street3, addressResult.Street3);
            Assert.AreEqual(country.LocationName, addressResult.Country);
            Assert.AreEqual(country.LocationId, addressResult.CountryId);
            Assert.AreEqual(location.City.LocationName, addressResult.City);
            Assert.AreEqual(location.PostalCode, addressResult.PostalCode);
        }

        [TestMethod]
        public void TestCreateGetSimpleParticipantPersonsDTOQuery_HostInstitution()
        {
            var country = new Location 
            {
                LocationId = 2,
                LocationTypeId = LocationType.Country.Id,
                LocationName = "country"
            };

            var city = new Location
            {
                LocationId = 3,
                LocationTypeId = LocationType.City.Id,
                LocationName = "city"
            };

            var location = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.Address.Id,
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
                Country = country, 
                City = city,
                PostalCode = "12345"
            };

            var address = new Address
            {
               AddressId = 1,
               Location = location
            };

            var hostInstitution = new Organization
            {
                OrganizationId = 1,
                Name = "hostInstitution"
            };

            hostInstitution.Addresses.Add(address);

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                HostInstitution = hostInstitution
            };

            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = participantPerson.ParticipantId,
                ProjectId = project.ProjectId,
                Project = project
            };
            participantPerson.Participant = participant;
            project.Participants.Add(participant);

            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(hostInstitution.Name, participantPersonResult.HostInstitution.Name);

            var addressResult = participantPersonResult.HostInstitution.Addresses.FirstOrDefault();

            Assert.AreEqual(location.Street1, addressResult.Street1);
            Assert.AreEqual(location.Street2, addressResult.Street2);
            Assert.AreEqual(location.Street3, addressResult.Street3);
            Assert.AreEqual(country.LocationName, addressResult.Country);
            Assert.AreEqual(country.LocationId, addressResult.CountryId);
            Assert.AreEqual(city.LocationName, addressResult.City);
            Assert.AreEqual(location.PostalCode, addressResult.PostalCode);
        }
    }
}
