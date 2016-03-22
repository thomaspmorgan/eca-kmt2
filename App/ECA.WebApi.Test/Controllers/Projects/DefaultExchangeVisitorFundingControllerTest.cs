using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ECA.Business.Service.Projects;
using ECA.WebApi.Controllers.Projects;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;
using System.Web.Http.Results;
using ECA.WebApi.Security;
using ECA.WebApi.Models.Projects;

namespace ECA.WebApi.Test.Controllers.Projects
{
    [TestClass]
    public class DefaultExchangeVisitorFundingControllerTest
    {
        private Mock<IDefaultExchangeVisitorFundingService> service;
        private Mock<IUserProvider> userProvider;
        private DefaultExchangeVisitorFundingController controller;

        [TestInitialize]
        public void TestInit()
        {
            service = new Mock<IDefaultExchangeVisitorFundingService>();
            userProvider = new Mock<IUserProvider>();
            controller = new DefaultExchangeVisitorFundingController(service.Object, userProvider.Object);
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

        [TestMethod]
        public async Task TestGetDefaultExchangeVisitorFundingByIdAsync_NotFound()
        {
            service.Setup(x => x.GetDefaultExchangeVisitorFundingAsync(It.IsAny<int>()))
                .ReturnsAsync(null);
            var response = await controller.GetDefaultExchangeVisitorFundingByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
            service.Verify(x => x.GetDefaultExchangeVisitorFundingAsync(It.IsAny<int>()));
        }

        [TestMethod]
        public async Task TestPutDefaultExchangeVisitorFunding_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PutDefaultExchangeVisitorFunding(1, new UpdatedDefaultExchangeVisitorFundingBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPutDefaultExchangeVisitorFunding()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(0));
            service.Setup(x => x.UpdateAsync(It.IsAny<UpdatedDefaultExchangeVisitorFunding>())).Returns(Task.FromResult<object>(null));
            service.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var response = await controller.PutDefaultExchangeVisitorFunding(1, new UpdatedDefaultExchangeVisitorFundingBindingModel());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<DefaultExchangeVisitorFundingDTO>));
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
        }
    }
}
