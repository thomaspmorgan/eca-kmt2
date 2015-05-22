using CAM.Business.Model;
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

namespace ECA.WebApi.Test.Controllers.Security
{
    [TestClass]
    public class PrincipalsControllerTest
    {
        private PrincipalsController controller;
        private Mock<IResourceAuthorizationHandler> handler;

        [TestInitialize]
        public void TestInit()
        {
            handler = new Mock<IResourceAuthorizationHandler>();
            controller = new PrincipalsController(handler.Object);
            controller.ControllerContext = ContextUtil.CreateControllerContext();
        }

        [TestMethod]
        public async Task TestPostGrantPermissionAsync()
        {
            var model = new PermissionBindingModel();
            model.ResourceType = ResourceType.Program.Value;
            var response = await controller.PostGrantPermissionAsync(model);
            handler.Verify(x => x.HandleGrantedPermissionBindingModelAsync(It.IsAny<IGrantedPermissionBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostRevokePermissionAsync()
        {
            var model = new PermissionBindingModel();
            model.ResourceType = ResourceType.Program.Value;
            var response = await controller.PostRevokePermissionAsync(model);
            handler.Verify(x => x.HandleRevokedPermissionBindingModelAsync(It.IsAny<IRevokedPermissionBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostDeletePermissionAsync()
        {
            var model1 = new PermissionBindingModel();
            model1.ResourceType = ResourceType.Program.Value;

            var response = await controller.PostDeletePermissionAsync(model1);
            handler.Verify(x => x.HandleDeletedPermissionBindingModelAsync(It.IsAny<IDeletedPermissionBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }
    }
}
