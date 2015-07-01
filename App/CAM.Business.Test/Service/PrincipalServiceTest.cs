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
using CAM.Business.Queries.Models;
using System.Collections.Generic;

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

        private List<ResourcePermissionDTO> GetAvailablePermissionsList(params CAM.Data.Permission[] permissions)
        {
            return permissions.ToList().Select(x => new ResourcePermissionDTO
            {
                PermissionId = x.PermissionId,
                PermissionName = x.PermissionName,
                PermissionDescription = x.PermissionDescription
            }).ToList();
        }

        #region Grant
        [TestMethod]
        public async Task TestGrantPermission_GranteeIsAUserAccount()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var grantee = new Principal
            {
                PrincipalId = 2
            };
            var user = new UserAccount
            {
                Principal = grantee,
                PrincipalId = grantee.PrincipalId
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            context.SetupActions.Add(() =>
            {
                context.Principals.Add(grantor);
                context.Principals.Add(grantee);
                context.Permissions.Add(permission);
                context.Resources.Add(resource);
                context.UserAccounts.Add(user);
                Assert.AreEqual(0, context.PermissionAssignments.Count());
            });
            
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            resourceService.Setup(x => x.GetResourcePermissions(It.IsAny<string>(), It.IsAny<int?>())).Returns(GetAvailablePermissionsList(permission));
            resourceService.Setup(x => x.GetResourcePermissionsAsync(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(GetAvailablePermissionsList(permission));

            
            Action tester = () =>
            {
                Assert.AreEqual(1, context.PermissionAssignments.Count());
                DateTimeOffset.Now.Should().BeCloseTo(context.UserAccounts.First().PermissionsRevisedOn.Value, 2000);
            };
            context.Revert();
            var grantedPermission = new GrantedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            service.GrantPermission(grantedPermission);
            tester();

            context.Revert();
            context.PermissionAssignments = new PermissionAssignmentTestDbSet();
            await service.GrantPermissionsAsync(grantedPermission);
            tester();
        }

        [TestMethod]
        public async Task TestGrantPermission_GranteeIsNotUserAccount()
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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            context.SetupActions.Add(() =>
            {
                context.Principals.Add(grantor);
                context.Principals.Add(grantee);
                context.Permissions.Add(permission);
                context.Resources.Add(resource);
                Assert.AreEqual(0, context.PermissionAssignments.Count());
            });
            
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            resourceService.Setup(x => x.GetResourcePermissions(It.IsAny<string>(), It.IsAny<int?>())).Returns(GetAvailablePermissionsList(permission));
            resourceService.Setup(x => x.GetResourcePermissionsAsync(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(GetAvailablePermissionsList(permission));
            
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
            context.Revert();
            var grantedPermission = new GrantedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            service.GrantPermission(grantedPermission);
            tester();

            context.Revert();
            context.PermissionAssignments = new PermissionAssignmentTestDbSet();
            await service.GrantPermissionsAsync(grantedPermission);
            tester();
        }

        [TestMethod]
        public async Task TestGrantPermission_PermissionIsNotAResourcePermission()
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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            resourceService.Setup(x => x.GetResourcePermissions(It.IsAny<string>(), It.IsAny<int?>())).Returns(GetAvailablePermissionsList());
            resourceService.Setup(x => x.GetResourcePermissionsAsync(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(GetAvailablePermissionsList());

            Assert.AreEqual(0, context.PermissionAssignments.Count());


            var grantedPermission = new GrantedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            service.Invoking(x => x.GrantPermission(grantedPermission)).ShouldThrow<NotSupportedException>()
                .WithMessage(String.Format("The requested permission with id [{0}] is not a valid permission for the resource.", permission.PermissionId));

            Func<Task> f = async () =>
            {
                await service.GrantPermissionsAsync(grantedPermission);
            };
            f.ShouldThrow<NotSupportedException>()
                .WithMessage(String.Format("The requested permission with id [{0}] is not a valid permission for the resource.", permission.PermissionId));

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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            var permissionAssignment = new PermissionAssignment
            {
                PrincipalId = grantee.PrincipalId,
                PermissionId = permission.PermissionId,
                ResourceId = resource.ResourceId,
                IsAllowed = false
            };
            context.SetupActions.Add(() =>
            {
                context.Principals.Add(grantor);
                context.Principals.Add(grantee);
                context.Permissions.Add(permission);
                context.Resources.Add(resource);
                context.PermissionAssignments.Add(permissionAssignment);
                permissionAssignment.IsAllowed = false;
                Assert.AreEqual(1, context.PermissionAssignments.Count());
            });
            
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);

            
            Action tester = () =>
            {
                Assert.AreEqual(1, context.PermissionAssignments.Count());
                Assert.IsTrue(context.PermissionAssignments.First().IsAllowed);
            };
            context.Revert();
            var grantedPermission = new GrantedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            service.GrantPermission(grantedPermission);

            context.Revert();            
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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            context.Principals.Add(grantor);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
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
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            var grantedPermission = new GrantedPermission(grantee.PrincipalId, 0, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            //invoking async
            Func<Task> grantAction = async () =>
            {
                await service.GrantPermissionsAsync(grantedPermission);
            };
            service.Invoking(x => x.GrantPermission(grantedPermission)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The permission with id [{0}] was not found.", grantedPermission.PermissionId));

            grantAction.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The permission with id [{0}] was not found.", grantedPermission.PermissionId));
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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            ForeignResourceCache foreignResourceCache = null;
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
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
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            resourceService.Setup(x => x.GetResourcePermissions(It.IsAny<string>(), It.IsAny<int?>())).Returns(GetAvailablePermissionsList(permission));
            resourceService.Setup(x => x.GetResourcePermissionsAsync(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(GetAvailablePermissionsList(permission));

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
        public async Task TestRevokePermission_PrincipalIsAUserAccount()
        {
            var grantor = new Principal
            {
                PrincipalId = 1,
            };
            var grantee = new Principal
            {
                PrincipalId = 2
            };
            var userAccount = new UserAccount
            {
                PrincipalId = grantee.PrincipalId,
                Principal = grantee
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            context.SetupActions.Add(() =>
            {
                userAccount.PermissionsRevisedOn = null;
                context.Principals.Add(grantor);
                context.Principals.Add(grantee);
                context.Permissions.Add(permission);
                context.Resources.Add(resource);
                context.UserAccounts.Add(userAccount);
                Assert.AreEqual(0, context.PermissionAssignments.Count());
            });
            
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            resourceService.Setup(x => x.GetResourcePermissions(It.IsAny<string>(), It.IsAny<int?>())).Returns(GetAvailablePermissionsList(permission));
            resourceService.Setup(x => x.GetResourcePermissionsAsync(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(GetAvailablePermissionsList(permission));
            
            Action tester = () =>
            {
                Assert.AreEqual(1, context.UserAccounts.Count());
                DateTimeOffset.Now.Should().BeCloseTo(context.UserAccounts.First().PermissionsRevisedOn.Value, 2000);
            };
            context.Revert();
            var revokedPermission = new RevokedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            service.RevokePermission(revokedPermission);
            tester();

            context.Revert();
            context.PermissionAssignments = new PermissionAssignmentTestDbSet();
            await service.RevokePermissionAsync(revokedPermission);
            tester();
        }

        [TestMethod]
        public async Task TestRevokePermission_PrincipalIsNotUserAccount()
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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            context.SetupActions.Add(() =>
            {
                context.Principals.Add(grantor);
                context.Principals.Add(grantee);
                context.Permissions.Add(permission);
                context.Resources.Add(resource);
                Assert.AreEqual(0, context.PermissionAssignments.Count());
            });
            
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            resourceService.Setup(x => x.GetResourcePermissions(It.IsAny<string>(), It.IsAny<int?>())).Returns(GetAvailablePermissionsList(permission));
            resourceService.Setup(x => x.GetResourcePermissionsAsync(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(GetAvailablePermissionsList(permission));

            
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
            context.Revert();
            var revokedPermission = new RevokedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            service.RevokePermission(revokedPermission);
            tester();

            context.Revert();
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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            var permissionAssignment = new PermissionAssignment
            {
                PrincipalId = grantee.PrincipalId,
                PermissionId = permission.PermissionId,
                ResourceId = resource.ResourceId,
                IsAllowed = true
            };
            context.SetupActions.Add(() =>
            {
                context.Principals.Add(grantor);
                context.Principals.Add(grantee);
                context.Permissions.Add(permission);
                context.Resources.Add(resource);
                context.PermissionAssignments.Add(permissionAssignment);
                permissionAssignment.IsAllowed = true;
                Assert.AreEqual(1, context.PermissionAssignments.Count());
            });
            
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            resourceService.Setup(x => x.GetResourcePermissions(It.IsAny<string>(), It.IsAny<int?>())).Returns(GetAvailablePermissionsList(permission));
            resourceService.Setup(x => x.GetResourcePermissionsAsync(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(GetAvailablePermissionsList(permission));
            
            Action tester = () =>
            {
                Assert.AreEqual(1, context.PermissionAssignments.Count());
                Assert.IsFalse(context.PermissionAssignments.First().IsAllowed);
            };

            context.Revert();
            var revokedPermission = new RevokedPermission(grantee.PrincipalId, permission.PermissionId, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            service.RevokePermission(revokedPermission);

            context.Revert();
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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            context.Principals.Add(grantor);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
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
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Resources.Add(resource);
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);
            var revokedPermission = new RevokedPermission(grantee.PrincipalId, 0, resource.ForeignResourceId, resourceType.Value, grantor.PrincipalId);
            //invoking async
            Func<Task> revokeAction = async () =>
            {
                await service.RevokePermissionAsync(revokedPermission);
            };
            service.Invoking(x => x.RevokePermission(revokedPermission)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The permission with id [{0}] was not found.", revokedPermission.PermissionId));

            revokeAction.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The permission with id [{0}] was not found.", revokedPermission.PermissionId));
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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            context.Principals.Add(grantor);
            context.Principals.Add(grantee);
            context.Permissions.Add(permission);
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(default(ForeignResourceCache));
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(ForeignResourceCache));
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
                PermissionId = CAM.Data.Permission.EditOffice.Id,
                PermissionName = CAM.Data.Permission.EditOffice.Value
            };
            var resourceType = ResourceType.Program;
            var resource = new Resource
            {
                ResourceId = 8,
                ForeignResourceId = 10,
                ResourceTypeId = resourceType.Id,
            };
            var foreignResourceCache = new ForeignResourceCache(resource.ForeignResourceId, resource.ResourceId, resource.ResourceTypeId, null, null, null);
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
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);

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

        #region Delete

        [TestMethod]
        public async Task TestDeletePermission_PrincipalIsUserAccount()
        {
            var foreignResourceId = 1;
            var user = new UserAccount
            {
                PrincipalId = 2
            };
            var permissionAssignment = new PermissionAssignment
            {
                PermissionId = 1,
                PrincipalId = user.PrincipalId,
                ResourceId = 3
            };
            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, permissionAssignment.ResourceId, 0, null, null, null);
            context.SetupActions.Add(() =>
            {
                user.PermissionsRevisedOn = null;
                context.PermissionAssignments.Add(permissionAssignment);
                context.UserAccounts.Add(user);
            });
            
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);

            var deletedPermission = new DeletedPermission(permissionAssignment.PrincipalId, foreignResourceId, permissionAssignment.PermissionId, ResourceType.Project.Value);

            context.Revert();
            Assert.AreEqual(1, context.PermissionAssignments.Count());
            service.DeletePermission(deletedPermission);
            Assert.AreEqual(0, context.PermissionAssignments.Count());
            DateTimeOffset.UtcNow.Should().BeCloseTo(user.PermissionsRevisedOn.Value, 2000);

            context.Revert();
            Assert.AreEqual(1, context.PermissionAssignments.Count());
            await service.DeletePermissionAsync(deletedPermission);
            Assert.AreEqual(0, context.PermissionAssignments.Count());
            DateTimeOffset.UtcNow.Should().BeCloseTo(user.PermissionsRevisedOn.Value, 2000);
        }

        [TestMethod]
        public async Task TestDeletePermission_PrincipalIsNotUserAccount()
        {
            var foreignResourceId = 1;
            var principal = new Principal
            {
                PrincipalId = 2
            };
            var permissionAssignment = new PermissionAssignment
            {
                PermissionId = 1,
                PrincipalId = principal.PrincipalId,
                ResourceId = 3
            };
            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, permissionAssignment.ResourceId, 0, null, null, null);
            context.SetupActions.Add(() =>
            {
                context.PermissionAssignments.Add(permissionAssignment);
                context.Principals.Add(principal);
            });

            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);

            var deletedPermission = new DeletedPermission(permissionAssignment.PrincipalId, foreignResourceId, permissionAssignment.PermissionId, ResourceType.Project.Value);

            context.Revert();
            Assert.AreEqual(1, context.PermissionAssignments.Count());
            service.DeletePermission(deletedPermission);
            Assert.AreEqual(0, context.PermissionAssignments.Count());

            context.Revert();
            Assert.AreEqual(1, context.PermissionAssignments.Count());
            await service.DeletePermissionAsync(deletedPermission);
            Assert.AreEqual(0, context.PermissionAssignments.Count());
        }

        [TestMethod]
        public void TestDeletePermission_ForeignResourceByIdDoesNotExist()
        {
            var user = new UserAccount
            {
                PrincipalId = 2
            };
            context.UserAccounts.Add(user);
            ForeignResourceCache foreignResourceCache = null;
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);

            var deletedPermission = new DeletedPermission(user.PrincipalId, 0, 0, ResourceType.Project.Value);
            Func<Task> f = async () =>
            {
                await service.DeletePermissionAsync(deletedPermission);
            };
            
            service.Invoking(x => x.DeletePermission(deletedPermission)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The foreign resource with id [{0}] and resource type [{1}] does not exist in CAM.", 0, ResourceType.Project.Value));
            f.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The foreign resource with id [{0}] and resource type [{1}] does not exist in CAM.", 0, ResourceType.Project.Value));
        }

        [TestMethod]
        public void TestDeletePermission_PermissionAssignmentDoesNotExist()
        {
            var foreignResourceId = 1;
            var user = new UserAccount
            {
                PrincipalId = 2
            };
            var permissionAssignment = new PermissionAssignment
            {
                PermissionId = 1,
                PrincipalId = user.PrincipalId,
                ResourceId = 3
            };
            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, permissionAssignment.ResourceId, 0, null, null, null);
            context.PermissionAssignments.Add(permissionAssignment);
            context.UserAccounts.Add(user);
            resourceService.Setup(x => x.GetResourceByForeignResourceId(It.IsAny<int>(), It.IsAny<int>())).Returns(foreignResourceCache);
            resourceService.Setup(x => x.GetResourceByForeignResourceIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(foreignResourceCache);

            var deletedPermission = new DeletedPermission(user.PrincipalId, 0, 0, ResourceType.Project.Value);
            Func<Task> f = async () =>
            {
                await service.DeletePermissionAsync(deletedPermission);
            };

            service.Invoking(x => x.DeletePermission(deletedPermission)).ShouldThrow<ModelNotFoundException>()
                .WithMessage("The permission assignment was not found.");
            f.ShouldThrow<ModelNotFoundException>()
                .WithMessage("The permission assignment was not found.");
        }

        #endregion
    }
}
