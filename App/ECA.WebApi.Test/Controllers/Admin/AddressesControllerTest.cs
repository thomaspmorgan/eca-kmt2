using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using Moq;
using ECA.WebApi.Security;
using ECA.WebApi.Controllers.Admin;
using ECA.WebApi.Models.Admin;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class AddressesControllerTest
    {
        private Mock<IUserProvider> userProvider;
        private Mock<ILocationService> locationService;
        private AddressesController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            locationService = new Mock<ILocationService>();
            controller = new AddressesController(locationService.Object, userProvider.Object);
        }

        [TestMethod]
        public async Task TestPutUpdateAddressAsync()
        {
            var model = new UpdatedAddressBindingModel
            {
                AddressTypeId = AddressType.Business.Id,
            };
            var response = await controller.PutUpdateAddressAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<AddressDTO>));
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            locationService.Verify(x => x.UpdateAsync(It.IsAny<UpdatedEcaAddress>()), Times.Once());
            locationService.Verify(x => x.SaveChangesAsync(), Times.Once());
            locationService.Verify(x => x.GetAddressByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPutUpdateAddressAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new UpdatedAddressBindingModel
            {
                AddressTypeId = AddressType.Business.Id,
            };
            var response = await controller.PutUpdateAddressAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
    }
}
