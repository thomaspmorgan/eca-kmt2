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
using ECA.Business.Service;
using ECA.Core.Exceptions;
using FluentAssertions;

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
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
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
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
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
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
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
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
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
            var revisedDate = DateTimeOffset.UtcNow;
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
                FullName = "full name"
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
            var status = new ParticipantStatus
            {
                Status = ParticipantStatus.Active.Value,
                ParticipantStatusId = ParticipantStatus.Active.Id
            };
            address.Person = person;
            address.PersonId = person.PersonId;
            person.Addresses.Add(address);
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                Project = project,
                ProjectId = project.ProjectId,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId
            };
            participant.History.RevisedOn = revisedDate;
            project.Participants.Add(participant);

            context.Addresses.Add(address);
            context.Locations.Add(address.Location);
            context.Locations.Add(address.Location.City);
            context.Locations.Add(address.Location.Country);
            context.ParticipantTypes.Add(participantType);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantStatuses.Add(status);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantResult = results.Results.First();
                Assert.AreEqual(address.Location.City.LocationName, participantResult.City);
                Assert.AreEqual(address.Location.Country.LocationName, participantResult.Country);
                Assert.AreEqual(person.FullName, participantResult.Name);
                Assert.IsFalse(participantResult.OrganizationId.HasValue);
                Assert.AreEqual(participant.ParticipantId, participantResult.ParticipantId);
                Assert.AreEqual(participantType.Name, participantResult.ParticipantType);
                Assert.AreEqual(participantType.ParticipantTypeId, participantResult.ParticipantTypeId);
                Assert.AreEqual(person.PersonId, participantResult.PersonId);
                Assert.AreEqual(project.ProjectId, participantResult.ProjectId);
                Assert.AreEqual(revisedDate, participantResult.RevisedOn);
                Assert.AreEqual(status.ParticipantStatusId, participantResult.StatusId);
                Assert.AreEqual(status.Status, participantResult.ParticipantStatus);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipantsByProjectId(project.ProjectId, queryOperator);
            var serviceResultsAsync = await service.GetParticipantsByProjectIdAsync(project.ProjectId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);

        }

        [TestMethod]
        public async Task TestGetParticipantsByProjectId_ParticipantIsPerson_NullParticipantStatus_CheckProperties()
        {
            var revisedDate = DateTimeOffset.UtcNow;
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
                FullName = "full name"
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
            address.Person = person;
            address.PersonId = person.PersonId;
            person.Addresses.Add(address);
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                Project = project,
                ProjectId = project.ProjectId,
            };
            participant.History.RevisedOn = revisedDate;
            project.Participants.Add(participant);

            context.Addresses.Add(address);
            context.Locations.Add(address.Location);
            context.Locations.Add(address.Location.City);
            context.Locations.Add(address.Location.Country);
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
                Assert.IsNull(participantResult.StatusId);
                Assert.IsNull(participantResult.ParticipantStatus);
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
            var revisedDate = DateTimeOffset.UtcNow;
            var project = new Project
            {
                ProjectId = 1,
            };
            var participantType = new ParticipantType
            {
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
            };
            var organization = new Organization
            {
                OrganizationId = 1,
                Name = "org name",
            };
            var status = new ParticipantStatus
            {
                Status = ParticipantStatus.Active.Value,
                ParticipantStatusId = ParticipantStatus.Active.Id
            };
            var participant = new Participant
            {
                Organization = organization,
                OrganizationId = organization.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                ProjectId = project.ProjectId,
                Project = project,
                ParticipantStatusId = status.ParticipantStatusId,
                Status = status,
            };
            participant.History.RevisedOn = revisedDate;
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
            address.Organization = organization;
            address.OrganizationId = organization.OrganizationId;
            organization.Addresses.Add(address);
            project.Participants.Add(participant);
            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantStatuses.Add(status);

            Action<PagedQueryResults<SimpleParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantResult = results.Results.First();

                Assert.IsFalse(participantResult.PersonId.HasValue);
                Assert.AreEqual(participantType.ParticipantTypeId, participantResult.ParticipantTypeId);
                Assert.AreEqual(participantType.Name, participantResult.ParticipantType);
                Assert.AreEqual(organization.Name, participantResult.Name);
                Assert.AreEqual(address.Location.City.LocationName, participantResult.City);
                Assert.AreEqual(address.Location.Country.LocationName, participantResult.Country);
                Assert.AreEqual(project.ProjectId, participantResult.ProjectId);
                Assert.AreEqual(revisedDate, participantResult.RevisedOn);
                Assert.AreEqual(status.ParticipantStatusId, participantResult.StatusId);
                Assert.AreEqual(status.Status, participantResult.ParticipantStatus);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipantsByProjectId(project.ProjectId, queryOperator);
            var serviceResultsAsync = await service.GetParticipantsByProjectIdAsync(project.ProjectId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);

        }

        [TestMethod]
        public async Task TestGetParticipantsByProjectId_ParticipantIsOrganization_NullParticipantStatus_CheckProperties()
        {
            var revisedDate = DateTimeOffset.UtcNow;
            var project = new Project
            {
                ProjectId = 1,
            };
            var participantType = new ParticipantType
            {
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
            };
            var organization = new Organization
            {
                OrganizationId = 1,
                Name = "org name",
            };
            var participant = new Participant
            {
                Organization = organization,
                OrganizationId = organization.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                ProjectId = project.ProjectId,
                Project = project,
            };
            participant.History.RevisedOn = revisedDate;
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
            address.Organization = organization;
            address.OrganizationId = organization.OrganizationId;
            organization.Addresses.Add(address);
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
                Assert.IsNull(participantResult.StatusId);
                Assert.IsNull(participantResult.ParticipantStatus);
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
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
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
                Project = project,
                ProjectId = project.ProjectId
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
                Project = project,
                ProjectId = project.ProjectId
            };

            project.Participants.Add(participant1);
            project.Participants.Add(participant2);
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
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
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
                Project = project,
                ProjectId = project.ProjectId
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
                Project = project,
                ProjectId = project.ProjectId
            };
            project.Participants.Add(participant1);
            project.Participants.Add(participant2);
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
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
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
                Project = project,
                ProjectId = project.ProjectId
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
                Project = project,
                ProjectId = project.ProjectId
            };
            project.Participants.Add(participant1);
            project.Participants.Add(participant2);
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
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
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
                Project = project,
                ProjectId = project.ProjectId
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
                Project = project,
                ProjectId = project.ProjectId
            };
            project.Participants.Add(participant1);
            project.Participants.Add(participant2);
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
        public async Task TestGetParticipantById_CheckProperties()
        {
            var person = new Person
            {
                PersonId = 1,
                FullName = "full name"
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
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                PersonId = person.PersonId,
                Person = person,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                History = history,
                Project = project,
                ProjectId = project.ProjectId,
                StatusDate = DateTimeOffset.UtcNow
            };
            project.Participants.Add(participant);
            var status = new ParticipantStatus
            {
                Status = "status",
                ParticipantStatusId = 1,
            };
            participant.Status = status;
            participant.ParticipantStatusId = status.ParticipantStatusId;
            status.Participants.Add(participant);

            context.ParticipantStatuses.Add(status);
            context.Projects.Add(project);
            context.ParticipantStatuses.Add(status);
            context.People.Add(person);
            context.ParticipantTypes.Add(participantType);
            context.Participants.Add(participant);

            Action<ParticipantDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);
                Assert.IsNull(result.Country);
                Assert.IsNull(result.City);
                Assert.AreEqual(person.FullName, result.Name);
                Assert.IsFalse(result.OrganizationId.HasValue);
                Assert.AreEqual(participant.ParticipantId, result.ParticipantId);
                Assert.AreEqual(participantType.Name, result.ParticipantType);
                Assert.AreEqual(participantType.ParticipantTypeId, result.ParticipantTypeId);
                Assert.AreEqual(person.PersonId, result.PersonId);
                Assert.AreEqual(project.ProjectId, result.ProjectId);
                Assert.AreEqual(history.RevisedOn, result.RevisedOn);
                Assert.AreEqual(status.Status, result.ParticipantStatus);
                Assert.AreEqual(status.ParticipantStatusId, result.StatusId);
                Assert.AreEqual(participant.StatusDate, result.StatusDate);
            };

            var serviceResult = service.GetParticipantById(participant.ParticipantId);
            var serviceResultAsync = await service.GetParticipantByIdAsync(participant.ParticipantId);

            tester(serviceResult);
            tester(serviceResultAsync);
        }
        #endregion

        #region Delete
        [TestMethod]
        public async Task TestDelete_ParticipantDoesNotExist()
        {
            var participantId = 1;
            var deletorId = 2;
            var projectId = 3;
            Assert.AreEqual(0, context.Participants.Count());

            var deletedParticipant = new DeletedParticipant(new User(deletorId), projectId, participantId);
            var message = String.Format("The model type [{0}] with Id [{1}] was not found.", typeof(Participant).Name, participantId);

            Action a = () => service.Delete(deletedParticipant);
            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedParticipant);
            };
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_ProjectDoesNotExist()
        {
            var participantId = 1;
            var deletorId = 2;
            var projectId = 3;
            var participant = new Participant
            {
                ParticipantId = participantId,
                ProjectId = projectId
            };
            context.Participants.Add(participant);
            Assert.AreEqual(1, context.Participants.Count());
            Assert.AreEqual(0, context.Projects.Count());

            var deletedParticipant = new DeletedParticipant(new User(deletorId), projectId, participantId);
            var message = String.Format("The model type [{0}] with Id [{1}] was not found.", typeof(Project).Name, projectId);

            Action a = () => service.Delete(deletedParticipant);
            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedParticipant);
            };
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_ParticipantDoesNotBelongToProject()
        {
            var participantId = 1;
            var deletorId = 2;
            var projectId = 3;
            var project = new Project
            {
                ProjectId = projectId + 1,
            };
            var participant = new Participant
            {
                ParticipantId = participantId,
                ProjectId = project.ProjectId,
                Project = project,
            };
            context.Projects.Add(project);
            context.Participants.Add(participant);
            Assert.AreEqual(1, context.Participants.Count());
            Assert.AreEqual(1, context.Projects.Count());

            var deletedParticipant = new DeletedParticipant(new User(deletorId), projectId, participantId);
            var message = String.Format("The user with id [{0}] attempted to delete a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        deletorId,
                        participant.ParticipantId,
                        projectId);

            Action a = () => service.Delete(deletedParticipant);
            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedParticipant);
            };
            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_CheckProperties()
        {
            var participantId = 1;
            var deletorId = 2;
            var projectId = 3;
            Project project = null;
            Participant participant = null;
            var user = new User(deletorId);
            var creatorId = 1;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            context.SetupActions.Add(() =>
            {
                project = new Project
                {
                    ProjectId = projectId
                };
                project.History.CreatedBy = creatorId;
                project.History.RevisedBy = creatorId;
                project.History.CreatedOn = yesterday;
                project.History.RevisedOn = yesterday;
                participant = new Participant
                {
                    ParticipantId = participantId,
                    ProjectId = project.ProjectId,
                    Project = project
                };

                context.Participants.Add(participant);
                context.Projects.Add(project);
                Assert.AreEqual(1, context.Participants.Count());
                Assert.AreEqual(1, context.Projects.Count());
            });
            Action tester = () =>
            {
                Assert.AreEqual(1, context.Projects.Count());
                Assert.AreEqual(0, context.Participants.Count());

                Assert.AreEqual(creatorId, project.History.CreatedBy);
                Assert.AreEqual(yesterday, project.History.CreatedOn);
                Assert.AreEqual(deletorId, project.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(project.History.RevisedOn, 20000);
            };
            var deletedParticipant = new DeletedParticipant(user, projectId, participantId);
            context.Revert();
            service.Delete(deletedParticipant);
            tester();

            context.Revert();
            await service.DeleteAsync(deletedParticipant);
            tester();
        }

        [TestMethod]
        public async Task TestDelete_ShouldDeleteRelatedEntities()
        {
            var participantId = 1;
            var deletorId = 2;
            var projectId = 3;
            Project project = null;
            Participant participant = null;
            ParticipantStudentVisitor studentVisitor = null;
            ParticipantPersonSevisCommStatus status = null;
            ParticipantPerson person = null;
            var user = new User(deletorId);
            var creatorId = 1;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            context.SetupActions.Add(() =>
            {
                project = new Project
                {
                    ProjectId = projectId
                };
                project.History.CreatedBy = creatorId;
                project.History.RevisedBy = creatorId;
                project.History.CreatedOn = yesterday;
                project.History.RevisedOn = yesterday;
                participant = new Participant
                {
                    ParticipantId = participantId,
                    ProjectId = project.ProjectId,
                    Project = project
                };
                studentVisitor = new ParticipantStudentVisitor
                {
                    Participant = participant,
                    ParticipantId = participant.ParticipantId
                };
                status = new ParticipantPersonSevisCommStatus
                {
                    ParticipantId = participant.ParticipantId,
                    Id = 1,
                };
                person = new ParticipantPerson
                {
                    Participant = participant,
                    ParticipantId = participant.ParticipantId
                };
                context.ParticipantStudentVisitors.Add(studentVisitor);
                context.ParticipantPersonSevisCommStatuses.Add(status);
                context.ParticipantPersons.Add(person);
                context.Participants.Add(participant);
                context.Projects.Add(project);
            });
            Action tester = () =>
            {
                Assert.AreEqual(1, context.Projects.Count());
                Assert.AreEqual(0, context.Participants.Count());
                Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.AreEqual(0, context.ParticipantPersons.Count());
                Assert.AreEqual(0, context.ParticipantStudentVisitors.Count());
            };
            var deletedParticipant = new DeletedParticipant(user, projectId, participantId);
            context.Revert();
            service.Delete(deletedParticipant);
            tester();

            context.Revert();
            await service.DeleteAsync(deletedParticipant);
            tester();
        }
        #endregion
    }
}
