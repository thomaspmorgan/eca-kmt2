using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using Moq;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Business.Models;
using ECA.WebApi.Controllers;
using ECA.WebApi.Models.Projects;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Results;
using ECA.Business.Queries.Models;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System.Collections.Generic;
using ECA.WebApi.Models.Query;
using ECA.Business.Service.Admin;

namespace ECA.WebApi.Test.Controllers
{
    [TestClass]
    public class SampleControllerTest
    {
        private Mock<IProjectService> serviceMock;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IProjectService>();
        }

        #region Get
        [TestMethod]
        public async Task TestGetProjectsByProgramIcAsync()
        {
            serviceMock.Setup(x => x.GetProjectsByProgramIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<SimpleProjectDTO>>()))
                .Returns(Task.FromResult<PagedQueryResults<SimpleProjectDTO>>(new PagedQueryResults<SimpleProjectDTO>(1, new List<SimpleProjectDTO>())));

            var controller = GetController(serviceMock.Object);
            var response = await controller.GetProjectsByProgramIdAsync(1, new PagingQueryBindingModel());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleProjectDTO>>));
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramIcAsync_InvalidModel()
        {
            
            var controller = GetController(serviceMock.Object);
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProjectsByProgramIdAsync(1, new PagingQueryBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Create
        [TestMethod]
        public async Task TestPostCreateProjectAsync()
        {
            var expectedProject = new Project
            {
                ProjectId = 1
            };
            serviceMock.Setup(x => x.Create(It.IsAny<DraftProject>())).Returns(expectedProject);
            serviceMock.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult<int>(1));
            var controller = GetController(serviceMock.Object);
            var bindingModel = new DraftProjectBindingModel
            {
                Description = "description",
                Name = "name",
                ProgramId = 1
            };
            var response = await controller.PostCreateProjectAsync(bindingModel);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<int>));
            var content = response as OkNegotiatedContentResult<int>;
            Assert.IsNotNull(content);
            Assert.AreEqual(expectedProject.ProjectId, content.Content);
        }

        [TestMethod]
        public async Task TestPostCreateProjectAsync_InvalidModel()
        {
            var expectedProject = new Project
            {
                ProjectId = 1
            };
            var controller = GetController(serviceMock.Object);
            controller.ModelState.AddModelError("key", "value");
            var bindingModel = new DraftProjectBindingModel();
            var response = await controller.PostCreateProjectAsync(bindingModel);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        #endregion

        private SampleController GetController(IProjectService service)
        {
            var controller = new SampleController(service);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            return controller;
        }
    }
}
