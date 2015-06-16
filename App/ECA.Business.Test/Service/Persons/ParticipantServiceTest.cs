using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Core.DynamicLinq;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.Query;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;
using System.Collections.Generic;
using ECA.Business.Queries.Persons;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ParticipantServiceTest
    {
        private TestEcaContext context;
        private ParticipantService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ParticipantService(context);
        }

        #region Get

        [TestMethod]
        public async Task TestGetParticipants_DefaultSorter()
        {
            var participantType = new ParticipantType
            {
                Name = ParticipantType.PublicInternationalOrganizationPio.Value,
                ParticipantTypeId = ParticipantType.PublicInternationalOrganizationPio.Id,
            };
            var organization1 = new Organization
            {
                OrganizationId = 1,
                Name = "A"
            };

            var participant1 = new Participant
            {
                Organization = organization1,
                OrganizationId = organization1.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };

            var organization2 = new Organization
            {
                OrganizationId = 2,
                Name = "B"
            };

            var participant2 = new Participant
            {
                Organization = organization2,
                OrganizationId = organization2.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            var address = new Address
            {
                Location = new Location
                {
                    City = new Location
                    {
                        LocationName = "city"
                    },
                    Country = new Location
                    {
                        LocationName = "country"
                    }
                }
            };
            organization1.Addresses.Add(address);
            organization2.Addresses.Add(address);
            context.Addresses.Add(address);
            context.Locations.Add(address.Location);
            context.Locations.Add(address.Location.Country);

            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization1);
            context.Participants.Add(participant1);
            context.Organizations.Add(organization2);
            context.Participants.Add(participant2);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantResult = results.Results.First();
                Assert.AreEqual(organization2.OrganizationId, participantResult.OrganizationId);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipants(queryOperator);
            var serviceResultsAsync = await service.GetParticipantsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipants_Filter()
        {
            var participantType = new ParticipantType
            {
                Name = ParticipantType.PublicInternationalOrganizationPio.Value,
                ParticipantTypeId = ParticipantType.PublicInternationalOrganizationPio.Id,
            };
            var organization1 = new Organization
            {
                OrganizationId = 1,
                Name = "A"
            };

            var participant1 = new Participant
            {
                Organization = organization1,
                OrganizationId = organization1.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };

            var organization2 = new Organization
            {
                OrganizationId = 2,
                Name = "B"
            };

            var participant2 = new Participant
            {
                Organization = organization2,
                OrganizationId = organization2.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization1);
            context.Participants.Add(participant1);
            context.Organizations.Add(organization2);
            context.Participants.Add(participant2);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantResult = results.Results.First();
                Assert.AreEqual(organization2.OrganizationId, participantResult.OrganizationId);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<SimpleParticipantDTO>(x => x.Name, ComparisonType.Equal, organization2.Name));
            var serviceResults = service.GetParticipants(queryOperator);
            var serviceResultsAsync = await service.GetParticipantsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipants_Sort()
        {
            var participantType = new ParticipantType
            {
                Name = ParticipantType.PublicInternationalOrganizationPio.Value,
                ParticipantTypeId = ParticipantType.PublicInternationalOrganizationPio.Id,
            };
            var organization1 = new Organization
            {
                OrganizationId = 1,
                Name = "A"
            };

            var participant1 = new Participant
            {
                Organization = organization1,
                OrganizationId = organization1.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };

            var organization2 = new Organization
            {
                OrganizationId = 2,
                Name = "B"
            };

            var participant2 = new Participant
            {
                Organization = organization2,
                OrganizationId = organization2.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization1);
            context.Participants.Add(participant1);
            context.Organizations.Add(organization2);
            context.Participants.Add(participant2);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantResult = results.Results.First();
                Assert.AreEqual(organization2.OrganizationId, participantResult.OrganizationId);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<SimpleParticipantDTO>(x => x.OrganizationId, SortDirection.Descending));
            var serviceResults = service.GetParticipants(queryOperator);
            var serviceResultsAsync = await service.GetParticipantsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipants_Paging()
        {
            var participantType = new ParticipantType
            {
                Name = ParticipantType.PublicInternationalOrganizationPio.Value,
                ParticipantTypeId = ParticipantType.PublicInternationalOrganizationPio.Id,
            };
            var organization1 = new Organization
            {
                OrganizationId = 1,
                Name = "A"
            };

            var participant1 = new Participant
            {
                Organization = organization1,
                OrganizationId = organization1.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };

            var organization2 = new Organization
            {
                OrganizationId = 2,
                Name = "B"
            };

            var participant2 = new Participant
            {
                Organization = organization2,
                OrganizationId = organization2.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization1);
            context.Participants.Add(participant1);
            context.Organizations.Add(organization2);
            context.Participants.Add(participant2);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantResult = results.Results.First();
                Assert.AreEqual(organization2.OrganizationId, participantResult.OrganizationId);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipants(queryOperator);
            var serviceResultsAsync = await service.GetParticipantsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        #endregion

        #region Get Participants By Project Id
        [TestMethod]
        public async Task TestGetParticipantsByProjectId_ParticipantIsPerson_CheckProperties()
        {
            var project = new Project
            {
                ProjectId = 1,
            };
            var participantType = new ParticipantType
            {
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 1,
                Gender = gender,
                GenderId = gender.GenderId,
            };

            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                Projects = new HashSet<Project> { project }
            };
            project.Participants = new HashSet<Participant> { participant };
            
            context.ParticipantTypes.Add(participantType);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantResult = results.Results.First();

            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipantsByProjectId(project.ProjectId, queryOperator);
            var serviceResultsAsync = await service.GetParticipantsByProjectIdAsync(project.ProjectId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);

        }

        [TestMethod]
        public async Task TestGetParticipantsByProjectId_ParticipantIsOrganization_CheckProperties()
        {
            var project = new Project
            {
                ProjectId = 1,
            };
            var participantType = new ParticipantType
            {
                Name = ParticipantType.PublicInternationalOrganizationPio.Value,
                ParticipantTypeId = ParticipantType.PublicInternationalOrganizationPio.Id,
            };
            var organization = new Organization
            {
                OrganizationId = 1,
                Name = "org name"
            };

            var participant = new Participant
            {
                Organization = organization,
                OrganizationId = organization.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            participant.Projects.Add(project);
            project.Participants.Add(participant);
            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization);
            context.Participants.Add(participant);
            context.Projects.Add(project);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantResult = results.Results.First();

                Assert.IsFalse(participantResult.PersonId.HasValue);
                Assert.AreEqual(participantType.ParticipantTypeId, participantResult.ParticipantTypeId);
                Assert.AreEqual(participantType.Name, participantResult.ParticipantType);
                Assert.AreEqual(organization.Name, participantResult.Name);

            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipantsByProjectId(project.ProjectId, queryOperator);
            var serviceResultsAsync = await service.GetParticipantsByProjectIdAsync(project.ProjectId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);

        }

        [TestMethod]
        public async Task TestGetParticipantsByProjectId_ProjectDoesNotExist()
        {
            var participantType = new ParticipantType
            {
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 1,
                Gender = gender,
                GenderId = gender.GenderId,
            };

            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                Projects = new HashSet<Project>()
            };

            context.ParticipantTypes.Add(participantType);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Participants.Add(participant);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Total);
                Assert.AreEqual(0, results.Results.Count);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipantsByProjectId(-1, queryOperator);
            var serviceResultsAsync = await service.GetParticipantsByProjectIdAsync(-1, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);

        }

        [TestMethod]
        public async Task TestGetParticipantsByProjectId_DefaultSorter()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var participantType = new ParticipantType
            {
                Name = ParticipantType.PublicInternationalOrganizationPio.Value,
                ParticipantTypeId = ParticipantType.PublicInternationalOrganizationPio.Id,
            };
            var organization1 = new Organization
            {
                OrganizationId = 1,
                Name = "A"
            };

            var participant1 = new Participant
            {
                ParticipantId = 1,
                Organization = organization1,
                OrganizationId = organization1.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                Projects = new HashSet<Project> { project }
            };

            var organization2 = new Organization
            {
                OrganizationId = 2,
                Name = "B"
            };

            var participant2 = new Participant
            {
                ParticipantId = 2,
                Organization = organization2,
                OrganizationId = organization2.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                Projects = new HashSet<Project> { project }
            };

            project.Participants = new HashSet<Participant> { participant1, participant2 };
            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization1);
            context.Participants.Add(participant1);
            context.Organizations.Add(organization2);
            context.Participants.Add(participant2);
            context.Projects.Add(project);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantResult = results.Results.First();
                Assert.AreEqual(organization2.OrganizationId, participantResult.OrganizationId);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipantsByProjectId(project.ProjectId, queryOperator);
            var serviceResultsAsync = await service.GetParticipantsByProjectIdAsync(project.ProjectId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantsByProjectId_Filter()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var participantType = new ParticipantType
            {
                Name = ParticipantType.PublicInternationalOrganizationPio.Value,
                ParticipantTypeId = ParticipantType.PublicInternationalOrganizationPio.Id,
            };
            var organization1 = new Organization
            {
                OrganizationId = 1,
                Name = "A"
            };

            var participant1 = new Participant
            {
                ParticipantId = 1,
                Organization = organization1,
                OrganizationId = organization1.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                Projects = new HashSet<Project> { project }
            };

            var organization2 = new Organization
            {
                OrganizationId = 2,
                Name = "B"
            };

            var participant2 = new Participant
            {
                ParticipantId = 2,
                Organization = organization2,
                OrganizationId = organization2.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                Projects = new HashSet<Project> { project }
            };
            project.Participants = new HashSet<Participant> { participant1, participant2 };
            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization1);
            context.Participants.Add(participant1);
            context.Organizations.Add(organization2);
            context.Participants.Add(participant2);
            context.Projects.Add(project);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantResult = results.Results.First();
                Assert.AreEqual(organization2.OrganizationId, participantResult.OrganizationId);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<SimpleParticipantDTO>(x => x.Name, ComparisonType.Equal, organization2.Name));
            var serviceResults = service.GetParticipantsByProjectId(project.ProjectId, queryOperator);
            var serviceResultsAsync = await service.GetParticipantsByProjectIdAsync(project.ProjectId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantsByProjectId_Sort()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var participantType = new ParticipantType
            {
                Name = ParticipantType.PublicInternationalOrganizationPio.Value,
                ParticipantTypeId = ParticipantType.PublicInternationalOrganizationPio.Id,
            };
            var organization1 = new Organization
            {
                OrganizationId = 1,
                Name = "A"
            };

            var participant1 = new Participant
            {
                ParticipantId = 1,
                Organization = organization1,
                OrganizationId = organization1.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                Projects = new HashSet<Project> { project }
            };

            var organization2 = new Organization
            {
                OrganizationId = 2,
                Name = "B"
            };

            var participant2 = new Participant
            {
                ParticipantId = 2,
                Organization = organization2,
                OrganizationId = organization2.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                Projects = new HashSet<Project> { project }
            };
            project.Participants = new HashSet<Participant> { participant1, participant2 };
            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization1);
            context.Participants.Add(participant1);
            context.Organizations.Add(organization2);
            context.Participants.Add(participant2);
            context.Projects.Add(project);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantResult = results.Results.First();
                Assert.AreEqual(organization2.OrganizationId, participantResult.OrganizationId);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<SimpleParticipantDTO>(x => x.OrganizationId, SortDirection.Descending));
            var serviceResults = service.GetParticipantsByProjectId(project.ProjectId, queryOperator);
            var serviceResultsAsync = await service.GetParticipantsByProjectIdAsync(project.ProjectId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantsByProjectId_Paging()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var participantType = new ParticipantType
            {
                Name = ParticipantType.PublicInternationalOrganizationPio.Value,
                ParticipantTypeId = ParticipantType.PublicInternationalOrganizationPio.Id,
            };
            var organization1 = new Organization
            {
                OrganizationId = 1,
                Name = "A"
            };

            var participant1 = new Participant
            {
                ParticipantId = 1,
                Organization = organization1,
                OrganizationId = organization1.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                Projects = new HashSet<Project> { project }
            };

            var organization2 = new Organization
            {
                OrganizationId = 2,
                Name = "B"
            };

            var participant2 = new Participant
            {
                ParticipantId = 2,
                Organization = organization2,
                OrganizationId = organization2.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                Projects = new HashSet<Project> { project }
            };
            project.Participants = new HashSet<Participant> { participant1, participant2 };
            context.Projects.Add(project);
            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization1);
            context.Participants.Add(participant1);
            context.Organizations.Add(organization2);
            context.Participants.Add(participant2);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantResult = results.Results.First();
                Assert.AreEqual(organization2.OrganizationId, participantResult.OrganizationId);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipantsByProjectId(project.ProjectId, queryOperator);
            var serviceResultsAsync = await service.GetParticipantsByProjectIdAsync(project.ProjectId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Get Participant By Id
        [TestMethod]
        public async Task TestGetParticipantById()
        {
            var person = new Person
            {
                PersonId = 1,
                FirstName = "firstName",
                LastName = "lastName"
            };

            var history = new History
            {
                RevisedOn = DateTimeOffset.Now
            };
            
            var participantType = new ParticipantType
            {
                ParticipantTypeId = ParticipantType.Individual.Id,
                Name = "name"
            };

            var participant = new Participant
            {
                ParticipantId = 1,
                PersonId = person.PersonId,
                Person = person,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                SevisId = "1234567890",
                History = history
            };
            var status = new ParticipantStatus
            {
                Status = "status",
            };
            participant.Status = status;
            status.Participants.Add(participant);

            context.ParticipantStatuses.Add(status);
            context.People.Add(person);
            context.ParticipantTypes.Add(participantType);
            context.Participants.Add(participant);

            Action<ParticipantDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);
            };

            var serviceResult = service.GetParticipantById(participant.ParticipantId);
            var serviceResultAsync = await service.GetParticipantByIdAsync(participant.ParticipantId);

            tester(serviceResult);
            tester(serviceResultAsync);
        }
        #endregion
    }
}
