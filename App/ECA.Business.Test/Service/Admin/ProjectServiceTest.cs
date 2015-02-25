using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Data;
using ECA.Data;
using ECA.Business.Models;
using ECA.Business.Service;
using System.Threading.Tasks;
using ECA.Business.Service.Admin;
using ECA.Business.Queries.Models;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using System.Collections.Generic;
using ECA.Core.Query;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class ProjectServiceTest
    {
        private TestEcaContext context;
        private ProjectService service;

        [TestInitialize]
        public void TestInit()
        {
            context = DbContextHelper.GetInMemoryContext();
            service = new ProjectService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Create Draft Project
        [TestMethod]
        public void TestCreate()
        {
            Assert.AreEqual(0, context.Programs.Count());
            var utcNow = DateTimeOffset.UtcNow;
            var projectName = "project";
            var userId = 1;
            var description = "description";
            var programId = 2;
            var draft = new DraftProject(projectName, description, programId, userId);
            var project = service.Create(draft);

            Assert.AreEqual(1, context.Projects.Count());
            var savedProject = context.Projects.First();
            Assert.IsNotNull(savedProject);
            Assert.AreEqual(project.Name, savedProject.Name);
            Assert.AreEqual(project.Description, savedProject.Description);
            Assert.AreEqual(project.ProgramId, savedProject.ProgramId);

            Assert.IsNotNull(savedProject.History);
            Assert.AreEqual(userId, savedProject.History.CreatedBy);
            Assert.AreEqual(userId, savedProject.History.RevisedBy);
            savedProject.History.CreatedOn.Should().BeCloseTo(utcNow, DbContextHelper.DATE_PRECISION);
            savedProject.History.RevisedOn.Should().BeCloseTo(utcNow, DbContextHelper.DATE_PRECISION);
        }
        #endregion

        #region Get Projects By Program Id
        [TestMethod]
        public async Task TestGetProjectsByProgramId_CheckProperties()
        {
            var now = DateTimeOffset.UtcNow;
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                StartDate = now,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
            };
            
            project.Locations = new List<Location>();
            project.Locations.Add(location1);
            project.Locations.Add(location2);

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();

                Assert.AreEqual(program.ProgramId, firstResult.ProgramId);
                Assert.AreEqual(project.Name, firstResult.ProjectName);
                Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
                Assert.AreEqual(project.StartDate, firstResult.StartDate);
                Assert.AreEqual(project.StartDate.Year, firstResult.StartYear);
                Assert.AreEqual(status.Status, firstResult.ProjectStatusName);
                Assert.AreEqual(status.ProjectStatusId, firstResult.ProjectStatusId);

                Assert.IsTrue(firstResult.LocationNames.Contains(location1.LocationName));
                Assert.IsTrue(firstResult.LocationNames.Contains(location2.LocationName));
            };

            var serviceResults = service.GetProjectsByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramId_NoProgramsWithGivenId()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
            };
            project.Locations = new List<Location>();
            project.Locations.Add(location1);
            project.Locations.Add(location2);

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(0, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(0, results.Count);
            };

            var otherProgramId = -1;
            var serviceResults = service.GetProjectsByProgramId(otherProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(otherProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramId_DefaultSorterOnly()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
            };

            project.Locations = new List<Location>();
            project.Locations.Add(location1);
            project.Locations.Add(location2);

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
            };

            var serviceResults = service.GetProjectsByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramId_Paging()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var project1 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project1",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
            };
            project1.Locations = new List<Location>();
            project1.Locations.Add(location1);
            project1.Locations.Add(location2);


            var project2 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project2",
                ParentProgram = program,
                ProjectId = 200,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
            };
            project2.Locations = new List<Location>();

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project1);
            context.Projects.Add(project2);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 1;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(2, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(project1.ProjectId, firstResult.ProjectId);
            };

            var serviceResults = service.GetProjectsByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramId_Filter()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var project1 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project1",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
            };
            project1.Locations = new List<Location>();
            project1.Locations.Add(location1);
            project1.Locations.Add(location2);

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project1);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 1;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<SimpleProjectDTO>(x => x.ProjectName, ComparisonType.Like, project1.Name));

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(project1.ProjectId, firstResult.ProjectId);
            };

            var serviceResults = service.GetProjectsByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramId_Sorted()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var project1 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project1",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
            };
            project1.Locations = new List<Location>();
            project1.Locations.Add(location1);
            project1.Locations.Add(location2);


            var project2 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project2",
                ParentProgram = program,
                ProjectId = 200,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
            };
            project2.Locations = new List<Location>();

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project1);
            context.Projects.Add(project2);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 1;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Descending));

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(2, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(project2.ProjectId, firstResult.ProjectId);
            };

            var serviceResults = service.GetProjectsByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

    }
}



