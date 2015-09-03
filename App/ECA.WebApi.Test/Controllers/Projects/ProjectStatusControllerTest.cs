using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Projects;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Projects;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Projects
{
    [TestClass]
    public class ProjectStatusControllerTest
    {
        private Mock<IProjectStatusService> serviceMock;
        private ProjectStatusesController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IProjectStatusService>();
            serviceMock.Setup(x => x.GetAsync(It.IsAny<QueryableOperator<ProjectStatusDTO>>()))
                .ReturnsAsync(new PagedQueryResults<ProjectStatusDTO>(1, new List<ProjectStatusDTO>()));
            controller = new ProjectStatusesController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetProjectStatiAsync()
        {
            var response = await controller.GetProjectStatiAsync(new PagingQueryBindingModel<ProjectStatusDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ProjectStatusDTO>>));
        }

        [TestMethod]
        public async Task TestGetProjectStatiAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProjectStatiAsync(new PagingQueryBindingModel<ProjectStatusDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
