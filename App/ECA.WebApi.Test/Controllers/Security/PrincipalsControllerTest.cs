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
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Security
{
    [TestClass]
    public class PrincipalsControllerTest
    {
        private PrincipalsController controller;
        private Mock<IUserProvider> userProvider;
        private Mock<IPrincipalService> principalService;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            userProvider.Setup(x => x.GetCurrentUser()).Returns(new DebugWebApiUser());
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(0));
            principalService = new Mock<IPrincipalService>();
            controller = new PrincipalsController(userProvider.Object, principalService.Object);
            controller.ControllerContext = ContextUtil.CreateControllerContext();
        }
        #region Grant
        [TestMethod]
        public async Task TestPostGrantPermissionAsync()
        {
            var model = new GrantedPermissionBindingModel();
            model.ResourceType = ResourceType.Program.Value;
            var response = await controller.PostGrantPermissionAsync(model);
            principalService.Verify(x => x.GrantPermissionsAsync(It.IsAny<GrantedPermission>()), Times.Once());
            principalService.Verify(x => x.RevokePermissionAsync(It.IsAny<RevokedPermission>()), Times.Never());
            principalService.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Once());
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
            var response = await controller.PostGrantPermissionsAsync(new List<GrantedPermissionBindingModel>{model1, model2});
            principalService.Verify(x => x.GrantPermissionsAsync(It.IsAny<GrantedPermission>()), Times.Exactly(2));
            principalService.Verify(x => x.RevokePermissionAsync(It.IsAny<RevokedPermission>()), Times.Never());
            principalService.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Once());
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
            principalService.Verify(x => x.GrantPermissionsAsync(It.IsAny<GrantedPermission>()), Times.Never());
            principalService.Verify(x => x.RevokePermissionAsync(It.IsAny<RevokedPermission>()), Times.Once());
            principalService.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Once());
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
            principalService.Verify(x => x.GrantPermissionsAsync(It.IsAny<GrantedPermission>()), Times.Never());
            principalService.Verify(x => x.RevokePermissionAsync(It.IsAny<RevokedPermission>()), Times.Exactly(2));
            principalService.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostRevokePermissionsAsync_InvalidModels()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostRevokePermissionsAsync(new List<RevokedPermissionBindingModel>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

    }
}
