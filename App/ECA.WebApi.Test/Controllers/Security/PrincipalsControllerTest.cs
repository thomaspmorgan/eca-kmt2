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
        #region Grant

        [TestMethod]
        public async Task TestPostGrantPermissionAsync()
        {
            var model = new GrantedPermissionBindingModel();
            model.ResourceType = ResourceType.Program.Value;
            var response = await controller.PostGrantPermissionAsync(model);
            handler.Verify(x => x.GrantPermissionAsync(It.IsAny<IGrantedPermissionBindingModel>()), Times.Once());
            handler.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostGrantPermissionAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostGrantPermissionAsync(new GrantedPermissionBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPostGrantPermissionsAsync()
        {
            var model1 = new GrantedPermissionBindingModel();
            model1.ResourceType = ResourceType.Program.Value;

            var model2 = new GrantedPermissionBindingModel();
            model2.ResourceType = ResourceType.Program.Value;
            var response = await controller.PostGrantPermissionsAsync(new List<GrantedPermissionBindingModel> { model1, model2 });
            handler.Verify(x => x.GrantPermissionAsync(It.IsAny<IGrantedPermissionBindingModel>()), Times.Exactly(2));
            handler.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostGrantPermissionsAsync_InvalidModels()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostGrantPermissionsAsync(new List<GrantedPermissionBindingModel>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Revoke
        [TestMethod]
        public async Task TestPostRevokePermissionAsync()
        {
            var model = new RevokedPermissionBindingModel();
            model.ResourceType = ResourceType.Program.Value;
            var response = await controller.PostRevokePermissionAsync(model);
            handler.Verify(x => x.RevokePermissionAsync(It.IsAny<IRevokedPermissionBindingModel>()), Times.Once());
            handler.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostRevokePermissionAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostRevokePermissionAsync(new RevokedPermissionBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPostRevokePermissionsAsync()
        {
            var model1 = new RevokedPermissionBindingModel();
            model1.ResourceType = ResourceType.Program.Value;

            var model2 = new RevokedPermissionBindingModel();
            model2.ResourceType = ResourceType.Program.Value;
            var response = await controller.PostRevokePermissionsAsync(new List<RevokedPermissionBindingModel> { model1, model2 });
            handler.Verify(x => x.RevokePermissionAsync(It.IsAny<IRevokedPermissionBindingModel>()), Times.Exactly(2));
            handler.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostRevokePermissionsAsync_InvalidModels()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostRevokePermissionsAsync(new List<RevokedPermissionBindingModel>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestDeletePermissionAsync()
        {
            var model1 = new DeletedPermissionBindingModel();
            model1.ResourceType = ResourceType.Program.Value;

            var response = await controller.DeletePermissionAsync(model1);
            handler.Verify(x => x.DeletePermissionAsync(It.IsAny<IDeletedPermissionBindingModel>()), Times.Once());
            handler.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestDeletePermissionsAsync()
        {
            var model1 = new DeletedPermissionBindingModel();
            model1.ResourceType = ResourceType.Program.Value;

            var model2 = new DeletedPermissionBindingModel();
            model2.ResourceType = ResourceType.Program.Value;
            var response = await controller.DeletePermissionsAsync(new List<DeletedPermissionBindingModel> { model1, model2 });
            handler.Verify(x => x.DeletePermissionAsync(It.IsAny<IDeletedPermissionBindingModel>()), Times.Exactly(2));
            handler.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestDeletePermissionsAsync_InvalidModels()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.DeletePermissionsAsync(new List<DeletedPermissionBindingModel>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

    }
}
