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
        public void TestConstructor_SingleModelPermission()
        {
            var modelType = typeof(SorterBindingModel);
            var property = "direction";
            var permissionName = "Read";
            var resourceType = "Program";

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, modelType, property);
            Assert.AreEqual(1, attribute.Permissions.Count());
            Assert.IsInstanceOfType(attribute.Permissions.First(), typeof(ModelPermission));
            var modelPermission = (ModelPermission)attribute.Permissions.First();
            Assert.AreEqual(modelType, modelPermission.ModelType);
            Assert.AreEqual(property, modelPermission.Property);
            Assert.AreEqual(permissionName, modelPermission.PermissionName);
            Assert.AreEqual(resourceType, modelPermission.ResourceType);
        }

        [TestMethod]
        public void TestConstructor_SingleActionPermission()
        {
            var actionArgument = "id";
            var permissionName = "Read";
            var resourceType = "Program";

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, actionArgument);
            Assert.AreEqual(1, attribute.Permissions.Count());
            Assert.IsInstanceOfType(attribute.Permissions.First(), typeof(ActionPermission));
            var firstPermission = (ActionPermission)attribute.Permissions.First();
            Assert.AreEqual(actionArgument, firstPermission.ArgumentName);
            Assert.AreEqual(permissionName, firstPermission.PermissionName);
            Assert.AreEqual(resourceType, firstPermission.ResourceType);
        }

        [TestMethod]
        public void TestConstructor_SingleActionPermission_DefaultIdActionArgument()
        {
            var permissionName = "Read";
            var resourceType = "Program";

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType);
            Assert.AreEqual(1, attribute.Permissions.Count());
            Assert.IsInstanceOfType(attribute.Permissions.First(), typeof(ActionPermission));
            var firstPermission = (ActionPermission)attribute.Permissions.First();
            Assert.AreEqual(ResourceAuthorizeAttribute.DEFAULT_ID_ARGUMENT_NAME, firstPermission.ArgumentName);
        }

        [TestMethod]
        public void TestConstructor_MultipleActionPermissions()
        {
            var actionPermissionStringFormat = "{0}:{1}({2})";
            var actionArgument1 = "id";
            var permissionName1 = "Read";
            var resourceType1 = "Program";
            var permissionAsString1 = String.Format(actionPermissionStringFormat, permissionName1, resourceType1, actionArgument1);

            var actionArgument2 = "projectId";
            var permissionName2 = "Edit";
            var resourceType2 = "Project";
            var permissionAsString2 = String.Format(actionPermissionStringFormat, permissionName2, resourceType2, actionArgument2);

            var attribute = new ResourceAuthorizeAttribute(String.Join(", ", permissionAsString1, permissionAsString2));
            Assert.AreEqual(2, attribute.Permissions.Count());
        }

        [TestMethod]
        public void TestConstructor_StaticPermission()
        {
            var permissionName = "read";
            var resourceType = "program";
            var id = 1;
            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, id);
            Assert.AreEqual(1, attribute.Permissions.Count());
            Assert.IsInstanceOfType(attribute.Permissions.First(), typeof(StaticPermission));
            var staticPermission = (StaticPermission)attribute.Permissions.First();
            Assert.AreEqual(permissionName, staticPermission.PermissionName);
            Assert.AreEqual(resourceType, staticPermission.ResourceType);
            Assert.AreEqual(id, staticPermission.ResourceId);

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
            logger.Verify(x => x.Warning(It.IsAny<string>(), It.IsAny<object[]>()),
                String.Format("The resource type [{0}] with id [{1}] was not found in the permission store, by default access is currently granted.", resourceType, id));
            
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
