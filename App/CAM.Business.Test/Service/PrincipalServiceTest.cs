using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Service;
using Moq;
using System.Threading.Tasks;
using CAM.Business.Model;
using CAM.Data;
using ECA.Core.Exceptions;

namespace CAM.Business.Test.Service
{
    [TestClass]
    public class PrincipalServiceTest
    {
        private PrincipalService service;
        private TestInMemoryCamModel context;
        private Mock<IResourceService> resourceService;

        [TestInitialize]
        public void TestInit()
        {
            resourceService = new Mock<IResourceService>();
            context = new TestInMemoryCamModel();
            service = new PrincipalService(context, resourceService.Object);
        }

        #region Grant
        [TestMethod]
        public async Task TestGrantPermission()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var grantee = new Principal
            {
                PrincipalId = 2
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editoffice.Id,
                PermissionName = CAM.Data.Permission.Editoffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resource.ResourceId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resource.ResourceId);

            Assert.AreEqual(0, context.PermissionAssignments.Count());
            Action tester = () =>
            {
                Assert.AreEqual(1, context.PermissionAssignments.Count());
                var firstAssignment = context.PermissionAssignments.First();
                Assert.IsTrue(firstAssignment.IsAllowed);
                Assert.AreEqual(grantor.PrincipalId, firstAssignment.AssignedBy);
                Assert.AreEqual(permission.PermissionId, firstAssignment.PermissionId);
                Assert.AreEqual(grantee.PrincipalId, firstAssignment.PrincipalId);
                Assert.AreEqual(resource.ResourceId, firstAssignment.ResourceId);
                DateTimeOffset.Now.Should().BeCloseTo(firstAssignment.AssignedOn, 2000);
            };

            var grantedPermission = new GrantedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            service.GrantPermission(grantedPermission);
            tester();

            context.PermissionAssignments = new PermissionAssignmentTestDbSet();
            await service.GrantPermissionsAsync(grantedPermission);
            tester();
        }

        [TestMethod]
        public async Task TestGrantPermission_PermissionIsAlreadyGrantedButNotAllowed()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var grantee = new Principal
            {
                PrincipalId = 2
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editoffice.Id,
                PermissionName = CAM.Data.Permission.Editoffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var permissionAssignment = new PermissionAssignment
            {
                PrincipalId = grantee.PrincipalId,
                PermissionId = permission.PermissionId,
                ResourceId = resource.ResourceId,
                IsAllowed = false
            };
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            context.PermissionAssignments.Add(permissionAssignment);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resource.ResourceId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resource.ResourceId);

            Assert.AreEqual(1, context.PermissionAssignments.Count());
            Action tester = () =>
            {
                Assert.AreEqual(1, context.PermissionAssignments.Count());
                Assert.IsTrue(context.PermissionAssignments.First().IsAllowed);
            };

            var grantedPermission = new GrantedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            service.GrantPermission(grantedPermission);
            permissionAssignment.IsAllowed = false;

            await service.GrantPermissionsAsync(grantedPermission);
            tester();
        }

        [TestMethod]
        public void TestGrantPermission_GranteeDoesNotExist()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editoffice.Id,
                PermissionName = CAM.Data.Permission.Editoffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            context.Principals.Add(grantor);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resource.ResourceId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resource.ResourceId);
            var grantedPermission = new GrantedPermission(0, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);

            //invoking async
            Func<Task> grantAction = async () =>
            {
                await service.GrantPermissionsAsync(grantedPermission);
            };
            service.Invoking(x => x.GrantPermission(grantedPermission)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The user with id [{0}] being granted the permission was not found.", grantedPermission.GranteePrincipalId));

            grantAction.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The user with id [{0}] being granted the permission was not found.", grantedPermission.GranteePrincipalId));

        }

        [TestMethod]
        public void TestGrantPermission_GrantorDoesNotExist()
        {
            var grantee = new Principal
            {
                PrincipalId = 1,
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editoffice.Id,
                PermissionName = CAM.Data.Permission.Editoffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resource.ResourceId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resource.ResourceId);
            var grantedPermission = new GrantedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, 0);

            //invoking async
            Func<Task> grantAction = async () =>
            {
                await service.GrantPermissionsAsync(grantedPermission);
            };
            service.Invoking(x => x.GrantPermission(grantedPermission)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The user with id [{0}] granting the permission could not be found.", grantedPermission.Audit.UserId));

            grantAction.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The user with id [{0}] granting the permission could not be found.", grantedPermission.Audit.UserId));

        }

        [TestMethod]
        public void TestGrantPermission_PermissionDoesNotExist()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var grantee = new Principal
            {
                PrincipalId = 2
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resource.ResourceId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resource.ResourceId);
            var grantedPermission = new GrantedPermission(grantee.PrincipalId, 0, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            //invoking async
            Func<Task> grantAction = async () =>
            {
                await service.GrantPermissionsAsync(grantedPermission);
            };
            service.Invoking(x => x.GrantPermission(grantedPermission)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The permission with id [{0}] was found.", grantedPermission.PermissionId));

            grantAction.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The permission with id [{0}] was found.", grantedPermission.PermissionId));
        }

        [TestMethod]
        public void TestGrantPermission_ResourceDoesNotExist()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var grantee = new Principal
            {
                PrincipalId = 2
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editoffice.Id,
                PermissionName = CAM.Data.Permission.Editoffice.Value
            };
            var resourceType = ResourceType.Program;
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(default(int?));
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(int?));
            var grantedPermission = new GrantedPermission(grantee.PrincipalId, permission.PermissionId, 0, resourceType.Value, grantor.PrincipalId);
            //invoking async
            Func<Task> grantAction = async () =>
            {
                await service.GrantPermissionsAsync(grantedPermission);
            };
            service.Invoking(x => x.GrantPermission(grantedPermission)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The foreign resource with id [{0}] and resource type [{1}] does not exist in CAM.",
                    grantedPermission.ForeignResourceId,
                    grantedPermission.ResourceTypeAsString));

            grantAction.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The foreign resource with id [{0}] and resource type [{1}] does not exist in CAM.",
                    grantedPermission.ForeignResourceId,
                    grantedPermission.ResourceTypeAsString));
        }

        [TestMethod]
        public void TestGrantPermission_MultiplePermissionsExist()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var grantee = new Principal
            {
                PrincipalId = 2
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editoffice.Id,
                PermissionName = CAM.Data.Permission.Editoffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var permissionAssignment1 = new PermissionAssignment
            {
                PrincipalId = grantee.PrincipalId,
                PermissionId = permission.PermissionId,
                ResourceId = resource.ResourceId,
                IsAllowed = false
            };
            var permissionAssignment2 = new PermissionAssignment
            {
                PrincipalId = grantee.PrincipalId,
                PermissionId = permission.PermissionId,
                ResourceId = resource.ResourceId,
                IsAllowed = false
            };
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            context.PermissionAssignments.Add(permissionAssignment1);
            context.PermissionAssignments.Add(permissionAssignment2);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resource.ResourceId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resource.ResourceId);

            var grantedPermission = new GrantedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);

            Func<Task> grantAction = async () =>
            {
                await service.GrantPermissionsAsync(grantedPermission);
            };
            service.Invoking(x => x.GrantPermission(grantedPermission)).ShouldThrow<NotSupportedException>()
                .WithMessage("There should not be more than one permission assignment to set is allowed true.");
            grantAction.ShouldThrow<NotSupportedException>()
                .WithMessage("There should not be more than one permission assignment to set is allowed true.");

        }
        #endregion

        #region Revoke
        [TestMethod]
        public async Task TestRevokePermission()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var grantee = new Principal
            {
                PrincipalId = 2
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editoffice.Id,
                PermissionName = CAM.Data.Permission.Editoffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resource.ResourceId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resource.ResourceId);

            Assert.AreEqual(0, context.PermissionAssignments.Count());
            Action tester = () =>
            {
                Assert.AreEqual(1, context.PermissionAssignments.Count());
                var firstAssignment = context.PermissionAssignments.First();
                Assert.IsFalse(firstAssignment.IsAllowed);
                Assert.AreEqual(grantor.PrincipalId, firstAssignment.AssignedBy);
                Assert.AreEqual(permission.PermissionId, firstAssignment.PermissionId);
                Assert.AreEqual(grantee.PrincipalId, firstAssignment.PrincipalId);
                Assert.AreEqual(resource.ResourceId, firstAssignment.ResourceId);
                DateTimeOffset.Now.Should().BeCloseTo(firstAssignment.AssignedOn, 2000);
            };

            var revokedPermission = new RevokedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            service.RevokePermission(revokedPermission);
            tester();

            context.PermissionAssignments = new PermissionAssignmentTestDbSet();
            await service.RevokePermissionAsync(revokedPermission);
            tester();
        }

        [TestMethod]
        public async Task TestRevokePermission_PermissionIsAlreadyGrantedButAllowed()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var grantee = new Principal
            {
                PrincipalId = 2
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editoffice.Id,
                PermissionName = CAM.Data.Permission.Editoffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var permissionAssignment = new PermissionAssignment
            {
                PrincipalId = grantee.PrincipalId,
                PermissionId = permission.PermissionId,
                ResourceId = resource.ResourceId,
                IsAllowed = true
            };
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            context.PermissionAssignments.Add(permissionAssignment);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resource.ResourceId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resource.ResourceId);

            Assert.AreEqual(1, context.PermissionAssignments.Count());
            Action tester = () =>
            {
                Assert.AreEqual(1, context.PermissionAssignments.Count());
                Assert.IsFalse(context.PermissionAssignments.First().IsAllowed);
            };

            var revokedPermission = new RevokedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            service.RevokePermission(revokedPermission);
            permissionAssignment.IsAllowed = false;

            await service.RevokePermissionAsync(revokedPermission);
            tester();
        }

        [TestMethod]
        public void TestRevokePermission_GranteeDoesNotExist()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editoffice.Id,
                PermissionName = CAM.Data.Permission.Editoffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            context.Principals.Add(grantor);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resource.ResourceId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resource.ResourceId);
            var revokePermission = new RevokedPermission(0, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);

            //invoking async
            Func<Task> revokeAction = async () =>
            {
                await service.RevokePermissionAsync(revokePermission);
            };
            service.Invoking(x => x.RevokePermission(revokePermission)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The user with id [{0}] being granted the permission was not found.", revokePermission.GranteePrincipalId));

            revokeAction.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The user with id [{0}] being granted the permission was not found.", revokePermission.GranteePrincipalId));

        }

        [TestMethod]
        public void TestRevokePermission_GrantorDoesNotExist()
        {
            var grantee = new Principal
            {
                PrincipalId = 1,
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editoffice.Id,
                PermissionName = CAM.Data.Permission.Editoffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resource.ResourceId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resource.ResourceId);
            var revokedPermission = new RevokedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, 0);

            //invoking async
            Func<Task> revokeAction = async () =>
            {
                await service.RevokePermissionAsync(revokedPermission);
            };
            service.Invoking(x => x.RevokePermission(revokedPermission)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The user with id [{0}] granting the permission could not be found.", revokedPermission.Audit.UserId));

            revokeAction.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The user with id [{0}] granting the permission could not be found.", revokedPermission.Audit.UserId));

        }

        [TestMethod]
        public void TestRevokePermission_PermissionDoesNotExist()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var grantee = new Principal
            {
                PrincipalId = 2
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resource.ResourceId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resource.ResourceId);
            var revokedPermission = new RevokedPermission(grantee.PrincipalId, 0, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            //invoking async
            Func<Task> revokeAction = async () =>
            {
                await service.RevokePermissionAsync(revokedPermission);
            };
            service.Invoking(x => x.RevokePermission(revokedPermission)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The permission with id [{0}] was found.", revokedPermission.PermissionId));

            revokeAction.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The permission with id [{0}] was found.", revokedPermission.PermissionId));
        }

        [TestMethod]
        public void TestRevokePermission_ResourceDoesNotExist()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var grantee = new Principal
            {
                PrincipalId = 2
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editoffice.Id,
                PermissionName = CAM.Data.Permission.Editoffice.Value
            };
            var resourceType = ResourceType.Program;
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(default(int?));
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(int?));
            var revokedPermission = new RevokedPermission(grantee.PrincipalId, permission.PermissionId, 0, resourceType.Value, grantor.PrincipalId);
            //invoking async
            Func<Task> revokeAction = async () =>
            {
                await service.RevokePermissionAsync(revokedPermission);
            };
            service.Invoking(x => x.RevokePermission(revokedPermission)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The foreign resource with id [{0}] and resource type [{1}] does not exist in CAM.",
                    revokedPermission.ForeignResourceId,
                    revokedPermission.ResourceTypeAsString));

            revokeAction.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The foreign resource with id [{0}] and resource type [{1}] does not exist in CAM.",
                    revokedPermission.ForeignResourceId,
                    revokedPermission.ResourceTypeAsString));
        }

        [TestMethod]
        public void TestRevokePermission_MultiplePermissionsExist()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var grantee = new Principal
            {
                PrincipalId = 2
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editoffice.Id,
                PermissionName = CAM.Data.Permission.Editoffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var permissionAssignment1 = new PermissionAssignment
            {
                PrincipalId = grantee.PrincipalId,
                PermissionId = permission.PermissionId,
                ResourceId = resource.ResourceId,
                IsAllowed = false
            };
            var permissionAssignment2 = new PermissionAssignment
            {
                PrincipalId = grantee.PrincipalId,
                PermissionId = permission.PermissionId,
                ResourceId = resource.ResourceId,
                IsAllowed = false
            };
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            context.PermissionAssignments.Add(permissionAssignment1);
            context.PermissionAssignments.Add(permissionAssignment2);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(resource.ResourceId);
            resourceService.Setup(x => x.GetResourceIdByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(resource.ResourceId);

            var revokedPermission = new RevokedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);

            Func<Task> revokeAction = async () =>
            {
                await service.RevokePermissionAsync(revokedPermission);
            };
            service.Invoking(x => x.RevokePermission(revokedPermission)).ShouldThrow<NotSupportedException>()
                .WithMessage("There should not be more than one permission assignment to set is allowed true.");
            revokeAction.ShouldThrow<NotSupportedException>()
                .WithMessage("There should not be more than one permission assignment to set is allowed true.");

        }
        #endregion
    }
}
