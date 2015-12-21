using ECA.Business.Queries.Itineraries;
using ECA.Business.Queries.Models.Itineraries;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Business.Test.Queries.Itineraries
{
    [TestClass]
    public class ItineraryGroupQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }
        #region Itinerary Groups
        [TestMethod]
        public void TestCreateGetItineraryGroupDTOQuery()
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


            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group);

            Action<List<ItineraryGroupDTO>> tester = (list) =>
            {
                Assert.AreEqual(1, list.Count);
                var firstResult = list.First();
                Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
                Assert.AreEqual(itinerary.ItineraryId, firstResult.ItineraryId);
                Assert.AreEqual(itinerary.Name, firstResult.ItineraryName);
                Assert.AreEqual(group.ItineraryGroupId, firstResult.ItineraryGroupId);
                Assert.AreEqual(group.Name, firstResult.ItineraryGroupName);
            };
            var results = ItineraryGroupQueries.CreateGetItineraryGroupDTOQuery(context).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetItineraryGroupDTOByItineraryIdQuery()
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
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);

            Action<List<ItineraryGroupDTO>> tester = (list) =>
            {
                Assert.AreEqual(1, list.Count);
                var firstResult = list.First();
                Assert.AreEqual(group1.ItineraryGroupId, firstResult.ItineraryGroupId);
            };
            var start = 0;
            var limit = 1;
            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(start, limit, defaultSorter);
            var results = ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(context, project.ProjectId, itinerary.ItineraryId, queryOperator).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetItineraryGroupDTOByItineraryIdQuery_ItineraryDoesNotExist()
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
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);

            Action<List<ItineraryGroupDTO>> tester = (list) =>
            {
                Assert.AreEqual(0, list.Count);
            };
            var start = 0;
            var limit = 1;
            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(start, limit, defaultSorter);
            var results = ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(context, project.ProjectId, itinerary.ItineraryId, queryOperator).ToList();
            Assert.AreEqual(1, results.Count);

            results = ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(context, project.ProjectId, 0, queryOperator).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestCreateGetItineraryGroupDTOByItineraryIdQuery_ProjectDoesNotExist()
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
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);

            var start = 0;
            var limit = 1;
            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(start, limit, defaultSorter);
            var results = ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(context, project.ProjectId, itinerary.ItineraryId, queryOperator).ToList();
            Assert.AreEqual(1, results.Count);

            results = ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(context, 0, itinerary.ItineraryId, queryOperator).ToList();
            Assert.AreEqual(0, results.Count);
        }
        #endregion

        #region Itinerary Group Participants
        [TestMethod]
        public void TestCreateGetItineraryGroupParticipantsQuery_CheckProperties()
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

            var results = ItineraryGroupQueries.CreateGetItineraryGroupParticipantsQuery(context).ToList();
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
        }

        [TestMethod]
        public void TestCreateGetItineraryGroupParticipantsQuery_MultiplePeople()
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
            var participant1 = new Participant
            {
                ParticipantId = 4,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var participant2 = new Participant
            {
                ParticipantId = 5,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var person1 = new Person
            {
                PersonId = 6,
                FullName = "full name",
            };
            var person2 = new Person
            {
                PersonId = 7,
                FullName = "full name2",
            };
            participant1.ItineraryGroups.Add(group);
            group.Participants.Add(participant1);
            person1.Participations.Add(participant1);
            participant1.Person = person1;
            participant1.PersonId = person1.PersonId;
            person1.Participations.Add(participant1);

            participant2.ItineraryGroups.Add(group);
            group.Participants.Add(participant2);
            person2.Participations.Add(participant2);
            participant2.Person = person2;
            participant2.PersonId = person2.PersonId;
            person2.Participations.Add(participant2);

            context.Participants.Add(participant1);
            context.Participants.Add(participant2);
            context.People.Add(person1);
            context.People.Add(person2);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            var results = ItineraryGroupQueries.CreateGetItineraryGroupParticipantsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var firstResult = results.First();

            Assert.AreEqual(2, firstResult.People.Count());
            var firstPerson = firstResult.People.First();
            Assert.AreEqual(person1.PersonId, firstPerson.PersonId);
            Assert.AreEqual(person1.FullName, firstPerson.FullName);
            Assert.AreEqual(participant1.ParticipantId, firstPerson.ParticipantId);
            Assert.AreEqual(participantType.ParticipantTypeId, firstPerson.ParticipantTypeId);

            var lastPerson = firstResult.People.Last();
            Assert.AreEqual(person2.PersonId, lastPerson.PersonId);
            Assert.AreEqual(person2.FullName, lastPerson.FullName);
            Assert.AreEqual(participant2.ParticipantId, lastPerson.ParticipantId);
            Assert.AreEqual(participantType.ParticipantTypeId, lastPerson.ParticipantTypeId);
        }

        [TestMethod]
        public void TestCreateGetItineraryGroupParticipantsQuery_ProjectDoesNotExist()
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

            var results = ItineraryGroupQueries.CreateGetItineraryGroupParticipantsQuery(context, project.ProjectId, itinerary.ItineraryId).ToList();
            Assert.AreEqual(1, results.Count);

            results = ItineraryGroupQueries.CreateGetItineraryGroupParticipantsQuery(context, 0, itinerary.ItineraryId).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestCreateGetItineraryGroupParticipantsQuery_ItineraryDoesNotExist()
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

            var results = ItineraryGroupQueries.CreateGetItineraryGroupParticipantsQuery(context, project.ProjectId, itinerary.ItineraryId).ToList();
            Assert.AreEqual(1, results.Count);

            results = ItineraryGroupQueries.CreateGetItineraryGroupParticipantsQuery(context, project.ProjectId, 0).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestCreateGetItineraryGroupParticipantsQuery_ParticipantIsOrganization()
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
            var org = new Organization
            {
                OrganizationId = 5,
                Name = "name",
            };
            participant.ItineraryGroups.Add(group);
            group.Participants.Add(participant);
            participant.Organization = org;
            participant.OrganizationId = org.OrganizationId;

            context.Participants.Add(participant);
            context.Organizations.Add(org);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            var results = ItineraryGroupQueries.CreateGetItineraryGroupParticipantsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var firstResult = results.First();
            Assert.AreEqual(group.ItineraryGroupId, firstResult.ItineraryGroupId);
            Assert.AreEqual(group.Name, firstResult.ItineraryGroupName);
            Assert.AreEqual(itinerary.ItineraryId, firstResult.ItineraryId);
            Assert.AreEqual(itinerary.Name, firstResult.ItineraryName);
            Assert.AreEqual(project.ProjectId, firstResult.ProjectId);

            Assert.AreEqual(0, firstResult.People.Count());
        }

        #endregion

        #region
        [TestMethod]
        public void TestCreateGetEqualItineraryGroupsQuery_CheckProperties()
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
            var participant1 = new Participant
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
            var participant2 = new Participant
            {
                ParticipantId = 6,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var person2 = new Person
            {
                PersonId = 7,
                FullName = "full name2",
            };
            participant1.ItineraryGroups.Add(group);
            group.Participants.Add(participant1);
            person1.Participations.Add(participant1);
            participant1.Person = person1;
            participant1.PersonId = person1.PersonId;
            person1.Participations.Add(participant1);

            participant2.ItineraryGroups.Add(group);
            group.Participants.Add(participant2);
            person2.Participations.Add(participant2);
            participant2.Person = person2;
            participant2.PersonId = person2.PersonId;
            person1.Participations.Add(participant2);

            context.Participants.Add(participant1);
            context.Participants.Add(participant2);
            context.People.Add(person1);
            context.People.Add(person2);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            var results = ItineraryGroupQueries.CreateGetEqualItineraryGroupsQuery(context, itinerary.ItineraryId, project.ProjectId, new List<int> { participant1.ParticipantId, participant2.ParticipantId }).ToList();
            Assert.AreEqual(1, results.Count);
            var firstResult = results.First();
            Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
            Assert.AreEqual(itinerary.ItineraryId, firstResult.ItineraryId);
            Assert.AreEqual(itinerary.Name, firstResult.ItineraryName);
            Assert.AreEqual(group.ItineraryGroupId, firstResult.ItineraryGroupId);
            Assert.AreEqual(group.Name, firstResult.ItineraryGroupName);
        }

        [TestMethod]
        public void TestCreateGetEqualItineraryGroupsQuery_ParticipantIdsDuplicatedAndOutOfOrder()
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
            var participant1 = new Participant
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
            var participant2 = new Participant
            {
                ParticipantId = 6,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var person2 = new Person
            {
                PersonId = 7,
                FullName = "full name2",
            };
            participant1.ItineraryGroups.Add(group);
            group.Participants.Add(participant1);
            person1.Participations.Add(participant1);
            participant1.Person = person1;
            participant1.PersonId = person1.PersonId;
            person1.Participations.Add(participant1);

            participant2.ItineraryGroups.Add(group);
            group.Participants.Add(participant2);
            person2.Participations.Add(participant2);
            participant2.Person = person2;
            participant2.PersonId = person2.PersonId;
            person1.Participations.Add(participant2);

            context.Participants.Add(participant1);
            context.Participants.Add(participant2);
            context.People.Add(person1);
            context.People.Add(person2);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            var results = ItineraryGroupQueries.CreateGetEqualItineraryGroupsQuery(context, itinerary.ItineraryId, project.ProjectId, new List<int>
            {
                participant2.ParticipantId,
                participant2.ParticipantId,
                participant2.ParticipantId,
                participant1.ParticipantId,
                participant2.ParticipantId,
                participant2.ParticipantId,
                participant1.ParticipantId,
            }).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestCreateGetEqualItineraryGroupsQuery_DifferentParticipants()
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
            var participant1 = new Participant
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
            var participant2 = new Participant
            {
                ParticipantId = 6,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var person2 = new Person
            {
                PersonId = 7,
                FullName = "full name2",
            };
            participant1.ItineraryGroups.Add(group);
            group.Participants.Add(participant1);
            person1.Participations.Add(participant1);
            participant1.Person = person1;
            participant1.PersonId = person1.PersonId;
            person1.Participations.Add(participant1);

            participant2.ItineraryGroups.Add(group);
            group.Participants.Add(participant2);
            person2.Participations.Add(participant2);
            participant2.Person = person2;
            participant2.PersonId = person2.PersonId;
            person1.Participations.Add(participant2);

            context.Participants.Add(participant1);
            context.Participants.Add(participant2);
            context.People.Add(person1);
            context.People.Add(person2);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            var results = ItineraryGroupQueries.CreateGetEqualItineraryGroupsQuery(context, itinerary.ItineraryId, project.ProjectId, new List<int>
            {
                3
            }).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestCreateGetEqualItineraryGroupsQuery_DifferentItinerary()
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
            var participant1 = new Participant
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
            var participant2 = new Participant
            {
                ParticipantId = 6,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var person2 = new Person
            {
                PersonId = 7,
                FullName = "full name2",
            };
            participant1.ItineraryGroups.Add(group);
            group.Participants.Add(participant1);
            person1.Participations.Add(participant1);
            participant1.Person = person1;
            participant1.PersonId = person1.PersonId;
            person1.Participations.Add(participant1);

            participant2.ItineraryGroups.Add(group);
            group.Participants.Add(participant2);
            person2.Participations.Add(participant2);
            participant2.Person = person2;
            participant2.PersonId = person2.PersonId;
            person1.Participations.Add(participant2);

            context.Participants.Add(participant1);
            context.Participants.Add(participant2);
            context.People.Add(person1);
            context.People.Add(person2);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            var results = ItineraryGroupQueries.CreateGetEqualItineraryGroupsQuery(context, itinerary.ItineraryId + 1, project.ProjectId, new List<int> { participant1.ParticipantId, participant2.ParticipantId }).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestCreateGetEqualItineraryGroupsQuery_DifferentProject()
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
            var participant1 = new Participant
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
            var participant2 = new Participant
            {
                ParticipantId = 6,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
            };
            var person2 = new Person
            {
                PersonId = 7,
                FullName = "full name2",
            };
            participant1.ItineraryGroups.Add(group);
            group.Participants.Add(participant1);
            person1.Participations.Add(participant1);
            participant1.Person = person1;
            participant1.PersonId = person1.PersonId;
            person1.Participations.Add(participant1);

            participant2.ItineraryGroups.Add(group);
            group.Participants.Add(participant2);
            person2.Participations.Add(participant2);
            participant2.Person = person2;
            participant2.PersonId = person2.PersonId;
            person1.Participations.Add(participant2);

            context.Participants.Add(participant1);
            context.Participants.Add(participant2);
            context.People.Add(person1);
            context.People.Add(person2);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ParticipantTypes.Add(participantType);
            context.ItineraryGroups.Add(group);

            var results = ItineraryGroupQueries.CreateGetEqualItineraryGroupsQuery(context, itinerary.ItineraryId, project.ProjectId + 1, new List<int> { participant1.ParticipantId, participant2.ParticipantId }).ToList();
            Assert.AreEqual(0, results.Count);
        }
        #endregion

    }
}
