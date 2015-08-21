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
        public async Task TestHandleAdditionalAddressAsync()
        {
            userProvider.Setup(x => x.GetCurrentUser()).Returns(new DebugWebApiUser());
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            locationService.Setup(x => x.GetAddressByIdAsync(It.IsAny<int>())).ReturnsAsync(new Business.Queries.Models.Admin.AddressDTO());
            locationService.Setup(x => x.CreateAsync(It.IsAny<AdditionalAddress<Organization>>())).ReturnsAsync(new Address());

            var organizationAddress = new OrganizationAddressBindingModel();
            organizationAddress.AddressTypeId = AddressType.Business.Id;
            var response = await handler.HandleAdditionalAddressAsync<Organization>(organizationAddress, controller);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<AddressDTO>));

            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            locationService.Verify(x => x.CreateAsync(It.IsAny<AdditionalAddress<Organization>>()), Times.Once());
            locationService.Verify(x => x.SaveChangesAsync(), Times.Once());
            locationService.Verify(x => x.GetAddressByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestHandleAdditionalAddressAsync_ModelIsInvalid()
        {
            controller.ModelState.AddModelError("key", "error");
            var organizationAddress = new OrganizationAddressBindingModel();
            var response = await handler.HandleAdditionalAddressAsync<Organization>(organizationAddress, controller);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestHandleDeleteAddressAsync()
        {
            var response = await handler.HandleDeleteAddressAsync(1, controller);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            locationService.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once());
            locationService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPutUpdateAddressAsync()
        {
            var model = new UpdatedAddressBindingModel
            {
                AddressTypeId = AddressType.Business.Id,
            };
            var response = await handler.HandleUpdateAddressAsync(model, controller);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<AddressDTO>));
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            locationService.Verify(x => x.UpdateAsync(It.IsAny<UpdatedEcaAddress>()), Times.Once());
            locationService.Verify(x => x.SaveChangesAsync(), Times.Once());
            locationService.Verify(x => x.GetAddressByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestHandleUpdateAddressAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new UpdatedAddressBindingModel
            {
                AddressTypeId = AddressType.Business.Id,
            };
            var response = await handler.HandleUpdateAddressAsync(model, controller);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
    }
}
