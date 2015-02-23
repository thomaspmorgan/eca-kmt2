using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Data;
using ECA.Data;
using ECA.Business.Models;
using ECA.Business.Service;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service
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

        #region GetProgramProjects
        [TestMethod]
        public async Task TestGetProgramProjectsById()
        {

        }
        #endregion

    }
}

