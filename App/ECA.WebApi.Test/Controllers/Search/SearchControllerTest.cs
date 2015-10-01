using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using Moq;
using ECA.WebApi.Security;
using ECA.WebApi.Controllers.Search;
using ECA.WebApi.Models.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using CAM.Business.Service;
using Microsoft.Azure.Search.Models;

namespace ECA.WebApi.Test.Controllers.Search
{
    [TestClass]
    public class SearchControllerTest
    {
        private Mock<IIndexService> indexService;
        private Mock<IUserProvider> userProvider;
        private SearchController controller;

        [TestInitialize]
        public void TestInit()
        {
            indexService = new Mock<IIndexService>();
            userProvider = new Mock<IUserProvider>();
            controller = new SearchController(indexService.Object, userProvider.Object);
        }

        [TestMethod]
        public async Task TestGetSearchDocumentsAsync()
        {
            userProvider.Setup(x => x.GetCurrentUser()).Returns(new DebugWebApiUser());
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(new List<IPermission>());
            indexService.Setup(x => x.SearchAsync(It.IsAny<ECASearchParameters>(), It.IsAny<List<DocumentKey>>())).ReturnsAsync(new DocumentSearchResponse<ECADocument>());
            var model = new ECASearchParametersBindingModel();
            var response = await controller.GetSearchDocumentsAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<DocumentSearchResponseViewModel>));
            indexService.Verify(x => x.SearchAsync(It.IsAny<ECASearchParameters>(), It.IsAny<List<DocumentKey>>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestTestGetSearchDocumentsAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new ECASearchParametersBindingModel();
            var response = await controller.GetSearchDocumentsAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetDocumentById()
        {
            indexService.Setup(x => x.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(new ECADocument());
            var model = new ECASearchParametersBindingModel();
            var response = await controller.GetDocumentByIdAsync("key");
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ECADocumentViewModel>));
        }

        [TestMethod]
        public async Task TestGetDocumentById_DocumentDoesNotExist()
        {
            indexService.Setup(x => x.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(null);
            var model = new ECASearchParametersBindingModel();
            var response = await controller.GetDocumentByIdAsync("key");
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
    }
}
