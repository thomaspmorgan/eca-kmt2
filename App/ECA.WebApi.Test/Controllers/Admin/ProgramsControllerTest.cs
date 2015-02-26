using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Admin;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Admin
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
            controller = new ProgramsController(serviceMock.Object);
            ControllerHelper.InitializeController(controller);
        }

        #region Get
        [TestMethod]
        public async Task TestGetProjectsByProgramIcAsync()
        {
            serviceMock.Setup(x => x.GetProgramsAsync(It.IsAny<QueryableOperator<SimpleProgramDTO>>()))
                .Returns(Task.FromResult<PagedQueryResults<SimpleProgramDTO>>(new PagedQueryResults<SimpleProgramDTO>(1, new List<SimpleProgramDTO>())));

            var response = await controller.GetProgramsAsync(new PagingQueryBindingModel());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleProgramDTO>>));
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramIcAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProgramsAsync(new PagingQueryBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
