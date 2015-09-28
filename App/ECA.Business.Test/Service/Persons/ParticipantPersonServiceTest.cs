﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using System.Threading.Tasks;
using ECA.Data;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ParticipantPersonServiceTest
    {
        private TestEcaContext context;
        private ParticipantPersonService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ParticipantPersonService(context);
        }

        [TestMethod]
        public async Task TestGetParticipantPersons_CheckProperties()
        {
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = "N0000000001",
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

            Action<PagedQueryResults<SimpleParticipantPersonDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantPersonResult = results.Results.First();

                Assert.AreEqual(participantPerson.SevisId, participantPersonResult.SevisId);
                Assert.AreEqual(participantPerson.StudyProject, participantPersonResult.StudyProject);
                Assert.AreEqual(project.ProjectId, participantPersonResult.ProjectId);

                Assert.IsNull(participantPersonResult.FieldOfStudy);
                Assert.IsNull(participantPersonResult.ProgramCategory);
                Assert.IsNull(participantPersonResult.Position);
                Assert.IsNull(participantPersonResult.HostInstitution);
                Assert.IsNull(participantPersonResult.HomeInstitution);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantPersonDTO>(x => x.RevisedOn, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantPersonDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipantPersons(queryOperator);
            var serviceResultsAsync = await service.GetParticipantPersonsAsync(queryOperator);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantPersons_Empty()
        {
            Action<PagedQueryResults<SimpleParticipantPersonDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Total);
                Assert.AreEqual(0, results.Results.Count);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantPersonDTO>(x => x.RevisedOn, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantPersonDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipantPersons(queryOperator);
            var serviceResultsAsync = await service.GetParticipantPersonsAsync(queryOperator);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantPersonsByProjectId_CheckProperties()
        {
            var project = new Project
            {
                ProjectId = 1
            };

            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = "N0000000001",
                StudyProject = "studyProject",
            };

            participantPerson.Participant = participant;
            project.Participants.Add(participant);
            participant.Project = project;
            participant.ProjectId = project.ProjectId;

            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Projects.Add(project);

            Action<PagedQueryResults<SimpleParticipantPersonDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantPersonResult = results.Results.First();

                Assert.AreEqual(project.ProjectId, participantPersonResult.ProjectId);
                Assert.AreEqual(participantPerson.SevisId, participantPersonResult.SevisId);
                Assert.AreEqual(participantPerson.StudyProject, participantPersonResult.StudyProject);

                Assert.IsNull(participantPersonResult.FieldOfStudy);
                Assert.IsNull(participantPersonResult.ProgramCategory);
                Assert.IsNull(participantPersonResult.Position);
                Assert.IsNull(participantPersonResult.HostInstitution);
                Assert.IsNull(participantPersonResult.HomeInstitution);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantPersonDTO>(x => x.RevisedOn, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantPersonDTO>(0, 1, defaultSorter);


            var serviceResults = service.GetParticipantPersonsByProjectId(1, queryOperator);
            var serviceResultsAsync = await service.GetParticipantPersonsByProjectIdAsync(1, queryOperator);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantPersonsByProjectId_Empty()
        {
            var project = new Project
            {
                ProjectId = 1
            };

            context.Projects.Add(project);

            Action<PagedQueryResults<SimpleParticipantPersonDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Total);
                Assert.AreEqual(0, results.Results.Count);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantPersonDTO>(x => x.RevisedOn, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantPersonDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetParticipantPersonsByProjectId(1, queryOperator);
            var serviceResultsAsync = await service.GetParticipantPersonsByProjectIdAsync(1, queryOperator);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantPersonDTOById_CheckProperties()
        {
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = "N0000000001",
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

            Action<SimpleParticipantPersonDTO> tester = (results) =>
            {
                Assert.AreEqual(participantPerson.SevisId, results.SevisId);
                Assert.AreEqual(participantPerson.StudyProject, results.StudyProject);
                Assert.AreEqual(project.ProjectId, results.ProjectId);

                Assert.IsNull(results.FieldOfStudy);
                Assert.IsNull(results.ProgramCategory);
                Assert.IsNull(results.Position);
                Assert.IsNull(results.HostInstitution);
                Assert.IsNull(results.HomeInstitution);
            };

            var serviceResults = service.GetParticipantPersonById(1);
            var serviceResultsAsync = await service.GetParticipantPersonByIdAsync(1);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantPersonDTOById_Empty()
        {
            Action<SimpleParticipantPersonDTO> tester = (results) =>
            {
                Assert.IsNull(results);
            };

            var serviceResults = service.GetParticipantPersonById(1);
            var serviceResultsAsync = await service.GetParticipantPersonByIdAsync(1);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }
    }
}
