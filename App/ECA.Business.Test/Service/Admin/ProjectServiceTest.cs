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
            var project = new Project
            {
                ProgramId = program.ProgramId,
                ParentProgram = program,
            };
            project.Locations = new List<Location>();
            project.Locations.Add(location1);
            project.Locations.Add(location2);

            context.Projects.Add(project);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<List<SimpleProjectDTO>> tester = (list) =>
            {
                Assert.AreEqual(1, list.Count);
                var firstResult = list.First();
                //Assert.AreEqual
            };
        }
        #endregion

    }
}

