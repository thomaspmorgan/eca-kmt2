using ECA.Business.Queries.Models.Itineraries;
using ECA.Business.Service.Itineraries;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Data;
using ECA.WebApi.Controllers.Itineraries;
using ECA.WebApi.Models.Itineraries;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Itineraries
{
    [TestClass]
    public class ItineraryGroupsControllerTest
    {
        private Mock<IItineraryGroupService> service;
        private Mock<IUserProvider> userProvider;
        private ItineraryGroupsController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            service = new Mock<IItineraryGroupService>();

            controller = new ItineraryGroupsController(service.Object, userProvider.Object);
        }
        #region Get
        [TestMethod]
        public async Task TestGetItinerariesByProjectIdAsync()
        {
            var projectId = 1;
            var itineraryId = 2;
            var model = new PagingQueryBindingModel<ItineraryGroupDTO>();
            var results = await controller.GetItinerariesByProjectIdAsync(projectId, itineraryId, model);
            Assert.IsInstanceOfType(results, typeof(OkNegotiatedContentResult<PagedQueryResults<ItineraryGroupDTO>>));
            service.Verify(x => x.GetItineraryGroupsByItineraryIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QueryableOperator<ItineraryGroupDTO>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetItinerariesByProjectIdAsync_InvalidModel()
        {
            var projectId = 1;
            var itineraryId = 2;
            var model = new PagingQueryBindingModel<ItineraryGroupDTO>();
            controller.ModelState.AddModelError("key", "error");
            var results = await controller.GetItinerariesByProjectIdAsync(projectId, itineraryId, model);
            Assert.IsInstanceOfType(results, typeof(InvalidModelStateResult));            
        }

        [TestMethod]
        public async Task TestGetItineraryGroupPersonsByItineraryIdAsync()
        {
            var projectId = 1;
            var itineraryId = 2;
            var model = new PagingQueryBindingModel<ItineraryGroupDTO>();
            var results = await controller.GetItineraryGroupPersonsByItineraryIdAsync(projectId, itineraryId);
            Assert.IsInstanceOfType(results, typeof(OkNegotiatedContentResult<List<ItineraryGroupParticipantsDTO>>));
            service.Verify(x => x.GetItineraryGroupPersonsByItineraryIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }
        #endregion

        #region Create
        [TestMethod]
        public async Task TestPostCreateItineraryGroupAsync()
        {
            service.Setup(x => x.CreateAsync(It.IsAny<AddedEcaItineraryGroup>())).ReturnsAsync(new ItineraryGroup());
            service.Setup(x => x.GetItineraryGroupByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new ItineraryGroupParticipantsDTO());

            var projectId = 1;
            var itineraryId = 2;
            var model = new AddedItineraryGroupBindingModel();
            var results = await controller.PostCreateItineraryGroupAsync(projectId, itineraryId, model);
            service.Verify(x => x.CreateAsync(It.IsAny<AddedEcaItineraryGroup>()), Times.Once());
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
            service.Verify(x => x.GetItineraryGroupByIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostCreateItineraryGroupAsync_InvalidModel()
        {
            var projectId = 1;
            var itineraryId = 2;
            var model = new AddedItineraryGroupBindingModel();
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostCreateItineraryGroupAsync(projectId, itineraryId, model);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
