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
        public void TestGetDocumentFieldNames()
        {
            indexService.Setup(x => x.GetDocumentFieldNames()).Returns(new List<string>());
            var response = controller.GetDocumentFieldNames(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<List<string>>));
            indexService.Verify(x => x.GetDocumentFieldNames(), Times.Once());
        }

        [TestMethod]
        public async Task TestPostSuggestionsAsync()
        {
            indexService.Setup(x => x.GetSuggestionsAsync(It.IsAny<ECASuggestionParameters>(), It.IsAny<List<DocumentKey>>())).ReturnsAsync(new DocumentSuggestResponse<ECADocument>());
            var model = new ECASuggestionParametersBindingModel();
            var response = await controller.PostSuggestionsAsync(1, model);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<DocumentSuggestResponseViewModel>));
            indexService.Verify(x => x.GetSuggestionsAsync(It.IsAny<ECASuggestionParameters>(), It.IsAny<List<DocumentKey>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostSuggestionsAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            indexService.Setup(x => x.GetSuggestionsAsync(It.IsAny<ECASuggestionParameters>(), It.IsAny<List<DocumentKey>>())).ReturnsAsync(new DocumentSuggestResponse<ECADocument>());
            var model = new ECASuggestionParametersBindingModel();
            var response = await controller.PostSuggestionsAsync(1, model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }


        [TestMethod]
        public async Task TestPostSearchDocumentsAsync()
        {
            userProvider.Setup(x => x.GetCurrentUser()).Returns(new DebugWebApiUser());
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(new List<IPermission>());
            indexService.Setup(x => x.SearchAsync(It.IsAny<ECASearchParameters>(), It.IsAny<List<DocumentKey>>())).ReturnsAsync(new DocumentSearchResponse<ECADocument>());
            var model = new ECASearchParametersBindingModel();
            var response = await controller.PostSearchDocumentsAsync(1, model);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<DocumentSearchResponseViewModel>));
            indexService.Verify(x => x.SearchAsync(It.IsAny<ECASearchParameters>(), It.IsAny<List<DocumentKey>>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostSearchDocumentsAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new ECASearchParametersBindingModel();
            var response = await controller.PostSearchDocumentsAsync(1, model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetDocumentById()
        {
            indexService.Setup(x => x.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(new ECADocument());
            var model = new ECASearchParametersBindingModel();
            var response = await controller.GetDocumentByIdAsync(1, "key");
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ECADocumentViewModel>));
        }

        [TestMethod]
        public async Task TestGetDocumentById_DocumentDoesNotExist()
        {
            indexService.Setup(x => x.GetDocumentByIdAsync(It.IsAny<string>())).ReturnsAsync(null);
            var model = new ECASearchParametersBindingModel();
            var response = await controller.GetDocumentByIdAsync(1, "key");
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task TestDeleteIndexAsync()
        {
            indexService.Setup(x => x.DeleteIndexAsync(It.IsAny<string>())).Returns(Task.FromResult<object>(null));
            var model = new DeleteIndexBindingModel();
            var response = await controller.DeleteIndexAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            indexService.Verify(x => x.DeleteIndexAsync(It.IsAny<string>()), Times.Once());
        }
    }
}
