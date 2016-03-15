using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ECA.Business.Service.Projects;
using ECA.WebApi.Controllers.Projects;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Projects
{
    [TestClass]
    public class DefaultExchangeVisitorFundingControllerTest
    {
        private Mock<IDefaultExchangeVisitorFundingService> service;
        private DefaultExchangeVisitorFundingController controller;

        [TestInitialize]
        public void TestInit()
        {
            service = new Mock<IDefaultExchangeVisitorFundingService>();
            controller = new DefaultExchangeVisitorFundingController(service.Object);
        }

        [TestMethod]
        public async Task TestGetDefaultExchangeVisitorFundingByIdAsync()
        {
            service.Setup(x => x.GetDefaultExchangeVisitorFundingAsync(It.IsAny<int>()))
                .ReturnsAsync(new DefaultExchangeVisitorFundingDTO());
            var response = await controller.GetDefaultExchangeVisitorFundingByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<DefaultExchangeVisitorFundingDTO>));
            service.Verify(x => x.GetDefaultExchangeVisitorFundingAsync(It.IsAny<int>()));
        }
    }
}
