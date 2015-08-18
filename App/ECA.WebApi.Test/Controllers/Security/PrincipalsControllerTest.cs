using CAM.Business.Model;
using FluentAssertions;
using CAM.Business.Queries.Models;
using CAM.Business.Service;
using CAM.Data;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.WebApi.Controllers.Security;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Models.Security;
using ECA.WebApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Net;

namespace ECA.WebApi.Test.Controllers.Security
{
    [TestClass]
    public class PrincipalsControllerTest
    {
        private PrincipalsController controller;
        private Mock<IResourceAuthorizationHandler> handler;
        private Mock<IResourceService> resourceService;

        [TestInitialize]
        public void TestInit()
        {
            handler = new Mock<IResourceAuthorizationHandler>();
            resourceService = new Mock<IResourceService>();
            controller = new PrincipalsController(handler.Object, resourceService.Object);
            controller.ControllerContext = ContextUtil.CreateControllerContext();
        }

        [TestMethod]
        public async Task TestPostGrantPermissionAsync()
        {
            var model = new PermissionBindingModel();
            model.ResourceType = ResourceType.Program.Value;
            var response = await controller.PostGrantPermissionAsync(1, model);
            handler.Verify(x => x.HandleGrantedPermissionBindingModelAsync(It.IsAny<IGrantedPermissionBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostRevokePermissionAsync()
        {
            var model = new PermissionBindingModel();
            model.ResourceType = ResourceType.Program.Value;
            var response = await controller.PostRevokePermissionAsync(1, model);
            handler.Verify(x => x.HandleRevokedPermissionBindingModelAsync(It.IsAny<IRevokedPermissionBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostDeletePermissionAsync()
        {
            var model = new PermissionBindingModel();
            model.ResourceType = ResourceType.Program.Value;
            var response = await controller.PostDeletePermissionAsync(1, model);
            handler.Verify(x => x.HandleDeletedPermissionBindingModelAsync(It.IsAny<IDeletedPermissionBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetResourceAuthorizationsAsync()
        {
            var response = await controller.GetResourceAuthorizationsAsync(1, new PagingQueryBindingModel<ResourceAuthorization>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ResourceAuthorization>>));
            resourceService.Verify(x => x.GetResourceAuthorizationsAsync(It.IsAny<QueryableOperator<ResourceAuthorization>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetResourceAuthorizationsAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetResourceAuthorizationsAsync(1, new PagingQueryBindingModel<ResourceAuthorization>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
    }
}
