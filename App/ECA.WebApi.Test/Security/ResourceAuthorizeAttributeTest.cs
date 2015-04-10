using System;
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

namespace ECA.WebApi.Test.Security
{
    public class TestUser : DebugWebApiUser
    {
        public int HasPermissionCount { get; set; }

        public TestUser(ILogger logger) : base(logger) { }

        public bool UserHasPermission { get; set; }

        public override bool HasPermission(ResourcePermission requestedPermission, System.Collections.Generic.IEnumerable<ResourcePermission> allUserPermissions)
        {
            this.HasPermissionCount++;
            return UserHasPermission;
        }
    }

    [TestClass]
    public class ResourceAuthorizeAttributeTest
    {
        private Mock<IUserCacheService> cacheService;
        private WebApiUserBase user;

        [TestInitialize]
        public void TestInit()
        {
            cacheService = new Mock<IUserCacheService>();

            ResourceAuthorizeAttribute.CacheServiceFactory = () =>
            {
                return cacheService.Object;
            };
            ResourceAuthorizeAttribute.LoggerFactory = () =>
            {
                return new TraceLogger();
            };
            ResourceAuthorizeAttribute.GetWebApiUser = () =>
            {
                return user;
            };
        }

        [TestMethod]
        public void TestConstructor_SingleActionPermission()
        {
            var actionArgument = "id";
            var permissionName = "Read";
            var resourceType = "Program";

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, actionArgument);
            Assert.AreEqual(1, attribute.ActionPermissions.Count());
            var firstPermission = attribute.ActionPermissions.First();
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
            Assert.AreEqual(1, attribute.ActionPermissions.Count());
            var firstPermission = attribute.ActionPermissions.First();
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
            Assert.AreEqual(2, attribute.ActionPermissions.Count());
        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_CheckPermissionsIterated()
        {
            var id = 1;
            var actionArgument = "id";
            var permissionName = "Read";
            var resourceType = "Program";
            var user = SetDebugUser();
            user.UserHasPermission = true;
            cacheService.Setup(x => x.GetUserCacheAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(new UserCache(user));

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, actionArgument);
            var actionContext = ContextUtil.CreateActionContext();
            actionContext.RequestContext.Principal = Thread.CurrentPrincipal;
            actionContext.ActionArguments.Add(actionArgument, id);

            var cts = new CancellationTokenSource();
            Assert.AreEqual(0, user.HasPermissionCount);
            await attribute.OnActionExecutingAsync(actionContext, cts.Token);
            Assert.AreEqual(1, user.HasPermissionCount);
        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_UserHasPermission()
        {
            var id = 1;
            var actionArgument = "id";
            var permissionName = "Read";
            var resourceType = "Program";
            var user = SetDebugUser();
            user.UserHasPermission = true;
            cacheService.Setup(x => x.GetUserCacheAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(new UserCache(user));

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, actionArgument);
            var actionContext = ContextUtil.CreateActionContext();
            actionContext.RequestContext.Principal = Thread.CurrentPrincipal;
            actionContext.ActionArguments.Add(actionArgument, id);

            var cts = new CancellationTokenSource();            
            await attribute.OnActionExecutingAsync(actionContext, cts.Token);
        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_UserDoesNotHavePermission()
        {
            var exceptionCaught = false;
            try
            {
                var id = 1;
                var actionArgument = "id";
                var permissionName = "Read";
                var resourceType = "Program";
                var user = SetDebugUser();
                user.UserHasPermission = false;
                cacheService.Setup(x => x.GetUserCacheAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(new UserCache(user));

                var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, actionArgument);
                var actionContext = ContextUtil.CreateActionContext();
                actionContext.RequestContext.Principal = Thread.CurrentPrincipal;
                actionContext.ActionArguments.Add(actionArgument, id);

                var cts = new CancellationTokenSource();
                await attribute.OnActionExecutingAsync(actionContext, cts.Token);
            }
            catch (HttpResponseException e)
            {
                exceptionCaught = true;
                Assert.AreEqual(HttpStatusCode.Unauthorized, e.Response.StatusCode);
            }
            Assert.IsTrue(exceptionCaught);
            
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public async Task TestConstructor_OnActionExecutingAsync_IdIsNotAnInteger()
        {
            var id = "hello";
            var actionArgument = "id";
            var permissionName = "Read";
            var resourceType = "Program";
            var user = SetDebugUser();
            user.UserHasPermission = false;
            cacheService.Setup(x => x.GetUserCacheAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(new UserCache(user));

            var attribute = new ResourceAuthorizeAttribute(permissionName, resourceType, actionArgument);
            var actionContext = ContextUtil.CreateActionContext();
            actionContext.RequestContext.Principal = Thread.CurrentPrincipal;
            actionContext.ActionArguments.Add(actionArgument, id);

            var cts = new CancellationTokenSource();
            await attribute.OnActionExecutingAsync(actionContext, cts.Token);
        }

        private TestUser SetDebugUser()
        {
            var debugUser = new TestUser(new TraceLogger());
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(debugUser.GetClaims(), "Bearer"));
            Thread.CurrentPrincipal = claimsPrincipal;
            this.user = debugUser;
            return debugUser;
        }
    }
}
