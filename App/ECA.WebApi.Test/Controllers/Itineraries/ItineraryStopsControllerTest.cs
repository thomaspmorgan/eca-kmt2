using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using Moq;
using ECA.Business.Service.Itineraries;
using ECA.WebApi.Controllers.Itineraries;
using System.Web.Http.Results;
using ECA.Business.Queries.Models.Itineraries;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECA.Data;
using ECA.WebApi.Models.Itineraries;

namespace ECA.WebApi.Test.Controllers.Itineraries
{
    [TestClass]
    public class ItineraryStopsControllerTest
    {
        private Mock<IItineraryStopService> service;
        private Mock<IUserProvider> userProvider;
        private ItineraryStopsController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            service = new Mock<IItineraryStopService>();

            controller = new ItineraryStopsController(service.Object, userProvider.Object);
        }
        #region Get
        [TestMethod]
        public async Task TestGetItineraryStopDTOsAsync()
        {
            var projectId = 1;
            var itineraryId = 2;
            var results = await controller.GetItineraryStopDTOsAsync(projectId, itineraryId);
            Assert.IsInstanceOfType(results, typeof(OkNegotiatedContentResult<List<ItineraryStopDTO>>));
            service.Verify(x => x.GetItineraryStopsByItineraryIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }
        #endregion

        #region Create
        [TestMethod]
        public async Task TestPostCreateItineraryStopAsync()
        {
            service.Setup(x => x.CreateAsync(It.IsAny<AddedEcaItineraryStop>())).ReturnsAsync(new ItineraryStop());
            service.Setup(x => x.GetItineraryStopByIdAsync(It.IsAny<int>())).ReturnsAsync(new ItineraryStopDTO());

            var projectId = 1;
            var itineraryId = 2;
            var model = new AddedEcaItineraryStopBindingModel();
            var results = await controller.PostCreateItineraryStopAsync(projectId, itineraryId, model);
            service.Verify(x => x.CreateAsync(It.IsAny<AddedEcaItineraryStop>()), Times.Once());
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
            service.Verify(x => x.GetItineraryStopByIdAsync(It.IsAny<int>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostCreateItineraryStopAsync_InvalidModel()
        {
            var projectId = 1;
            var itineraryId = 2;
            var model = new AddedEcaItineraryStopBindingModel();
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostCreateItineraryStopAsync(projectId, itineraryId, model);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task TestPutUpdateItineraryStopAsync()
        {
            service.Setup(x => x.CreateAsync(It.IsAny<AddedEcaItineraryStop>())).ReturnsAsync(new ItineraryStop());
            service.Setup(x => x.GetItineraryStopByIdAsync(It.IsAny<int>())).ReturnsAsync(new ItineraryStopDTO());

            var projectId = 1;
            var itineraryId = 2;
            var model = new UpdatedEcaItineraryStopBindingModel();
            var results = await controller.PutUpdateItineraryStopAsync(projectId, itineraryId, model);
            service.Verify(x => x.UpdateAsync(It.IsAny<UpdatedEcaItineraryStop>()), Times.Once());
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
            service.Verify(x => x.GetItineraryStopByIdAsync(It.IsAny<int>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPutUpdateItineraryStopAsync_InvalidModel()
        {
            var projectId = 1;
            var itineraryId = 2;
            var model = new UpdatedEcaItineraryStopBindingModel();
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PutUpdateItineraryStopAsync(projectId, itineraryId, model);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
