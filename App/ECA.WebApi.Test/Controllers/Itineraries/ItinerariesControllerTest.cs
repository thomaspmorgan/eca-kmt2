using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using Moq;
using ECA.Business.Service.Itineraries;
using ECA.WebApi.Controllers.Itineraries;
using System.Web.Http.Results;
using System.Collections.Generic;
using ECA.Business.Queries.Itineraries;
using System.Threading.Tasks;

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
    }
}
