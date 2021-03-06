﻿using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using System.Threading.Tasks;
using Moq;
using System.Web.Http.Controllers;
using System.Threading;
using System.Security.Claims;
using System.Web.Http;
using System.Net;
using ECA.WebApi.Models.Query;
using CAM.Business.Service;
using System.Collections.Generic;
using CAM.Data;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class ResourceAuthorizeAttributeTest
    {
        private Mock<IUserProvider> userProvider;
        private Mock<IPermissionService> permissionService;
        private Mock<IResourceService> resourceService;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            permissionService = new Mock<IPermissionService>();
            resourceService = new Mock<IResourceService>();
            ResourceAuthorizeAttribute.ResourceServiceFactory = (msg) =>
            {
                return resourceService.Object;
            };
            ResourceAuthorizeAttribute.UserProviderFactory = (msg) =>
            {
                return userProvider.Object;
            };
            ResourceAuthorizeAttribute.PermissionServiceFactory = (msg) =>
            {
                return permissionService.Object;
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
        public async Task TestConstructor_OnActionExecutingAsync_UserHasPermission()
        {
            var permissionModel = new PermissionModel
            {
                Id = Permission.ViewProgram.Id,
                Name = Permission.ViewProgram.Value
            };
            var id = 1;
            var user = GetTestUser();
            var foreignResourceCache = new ForeignResourceCache(0, 1, 0, null, null, null);
            
            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            permissionService.Setup(x => x.HasPermission(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<List<IPermission>>())).Returns(true);
            permissionService.Setup(x => x.GetPermissionByNameAsync(It.IsAny<string>())).ReturnsAsync(permissionModel);

            var attribute = new ResourceAuthorizeAttribute(permissionModel.Name, ResourceType.Program.Value, id);
            var actionContext = ContextUtil.CreateActionContext();
            actionContext.RequestContext.Principal = Thread.CurrentPrincipal;

            var cts = new CancellationTokenSource();
            await attribute.OnActionExecutingAsync(actionContext, cts.Token);
            Assert.AreEqual(AuthorizationResult.Allowed, attribute.GetAuthorizationResult());

        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_UserIsNotValid()
        {   
            var permissionModel = new PermissionModel
            {
                Id = Permission.ViewProgram.Id,
                Name = Permission.ViewProgram.Value
            };
            var id = 1;
            var user = GetTestUser();
            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(false);
            permissionService.Setup(x => x.HasPermission(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<List<IPermission>>())).Returns(true);

            var attribute = new ResourceAuthorizeAttribute(permissionModel.Name, ResourceType.Program.Value, id);
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
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }
            Assert.IsTrue(exceptionCaught);
            //make sure we do not fall into into checking permissions
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Never());
        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_UserDoesNotHavePermission()
        {
            var permissionModel = new PermissionModel
            {
                Id = Permission.ViewProgram.Id,
                Name = Permission.ViewProgram.Value
            };
            var exceptionCaught = false;
            var id = 1;
            var foreignResourceCache = new ForeignResourceCache(0, id, 0, null, null, null);            
            var user = GetTestUser();
            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            permissionService.Setup(x => x.HasPermission(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<List<IPermission>>())).Returns(false);
            permissionService.Setup(x => x.GetPermissionByNameAsync(It.IsAny<string>())).ReturnsAsync(permissionModel);

            var attribute = new ResourceAuthorizeAttribute(permissionModel.Name, ResourceType.Program.Value, id);
            try
            {                
                var actionContext = ContextUtil.CreateActionContext();
                actionContext.RequestContext.Principal = Thread.CurrentPrincipal;

                var cts = new CancellationTokenSource();
                await attribute.OnActionExecutingAsync(actionContext, cts.Token);
            }
            catch (HttpResponseException e)
            {
                exceptionCaught = true;
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }
            Assert.IsTrue(exceptionCaught);
            Assert.AreEqual(AuthorizationResult.Denied, attribute.GetAuthorizationResult());
            //make sure we fall into checking permissions
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());

        }

        [TestMethod]
        public async Task TestConstructor_OnActionExecutingAsync_ResourceTypeIsNotKnown()
        {
            var id = 1;
            var foreignResourceCache = new ForeignResourceCache(0, id, 0, null, null, null);
            var actionArgument = "id";
            var permissionModel = new PermissionModel
            {
                Id = Permission.ViewProgram.Id,
                Name = Permission.ViewProgram.Value
            };
            var resourceType = "idontexist";
            var user = GetTestUser();
            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(default(int?));
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            permissionService.Setup(x => x.HasPermission(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<List<IPermission>>())).Returns(true);
            permissionService.Setup(x => x.GetPermissionByNameAsync(It.IsAny<string>())).ReturnsAsync(permissionModel);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);

            var attribute = new ResourceAuthorizeAttribute(permissionModel.Name, resourceType, actionArgument);
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
            var exceptionCaught = false;
            var id = 1;
            var actionArgument = "id";
            var permissionModel = new PermissionModel
            {
                Id = Permission.ViewProgram.Id,
                Name = Permission.ViewProgram.Value
            };
            var user = GetTestUser();
            var warningMessage = string.Empty;

            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            resourceService.Setup(x => x.GetResourceTypeId(It.IsAny<string>())).Returns(1);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(ForeignResourceCache));
            permissionService.Setup(x => x.HasPermission(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<List<IPermission>>())).Returns(true);
            permissionService.Setup(x => x.GetPermissionByNameAsync(It.IsAny<string>())).ReturnsAsync(permissionModel);
            userProvider.Setup(x => x.IsUserValidAsync(It.IsAny<IWebApiUser>())).ReturnsAsync(true);

            var attribute = new ResourceAuthorizeAttribute(permissionModel.Name, ResourceType.Program.Value, actionArgument);
            try
            {
                
                var actionContext = ContextUtil.CreateActionContext();
                actionContext.RequestContext.Principal = Thread.CurrentPrincipal;
                actionContext.ActionArguments.Add(actionArgument, id);
                var cts = new CancellationTokenSource();
                await attribute.OnActionExecutingAsync(actionContext, cts.Token);
            }
            catch (HttpResponseException e)
            {
                exceptionCaught = true;
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }
            Assert.IsTrue(exceptionCaught);
            Assert.AreEqual(AuthorizationResult.ResourceDoesNotExist, attribute.GetAuthorizationResult());
            //make sure we fall into checking permissions
            userProvider.Verify(x => x.GetPermissionsAsync(It.IsAny<IWebApiUser>()), Times.Once());
        }

        private DebugWebApiUser GetTestUser()
        {
            var debugUser = new DebugWebApiUser();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(debugUser.GetClaims(), "Bearer"));
            Thread.CurrentPrincipal = claimsPrincipal;
            return debugUser;
        }
    }
}
