using ECA.Business.Queries.Models.Itineraries;
using FluentAssertions;
using ECA.Business.Service;
using ECA.Business.Service.Itineraries;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class ItineraryGroupServiceTest
    {
        private TestEcaContext context;
        private ItineraryGroupService service;
        private Mock<IBusinessValidator<AddedEcaItineraryGroupValidationEntity, object>> validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new Mock<IBusinessValidator<AddedEcaItineraryGroupValidationEntity, object>>();
            context = new TestEcaContext();
            service = new ItineraryGroupService(context, validator.Object);
        }

        #region GetItineraryGroupsByItineraryId

        [TestMethod]
        public async Task TestGetItineraryGroupsByItineraryId_ProjectDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group1 = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group1",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            var group2 = new ItineraryGroup
            {
                ItineraryGroupId = 4,
                Name = "group2",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);
            itinerary.ItineraryGroups.Add(group2);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);
            context.ItineraryGroups.Add(group2);

            Action<PagedQueryResults<ItineraryGroupDTO>> beforeTester = (results) =>
            {
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(2, results.Total);
            };

            Action<PagedQueryResults<ItineraryGroupDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Results.Count);
                Assert.AreEqual(0, results.Total);
            };

            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetItineraryGroupsByItineraryId(project.ProjectId, itinerary.ItineraryId, queryOperator);
            var serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(project.ProjectId, itinerary.ItineraryId, queryOperator);
            beforeTester(serviceResults);
            beforeTester(serviceResultsAsync);

            serviceResults = service.GetItineraryGroupsByItineraryId(0, itinerary.ItineraryId, queryOperator);
            serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(0, itinerary.ItineraryId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetItineraryGroupsByItineraryId_ItineraryDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group1 = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group1",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            var group2 = new ItineraryGroup
            {
                ItineraryGroupId = 4,
                Name = "group2",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);
            itinerary.ItineraryGroups.Add(group2);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);
            context.ItineraryGroups.Add(group2);

            Action<PagedQueryResults<ItineraryGroupDTO>> beforeTester = (results) =>
            {
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(2, results.Total);
            };

            Action<PagedQueryResults<ItineraryGroupDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Results.Count);
                Assert.AreEqual(0, results.Total);
            };

            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetItineraryGroupsByItineraryId(project.ProjectId, itinerary.ItineraryId, queryOperator);
            var serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(project.ProjectId, itinerary.ItineraryId, queryOperator);
            beforeTester(serviceResults);
            beforeTester(serviceResultsAsync);

            serviceResults = service.GetItineraryGroupsByItineraryId(project.ProjectId, 0, queryOperator);
            serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(project.ProjectId, 0, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetItineraryGroupsByItineraryId_Paged()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group1 = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group1",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            var group2 = new ItineraryGroup
            {
                ItineraryGroupId = 4,
                Name = "group2",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);
            itinerary.ItineraryGroups.Add(group2);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);
            context.ItineraryGroups.Add(group2);

            Action<PagedQueryResults<ItineraryGroupDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(group1.ItineraryGroupId, results.Results.First().ItineraryGroupId);
            };

            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetItineraryGroupsByItineraryId(project.ProjectId, itinerary.ItineraryId, queryOperator);
            var serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(project.ProjectId, itinerary.ItineraryId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetItineraryGroupsByItineraryId_Sorted()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group1 = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group1",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            var group2 = new ItineraryGroup
            {
                ItineraryGroupId = 4,
                Name = "group2",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);
            itinerary.ItineraryGroups.Add(group2);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);
            context.ItineraryGroups.Add(group2);

            Action<PagedQueryResults<ItineraryGroupDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Results.Count);
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(group1.ItineraryGroupId, results.Results.Last().ItineraryGroupId);
                Assert.AreEqual(group2.ItineraryGroupId, results.Results.First().ItineraryGroupId);
            };

            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var sorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(0, 10, defaultSorter, null, new List<ISorter> { sorter });
            var serviceResults = service.GetItineraryGroupsByItineraryId(project.ProjectId, itinerary.ItineraryId, queryOperator);
            var serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(project.ProjectId, itinerary.ItineraryId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetItineraryGroupsByItineraryId_Filtered()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group1 = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group1",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            var group2 = new ItineraryGroup
            {
                ItineraryGroupId = 4,
                Name = "group2",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);
            itinerary.ItineraryGroups.Add(group2);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);
            context.ItineraryGroups.Add(group2);

            Action<PagedQueryResults<ItineraryGroupDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(group1.ItineraryGroupId, results.Results.First().ItineraryGroupId);
            };

            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var filter = new ExpressionFilter<ItineraryGroupDTO>(x => x.ItineraryGroupName, ComparisonType.Equal, group1.Name);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(0, 10, defaultSorter, new List<IFilter> { filter }, null);
            var serviceResults = service.GetItineraryGroupsByItineraryId(project.ProjectId, itinerary.ItineraryId, queryOperator);
            var serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(project.ProjectId, itinerary.ItineraryId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        #endregion

        #region GetItineraryGroupPersonsByItineraryId
        [TestMethod]
        public async Task TestCreateGetItineraryGroupParticipantsQuery()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group);

            var participantType = new ParticipantType
            {
                ParticipantTypeId = ParticipantType.Individual.Id,
                IsPerson = true,
                Name = ParticipantType.Individual.Value
            };
            var participant = new Participant
            {
                ParticipantId = 4,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var person1 = new Person
            {
                PersonId = 5,
                FullName = "full name",
            };
            participant.ItineraryGroups.Add(group);
            group.Participants.Add(participant);
            person1.Participations.Add(participant);
            participant.Person = person1;
            participant.PersonId = person1.PersonId;
            person1.Participations.Add(participant);

            context.Participants.Add(participant);
            context.People.Add(person1);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            Action<List<ItineraryGroupParticipantsDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(group.ItineraryGroupId, firstResult.ItineraryGroupId);
                Assert.AreEqual(group.Name, firstResult.ItineraryGroupName);
                Assert.AreEqual(itinerary.ItineraryId, firstResult.ItineraryId);
                Assert.AreEqual(itinerary.Name, firstResult.ItineraryName);
                Assert.AreEqual(project.ProjectId, firstResult.ProjectId);

                Assert.AreEqual(1, firstResult.People.Count());
                var firstPerson = firstResult.People.First();
                Assert.AreEqual(person1.PersonId, firstPerson.PersonId);
                Assert.AreEqual(person1.FullName, firstPerson.FullName);
                Assert.AreEqual(participant.ParticipantId, firstPerson.ParticipantId);
                Assert.AreEqual(participantType.ParticipantTypeId, firstPerson.ParticipantTypeId);
            };

            var serviceResults = service.GetItineraryGroupPersonsByItineraryId(project.ProjectId, itinerary.ItineraryId);
            var serviceResultsAsync = await service.GetItineraryGroupPersonsByItineraryIdAsync(project.ProjectId, itinerary.ItineraryId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCreateGetItineraryGroupParticipantsQuery_DifferentProject()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group);

            var participantType = new ParticipantType
            {
                ParticipantTypeId = ParticipantType.Individual.Id,
                IsPerson = true,
                Name = ParticipantType.Individual.Value
            };
            var participant = new Participant
            {
                ParticipantId = 4,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var person1 = new Person
            {
                PersonId = 5,
                FullName = "full name",
            };
            participant.ItineraryGroups.Add(group);
            group.Participants.Add(participant);
            person1.Participations.Add(participant);
            participant.Person = person1;
            participant.PersonId = person1.PersonId;
            person1.Participations.Add(participant);

            context.Participants.Add(participant);
            context.People.Add(person1);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            Action<List<ItineraryGroupParticipantsDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };

            var serviceResults = service.GetItineraryGroupPersonsByItineraryId(0, itinerary.ItineraryId);
            var serviceResultsAsync = await service.GetItineraryGroupPersonsByItineraryIdAsync(0, itinerary.ItineraryId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCreateGetItineraryGroupParticipantsQuery_ItineraryDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group);

            var participantType = new ParticipantType
            {
                ParticipantTypeId = ParticipantType.Individual.Id,
                IsPerson = true,
                Name = ParticipantType.Individual.Value
            };
            var participant = new Participant
            {
                ParticipantId = 4,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var person1 = new Person
            {
                PersonId = 5,
                FullName = "full name",
            };
            participant.ItineraryGroups.Add(group);
            group.Participants.Add(participant);
            person1.Participations.Add(participant);
            participant.Person = person1;
            participant.PersonId = person1.PersonId;
            person1.Participations.Add(participant);

            context.Participants.Add(participant);
            context.People.Add(person1);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            Action<List<ItineraryGroupParticipantsDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };

            var serviceResults = service.GetItineraryGroupPersonsByItineraryId(project.ProjectId, 0);
            var serviceResultsAsync = await service.GetItineraryGroupPersonsByItineraryIdAsync(project.ProjectId, 0);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region GetItineraryGroupById
        [TestMethod]
        public async Task TestCGetItineraryGroupById()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group);

            var participantType = new ParticipantType
            {
                ParticipantTypeId = ParticipantType.Individual.Id,
                IsPerson = true,
                Name = ParticipantType.Individual.Value
            };
            var participant = new Participant
            {
                ParticipantId = 4,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var person1 = new Person
            {
                PersonId = 5,
                FullName = "full name",
            };
            participant.ItineraryGroups.Add(group);
            group.Participants.Add(participant);
            person1.Participations.Add(participant);
            participant.Person = person1;
            participant.PersonId = person1.PersonId;
            person1.Participations.Add(participant);

            context.Participants.Add(participant);
            context.People.Add(person1);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            Action<ItineraryGroupParticipantsDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);
                Assert.AreEqual(group.ItineraryGroupId, result.ItineraryGroupId);
                Assert.AreEqual(group.Name, result.ItineraryGroupName);
                Assert.AreEqual(itinerary.ItineraryId, result.ItineraryId);
                Assert.AreEqual(itinerary.Name, result.ItineraryName);
                Assert.AreEqual(project.ProjectId, result.ProjectId);

                Assert.AreEqual(1, result.People.Count());
                var firstPerson = result.People.First();
                Assert.AreEqual(person1.PersonId, firstPerson.PersonId);
                Assert.AreEqual(person1.FullName, firstPerson.FullName);
                Assert.AreEqual(participant.ParticipantId, firstPerson.ParticipantId);
                Assert.AreEqual(participantType.ParticipantTypeId, firstPerson.ParticipantTypeId);
            };

            var serviceResults = service.GetItineraryGroupById(project.ProjectId, group.ItineraryGroupId);
            var serviceResultsAsync = await service.GetItineraryGroupByIdAsync(project.ProjectId, group.ItineraryGroupId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCGetItineraryGroupById_ItineraryGroupDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group);

            var participantType = new ParticipantType
            {
                ParticipantTypeId = ParticipantType.Individual.Id,
                IsPerson = true,
                Name = ParticipantType.Individual.Value
            };
            var participant = new Participant
            {
                ParticipantId = 4,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var person1 = new Person
            {
                PersonId = 5,
                FullName = "full name",
            };
            participant.ItineraryGroups.Add(group);
            group.Participants.Add(participant);
            person1.Participations.Add(participant);
            participant.Person = person1;
            participant.PersonId = person1.PersonId;
            person1.Participations.Add(participant);

            context.Participants.Add(participant);
            context.People.Add(person1);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            Action<ItineraryGroupParticipantsDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);
            };

            var serviceResults = service.GetItineraryGroupById(project.ProjectId, group.ItineraryGroupId);
            var serviceResultsAsync = await service.GetItineraryGroupByIdAsync(project.ProjectId, group.ItineraryGroupId);

            tester(serviceResults);
            tester(serviceResultsAsync);

            Assert.IsNull(service.GetItineraryGroupById(project.ProjectId, group.ItineraryGroupId + 1));
            Assert.IsNull(await service.GetItineraryGroupByIdAsync(project.ProjectId, group.ItineraryGroupId + 1));
        }

        [TestMethod]
        public async Task TestCGetItineraryGroupById_DifferentProjectId()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group);

            var participantType = new ParticipantType
            {
                ParticipantTypeId = ParticipantType.Individual.Id,
                IsPerson = true,
                Name = ParticipantType.Individual.Value
            };
            var participant = new Participant
            {
                ParticipantId = 4,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var person1 = new Person
            {
                PersonId = 5,
                FullName = "full name",
            };
            participant.ItineraryGroups.Add(group);
            group.Participants.Add(participant);
            person1.Participations.Add(participant);
            participant.Person = person1;
            participant.PersonId = person1.PersonId;
            person1.Participations.Add(participant);

            context.Participants.Add(participant);
            context.People.Add(person1);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            Action<ItineraryGroupParticipantsDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);
            };

            var serviceResults = service.GetItineraryGroupById(project.ProjectId, group.ItineraryGroupId);
            var serviceResultsAsync = await service.GetItineraryGroupByIdAsync(project.ProjectId, group.ItineraryGroupId);

            tester(serviceResults);
            tester(serviceResultsAsync);

            Assert.IsNull(service.GetItineraryGroupById(project.ProjectId + 1, group.ItineraryGroupId));
            Assert.IsNull(await service.GetItineraryGroupByIdAsync(project.ProjectId + 1, group.ItineraryGroupId));
        }
        #endregion

        #region Create
        [TestMethod]
        public async Task TestCreate_CheckProperties()
        {
            var userId = 1;
            var creator = new User(userId);
            var projectId = 2;
            var participantId = 3;
            var itineraryId = 4;
            Project project = null;
            Itinerary itinerary = null;
            ParticipantType participantType = null;
            Participant participant = null;
            context.SetupActions.Add(() =>
            {
                participantType = new ParticipantType
                {
                    IsPerson = true,
                    Name = ParticipantType.Individual.Value,
                    ParticipantTypeId = ParticipantType.Individual.Id
                };
                participant = new Participant
                {
                    ParticipantId = participantId,
                    ParticipantType = participantType,
                    ParticipantTypeId = participantType.ParticipantTypeId
                };
                project = new Project
                {
                    ProjectId = projectId,
                };
                itinerary = new Itinerary
                {
                    ItineraryId = itineraryId,
                    Name = "itinerary"
                };
                itinerary.ProjectId = project.ProjectId;
                itinerary.Project = project;
                project.Itineraries.Add(itinerary);
                context.Participants.Add(participant);
                context.ParticipantTypes.Add(participantType);
                context.Projects.Add(project);
                context.Itineraries.Add(itinerary);
            });

            var addedItineraryGroup = new AddedEcaItineraryGroup(creator, projectId, itineraryId, "group name", new List<int> { participantId });
            Action<AddedEcaItineraryGroupValidationEntity> validationEntityTester = (entity) =>
            {
                Assert.AreEqual(1, entity.Participants.Count());
                Assert.AreEqual(participantId, entity.Participants.Select(x => x.ParticipantId).First());
                Assert.IsNotNull(entity.ExistingItineraryGroups);
                Assert.AreEqual(0, entity.ExistingItineraryGroups.Count());
            };
            Action tester = () =>
            {
                Assert.AreEqual(1, context.ItineraryGroups.Count());
                var firstGroup = context.ItineraryGroups.First();
                Assert.AreEqual(addedItineraryGroup.Name, firstGroup.Name);
                Assert.AreEqual(itineraryId, firstGroup.ItineraryId);

                Assert.AreEqual(1, firstGroup.Participants.Count());
                Assert.AreEqual(participantId, firstGroup.Participants.First().ParticipantId);

                Assert.AreEqual(userId, firstGroup.History.CreatedBy);
                Assert.AreEqual(userId, firstGroup.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(firstGroup.History.CreatedOn, 20000);
                DateTimeOffset.UtcNow.Should().BeCloseTo(firstGroup.History.RevisedOn, 20000);
            };
            validator.Setup(x => x.ValidateCreate(It.IsAny<AddedEcaItineraryGroupValidationEntity>())).Callback(validationEntityTester);
            context.Revert();
            service.Create(addedItineraryGroup);
            tester();

            context.Revert();
            await service.CreateAsync(addedItineraryGroup);
            tester();
            validator.Verify(x => x.ValidateCreate(It.IsAny<AddedEcaItineraryGroupValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestCreate_ItineraryDoesNotExist()
        {
            var userId = 1;
            var creator = new User(userId);
            var projectId = 2;
            var participantId = 3;
            var itineraryId = 4;
            var participantType = new ParticipantType
            {
                IsPerson = true,
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            var participant = new Participant
            {
                ParticipantId = participantId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            var project = new Project
            {
                ProjectId = projectId,
            };
            var itinerary = new Itinerary
            {
                ItineraryId = itineraryId,
                Name = "itinerary"
            };
            itinerary.ProjectId = project.ProjectId;
            itinerary.Project = project;
            project.Itineraries.Add(itinerary);
            context.Participants.Add(participant);
            context.ParticipantTypes.Add(participantType);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);

            var addedItineraryGroup = new AddedEcaItineraryGroup(creator, projectId, itineraryId + 1, "group name", new List<int> { participantId });
            var message = String.Format("The [{0}] with id [{1}] does not exist.", typeof(Itinerary).Name, addedItineraryGroup.ItineraryId);
            Action a = () => service.Create(addedItineraryGroup);
            Func<Task> f = () => service.CreateAsync(addedItineraryGroup);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestCreate_ProjectSpecifiedIsNotItineraryProject()
        {
            var userId = 1;
            var creator = new User(userId);
            var projectId = 2;
            var participantId = 3;
            var itineraryId = 4;
            var participantType = new ParticipantType
            {
                IsPerson = true,
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            var participant = new Participant
            {
                ParticipantId = participantId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            var project = new Project
            {
                ProjectId = projectId,
            };
            var itinerary = new Itinerary
            {
                ItineraryId = itineraryId,
                Name = "itinerary"
            };
            itinerary.ProjectId = project.ProjectId;
            itinerary.Project = project;
            project.Itineraries.Add(itinerary);
            context.Participants.Add(participant);
            context.ParticipantTypes.Add(participantType);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);

            var addedItineraryGroup = new AddedEcaItineraryGroup(creator, projectId + 1, itineraryId, "group name", new List<int> { participantId });
            var message = String.Format("The user with id [{0}] attempted to edit an itinerary on a project with id [{1}] but should have been denied access.",
                        userId,
                        addedItineraryGroup.ProjectId);
            Action a = () => service.Create(addedItineraryGroup);
            Func<Task> f = () => service.CreateAsync(addedItineraryGroup);
            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }
        #endregion

        [TestMethod]
        public void TestSetParticipants_IsDetached()
        {
            var contextMock = new Mock<TestEcaContext>();
            var state = System.Data.Entity.EntityState.Detached;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Participant>(It.IsAny<Participant>())).Returns(() =>
            {
                return state;
            });
            var original = new Participant { ParticipantId = 1 };

            var group = new ItineraryGroup();
            group.Participants.Add(original);

            var newParticipant = new Participant { ParticipantId = 2 };
            var newParticipantIds = new List<int> { newParticipant.ParticipantId };

            var testService = new ItineraryGroupService(contextMock.Object, validator.Object);
            testService.SetParticipants(newParticipantIds, group);
            Assert.AreEqual(1, group.Participants.Count);
            Assert.AreEqual(newParticipant.ParticipantId, group.Participants.First().ParticipantId);
        }

        [TestMethod]
        public void TestSetParticipants_IsAdded()
        {
            var contextMock = new Mock<TestEcaContext>();
            var state = System.Data.Entity.EntityState.Detached;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Participant>(It.IsAny<Participant>())).Returns(() =>
            {
                return state;
            });
            var original = new Participant { ParticipantId = 1 };

            var group = new ItineraryGroup();
            group.Participants.Add(original);

            var newParticipant = new Participant { ParticipantId = 2 };
            var newParticipantIds = new List<int> { newParticipant.ParticipantId };
            var testService = new ItineraryGroupService(contextMock.Object, validator.Object);
            testService.SetParticipants(newParticipantIds, group);
            Assert.AreEqual(1, group.Participants.Count);
            Assert.AreEqual(newParticipant.ParticipantId, group.Participants.First().ParticipantId);
        }

        [TestMethod]
        public void TestSetParticipants_Local()
        {
            var contextMock = new Mock<TestEcaContext>();
            var state = System.Data.Entity.EntityState.Detached;
            var original = new Participant { ParticipantId = 1 };

            var group = new ItineraryGroup();
            group.Participants.Add(original);

            var newParticipant = new Participant { ParticipantId = 2 };
            var newParticipantIds = new List<int> { newParticipant.ParticipantId };
            Action<Func<Participant, bool>> callbackTester = (f) =>
            {
                var testParticipants = new List<Participant> { newParticipant };

                Assert.IsTrue(Object.ReferenceEquals(newParticipant, testParticipants.Where(f).First()));
                testParticipants.Clear();
                testParticipants.Add(new Participant
                {
                    ParticipantId = newParticipant.ParticipantId - 1
                });
                Assert.AreEqual(0, testParticipants.Where(f).Count());
            };

            contextMock.Setup(x => x.GetLocalEntity<Participant>(It.IsAny<Func<Participant, bool>>())).Callback(callbackTester)
            .Returns(newParticipant);
            var testService = new ItineraryGroupService(contextMock.Object, validator.Object);
            testService.SetParticipants(newParticipantIds, group);
            Assert.AreEqual(1, group.Participants.Count);
            Assert.AreEqual(newParticipant.ParticipantId, group.Participants.First().ParticipantId);
        }
    }
}
