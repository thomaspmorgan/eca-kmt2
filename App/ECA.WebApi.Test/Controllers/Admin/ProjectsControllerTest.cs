using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Admin;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class ProjectsControllerTest
    {
        private Mock<IProjectService> serviceMock;
        private ProjectsController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IProjectService>();
            serviceMock.Setup(x => x.GetProjectsByProgramIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<SimpleProjectDTO>>()))
                .ReturnsAsync(new PagedQueryResults<SimpleProjectDTO>(1, new List<SimpleProjectDTO>()));
            controller = new ProjectsController(serviceMock.Object);
            ControllerHelper.InitializeController(controller);
        }

        #region Get
        [TestMethod]
        public async Task TestGetProjectsByProgramIcAsync()
        {
            var response = await controller.GetProjectsByProgramAsync(1, new MultipleFilterBindingModel());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleProjectDTO>>));
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramIcAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProjectsByProgramAsync(1, new MultipleFilterBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Get Project By Id
        [TestMethod]
        public async Task TestGetProjectByIdAsync()
        {
            serviceMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new ProjectDTO());
            var response = await controller.GetProjectByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ProjectDTO>));
        }

        [TestMethod]
        public async Task TestGetProjectByIdAsync_InvalidModel()
        {
            serviceMock.Setup(x => x.GetProjectByIdAsync(It.IsAny<int>())).ReturnsAsync(null);
            var response = await controller.GetProjectByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
        #endregion
    }
}
