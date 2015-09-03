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
    public class ProgramStatusControllerTest
    {
        private Mock<IProgramStatusService> serviceMock;
        private ProgramStatusesController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IProgramStatusService>();
            serviceMock.Setup(x => x.GetAsync(It.IsAny<QueryableOperator<ProgramStatusDTO>>()))
                .ReturnsAsync(new PagedQueryResults<ProgramStatusDTO>(1, new List<ProgramStatusDTO>()));
            controller = new ProgramStatusesController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetProjectStatiAsync()
        {
            var response = await controller.GetProgramStatiAsync(new PagingQueryBindingModel<ProgramStatusDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ProgramStatusDTO>>));
        }

        [TestMethod]
        public async Task TestGetProjectStatiAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProgramStatiAsync(new PagingQueryBindingModel<ProgramStatusDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
