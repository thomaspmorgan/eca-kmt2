using ECA.Business.Queries.Models.Itineraries;
using ECA.Business.Service;
using ECA.Business.Service.Itineraries;
using ECA.Data;
using ECA.WebApi.Controllers.Itineraries;
using ECA.WebApi.Models.Itineraries;
using ECA.WebApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Itineraries
{
    [TestClass]
    public class ItinerariesControllerTest
    {
        private Mock<IItineraryService> service;
        private Mock<IUserProvider> userProvider;
        private ItinerariesController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            service = new Mock<IItineraryService>();

            controller = new ItinerariesController(service.Object, userProvider.Object);
        }

        [TestMethod]
        public async Task TestGetItinerariesByProjectIdAsync()
        {
            var response = await controller.GetItinerariesByProjectIdAsync(1);
            service.Verify(x => x.GetItinerariesByProjectIdAsync(It.IsAny<int>()), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<List<ItineraryDTO>>));
        }

        #region Create
        [TestMethod]
        public async Task TestPostCreateItineraryAsync()
        {
            var debugUser = new DebugWebApiUser();
            var businessUser = new User(1);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(debugUser);
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(businessUser);

            service.Setup(x => x.CreateAsync(It.IsAny<AddedEcaItinerary>())).ReturnsAsync(new Itinerary());
            service.Setup(x => x.GetItineraryByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new ItineraryDTO());

            var model = new AddedItineraryBindingModel();
            var projectId = 1;
            var response = await controller.PostCreateItineraryAsync(projectId, model);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ItineraryDTO>));

            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            service.Verify(x => x.CreateAsync(It.IsAny<AddedEcaItinerary>()), Times.Once());
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
            service.Verify(x => x.GetItineraryByIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostCreateItineraryAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new AddedItineraryBindingModel();
            var projectId = 1;
            var response = await controller.PostCreateItineraryAsync(projectId, model);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task TestPutUpdateItineraryAsync()
        {
            var debugUser = new DebugWebApiUser();
            var businessUser = new User(1);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(debugUser);
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(businessUser);

            service.Setup(x => x.GetItineraryByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new ItineraryDTO());

            var model = new UpdatedItineraryBindingModel();
            var projectId = 1;
            var response = await controller.PutUpdateItineraryAsync(projectId, model);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ItineraryDTO>));

            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            service.Verify(x => x.UpdateAsync(It.IsAny<UpdatedEcaItinerary>()), Times.Once());
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
            service.Verify(x => x.GetItineraryByIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPutUpdateItineraryAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new UpdatedItineraryBindingModel();
            var projectId = 1;
            var response = await controller.PutUpdateItineraryAsync(projectId, model);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
