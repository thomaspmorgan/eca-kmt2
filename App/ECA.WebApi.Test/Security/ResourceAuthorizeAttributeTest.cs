using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using System.Threading.Tasks;
using ECA.Core.Logging;
using Moq;
using System.Web.Http.Controllers;
using System.Threading;
using System.Security.Claims;
using System.Web.Http;
using System.Net;
using ECA.WebApi.Models.Query;
using CAM.Business.Service;
using System.Collections.Generic;

namespace ECA.WebApi.Test.Security
{
    public class TestUser : DebugWebApiUser
    {
        public int HasPermissionCount { get; set; }

        public TestUser(ILogger logger) : base(logger) { }

        public bool UserHasPermission { get; set; }

        public override bool HasPermission(CAM.Business.Service.IPermission requestedPermission, System.Collections.Generic.IEnumerable<CAM.Business.Service.IPermission> allUserPermissions)
        {
            HasPermissionCount++;
            return UserHasPermission;
        }
    }

    [TestClass]
    public class ResourceAuthorizeAttributeTest
    {
        private Mock<IUserProvider> userProvider;
        private Mock<IPermissionStore<IPermission>> permissionStore;
        private Mock<ILogger> logger;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            permissionStore = new Mock<IPermissionStore<IPermission>>();
            logger = new Mock<ILogger>();
            ResourceAuthorizeAttribute.LoggerFactory = () =>
            {
                return logger.Object;
            };
            ResourceAuthorizeAttribute.UserProviderFactory = () =>
            {
                return userProvider.Object;
            };
            ResourceAuthorizeAttribute.PermissionLookupFactory = () =>
            {
                return permissionStore.Object;
            };
        }

        [TestMethod]
        public void TestConstructor_ModelPermission()
        {
            var modelType = typeof(SorterBindingModel);
            var property = "direction";
            var permissionName = "Read";
            var resourceType = "Program";

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, modelType, property);
            Assert.IsNotNull(attribute.Permission);
            Assert.IsInstanceOfType(attribute.Permission, typeof(ModelPermission));
            var permission = (ModelPermission)attribute.Permission;
            Assert.AreEqual(modelType, permission.ModelType);
            Assert.AreEqual(property, permission.Property);
            Assert.AreEqual(permissionName, permission.PermissionName);
            Assert.AreEqual(resourceType, permission.ResourceType);
        }

        [TestMethod]
        public void TestConstructor_ActionPermission()
        {
            var actionArgument = "id";
            var permissionName = "Read";
            var resourceType = "Program";

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, actionArgument);
            Assert.IsNotNull(attribute.Permission);
            Assert.IsInstanceOfType(attribute.Permission, typeof(ActionPermission));
            var permission = (ActionPermission)attribute.Permission;
            Assert.AreEqual(actionArgument, permission.ArgumentName);
            Assert.AreEqual(permissionName, permission.PermissionName);
            Assert.AreEqual(resourceType, permission.ResourceType);
        }

        [TestMethod]
        public void TestConstructor_ActionPermission_DefaultIdActionArgument()
        {
            var permissionName = "Read";
            var resourceType = "Program";

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType);
            Assert.IsNotNull(attribute.Permission);
            Assert.IsInstanceOfType(attribute.Permission, typeof(ActionPermission));
            var permission = (ActionPermission)attribute.Permission;
            Assert.AreEqual(ResourceAuthorizeAttribute.DEFAULT_ID_ARGUMENT_NAME, permission.ArgumentName);
        }

        [TestMethod]
        public void TestConstructor_StaticPermission()
        {
            var permissionName = "read";
            var resourceType = "program";
            var id = 1;
            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, id);
            Assert.IsNotNull(attribute.Permission);
            Assert.IsInstanceOfType(attribute.Permission, typeof(StaticPermission));
            var staticPermission = (StaticPermission)attribute.Permission;
            Assert.AreEqual(permissionName, staticPermission.PermissionName);
            Assert.AreEqual(resourceType, staticPermission.ResourceType);
            Assert.AreEqual(id, staticPermission.ForeignResourceId);

        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_CheckPermissionsIterated()
        {
            var id = 1;
            var permissionName = "Read";
            var resourceType = "Program";
            var user = GetTestUser();
            user.UserHasPermission = true;
            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);
            permissionStore.Setup(x => x.GetPermissionIdByName(It.IsAny<string>())).Returns(1);
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(id);

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, id);
            var actionContext = ContextUtil.CreateActionContext();
            actionContext.RequestContext.Principal = Thread.CurrentPrincipal;

            var cts = new CancellationTokenSource();
            Assert.AreEqual(0, user.HasPermissionCount);
            await attribute.OnActionExecutingAsync(actionContext, cts.Token);
            Assert.AreEqual(1, user.HasPermissionCount);
        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_UserHasPermission()
        {
            var id = 1;
            var permissionName = "Read";
            var resourceType = "Program";
            var user = GetTestUser();
            user.UserHasPermission = true;
            var infoMessage = string.Empty;
            Action<string, object[]> infoCallback = (fmt, objParams) =>
            {
                infoMessage = String.Format(fmt, objParams);
            };

            logger.Setup(x => x.Information(It.IsAny<string>(), It.IsAny<object[]>())).Callback(infoCallback);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);
            permissionStore.Setup(x => x.GetPermissionIdByName(It.IsAny<string>())).Returns(1);
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(id);

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, id);
            var actionContext = ContextUtil.CreateActionContext();
            actionContext.RequestContext.Principal = Thread.CurrentPrincipal;

            var cts = new CancellationTokenSource();
            await attribute.OnActionExecutingAsync(actionContext, cts.Token);

            var expectedMessage = String.Format("User [{0}] granted access to resource with id [{1}] with foreign key [{2}] of type [{3}] on web api action [{4}].[{5}].",
                user.GetUsername(),
                id,
                id,
                resourceType,
                actionContext.ControllerContext.ControllerDescriptor.ControllerName,
                actionContext.ActionDescriptor.ActionName);
            Assert.AreEqual(expectedMessage, infoMessage);
        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_UserIsNotValid()
        {
            var id = 1;
            var permissionName = "Read";
            var resourceType = "Program";
            var user = GetTestUser();
            user.UserHasPermission = true;
            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(false);

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, id);
            var actionContext = ContextUtil.CreateActionContext();
            actionContext.RequestContext.Principal = Thread.CurrentPrincipal;

            var exceptionCaught = false;
            try
            {
                var cts = new CancellationTokenSource();
                await attribute.OnActionExecutingAsync(actionContext, cts.Token);
            }
            catch (HttpResponseException e)
            {
                exceptionCaught = true;
                Assert.AreEqual(HttpStatusCode.Unauthorized, e.Response.StatusCode);
            }
            Assert.IsTrue(exceptionCaught);
            //make sure we do not fall into into checking permissions
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Never());
        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_UserDoesNotHavePermission()
        {
            var exceptionCaught = false;
            try
            {
                var id = 1;
                var permissionName = "Read";
                var resourceType = "Program";
                var user = GetTestUser();
                user.UserHasPermission = false;
                userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
                userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);
                permissionStore.Setup(x => x.GetPermissionIdByName(It.IsAny<string>())).Returns(1);
                permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
                permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(id);

                var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, id);
                var actionContext = ContextUtil.CreateActionContext();
                actionContext.RequestContext.Principal = Thread.CurrentPrincipal;

                var cts = new CancellationTokenSource();
                await attribute.OnActionExecutingAsync(actionContext, cts.Token);
            }
            catch (HttpResponseException e)
            {
                exceptionCaught = true;
                Assert.AreEqual(HttpStatusCode.Unauthorized, e.Response.StatusCode);
            }
            Assert.IsTrue(exceptionCaught);
            //make sure we fall into checking permissions
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());

        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_ResourceTypeIsNotKnown()
        {
            var id = 1;
            var actionArgument = "id";
            var permissionName = "Read";
            var resourceType = "Program";
            var user = GetTestUser();
            user.UserHasPermission = true;
            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            permissionStore.Setup(x => x.GetPermissionIdByName(It.IsAny<string>())).Returns(1);
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(default(int?));
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(id);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, actionArgument);
            var actionContext = ContextUtil.CreateActionContext();
            actionContext.RequestContext.Principal = Thread.CurrentPrincipal;
            actionContext.ActionArguments.Add(actionArgument, id);

            var cts = new CancellationTokenSource();
            var exceptionCaught = false;
            try
            {
                await attribute.OnActionExecutingAsync(actionContext, cts.Token);
            }
            catch (NotSupportedException e)
            {
                exceptionCaught = true;
                Assert.AreEqual(String.Format("The resource type name [{0}] does not have a matching resource id in CAM.", resourceType), e.Message);
            }
            Assert.IsTrue(exceptionCaught);

        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_ResourceIdDoesNotExist()
        {
            var id = 1;
            var actionArgument = "id";
            var permissionName = "Read";
            var resourceType = "Program";
            var user = GetTestUser();
            user.UserHasPermission = true;
            var warningMessage = string.Empty;
            Action<string, object[]> warningCallback = (fmt, objParams) =>
            {
                warningMessage = String.Format(fmt, objParams);
            };

            logger.Setup(x => x.Warning(It.IsAny<string>(), It.IsAny<object[]>())).Callback(warningCallback);

            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(default(int?));
            permissionStore.Setup(x => x.GetPermissionIdByName(It.IsAny<string>())).Returns(1);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, actionArgument);
            var actionContext = ContextUtil.CreateActionContext();
            actionContext.RequestContext.Principal = Thread.CurrentPrincipal;
            actionContext.ActionArguments.Add(actionArgument, id);

            var cts = new CancellationTokenSource();
            await attribute.OnActionExecutingAsync(actionContext, cts.Token);
            logger.Verify(x => x.Warning(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once());
            var expectedWarningMessage = String.Format("User [{0}] granted access to resource of type [{1}] with foreign key of [{2}] because the object is NOT in the CAM resources.",
                user.GetUsername(),
                resourceType,
                id);
            Assert.AreEqual(expectedWarningMessage, warningMessage);
        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_ResourceIdIsZero()
        {
            var id = 1;
            var actionArgument = "id";
            var permissionName = "Read";
            var resourceType = "Program";
            var user = GetTestUser();
            var warningMessage = string.Empty;
            Action<string, object[]> warningCallback = (fmt, objParams) =>
            {
                warningMessage = String.Format(fmt, objParams);
            };

            logger.Setup(x => x.Warning(It.IsAny<string>(), It.IsAny<object[]>())).Callback(warningCallback);
            user.UserHasPermission = true;
            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(0);
            permissionStore.Setup(x => x.GetPermissionIdByName(It.IsAny<string>())).Returns(1);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, actionArgument);
            var actionContext = ContextUtil.CreateActionContext();
            actionContext.RequestContext.Principal = Thread.CurrentPrincipal;
            actionContext.ActionArguments.Add(actionArgument, id);

            var cts = new CancellationTokenSource();
            await attribute.OnActionExecutingAsync(actionContext, cts.Token);
            logger.Verify(x => x.Warning(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once());

            var expectedWarningMessage = String.Format("User [{0}] granted access to resource of type [{1}] with foreign key of [{2}] because the object is NOT in the CAM resources.",
                    user.GetUsername(),
                    resourceType,
                    id);
            Assert.AreEqual(expectedWarningMessage, warningMessage);

        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_RequestedPermissionDoesNotExist()
        {
            var id = 1;
            var actionArgument = "id";
            var permissionName = "Read";
            var resourceType = "Program";
            var user = GetTestUser();
            user.UserHasPermission = true;
            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            permissionStore.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            permissionStore.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(1);
            permissionStore.Setup(x => x.GetPermissionIdByName(It.IsAny<string>())).Returns(0);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);
            logger.Setup(x => x.Warning(It.IsAny<string>(), It.IsAny<object[]>()));

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, actionArgument);
            var actionContext = ContextUtil.CreateActionContext();
            actionContext.RequestContext.Principal = Thread.CurrentPrincipal;
            actionContext.ActionArguments.Add(actionArgument, id);
            var exceptionCaught = false;
            try
            {
                var cts = new CancellationTokenSource();
                await attribute.OnActionExecutingAsync(actionContext, cts.Token);
            }
            catch (NotSupportedException e)
            {
                Assert.AreEqual(String.Format("The requested permission [{0}] does not exist in CAM.", permissionName), e.Message);
                exceptionCaught = true;
            }
            Assert.IsTrue(exceptionCaught);


        }

        private TestUser GetTestUser()
        {
            var debugUser = new TestUser(new TraceLogger());
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(debugUser.GetClaims(), "Bearer"));
            Thread.CurrentPrincipal = claimsPrincipal;
            return debugUser;
        }
    }
}
