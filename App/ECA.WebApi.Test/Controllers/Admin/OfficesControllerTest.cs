using CAM.Business.Service;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Office;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Admin;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
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
        private Mock<IFocusCategoryService> focusCategoryService;
        private Mock<IJustificationObjectiveService> justificationObjectiveService;
        private Mock<IResourceService> resourceService;
        private Mock<IResourceAuthorizationHandler> authorizationHandler;
        private Mock<IUserProvider> userProvider;
        private OfficesController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IOfficeService>();
            focusCategoryService = new Mock<IFocusCategoryService>();
            justificationObjectiveService = new Mock<IJustificationObjectiveService>();
            resourceService = new Mock<IResourceService>();
            authorizationHandler = new Mock<IResourceAuthorizationHandler>();
            userProvider = new Mock<IUserProvider>();
            serviceMock.Setup(x => x.GetOfficeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new OfficeDTO());
            serviceMock.Setup(x => x.GetProgramsAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<OrganizationProgramDTO>>()))
                .ReturnsAsync(new PagedQueryResults<OrganizationProgramDTO>(0, new List<OrganizationProgramDTO>()));
            serviceMock.Setup(x => x.GetOfficeSettingsAsync(It.IsAny<int>())).ReturnsAsync(new OfficeSettings());
            controller = new OfficesController(serviceMock.Object, focusCategoryService.Object, justificationObjectiveService.Object, resourceService.Object, authorizationHandler.Object, userProvider.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetOfficeByIdAsync()
        {
            var response = await controller.GetOfficeByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<OfficeDTO>));
        }

        [TestMethod]
        public async Task TestGetOfficeByIdAsync_OfficeDoesNotExist()
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

        [TestMethod]
        public async Task TestGetCategoriesByOfficeIdAsync()
        {
            var response = await controller.GetCategoriesByOfficeIdAsync(1, new PagingQueryBindingModel<FocusCategoryDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<FocusCategoryDTO>>));

        }

        [TestMethod]
        public async Task TestGetCategoriesByOfficeIdAsync_InvalidQueryModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetCategoriesByOfficeIdAsync(1, new PagingQueryBindingModel<FocusCategoryDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetObjectivesByOfficeIdAsync()
        {
            var response = await controller.GetObjectivesByOfficeIdAsync(1, new PagingQueryBindingModel<JustificationObjectiveDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<JustificationObjectiveDTO>>));
        }

        [TestMethod]
        public async Task TestGetObjectivesByOfficeIdAsync_InvalidQueryModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetObjectivesByOfficeIdAsync(1, new PagingQueryBindingModel<JustificationObjectiveDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetChildOfficesByOfficeIdAsync()
        {
            serviceMock.Setup(x => x.GetChildOfficesAsync(It.IsAny<int>())).ReturnsAsync(new List<SimpleOfficeDTO>());
            var response = await controller.GetChildOfficesByOfficeIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<List<SimpleOfficeDTO>>));
        }

        [TestMethod]
        public async Task TestGetChildOfficesByOfficeIdAsync_IdNotExist()
        {
            serviceMock.Setup(x => x.GetChildOfficesAsync(It.IsAny<int>())).ReturnsAsync(null);
            var response = await controller.GetChildOfficesByOfficeIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task TestGetOfficesAsync()
        {
            serviceMock.Setup(x => x.GetOfficesAsync(It.IsAny<QueryableOperator<SimpleOfficeDTO>>()))
                .ReturnsAsync(new PagedQueryResults<SimpleOfficeDTO>(1, new List<SimpleOfficeDTO>()));
            var response = await controller.GetOfficesAsync(new PagingQueryBindingModel<SimpleOfficeDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleOfficeDTO>>));
        }

        [TestMethod]
        public async Task TestGetOfficesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetOfficesAsync(new PagingQueryBindingModel<SimpleOfficeDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        #endregion

        #region Update
        [TestMethod]
        public async Task TestPutOfficeAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            var model = new UpdatedOfficeBindingModel();
            var response = await controller.PutOfficeAsync(model);
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once);
            serviceMock.Verify(x => x.UpdateOfficeAsync(It.IsAny<UpdatedOffice>()), Times.Once);
            serviceMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            serviceMock.Verify(x => x.GetOfficeByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPutOfficeAync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new UpdatedOfficeBindingModel();
            var response = await controller.PutOfficeAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Settings
        [TestMethod]
        public async Task TestGetOfficeSettingsAsync()
        {
            var result = await controller.GetOfficeSettingsByIdAsync(1);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<OfficeSettings>));
        }
        #endregion
    }
}
