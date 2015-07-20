using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using Moq;
using ECA.WebApi.Security;
using ECA.WebApi.Models.Admin;
using System.Threading.Tasks;
using ECA.Data;
using ECA.WebApi.Test.Security;
using System.Web.Http.Results;
using ECA.Business.Queries.Models.Admin;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class AddressModelHandlerTest
    {
        private Mock<ILocationService> locationService;
        private Mock<IUserProvider> userProvider;
        private AddressModelHandler handler;
        private TestController controller;

        [TestInitialize]
        public void TestInit()
        {
            locationService = new Mock<ILocationService>();
            userProvider = new Mock<IUserProvider>();
            controller = new TestController();
            handler = new AddressModelHandler(locationService.Object, userProvider.Object);
        }

        [TestMethod]
        public async Task TestHandleAdditionalAddress()
        {
            userProvider.Setup(x => x.GetCurrentUser()).Returns(new DebugWebApiUser());
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            locationService.Setup(x => x.GetAddressByIdAsync(It.IsAny<int>())).ReturnsAsync(new Business.Queries.Models.Admin.AddressDTO());
            locationService.Setup(x => x.CreateAsync(It.IsAny<AdditionalAddress<Organization>>())).ReturnsAsync(new Address());

            var organizationAddress = new OrganizationAddressBindingModel();
            organizationAddress.AddressTypeId = AddressType.Business.Id;
            var response = await handler.HandleAdditionalAddress<Organization>(organizationAddress, controller);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<AddressDTO>));

            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            locationService.Verify(x => x.CreateAsync(It.IsAny<AdditionalAddress<Organization>>()), Times.Once());
            locationService.Verify(x => x.SaveChangesAsync(), Times.Once());
            locationService.Verify(x => x.GetAddressByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestHandleAdditionalAddress_ModelIsInvalid()
        {
            controller.ModelState.AddModelError("key", "error");
            var organizationAddress = new OrganizationAddressBindingModel();
            var response = await handler.HandleAdditionalAddress<Organization>(organizationAddress, controller);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));

        }
    }
}
