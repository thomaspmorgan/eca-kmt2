using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using Moq;
using ECA.WebApi.Controllers.Admin;
using System.Web.Http.Results;
using System.Threading.Tasks;
using ECA.WebApi.Models.Admin;

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class DataPointConfigurationControllerTest
    {

        private Mock<IDataPointConfigurationService> service;
        private DataPointConfigurationsController controller;

        [TestInitialize]
        public void TestInit()
        {
            service = new Mock<IDataPointConfigurationService>();
            controller = new DataPointConfigurationsController(service.Object);
        }

        [TestMethod]
        public async Task TestDeleteDataPointConfigurationAsync()
        {
            var response = await controller.DeleteDataPointConfigurationAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            service.Verify(x => x.DeleteDataPointConfigurationAsync(It.IsAny<int>()), Times.Once());
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPostDataPointConfigurationAsync()
        {
            var model = new DataPointConfigurationBindingModel();
            var response = await controller.PostDataPointConfigurationAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            service.Verify(x => x.CreateDataPointConfigurationAsync(It.IsAny<NewDataPointConfiguration>()), Times.Once());
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPostDataPointConfigurationAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new DataPointConfigurationBindingModel();
            var response = await controller.PostDataPointConfigurationAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
    }
}
