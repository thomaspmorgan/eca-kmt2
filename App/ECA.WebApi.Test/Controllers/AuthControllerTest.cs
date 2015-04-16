using CAM.Business.Service;
using ECA.Core.Logging;
using ECA.WebApi.Controllers;
using ECA.WebApi.Models.Security;
using ECA.WebApi.Security;
using ECA.WebApi.Test.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        private Mock<IPermissionStore<IPermission>> permissionStore;
        private Mock<IUserProvider> userProvider;
        private AuthController controller;

        [TestInitialize]
        public void TestInit()
        {
            permissionStore = new Mock<IPermissionStore<IPermission>>();
            userProvider = new Mock<IUserProvider>();
            controller = new AuthController(userProvider.Object, permissionStore.Object);
        }

        [TestMethod]
        public async Task TestPostStopImpersonationAsync()
        {
            userProvider.Setup(x => x.GetCurrentUser()).Returns(new DebugWebApiUser(new TraceLogger()));
            var response = await controller.PostStopImpersonationAsync();
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.Clear(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostStartImpersonationAsync()
        {
            userProvider.Setup(x => x.GetCurrentUser()).Returns(new DebugWebApiUser(new TraceLogger()));
            var response = await controller.PostStartImpersonationAsync(Guid.NewGuid());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.ImpersonateAsync(It.IsAny<IWebApiUser>(), It.IsAny<Guid>()), Times.Once());
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
        public async Task TestGetUserAsync()
        {
            var principalId = 1;
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid(),
                Username = "user"
            };
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(simpleUser);

            var results = await controller.GetUserAsync();
            Assert.IsInstanceOfType(results, typeof(OkNegotiatedContentResult<UserViewModel>));
            var okResult = (OkNegotiatedContentResult<UserViewModel>)results;
            Assert.AreEqual(principalId, okResult.Content.PrincipalId);
            Assert.AreEqual(simpleUser.Id, okResult.Content.UserId);
            Assert.AreEqual(simpleUser.Username, okResult.Content.UserName);
            
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
            userPermissions.Add(new Permission
            {
                IsAllowed = true,
                PermissionId = permissionId,
                PrincipalId = principalId,
                ResourceId = resourceId
            });
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resourceId);
            permissionStore.Setup(x => x.LoadUserPermissions(It.IsAny<int>()));
            permissionStore.Setup(x => x.GetPermissionNameById(It.IsAny<int>())).Returns(permissionName);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(simpleUser);

            var results = await controller.GetUserPermissionsForResourceAsync(resourceType, resourceId);
            Assert.IsInstanceOfType(results, typeof(OkNegotiatedContentResult<List<ResourcePermissionViewModel>>));
            var okResult = (OkNegotiatedContentResult<List<ResourcePermissionViewModel>>)results;

            Assert.AreEqual(1, okResult.Content.Count());
            var firstPermission = okResult.Content.First();
            Assert.AreEqual(permissionName, firstPermission.PermissionName);
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
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
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(default(int?));

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(0, permissions.Count());
            permissionStore.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
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
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(default(int?));

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(0, permissions.Count());
            permissionStore.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            permissionStore.Verify(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
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
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(1);
            permissionStore.Setup(x => x.LoadUserPermissions(It.IsAny<int>()));
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(0, permissions.Count());
            permissionStore.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            permissionStore.Verify(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            permissionStore.Verify(x => x.LoadUserPermissions(It.IsAny<int>()), Times.Once());
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
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resourceId);
            permissionStore.Setup(x => x.LoadUserPermissions(It.IsAny<int>()));
            permissionStore.Setup(x => x.GetPermissionNameById(It.IsAny<int>())).Returns(permissionName);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(0, permissions.Count());
            
            permissionStore.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            permissionStore.Verify(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            permissionStore.Verify(x => x.LoadUserPermissions(It.IsAny<int>()), Times.Once());
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
            userPermissions.Add(new Permission
            {
                IsAllowed = true,
                PermissionId = permissionId,
                PrincipalId = principalId,
                ResourceId = resourceId
            });
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resourceId - 1);
            permissionStore.Setup(x => x.LoadUserPermissions(It.IsAny<int>()));
            permissionStore.Setup(x => x.GetPermissionNameById(It.IsAny<int>())).Returns(permissionName);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(0, permissions.Count());
            permissionStore.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            permissionStore.Verify(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            permissionStore.Verify(x => x.LoadUserPermissions(It.IsAny<int>()), Times.Once());
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
            userPermissions.Add(new Permission
            {
                IsAllowed = true,
                PermissionId = permissionId,
                PrincipalId = principalId,
                ResourceId = resourceId
            });
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resourceId);
            permissionStore.Setup(x => x.LoadUserPermissions(It.IsAny<int>()));
            permissionStore.Setup(x => x.GetPermissionNameById(It.IsAny<int>())).Returns(permissionName);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId - 1);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(0, permissions.Count());
            permissionStore.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            permissionStore.Verify(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            permissionStore.Verify(x => x.LoadUserPermissions(It.IsAny<int>()), Times.Once());
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
            userPermissions.Add(new Permission
            {
                IsAllowed = true,
                PermissionId = permissionId,
                PrincipalId = principalId,
                ResourceId = resourceId
            });
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resourceId);
            permissionStore.Setup(x => x.LoadUserPermissions(It.IsAny<int>()));
            permissionStore.Setup(x => x.GetPermissionNameById(It.IsAny<int>())).Returns(permissionName);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, resourceId);
            Assert.AreEqual(1, permissions.Count());
            var firstPermission = permissions.First();
            Assert.AreEqual(permissionName, firstPermission.PermissionName);
            permissionStore.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            permissionStore.Verify(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            permissionStore.Verify(x => x.LoadUserPermissions(It.IsAny<int>()), Times.Once());
            permissionStore.Verify(x => x.GetPermissionNameById(It.IsAny<int>()), Times.Once());
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>()), Times.Once());
        }
    }
}
