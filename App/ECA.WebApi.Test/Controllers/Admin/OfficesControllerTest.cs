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
    public class OfficesControllerTest
    {
        private Mock<IOfficeService> serviceMock;
        private OfficesController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IOfficeService>();
            serviceMock.Setup(x => x.GetOfficeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new OfficeDTO());
            serviceMock.Setup(x => x.GetProgramsAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<OrganizationProgramDTO>>()))
                .ReturnsAsync(new PagedQueryResults<OrganizationProgramDTO>(0, new List<OrganizationProgramDTO>()));
            controller = new OfficesController(serviceMock.Object);
            ControllerHelper.InitializeController(controller);
        }

        #region Get
        [TestMethod]
        public async Task TestGetProjectsByProgramIcAsync()
        {
            var response = await controller.GetOfficeByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<OfficeDTO>));
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramIcAsync_OfficeDoesNotExist()
        {
            serviceMock.Setup(x => x.GetOfficeByIdAsync(It.IsAny<int>())).ReturnsAsync(null);
            var response = await controller.GetOfficeByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task TestGetProgramsByOfficeIdAsync()
        {
            var response = await controller.GetProgramsByOfficeIdAsync(1, new PagingQueryBindingModel<OrganizationProgramDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<OrganizationProgramDTO>>));

        }

        [TestMethod]
        public async Task TestGetProgramsByOfficeIdAsync_InvalidQueryModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProgramsByOfficeIdAsync(1, new PagingQueryBindingModel<OrganizationProgramDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        #endregion
    }
}
