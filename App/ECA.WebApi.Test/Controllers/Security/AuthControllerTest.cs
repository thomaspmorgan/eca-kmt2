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
        private Mock<IPermissionService> permissionService;
        private AuthController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            userService = new Mock<IUserService>();
            resourceService = new Mock<IResourceService>();
            permissionService = new Mock<IPermissionService>();
            controller = new AuthController(userProvider.Object, userService.Object, resourceService.Object, permissionService.Object);
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
                DisplayName = "display Name",
                PrincipalId = 1

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
            Assert.AreEqual(camUser.PrincipalId, okResult.Content.EcaUserId);
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
            var foreignResourceId = 3;
            var resourceType = "Program";
            var resourceTypeId = 1;
            var principalId = 1;
            var permissionId = 2;
            var permissionName = "my permission";
            var permissionModel = new PermissionModel
            {
                Id = permissionId,
                Name = permissionName
            };
            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, null, null, null);
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
                ResourceId = resourceId,
                ForeignResourceId = foreignResourceId,
                ResourceTypeId = resourceTypeId
            });
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            permissionService.Setup(x => x.GetPermissionByIdAsync(It.IsAny<int>())).ReturnsAsync(permissionModel);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(simpleUser);

            var results = await controller.GetUserPermissionsForResourceAsync(resourceType, foreignResourceId);
            Assert.IsInstanceOfType(results, typeof(OkNegotiatedContentResult<List<ResourcePermissionViewModel>>));
            var okResult = (OkNegotiatedContentResult<List<ResourcePermissionViewModel>>)results;

            Assert.AreEqual(1, okResult.Content.Count());
            var firstPermission = okResult.Content.First();
            Assert.AreEqual(permissionName, firstPermission.PermissionName);
            Assert.AreEqual(permissionId, firstPermission.PermissionId);
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsForResourceAsync_CheckDistinctPermissionsReturned()
        {
            var resourceId = 1;
            var foreignResourceId = 3;
            var resourceType = "Program";
            var resourceTypeId = 1;
            var principalId = 1;
            var permissionId = 2;
            var permissionName = "my permission";
            var permissionModel = new PermissionModel
            {
                Id = permissionId,
                Name = permissionName
            };
            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, null, null, null);
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
                ResourceId = resourceId,
                ForeignResourceId = foreignResourceId,
                ResourceTypeId = resourceTypeId
            });
            userPermissions.Add(new SimplePermission
            {
                IsAllowed = true,
                PermissionId = permissionId,
                PrincipalId = principalId,
                ResourceId = resourceId,
                ForeignResourceId = foreignResourceId,
                ResourceTypeId = resourceTypeId
            });
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            permissionService.Setup(x => x.GetPermissionByIdAsync(It.IsAny<int>())).ReturnsAsync(permissionModel);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(simpleUser);

            var results = await controller.GetUserPermissionsForResourceAsync(resourceType, foreignResourceId);
            Assert.IsInstanceOfType(results, typeof(OkNegotiatedContentResult<List<ResourcePermissionViewModel>>));
            var okResult = (OkNegotiatedContentResult<List<ResourcePermissionViewModel>>)results;

            Assert.AreEqual(1, okResult.Content.Count());
            var firstPermission = okResult.Content.First();
            Assert.AreEqual(permissionName, firstPermission.PermissionName);
            Assert.AreEqual(permissionId, firstPermission.PermissionId);
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsForResourceAsync_PermissionIsForParent()
        {
            var resourceId = 1;
            var parentResourceId = 20;
            var parentForeignResourceId = 30;
            var foreignResourceId = 3;
            var resourceType = "Program";
            var resourceTypeId = 1;
            var parentResourceTypeId = 2;
            var principalId = 1;
            var permissionId = 2;
            var permissionName = "my permission";
            var permissionModel = new PermissionModel
            {
                Id = permissionId,
                Name = permissionName
            };
            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
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
                ResourceId = parentResourceId,
                ForeignResourceId = parentForeignResourceId,
                ResourceTypeId = parentResourceTypeId
            });
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            permissionService.Setup(x => x.GetPermissionByIdAsync(It.IsAny<int>())).ReturnsAsync(permissionModel);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(simpleUser);

            var results = await controller.GetUserPermissionsForResourceAsync(resourceType, foreignResourceId);
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
            userService.Verify(x => x.SaveChangesAsync(), Times.Once());
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
            userService.Verify(x => x.SaveChangesAsync(), Times.Never());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_ResourceTypeDoesNotExist()
        {
            var foreignResourceId = 3;
            var resourceType = "Program";
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(default(int?));

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, foreignResourceId);
            Assert.AreEqual(0, permissions.Count());
            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_ResourceIdDoesNotExist()
        {
            var resourceId = 1;
            var foreignResourceId = 3;
            var resourceType = "Program";
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(ForeignResourceCache));

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, foreignResourceId);
            Assert.AreEqual(0, permissions.Count());
            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_UserHasZeroPermissions()
        {
            var resourceId = 1;
            var foreignResourceId = 3;
            var foreignResourceCache = new ForeignResourceCache(0, resourceId, 0, null, null, null);
            var resourceType = "Program";
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var userPermissions = new List<IPermission>();
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, foreignResourceId);
            Assert.AreEqual(0, permissions.Count());
            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_UserHasDeniedResourcePermission()
        {
            var resourceId = 1;
            var foreignResourceId = 3;
            var resourceType = "Program";
            var resourceTypeId = 1;
            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, null, null, null);
            var principalId = 1;
            var permissionName = "my permission";
            var permissionModel = new PermissionModel
            {
                Id = 1,
                Name = permissionName
            };
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var userPermissions = new List<IPermission>();
            userPermissions.Add(new SimplePermission
            {
                IsAllowed = false,
                ForeignResourceId = foreignResourceId,
                PermissionId = permissionModel.Id,
                PrincipalId = principalId,
                ResourceId = resourceId,
                ResourceTypeId = resourceTypeId
            });

            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            permissionService.Setup(x => x.GetPermissionByNameAsync(It.IsAny<string>())).ReturnsAsync(permissionModel);
            permissionService.Setup(x => x.GetPermissionByIdAsync(It.IsAny<int>())).ReturnsAsync(permissionModel);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, foreignResourceId);
            Assert.AreEqual(0, permissions.Count());

            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_UserHasPermissionGrantedByParentButDeniedByResource()
        {
            var resourceId = 1;
            var parentResourceId = 2;
            var foreignResourceId = 3;
            var resourceType = "Program";
            var resourceTypeId = 1;
            var parentResourceTypeId = 2;
            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, null, null, null);
            var principalId = 1;
            var permissionName = "my permission";
            var permissionModel = new PermissionModel
            {
                Id = 1,
                Name = permissionName
            };
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var userPermissions = new List<IPermission>();
            userPermissions.Add(new SimplePermission
            {
                IsAllowed = false,
                ForeignResourceId = foreignResourceId,
                PermissionId = permissionModel.Id,
                PrincipalId = principalId,
                ResourceId = resourceId,
                ResourceTypeId = resourceTypeId
            });
            userPermissions.Add(new SimplePermission
            {
                IsAllowed = true,
                ForeignResourceId = foreignResourceId,
                PermissionId = permissionModel.Id,
                PrincipalId = principalId,
                ResourceId = parentResourceId,
                ResourceTypeId = parentResourceTypeId
            });

            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            permissionService.Setup(x => x.GetPermissionByNameAsync(It.IsAny<string>())).ReturnsAsync(permissionModel);
            permissionService.Setup(x => x.GetPermissionByIdAsync(It.IsAny<int>())).ReturnsAsync(permissionModel);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, foreignResourceId);
            Assert.AreEqual(0, permissions.Count());

            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_UserHasPermissionDeniedByParentButGrantedByResource()
        {
            var resourceId = 1;
            var parentResourceId = 2;
            var foreignResourceId = 3;
            var resourceType = "Program";
            var resourceTypeId = 1;
            var parentResourceTypeId = 2;
            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, null, null, null);
            var principalId = 1;
            var permissionName = "my permission";
            var permissionModel = new PermissionModel
            {
                Id = 1,
                Name = permissionName
            };
            var simpleUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var userPermissions = new List<IPermission>();
            userPermissions.Add(new SimplePermission
            {
                IsAllowed = true,
                ForeignResourceId = foreignResourceId,
                PermissionId = permissionModel.Id,
                PrincipalId = principalId,
                ResourceId = resourceId,
                ResourceTypeId = resourceTypeId
            });
            userPermissions.Add(new SimplePermission
            {
                IsAllowed = false,
                ForeignResourceId = foreignResourceId,
                PermissionId = permissionModel.Id,
                PrincipalId = principalId,
                ResourceId = parentResourceId,
                ResourceTypeId = parentResourceTypeId
            });

            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            permissionService.Setup(x => x.GetPermissionByNameAsync(It.IsAny<string>())).ReturnsAsync(permissionModel);
            permissionService.Setup(x => x.GetPermissionByIdAsync(It.IsAny<int>())).ReturnsAsync(permissionModel);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, foreignResourceId);
            Assert.AreEqual(1, permissions.Count());
            Assert.AreEqual(permissionName, permissions.First().PermissionName);
            Assert.AreEqual(permissionModel.Id, permissions.First().PermissionId);

            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_PermissionIsNotForRequestedResourceId()
        {
            var resourceId = 1;
            var foreignResourceId = 3;
            var resourceType = "Program";
            var resourceTypeId = 1;
            var principalId = 1;
            var permissionId = 2;
            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, resourceId - 1, resourceTypeId, null, null, null);
            var permissionName = "my permission";
            var permissionModel = new PermissionModel
            {
                Id = permissionId,
                Name = permissionName
            };
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
                ResourceId = resourceId,
                ForeignResourceId = foreignResourceId
            });
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            permissionService.Setup(x => x.GetPermissionByIdAsync(It.IsAny<int>())).ReturnsAsync(permissionModel);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, foreignResourceId);
            Assert.AreEqual(0, permissions.Count());
            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetUserPermissionsAsync_UserHasPermission()
        {
            var resourceId = 1;
            var resourceType = "Program";
            var foreignResourceId = 3;
            var resourceTypeId = 1;
            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, null, null, null);
            var principalId = 1;
            var permissionId = 2;
            var permissionName = "my permission";
            var permissionModel = new PermissionModel
            {
                Id = permissionId,
                Name = permissionName
            };
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
                ResourceId = resourceId,
                ForeignResourceId = foreignResourceId,
                ResourceTypeId = resourceTypeId
            });
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(resourceTypeId);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            permissionService.Setup(x => x.GetPermissionByIdAsync(It.IsAny<int>())).ReturnsAsync(permissionModel);
            userProvider.Setup(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(userPermissions);
            userProvider.Setup(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(principalId);

            var permissions = await controller.GetUserPermissionsAsync(simpleUser, resourceType, foreignResourceId);
            Assert.AreEqual(1, permissions.Count());
            var firstPermission = permissions.First();
            Assert.AreEqual(permissionName, firstPermission.PermissionName);
            Assert.AreEqual(permissionModel.Id, firstPermission.PermissionId);
            resourceService.Verify(x => x.GetResourceTypeId(It.IsAny<string>()), Times.Once());
            permissionService.Verify(x => x.GetPermissionByIdAsync(It.IsAny<int>()), Times.Once());
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetPrincipalIdAsync(It.IsAny<IWebApiUser>()), Times.Once());
        }
    }
}
