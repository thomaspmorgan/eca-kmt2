using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Programs;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Programs
{
    [TestClass]
    public class ProgramsControllerTest
    {
        private Mock<IProgramService> serviceMock;
        private ProgramsController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IProgramService>();
            serviceMock.Setup(x => x.GetProgramsAsync(It.IsAny<QueryableOperator<SimpleProgramDTO>>()))
                .ReturnsAsync(new PagedQueryResults<SimpleProgramDTO>(1, new List<SimpleProgramDTO>()));

            controller = new ProgramsController(serviceMock.Object);
            ControllerHelper.InitializeController(controller);
        }

        #region Get
        [TestMethod]
        public async Task TestGetProgramsAsync()
        {
            var response = await controller.GetProgramsAsync(new PagingQueryBindingModel());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleProgramDTO>>));
        }

        [TestMethod]
        public async Task TestGetProgramsAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProgramsAsync(new PagingQueryBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetProgramByIdAsync()
        {
            serviceMock.Setup(x => x.GetProgramByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new EcaProgram());
            var response = await controller.GetProgramByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<EcaProgram>));
        }

        [TestMethod]
        public async Task TestGetProgramByIdAsync_ProgramDoesNotExist()
        {
            serviceMock.Setup(x => x.GetProgramByIdAsync(It.IsAny<int>())).ReturnsAsync(null);
            var response = await controller.GetProgramByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
        #endregion
    }
}
