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

    [TestClass]
    public class ResourceAuthorizationHandlerTest
    {
        private ResourceAuthorizationHandler handler;
        private Mock<IResourceService> resourceService;
        private Mock<IPrincipalService> principalService;
        private Mock<IUserProvider> userProvider;
        private Mock<IUserService> userService;

        [TestInitialize]
        public void TestInit()
        {
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

            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
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

            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
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

            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(camUser);
            await handler.DeletePermissionAsync(grantedPermission);

            userProvider.Verify(x => x.Clear(It.IsAny<Guid>()), Times.Once());
            principalService.Verify(x => x.DeletePermissionAsync(It.IsAny<DeletedPermission>()), Times.Once());
        }

        [TestMethod]
        public async Task TestSaveChanges()
        {
            var returnValue = 1;
            principalService.Setup(x => x.SaveChanges(It.IsAny<IList<ISaveAction>>())).Returns(returnValue);
            principalService.Setup(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>())).ReturnsAsync(returnValue);
            principalService.Verify(x => x.SaveChanges(It.IsAny<IList<ISaveAction>>()), Times.Never());
            principalService.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Never());
            handler.SaveChanges();
            await handler.SaveChangesAsync();
            principalService.Verify(x => x.SaveChanges(It.IsAny<IList<ISaveAction>>()), Times.Once());
            principalService.Verify(x => x.SaveChangesAsync(It.IsAny<IList<ISaveAction>>()), Times.Once());
            
        }
    }
}
