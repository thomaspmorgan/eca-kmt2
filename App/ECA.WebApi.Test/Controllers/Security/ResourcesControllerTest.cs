using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Controllers.Security;
using Moq;
using CAM.Business.Service;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Collections.Generic;
using CAM.Business.Queries.Models;
using CAM.Data;

namespace ECA.WebApi.Test.Controllers.Security
{
    [TestClass]
    public class ResourcesControllerTest
    {
        private ResourcesController controller;
        private Mock<IResourceService> service;

        [TestInitialize]
        public void TestInit()
        {
            service = new Mock<IResourceService>();
            controller = new ResourcesController(service.Object);
        }

        [TestMethod]
        public async Task TestGetResourceTypesAsync()
        {
            service.Setup(x => x.GetResourceTypesAsync()).ReturnsAsync(new System.Collections.Generic.List<CAM.Business.Queries.Models.ResourceTypeDTO>());
            var response = await controller.GetResourceTypesAsync();
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<List<ResourceTypeDTO>>));
        }

        [TestMethod]
        public async Task TestGetPermissionsAsync_ResourceTypeOnly()
        {
            service.Setup(x => x.GetResourcePermissionsAsync(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(new List<ResourcePermissionDTO>());
            var response = await controller.GetPermissionsAsync(ResourceType.Project.Value);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<List<ResourcePermissionDTO>>));
        }

        [TestMethod]
        public async Task TestGetPermissionsAsync_ResourceTypeAndForeignResourceId()
        {
            service.Setup(x => x.GetResourcePermissionsAsync(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(new List<ResourcePermissionDTO>());
            var response = await controller.GetPermissionsAsync(ResourceType.Project.Value, 1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<List<ResourcePermissionDTO>>));
        }
    }
}
