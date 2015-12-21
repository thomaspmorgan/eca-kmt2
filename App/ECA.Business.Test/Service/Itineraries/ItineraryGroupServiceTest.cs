using ECA.Business.Queries.Models.Itineraries;
using ECA.Business.Service.Itineraries;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class ItineraryGroupServiceTest
    {
        private TestEcaContext context;
        private ItineraryGroupService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ItineraryGroupService(context);
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
    }
}
