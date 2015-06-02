using CAM.Business.Model;
using CAM.Business.Service;
using ECA.Core.Service;
using ECA.WebApi.Controllers.Security;
using ECA.WebApi.Models.Security;
using ECA.WebApi.Security;
using ECA.WebApi.Test.Security;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Security
{
    [TestClass]
    public class AuthControllerTest
    {
        private Mock<IUserProvider> userProvider;
        private Mock<IUserService> userService;
        private Mock<IResourceService> resourceService;
        private Mock<IPermissionModelService> permissionModelService;
        private AuthController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            userService = new Mock<IUserService>();
            resourceService = new Mock<IResourceService>();
            permissionModelService = new Mock<IPermissionModelService>();
            controller = new AuthController(userProvider.Object, userService.Object, resourceService.Object, permissionModelService.Object);
        }

        [TestMethod]
        public void TestPostStopImpersonationAsync()
        {
            userProvider.Setup(x => x.GetCurrentUser()).Returns(new DebugWebApiUser());
            var response = controller.PostStopImpersonation();
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.Clear(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostStartImpersonationAsync()
        {
            Func<Task> f = async () => await controller.PostStartImpersonationAsync(Guid.NewGuid());
            f.ShouldThrow<HttpResponseException>();
            //userProvider.Setup(x => x.GetCurrentUser()).Returns(new DebugWebApiUser(new TraceLogger()));
            //var response = await controller.PostStartImpersonationAsync(Guid.NewGuid());
            //userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            //userProvider.Verify(x => x.ImpersonateAsync(It.IsAny<IWebApiUser>(), It.IsAny<Guid>()), Times.Once());
        }

        [TestMethod]
        public void TestLogout()
        {
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid(),
                Username = "user"
            };
            userProvider.Setup(x => x.GetCurrentUser()).Returns(simpleUser);

            var results = controller.PostLogout();
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.Clear(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserAsync_IsRegistered()
        {
            var camUser = new User
            {
                DisplayName = "display Name"

            };
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid(),
                Username = "user"
            };
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(camUser);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(simpleUser);

            var results = await controller.GetUserAsync();
            Assert.IsInstanceOfType(results, typeof(OkNegotiatedContentResult<UserViewModel>));
            var okResult = (OkNegotiatedContentResult<UserViewModel>)results;
            Assert.IsTrue(okResult.Content.IsRegistered);
            Assert.AreEqual(simpleUser.Id, okResult.Content.UserId);
            Assert.AreEqual(simpleUser.Username, okResult.Content.UserName);
            Assert.AreEqual(camUser.DisplayName, okResult.Content.DisplayName);
        }

        [TestMethod]
        public async Task TestGetUserAsync_IsNotRegistered()
        {
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid(),
                Username = "user",
            };
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(simpleUser);

            var results = await controller.GetUserAsync();
            Assert.IsInstanceOfType(results, typeof(OkNegotiatedContentResult<UserViewModel>));
            var okResult = (OkNegotiatedContentResult<UserViewModel>)results;
            Assert.IsFalse(okResult.Content.IsRegistered);
            Assert.AreEqual(simpleUser.Id, okResult.Content.UserId);
            Assert.AreEqual(simpleUser.Username, okResult.Content.UserName);
            Assert.IsNull(okResult.Content.DisplayName);

        }

        [TestMethod]
        public async Task TestGetUserPermissionsForResourceAsync()
        {
            var resourceId = 1;
            var resourceType = "Program";
            var resourceTypeId = 1;
            var principalId = 1;
            var permissionId = 2;
            var permissionName = "my permission";
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var userPermissions = new List<IPermission>();
            userPermissions.Add(new SimplePermission
            {
                IsAllowed = true,
                PermissionId = permissionId,
                PrincipalId = principalId,
                ResourceId = resourceId
            });
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resourceId);
            permissionModelService.Setup(x => x.GetPermissionNameById(It.IsAny<int>())).Returns(permissionName);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(simpleUser);

            var results = await controller.GetUserPermissionsForResourceAsync(resourceType, resourceId);
            Assert.IsInstanceOfType(results, typeof(OkNegotiatedContentResult<List<ResourcePermissionViewModel>>));
            var okResult = (OkNegotiatedContentResult<List<ResourcePermissionViewModel>>)results;

            Assert.AreEqual(1, okResult.Content.Count());
            var firstPermission = okResult.Content.First();
            Assert.AreEqual(permissionName, firstPermission.PermissionName);
            Assert.AreEqual(permissionId, firstPermission.PermissionId);
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
        }

        [TestMethod]
        public async Task TestPostRegisterAsync_UserDoesNotExist()
        {
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid(),
                Username = "user"
            };
            User nullUser = null;
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(nullUser);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(simpleUser);

            var result = await controller.PostRegisterAsync();
            userService.Verify(x => x.GetUserByIdAsync(It.IsAny<Guid>()), Times.Once());
            userService.Verify(x => x.Create(It.IsAny<AzureUser>()), Times.Once());
            userService.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostRegisterAsync_UserExists()
        {
            var camUser = new User
            {
                DisplayName = "display Name"

            };
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid(),
                Username = "user"
            };
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(camUser);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(simpleUser);

            var result = await controller.PostRegisterAsync();
            Assert.IsInstanceOfType(result, typeof(OkResult));
            userService.Verify(x => x.GetUserByIdAsync(It.IsAny<Guid>()), Times.Once());
            userService.Verify(x => x.Create(It.IsAny<AzureUser>()), Times.Never());
            userService.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Never());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_ResourceDoesNotExist()
        {
            var resourceId = 1;
            var resourceType = "Program";
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(default(int?));

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(0, permissions.Count());
            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_ResourceIdDoesNotExist()
        {
            var resourceId = 1;
            var resourceType = "Program";
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(int?));

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(0, permissions.Count());
            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            resourceService.Verify(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_UserHasZeroPermissions()
        {
            var resourceId = 1;
            var resourceType = "Program";
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var userPermissions = new List<IPermission>();
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(1);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(0, permissions.Count());
            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            resourceService.Verify(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_UserHasZeroAllowedPermissions()
        {
            var resourceId = 1;
            var resourceType = "Program";
            var resourceTypeId = 1;
            var principalId = 1;
            var permissionName = "my permission";
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var userPermissions = new List<IPermission>();
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resourceId);
            permissionModelService.Setup(x => x.GetPermissionNameById(It.IsAny<int>())).Returns(permissionName);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(0, permissions.Count());

            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            resourceService.Verify(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_PermissionIsNotForRequestedResourceId()
        {
            var resourceId = 1;
            var resourceType = "Program";
            var resourceTypeId = 1;
            var principalId = 1;
            var permissionId = 2;
            var permissionName = "my permission";
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var userPermissions = new List<IPermission>();
            userPermissions.Add(new SimplePermission
            {
                IsAllowed = true,
                PermissionId = permissionId,
                PrincipalId = principalId,
                ResourceId = resourceId
            });
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resourceId - 1);
            permissionModelService.Setup(x => x.GetPermissionNameById(It.IsAny<int>())).Returns(permissionName);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(0, permissions.Count());
            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            resourceService.Verify(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_PermissionIsForDifferentPrincipal()
        {
            var resourceId = 1;
            var resourceType = "Program";
            var resourceTypeId = 1;
            var principalId = 1;
            var permissionId = 2;
            var permissionName = "my permission";
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var userPermissions = new List<IPermission>();
            userPermissions.Add(new SimplePermission
            {
                IsAllowed = true,
                PermissionId = permissionId,
                PrincipalId = principalId,
                ResourceId = resourceId
            });
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resourceId);
            permissionModelService.Setup(x => x.GetPermissionNameById(It.IsAny<int>())).Returns(permissionName);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId - 1);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(0, permissions.Count());
            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            resourceService.Verify(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_UserHasPermission()
        {
            var resourceId = 1;
            var resourceType = "Program";
            var resourceTypeId = 1;
            var principalId = 1;
            var permissionId = 2;
            var permissionName = "my permission";
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var userPermissions = new List<IPermission>();
            userPermissions.Add(new SimplePermission
            {
                IsAllowed = true,
                PermissionId = permissionId,
                PrincipalId = principalId,
                ResourceId = resourceId
            });
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resourceId);
            permissionModelService.Setup(x => x.GetPermissionNameById(It.IsAny<int>())).Returns(permissionName);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(1, permissions.Count());
            var firstPermission = permissions.First();
            Assert.AreEqual(permissionName, firstPermission.PermissionName);
            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            resourceService.Verify(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            permissionModelService.Verify(x => x.GetPermissionNameById(It.IsAny<int>()), Times.Once());
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>()), Times.Once());
        }
    }
}
