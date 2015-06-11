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
                StudyProject = "studyProject"
            };

            context.ParticipantPersons.Add(participantPerson);

            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(participantPerson.SevisId, participantPersonResult.SevisId);
            Assert.AreEqual(participantPerson.ContactAgreement, participantPersonResult.ContactAgreement);
            Assert.AreEqual(participantPerson.StudyProject, participantPersonResult.StudyProject);

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
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                FieldOfStudy = fieldOfStudy
            };

            context.ParticipantPersons.Add(participantPerson);

            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(fieldOfStudy.FieldOfStudyCode, participantPersonResult.FieldOfStudy);
        }

        [TestMethod]
        public void TestCreateGetSimpleParticipantPersonsDTOQuery_CheckProgramSubject()
        {
            var programSubject = new ProgramSubject
            {
                ProgramSubjectId = 1,
                ProgramSubjectCode = "123"
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                ProgramSubject = programSubject
            };

            context.ParticipantPersons.Add(participantPerson);

            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(programSubject.ProgramSubjectCode, participantPersonResult.ProgramSubject);
        }

        [TestMethod]
        public void TestCreateGetSimpleParticipantPersonsDTOQuery_CheckPosition()
        {
            var position = new Position
            {
                PositionId = 1,
                PositionCode = "123"
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                Position = position
            };

            context.ParticipantPersons.Add(participantPerson);

            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(position.PositionCode, participantPersonResult.Position);
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

            var location = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.Address.Id,
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
                Country = country, 
                City = "city",
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

            context.ParticipantPersons.Add(participantPerson);

            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(homeInstitution.Name, participantPersonResult.HomeInstitution.Name);

            var addressResult = participantPersonResult.HomeInstitution.Addresses.FirstOrDefault();

            Assert.AreEqual(location.Street1, addressResult.Street1);
            Assert.AreEqual(location.Street2, addressResult.Street2);
            Assert.AreEqual(location.Street3, addressResult.Street3);
            Assert.AreEqual(country.LocationName, addressResult.Country);
            Assert.AreEqual(location.City, addressResult.City);
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

            var location = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.Address.Id,
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
                Country = country, 
                City = "city",
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

            context.ParticipantPersons.Add(participantPerson);

            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(hostInstitution.Name, participantPersonResult.HostInstitution.Name);

            var addressResult = participantPersonResult.HostInstitution.Addresses.FirstOrDefault();

            Assert.AreEqual(location.Street1, addressResult.Street1);
            Assert.AreEqual(location.Street2, addressResult.Street2);
            Assert.AreEqual(location.Street3, addressResult.Street3);
            Assert.AreEqual(country.LocationName, addressResult.Country);
            Assert.AreEqual(location.City, addressResult.City);
            Assert.AreEqual(location.PostalCode, addressResult.PostalCode);
        }
    }
}
