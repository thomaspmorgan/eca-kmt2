using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using CAM.Business.Service;
using Moq;
using ECA.WebApi.Models.Security;
using CAM.Business.Model;
using CAM.Data;
using System.Threading.Tasks;
using ECA.Core.Service;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using System.Net;

namespace ECA.WebApi.Test.Security
{
    public class TestGrantedPermission : IGrantedPermissionBindingModel, IRevokedPermissionBindingModel, IDeletedPermissionBindingModel
    {

        public GrantedPermission GrantedPermission { get; set; }

        public RevokedPermission RevokedPermission { get; set; }

        public DeletedPermission DeletedPermission { get; set; }

        public CAM.Business.Model.GrantedPermission ToGrantedPermission(int grantorUserId)
        {
            return GrantedPermission;
        }

        public RevokedPermission ToRevokedPermission(int grantorUserId)
        {
            return RevokedPermission;
        }

        public DeletedPermission ToDeletedPermission(int grantorUserId)
        {
            return DeletedPermission;
        }
    }

    public class TestController : ApiController
    {

    }

    [TestClass]
    public class ResourceAuthorizationHandlerTest
    {
        private TestController controller;
        private ResourceAuthorizationHandler handler;
        private Mock<IResourceService> resourceService;
        private Mock<IPrincipalService> principalService;
        private Mock<IUserProvider> userProvider;
        private Mock<IUserService> userService;

        [TestInitialize]
        public void TestInit()
        {
            controller = new TestController();
            resourceService = new Mock<IResourceService>();
            principalService = new Mock<IPrincipalService>();
            userProvider = new Mock<IUserProvider>();
            userService = new Mock<IUserService>();
            handler = new ResourceAuthorizationHandler(resourceService.Object, userProvider.Object, principalService.Object, userService.Object);
        }

        [TestMethod]
        public async Task TestClearUserCacheAsync()
        {
            var camUser = new User
            {
                PrincipalId = 1,
                AdGuid = Guid.NewGuid()
            };
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(camUser);
            await handler.ClearUserCacheAsync(1);
            userProvider.Verify(x => x.Clear(It.IsAny<Guid>()), Times.Once());
        }

        [TestMethod]
        public async Task TestClearUserCacheAsync_UserDoesNotExist()
        {
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(null);
            var id = 1;
            Func<Task> f = async () =>
            {
                await handler.ClearUserCacheAsync(id);
            };
            f.ShouldThrow<ArgumentException>().WithMessage(String.Format("The grantee user could not be found with id [{0}].", id));
        }

        [TestMethod]
        public async Task TestGrantPermissionsAsync()
        {
            var granteePrincipalId = 1;
            var foreignResourceId = 2;
            var permissionId = 3;
            var grantorUserId = 4;
            var resourceType = ResourceType.Program.Value;
            var camUser = new User
            {
                PrincipalId = 1,
                AdGuid = Guid.NewGuid()
            };

            var grantedPermission = new TestGrantedPermission();
            grantedPermission.GrantedPermission = new GrantedPermission(granteePrincipalId, permissionId, foreignResourceId, resourceType, grantorUserId);

            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(10));
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(camUser);
            await handler.GrantPermissionAsync(grantedPermission);

            userProvider.Verify(x => x.Clear(It.IsAny<Guid>()), Times.Once());
            principalService.Verify(x => x.GrantPermissionsAsync(It.IsAny<GrantedPermission>()), Times.Once());
        }

        [TestMethod]
        public async Task TestRevokePermissionsAsync()
        {
            var granteePrincipalId = 1;
            var foreignResourceId = 2;
            var permissionId = 3;
            var grantorUserId = 4;
            var resourceType = ResourceType.Program.Value;
            var camUser = new User
            {
                PrincipalId = 1,
                AdGuid = Guid.NewGuid()
            };

            var grantedPermission = new TestGrantedPermission();
            grantedPermission.RevokedPermission = new RevokedPermission(granteePrincipalId, permissionId, foreignResourceId, resourceType, grantorUserId);

            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(10));
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(camUser);
            await handler.RevokePermissionAsync(grantedPermission);

            userProvider.Verify(x => x.Clear(It.IsAny<Guid>()), Times.Once());
            principalService.Verify(x => x.RevokePermissionAsync(It.IsAny<RevokedPermission>()), Times.Once());
        }

        [TestMethod]
        public async Task TestDeletePermissionsAsync()
        {
            var granteePrincipalId = 1;
            var foreignResourceId = 2;
            var permissionId = 3;
            var resourceType = ResourceType.Program.Value;
            var camUser = new User
            {
                PrincipalId = 1,
                AdGuid = Guid.NewGuid()
            };

            var grantedPermission = new TestGrantedPermission();
            grantedPermission.DeletedPermission = new DeletedPermission(granteePrincipalId, foreignResourceId, permissionId, resourceType);

            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(10));
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(camUser);
            await handler.DeletePermissionAsync(grantedPermission);

            userProvider.Verify(x => x.Clear(It.IsAny<Guid>()), Times.Once());
            principalService.Verify(x => x.DeletePermissionAsync(It.IsAny<DeletedPermission>()), Times.Once());
        }

        [TestMethod]
        public async Task TestSaveChanges()
        {
            var returnValue = 1;
            principalService.Setup(x => x.SaveChanges()).Returns(returnValue);
            principalService.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnValue);
            principalService.Verify(x => x.SaveChanges(), Times.Never());
            principalService.Verify(x => x.SaveChangesAsync(), Times.Never());
            handler.SaveChanges();
            await handler.SaveChangesAsync();
            principalService.Verify(x => x.SaveChanges(), Times.Once());
            principalService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task HandleGrantedPermissionBindingModelAsync_GrantedPermission()
        {
            var resourceType = ResourceType.Program.Value;
            var grantedPermission = new PermissionBindingModel();
            grantedPermission.ResourceType = resourceType;

            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User
            {
                AdGuid = Guid.NewGuid()
            });
            var response = await handler.HandleGrantedPermissionBindingModelAsync(grantedPermission, controller);

            userProvider.Verify(x => x.Clear(It.IsAny<Guid>()), Times.Once());
            principalService.Verify(x => x.GrantPermissionsAsync(It.IsAny<GrantedPermission>()), Times.Once());
            principalService.Verify(x => x.SaveChangesAsync(), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task TestHandleGrantedPermissionBindingModelAsync_GrantedPermission_InvalidModel()
        {
            var resourceType = ResourceType.Program.Value;
            var grantedPermission = new PermissionBindingModel();
            grantedPermission.ResourceType = resourceType;
            controller.ModelState.AddModelError("key", "error");
            var response = await handler.HandleGrantedPermissionBindingModelAsync(grantedPermission, controller);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestHandleGrantedPermissionBindingModelAsync_GrantorGranteeEqual()
        {
            var resourceType = ResourceType.Program.Value;
            var userId = 1;
            var model = new PermissionBindingModel();
            model.ResourceType = resourceType;
            model.PrincipalId = userId;
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(userId));
            Func<Task> f = () =>
            {
                return handler.HandleGrantedPermissionBindingModelAsync(model, controller);
            };
            f.ShouldThrow<HttpResponseException>().And.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public async Task TestHandleRevokedPermissionBindingModelAsync_RevokedPermission()
        {
            var resourceType = ResourceType.Program.Value;
            var revokedPermission = new PermissionBindingModel();
            revokedPermission.ResourceType = resourceType;

            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User
            {
                AdGuid = Guid.NewGuid()
            });
            var response = await handler.HandleRevokedPermissionBindingModelAsync(revokedPermission, controller);

            userProvider.Verify(x => x.Clear(It.IsAny<Guid>()), Times.Once());
            principalService.Verify(x => x.RevokePermissionAsync(It.IsAny<RevokedPermission>()), Times.Once());
            principalService.Verify(x => x.SaveChangesAsync(), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task TestHandleRevokedPermissionBindingModelAsync_RevokedPermission_InvalidModel()
        {
            var resourceType = ResourceType.Program.Value;
            var revokedPermission = new PermissionBindingModel();
            revokedPermission.ResourceType = resourceType;
            controller.ModelState.AddModelError("key", "error");
            var response = await handler.HandleRevokedPermissionBindingModelAsync(revokedPermission, controller);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestHandleRevokedPermissionBindingModelAsync_GrantorGranteeEqual()
        {
            var resourceType = ResourceType.Program.Value;
            var userId = 1;
            var model = new PermissionBindingModel();
            model.ResourceType = resourceType;
            model.PrincipalId = userId;
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(userId));
            Func<Task> f = () =>
            {
                return handler.HandleRevokedPermissionBindingModelAsync(model, controller);
            };
            f.ShouldThrow<HttpResponseException>().And.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public async Task TestHandleDeletedPermissionBindingModelAsync_DeletedPermission()
        {
            var resourceType = ResourceType.Program.Value;
            var deletedPermission = new PermissionBindingModel();
            deletedPermission.ResourceType = resourceType;

            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User
            {
                AdGuid = Guid.NewGuid()
            });
            var response = await handler.HandleDeletedPermissionBindingModelAsync(deletedPermission, controller);

            userProvider.Verify(x => x.Clear(It.IsAny<Guid>()), Times.Once());
            principalService.Verify(x => x.DeletePermissionAsync(It.IsAny<DeletedPermission>()), Times.Once());
            principalService.Verify(x => x.SaveChangesAsync(), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task TestHandleDeletedPermissionBindingModelAsync_DeletedPermission_InvalidModel()
        {
            var resourceType = ResourceType.Program.Value;
            var deletedPermission = new PermissionBindingModel();
            deletedPermission.ResourceType = resourceType;
            controller.ModelState.AddModelError("key", "error");
            var response = await handler.HandleDeletedPermissionBindingModelAsync(deletedPermission, controller);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestHandleDeletedPermissionBindingModelAsync_GrantorGranteeEqual()
        {
            var resourceType = ResourceType.Program.Value;
            var userId = 1;
            var deletedPermission = new PermissionBindingModel();
            deletedPermission.ResourceType = resourceType;
            deletedPermission.PrincipalId = userId;


            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(userId));
            Func<Task> f = () => 
            {
                return handler.HandleDeletedPermissionBindingModelAsync(deletedPermission, controller);
            };
            f.ShouldThrow<HttpResponseException>().And.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
