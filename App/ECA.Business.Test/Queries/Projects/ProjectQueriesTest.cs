using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Projects;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ECA.Business.Test.Queries.Projects
{
    [TestClass]
    public class ProjectQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestCreateGetProjectsByPersonIdQuery_CheckRelationships()
        {
            var person = new Person
            {
                PersonId = 1
            };

            var org = new Organization
            {
                OrganizationId = 1
            };

            var program = new Program
            {
                ProgramId = 1,
                Owner = org,
                OwnerId = org.OrganizationId
            };

            var project = new Project
            {
                ProjectId = 1,
                ProgramId = program.ProgramId,
                ParentProgram = program
            };


            var participant = new Participant
            {
                ParticipantId = 1,
                Person = person,
                PersonId = person.PersonId,
                Project = project,
                ProjectId = project.ProjectId
            };

            context.Participants.Add(participant);

            var defaultSorter = new ExpressionSorter<ParticipantTimelineDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ParticipantTimelineDTO>(0, 10, defaultSorter);
            var projects = ProjectQueries.CreateGetProjectsByPersonIdQuery(context, person.PersonId, queryOperator);

            Assert.AreEqual(1, projects.Count());

            var projectResult = projects.FirstOrDefault();

            Assert.AreEqual(project.ProjectId, projectResult.ProjectId);
            Assert.AreEqual(program.ProgramId, projectResult.ProgramId);
            Assert.AreEqual(org.OrganizationId, projectResult.OfficeId);
        }

        [TestMethod]
        public void TestCreateGetProjectsByPersonIdQuery_NoProjects()
        {
            var defaultSorter = new ExpressionSorter<ParticipantTimelineDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ParticipantTimelineDTO>(0, 10, defaultSorter);
            var projects = ProjectQueries.CreateGetProjectsByPersonIdQuery(context, 1, queryOperator);

            Assert.AreEqual(0, projects.Count());
        }

        [TestMethod]
        public void TestCreateGetProjectsByPersonIdQuery_CheckProperties()
        {
            var person = new Person
            {
                PersonId = 1
            };

            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                StartDate = new DateTime(2013, 5, 1, 06, 32, 00),
                EndDate = new DateTime(2017, 5, 1, 06, 32, 00),
                Description = "description"
            };

            var participant = new Participant
            {
                ParticipantId = 1,
                Person = person,
                PersonId = person.PersonId,
                Project = project,
                ProjectId = project.ProjectId
            };

            context.Participants.Add(participant);

            var defaultSorter = new ExpressionSorter<ParticipantTimelineDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ParticipantTimelineDTO>(0, 10, defaultSorter);
            var projects = ProjectQueries.CreateGetProjectsByPersonIdQuery(context, person.PersonId, queryOperator);

            Assert.AreEqual(1, projects.Count());

            var projectResult = projects.FirstOrDefault();

            Assert.AreEqual(project.ProjectId, projectResult.ProjectId);
            Assert.AreEqual(participant.ParticipantId, projectResult.ParticipantId);
            Assert.AreEqual(project.Name, projectResult.Name);
            Assert.AreEqual(project.StartDate, projectResult.StartDate);
            Assert.AreEqual(project.EndDate, projectResult.EndDate);
            Assert.AreEqual(project.Description, projectResult.Description);

            Assert.IsNull(projectResult.OfficeSymbol);
            Assert.IsNull(projectResult.Status);
        }

        [TestMethod]
        public void TestCreateGetProjectsByPersonIdQuery_CheckOfficeSymbol()
        {
            var person = new Person
            {
                PersonId = 1
            };

            var org = new Organization
            {
                OrganizationId = 1,
                OfficeSymbol = "ABCDE"
            };

            var program = new Program
            {
                ProgramId = 1,
                Owner = org,
                OwnerId = org.OrganizationId
            };

            var project = new Project
            {
                ProjectId = 1,
                ParentProgram = program
            };


            var participant = new Participant
            {
                ParticipantId = 1,
                Person = person,
                PersonId = person.PersonId,
                Project = project,
                ProjectId = project.ProjectId
            };

            context.Participants.Add(participant);

            var defaultSorter = new ExpressionSorter<ParticipantTimelineDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ParticipantTimelineDTO>(0, 10, defaultSorter);
            var projects = ProjectQueries.CreateGetProjectsByPersonIdQuery(context, person.PersonId, queryOperator);

            Assert.AreEqual(1, projects.Count());

            var projectResult = projects.FirstOrDefault();

            Assert.AreEqual(org.OfficeSymbol, projectResult.OfficeSymbol);
        }

        [TestMethod]
        public void TestCreateGetProjectsByPersonIdQuery_CheckStatus()
        {
            var person = new Person
            {
                PersonId = 1
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };

            var project = new Project
            {
                ProjectId = 1,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
            };


            var participant = new Participant
            {
                ParticipantId = 1,
                Person = person,
                PersonId = person.PersonId,
                Project = project,
                ProjectId = project.ProjectId
            };

            context.Participants.Add(participant);

            var defaultSorter = new ExpressionSorter<ParticipantTimelineDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ParticipantTimelineDTO>(0, 10, defaultSorter);
            var projects = ProjectQueries.CreateGetProjectsByPersonIdQuery(context, person.PersonId, queryOperator);

            Assert.AreEqual(1, projects.Count());

            var projectResult = projects.FirstOrDefault();

            Assert.AreEqual(status.Status, projectResult.Status);
        }
    }
}
