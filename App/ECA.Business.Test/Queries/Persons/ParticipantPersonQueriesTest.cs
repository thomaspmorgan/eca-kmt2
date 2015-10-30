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
                StudyProject = "studyProject",
                HomeInstitutionAddressId = 3,
                HostInstitutionAddressId = 4,
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
            Assert.AreEqual(participantPerson.StudyProject, participantPersonResult.StudyProject);
            Assert.AreEqual(project.ProjectId, participantPersonResult.ProjectId);
            Assert.AreEqual(participantPerson.HomeInstitutionAddressId, participantPersonResult.HomeInstitutionAddressId);
            Assert.AreEqual(participantPerson.HostInstitutionAddressId, participantPersonResult.HostInstitutionAddressId);

            Assert.AreEqual(0, participantPersonResult.ParticipantTypeId);
            Assert.IsFalse(participantPersonResult.ParticipantStatusId.HasValue);
            Assert.IsNull(participantPersonResult.FieldOfStudy);
            Assert.IsNull(participantPersonResult.ProgramCategory);
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
            var programCategory = new ProgramCategory
            {
                ProgramCategoryId = 1,
                ProgramCategoryCode = "123",
                Description = "description"
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                ProgramCategory = programCategory
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

            Assert.AreEqual(programCategory.Description, participantPersonResult.ProgramCategory);
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
            var division = new Location
            {
                LocationId = 10,
                LocationTypeId = LocationType.Division.Id,
                LocationName = "state"
            };

            var location = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.Address.Id,
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
                Country = country, 
                CountryId = country.LocationId,
                CityId = city.LocationId,
                City = city,
                PostalCode = "12345",
                DivisionId = division.LocationId,
                Division = division

            };
            var addressType = new AddressType
            {
                AddressTypeId = AddressType.Home.Id,
                AddressName = AddressType.Home.Value
            };
            var address = new Address
            {
               AddressId = 1,
               Location = location,
               AddressType = addressType,
               AddressTypeId = addressType.AddressTypeId,
               IsPrimary = true
            };

            var orgType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };
            var homeInstitution = new Organization
            {
                OrganizationId = 1,
                Name = "homeInstitution",
                OrganizationType = orgType,
                OrganizationTypeId = orgType.OrganizationTypeId,
            };

            homeInstitution.Addresses.Add(address);

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                HomeInstitution = homeInstitution,
                HomeInstitutionId = homeInstitution.OrganizationId,
                HomeInstitutionAddressId = address.AddressId,
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

            context.Organizations.Add(homeInstitution);
            context.OrganizationTypes.Add(orgType);
            context.Addresses.Add(address);
            context.AddressTypes.Add(addressType);
            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();
            Assert.IsNotNull(participantPersonResult.HomeInstitution);
            Assert.AreEqual(homeInstitution.OrganizationId, participantPersonResult.HomeInstitution.OrganizationId);
            Assert.AreEqual(homeInstitution.Name, participantPersonResult.HomeInstitution.Name);
            Assert.AreEqual(address.AddressId, participantPersonResult.HomeInstitutionAddressId);
            var addressResult = participantPersonResult.HomeInstitution.Addresses.FirstOrDefault();

            Assert.AreEqual(location.Street1, addressResult.Street1);
            Assert.AreEqual(location.Street2, addressResult.Street2);
            Assert.AreEqual(location.Street3, addressResult.Street3);
            Assert.AreEqual(country.LocationName, addressResult.Country);
            Assert.AreEqual(country.LocationId, addressResult.CountryId);
            Assert.AreEqual(location.City.LocationName, addressResult.City);
            Assert.AreEqual(location.PostalCode, addressResult.PostalCode);
            Assert.AreEqual(location.LocationId, addressResult.LocationId);
            Assert.AreEqual(address.AddressId, addressResult.AddressId);
            Assert.AreEqual(addressType.AddressTypeId, addressResult.AddressTypeId);
            Assert.AreEqual(addressType.AddressName, addressResult.AddressType);
            Assert.AreEqual(address.IsPrimary, addressResult.IsPrimary);
            Assert.AreEqual(division.LocationId, addressResult.DivisionId);
            Assert.AreEqual(division.LocationName, addressResult.Division);
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
            var division = new Location
            {
                LocationId = 10,
                LocationTypeId = LocationType.Division.Id,
                LocationName = "state"
            };

            var location = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.Address.Id,
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
                CountryId = country.LocationId,
                CityId = city.LocationId,
                Country = country, 
                City = city,
                PostalCode = "12345",
                Division = division,
                DivisionId = division.LocationId
            };
            var addressType = new AddressType
            {
                AddressTypeId = AddressType.Home.Id,
                AddressName = AddressType.Home.Value
            };
            var address = new Address
            {
               AddressId = 1,
               Location = location,
               AddressType = addressType,
               AddressTypeId = addressType.AddressTypeId,
               IsPrimary = true
            };

            var orgType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };
            var hostInstitution = new Organization
            {
                OrganizationId = 1,
                Name = "hostInstitution",
                OrganizationType = orgType,
                OrganizationTypeId = orgType.OrganizationTypeId,
            };

            hostInstitution.Addresses.Add(address);

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                HostInstitution = hostInstitution,
                HostInstitutionId = hostInstitution.OrganizationId,
                HostInstitutionAddressId = address.AddressId
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

            context.Organizations.Add(hostInstitution);
            context.OrganizationTypes.Add(orgType);
            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.IsNotNull(participantPersonResult.HostInstitution);
            Assert.AreEqual(hostInstitution.OrganizationId, participantPersonResult.HostInstitution.OrganizationId);
            Assert.AreEqual(hostInstitution.Name, participantPersonResult.HostInstitution.Name);
            Assert.AreEqual(address.AddressId, participantPersonResult.HostInstitutionAddressId);
            var addressResult = participantPersonResult.HostInstitution.Addresses.FirstOrDefault();

            Assert.AreEqual(location.Street1, addressResult.Street1);
            Assert.AreEqual(location.Street2, addressResult.Street2);
            Assert.AreEqual(location.Street3, addressResult.Street3);
            Assert.AreEqual(country.LocationName, addressResult.Country);
            Assert.AreEqual(country.LocationId, addressResult.CountryId);
            Assert.AreEqual(city.LocationName, addressResult.City);
            Assert.AreEqual(location.PostalCode, addressResult.PostalCode);
            Assert.AreEqual(address.AddressId, addressResult.AddressId);
            Assert.AreEqual(addressType.AddressTypeId, addressResult.AddressTypeId);
            Assert.AreEqual(addressType.AddressName, addressResult.AddressType);
            Assert.AreEqual(address.IsPrimary, addressResult.IsPrimary);
            Assert.AreEqual(division.LocationId, addressResult.DivisionId);
            Assert.AreEqual(division.LocationName, addressResult.Division);
        }

        [TestMethod]
        public void TestCreateGetSimpleParticipantPersonsDTOQuery_CheckParticipantType()
        {
            var participantType = new ParticipantType
            {
                ParticipantTypeId = 1,
                Name = "type"
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = participantPerson.ParticipantId,
                ProjectId = project.ProjectId,
                Project = project,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            participantPerson.Participant = participant;
            project.Participants.Add(participant);

            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(participantType.Name, participantPersonResult.ParticipantType);
            Assert.AreEqual(participantType.ParticipantTypeId, participantPersonResult.ParticipantTypeId);
        }


        [TestMethod]
        public void TestCreateGetSimpleParticipantPersonsDTOQuery_CheckParticipantStatus()
        {
            var participantStatus = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status"
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = participantPerson.ParticipantId,
                ProjectId = project.ProjectId,
                Project = project,
                Status = participantStatus,
                ParticipantStatusId = participantStatus.ParticipantStatusId,
            };
            participantPerson.Participant = participant;
            project.Participants.Add(participant);

            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            var participantPersonResult = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(context).FirstOrDefault();

            Assert.AreEqual(participantStatus.Status, participantPersonResult.ParticipantStatus);
            Assert.AreEqual(participantStatus.ParticipantStatusId, participantPersonResult.ParticipantStatusId);
        }
    }
}
