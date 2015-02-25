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
            var response = await controller.GetProjectsByProgramAsync(1, new PagingQueryBindingModel());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleProjectDTO>>));
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramIcAsync_InvalidModel()
        {

            var controller = GetController(serviceMock.Object);
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProjectsByProgramAsync(1, new PagingQueryBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion


        private ProjectsController GetController(IProjectService service)
        {
            var controller = new ProjectsController(service);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            return controller;
        }
    }
}
