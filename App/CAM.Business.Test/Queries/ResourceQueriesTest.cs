using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Data;
using CAM.Business.Queries;
using ECA.Core.DynamicLinq;
using CAM.Business.Model;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;

namespace CAM.Business.Test.Queries
{
    [TestClass]
    public class ResourceQueriesTest
    {
        private TestInMemoryCamModel context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestInMemoryCamModel();
        }

        #region CreateGetResourceAuthorizationsByRoleQuery

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByRoleQuery_PrincipalHasRole()
        {
            var resourceId = 10;
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                ResourceId = resourceId,
                Role = role,
                RoleId = role.RoleId,
                AssignedOn = DateTimeOffset.UtcNow,
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByRoleQuery(context);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(roleResourcePermission.AssignedOn, firstResult.AssignedOn);
            Assert.AreEqual(true, firstResult.IsAllowed);
            Assert.AreEqual(false, firstResult.IsGrantedByInheritance);
            Assert.AreEqual(false, firstResult.IsGrantedByPermission);
            Assert.AreEqual(true, firstResult.IsGrantedByRole);
            Assert.AreEqual(permission.PermissionId, firstResult.PermissionId);
            Assert.AreEqual(principal.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(resourceId, firstResult.ResourceId);
            Assert.AreEqual(role.RoleId, firstResult.RoleId);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByRoleQuery_RoleIsInactive()
        {
            var resourceId = 10;
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = false
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                ResourceId = resourceId,
                Role = role,
                RoleId = role.RoleId,
                AssignedOn = DateTimeOffset.UtcNow,
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByRoleQuery(context);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByRoleQuery_PrincipalDoesNotHaveRole()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByRoleQuery(context);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByRoleQuery_RoleDoesNotHaveResourcePermission()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.PrincipalRoles.Add(principalRole);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByRoleQuery(context);
            Assert.AreEqual(0, results.Count());
        }
        #endregion

        #region CreateGetResourceAuthorizationsByInheritedRolePermissionsQuery
        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByInheritedRolePermissionsQuery_PrincipalHasRole()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var projectResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Program.Value,
                ResourceTypeId = ResourceType.Program.Id
            };
            var projectResource = new Resource
            {
                ResourceId = 1,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var programResource = new Resource
            {
                ResourceId = 2,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 20
            };
            projectResource.ParentResource = programResource;
            projectResource.ParentResourceId = programResource.ResourceId;
            programResource.ChildResources.Add(projectResource);
            var editProjectPermission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc",
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ParentResourceType = programResourceType,
                ParentResourceTypeId = programResourceType.ResourceTypeId,
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = editProjectPermission,
                PermissionId = editProjectPermission.PermissionId,
                Resource = programResource,
                ResourceId = programResource.ResourceId,
                Role = role,
                RoleId = role.RoleId,
                AssignedOn = DateTimeOffset.UtcNow,
            };
            var principalRole = new PrincipalRole
            {
                Role = role,
                RoleId = role.RoleId,
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            context.PrincipalRoles.Add(principalRole);
            context.Principals.Add(principal);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(projectResource);
            context.Resources.Add(programResource);
            context.Permissions.Add(editProjectPermission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByInheritedRolePermissionsQuery(context);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(roleResourcePermission.AssignedOn, firstResult.AssignedOn);
            Assert.AreEqual(true, firstResult.IsAllowed);
            Assert.AreEqual(true, firstResult.IsGrantedByInheritance);
            Assert.AreEqual(false, firstResult.IsGrantedByPermission);
            Assert.AreEqual(true, firstResult.IsGrantedByRole);
            Assert.AreEqual(editProjectPermission.PermissionId, firstResult.PermissionId);
            Assert.AreEqual(principal.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(projectResource.ResourceId, firstResult.ResourceId);
            Assert.AreEqual(role.RoleId, firstResult.RoleId);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByInheritedRolePermissionsQuery_PrincipalHasRole_RoleIsInactive()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var projectResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Program.Value,
                ResourceTypeId = ResourceType.Program.Id
            };
            var projectResource = new Resource
            {
                ResourceId = 1,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var programResource = new Resource
            {
                ResourceId = 2,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 20
            };
            projectResource.ParentResource = programResource;
            projectResource.ParentResourceId = programResource.ResourceId;
            programResource.ChildResources.Add(projectResource);
            var editProjectPermission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc",
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ParentResourceType = programResourceType,
                ParentResourceTypeId = programResourceType.ResourceTypeId,
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = false
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = editProjectPermission,
                PermissionId = editProjectPermission.PermissionId,
                Resource = programResource,
                ResourceId = programResource.ResourceId,
                Role = role,
                RoleId = role.RoleId,
                AssignedOn = DateTimeOffset.UtcNow,
            };
            var principalRole = new PrincipalRole
            {
                Role = role,
                RoleId = role.RoleId,
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            context.PrincipalRoles.Add(principalRole);
            context.Principals.Add(principal);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(projectResource);
            context.Resources.Add(programResource);
            context.Permissions.Add(editProjectPermission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByInheritedRolePermissionsQuery(context);
            Assert.AreEqual(0, results.Count());
        }
        
        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByInheritedRolePermissionsQuery_PrincipalDoesNotHaveRole()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;
            var projectResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Program.Value,
                ResourceTypeId = ResourceType.Program.Id
            };
            var projectResource = new Resource
            {
                ResourceId = 1,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var programResource = new Resource
            {
                ResourceId = 2,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 20
            };
            projectResource.ParentResource = programResource;
            projectResource.ParentResourceId = programResource.ResourceId;
            programResource.ChildResources.Add(projectResource);
            var editProjectPermission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = editProjectPermission,
                PermissionId = editProjectPermission.PermissionId,
                Resource = programResource,
                ResourceId = programResource.ResourceId,
                Role = role,
                RoleId = role.RoleId,
                AssignedOn = DateTimeOffset.UtcNow,
            };
            
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(projectResource);
            context.Resources.Add(programResource);
            context.Permissions.Add(editProjectPermission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByInheritedRolePermissionsQuery(context);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByInheritedRolePermissionsQuery_RoleDoesNotHaveResourcePermission()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.PrincipalRoles.Add(principalRole);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByInheritedRolePermissionsQuery(context);
            Assert.AreEqual(0, results.Count());
        }
        #endregion

        #region CreateGetResourceAuthorizationsByPermissionAssignmentsQuery
        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByPermissionAssignmentsQuery_PrincipalHasPermission()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow,
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);


            var results = ResourceQueries.CreateGetResourceAuthorizationsByPermissionAssignmentsQuery(context);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(permissionAssignment.AssignedOn, firstResult.AssignedOn);
            Assert.AreEqual(permissionAssignment.IsAllowed, firstResult.IsAllowed);
            Assert.AreEqual(true, firstResult.IsGrantedByPermission);
            Assert.AreEqual(false, firstResult.IsGrantedByInheritance);
            Assert.AreEqual(false, firstResult.IsGrantedByRole);
            Assert.AreEqual(permission.PermissionId, firstResult.PermissionId);
            Assert.AreEqual(principal.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(resource.ResourceId, firstResult.ResourceId);
            Assert.AreEqual(ResourceQueries.AUTHORIZATION_ASSIGNED_BY_PERMISSION_ROLE_ID, firstResult.RoleId);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByPermissionAssignmentsQuery_PermissionIsNotAllowed()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);


            var results = ResourceQueries.CreateGetResourceAuthorizationsByPermissionAssignmentsQuery(context);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(permissionAssignment.IsAllowed, firstResult.IsAllowed);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByPermissionAssignmentsQuery_UserDoesNotHavePermissionAssignment()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByPermissionAssignmentsQuery(context);
            Assert.AreEqual(0, results.Count());
        }
        #endregion

        #region CreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery
        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery_CheckProperties()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                PrincipalId = principal.PrincipalId,
                Principal = principal,
                DisplayName = "display",
                EmailAddress = "email"
            };
            var projectResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Program.Id,
                ResourceTypeName = ResourceType.Program.Value
            };
            var programResource = new Resource
            {
                ResourceId = 1,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 10
            };
            var projectResource = new Resource
            {
                ResourceId = 2,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 20
            };
            projectResource.ParentResourceId = programResource.ResourceId;
            projectResource.ParentResource = programResource;
            programResource.ChildResources.Add(projectResource);

            var viewProjectPermission = new Permission
            {
                PermissionId = Permission.ViewProject.Id,
                PermissionName = Permission.ViewProject.Value,
                PermissionDescription = "desc",
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ParentResourceType = programResourceType,
                ParentResourceTypeId = programResourceType.ResourceTypeId
            };
            var viewProjectPermissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = viewProjectPermission,
                PermissionId = viewProjectPermission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = programResource,
                ResourceId = programResource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow
            };
            context.UserAccounts.Add(userAccount);
            context.Principals.Add(principal);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(programResource);
            context.Resources.Add(projectResource);
            context.Permissions.Add(viewProjectPermission);
            context.PermissionAssignments.Add(viewProjectPermissionAssignment);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var viewPermissionAuthorization = results.Where(x => x.PermissionId == viewProjectPermission.PermissionId).FirstOrDefault();
            Assert.IsNotNull(viewPermissionAuthorization);
            Assert.AreEqual(viewProjectPermissionAssignment.AssignedOn, viewPermissionAuthorization.AssignedOn);
            Assert.IsTrue(viewPermissionAuthorization.IsAllowed);
            Assert.IsTrue(viewPermissionAuthorization.IsGrantedByInheritance);
            Assert.IsTrue(viewPermissionAuthorization.IsGrantedByPermission);
            Assert.IsFalse(viewPermissionAuthorization.IsGrantedByRole);
            Assert.AreEqual(viewProjectPermission.PermissionId, viewPermissionAuthorization.PermissionId);
            Assert.AreEqual(userAccount.PrincipalId, viewPermissionAuthorization.PrincipalId);
            Assert.AreEqual(projectResource.ResourceId, viewPermissionAuthorization.ResourceId);
            Assert.AreEqual(ResourceQueries.AUTHORIZATION_ASSIGNED_BY_PERMISSION_ROLE_ID, viewPermissionAuthorization.RoleId);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery_ParentResourceAllowed_ChildResourceRevoked()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                PrincipalId = principal.PrincipalId,
                Principal = principal,
                DisplayName = "display",
                EmailAddress = "email"
            };
            var projectResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Program.Id,
                ResourceTypeName = ResourceType.Program.Value
            };
            var programResource = new Resource
            {
                ResourceId = 1,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 10
            };
            var projectResource = new Resource
            {
                ResourceId = 2,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 20
            };
            projectResource.ParentResourceId = programResource.ResourceId;
            projectResource.ParentResource = programResource;
            programResource.ChildResources.Add(projectResource);

            var viewProjectPermission = new Permission
            {
                PermissionId = Permission.ViewProject.Id,
                PermissionName = Permission.ViewProject.Value,
                PermissionDescription = "desc",
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ParentResourceType = programResourceType,
                ParentResourceTypeId = programResourceType.ResourceTypeId
            };
            var viewProjectParentPermissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = viewProjectPermission,
                PermissionId = viewProjectPermission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = programResource,
                ResourceId = programResource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow
            };
            var revokedViewProjectParentPermissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = viewProjectPermission,
                PermissionId = viewProjectPermission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = projectResource,
                ResourceId = projectResource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow
            };
            context.UserAccounts.Add(userAccount);
            context.Principals.Add(principal);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(programResource);
            context.Resources.Add(projectResource);
            context.Permissions.Add(viewProjectPermission);
            context.PermissionAssignments.Add(viewProjectParentPermissionAssignment);
            context.PermissionAssignments.Add(revokedViewProjectParentPermissionAssignment);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var viewPermissionAuthorization = results.Where(x => x.PermissionId == viewProjectPermission.PermissionId).FirstOrDefault();
            Assert.IsNotNull(viewPermissionAuthorization);
            Assert.IsTrue(viewPermissionAuthorization.IsAllowed);
        }


        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery_ParentResourceRevoked_ChildResourceAllowed()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                PrincipalId = principal.PrincipalId,
                Principal = principal,
                DisplayName = "display",
                EmailAddress = "email"
            };
            var projectResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Program.Id,
                ResourceTypeName = ResourceType.Program.Value
            };
            var programResource = new Resource
            {
                ResourceId = 1,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 10
            };
            var projectResource = new Resource
            {
                ResourceId = 2,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 20
            };
            projectResource.ParentResourceId = programResource.ResourceId;
            projectResource.ParentResource = programResource;
            programResource.ChildResources.Add(projectResource);

            var viewProjectPermission = new Permission
            {
                PermissionId = Permission.ViewProject.Id,
                PermissionName = Permission.ViewProject.Value,
                PermissionDescription = "desc",
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ParentResourceType = programResourceType,
                ParentResourceTypeId = programResourceType.ResourceTypeId
            };
            var revokedViewProjectParentPermissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = viewProjectPermission,
                PermissionId = viewProjectPermission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = programResource,
                ResourceId = programResource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow
            };
            var allowedViewProjectParentPermissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = viewProjectPermission,
                PermissionId = viewProjectPermission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = projectResource,
                ResourceId = projectResource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow
            };
            context.UserAccounts.Add(userAccount);
            context.Principals.Add(principal);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(programResource);
            context.Resources.Add(projectResource);
            context.Permissions.Add(viewProjectPermission);
            context.PermissionAssignments.Add(revokedViewProjectParentPermissionAssignment);
            context.PermissionAssignments.Add(allowedViewProjectParentPermissionAssignment);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var viewPermissionAuthorization = results.Where(x => x.PermissionId == viewProjectPermission.PermissionId).FirstOrDefault();
            Assert.IsNotNull(viewPermissionAuthorization);
            Assert.IsFalse(viewPermissionAuthorization.IsAllowed);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery_InheritedPermissionIsNotForResourceType()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                PrincipalId = principal.PrincipalId,
                Principal = principal,
                DisplayName = "display",
                EmailAddress = "email"
            };
            var projectResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Program.Id,
                ResourceTypeName = ResourceType.Program.Value
            };
            var programResource = new Resource
            {
                ResourceId = 1,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 10
            };
            var projectResource = new Resource
            {
                ResourceId = 2,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 20
            };
            projectResource.ParentResourceId = programResource.ResourceId;
            projectResource.ParentResource = programResource;
            programResource.ChildResources.Add(projectResource);

            var viewProgramPermission = new Permission
            {
                PermissionId = Permission.ViewProgram.Id,
                PermissionName = Permission.ViewProgram.Value,
                PermissionDescription = "desc",
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
            };
            var viewProgramPermissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = viewProgramPermission,
                PermissionId = viewProgramPermission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = programResource,
                ResourceId = programResource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow
            };
            context.UserAccounts.Add(userAccount);
            context.Principals.Add(principal);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(programResource);
            context.Resources.Add(projectResource);
            context.Permissions.Add(viewProgramPermission);
            context.PermissionAssignments.Add(viewProgramPermissionAssignment);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery(context).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery_TwoPermissionsInherited()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                PrincipalId = principal.PrincipalId,
                Principal = principal
            };
            var projectResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Program.Id,
                ResourceTypeName = ResourceType.Program.Value
            };
            var programResource = new Resource
            {
                ResourceId = 1,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 1
            };
            var projectResource = new Resource
            {
                ResourceId = 2,
                ResourceType= projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 2
            };
            projectResource.ParentResourceId = programResource.ResourceId;
            projectResource.ParentResource = programResource;
            programResource.ChildResources.Add(projectResource);

            var viewProjectPermission = new Permission
            {
                PermissionId = Permission.ViewProject.Id,
                PermissionName = Permission.ViewProject.Value,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ParentResourceType = programResourceType,
                ParentResourceTypeId = programResourceType.ResourceTypeId
            };
            var editProjectPermission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ParentResourceType = programResourceType,
                ParentResourceTypeId = programResourceType.ResourceTypeId
            };
            var viewProjectPermissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = viewProjectPermission,
                PermissionId = viewProjectPermission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = programResource,
                ResourceId = programResource.ResourceId
            };
            var editProjectPermissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = editProjectPermission,
                PermissionId = editProjectPermission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = programResource,
                ResourceId = programResource.ResourceId
            };
            context.UserAccounts.Add(userAccount);
            context.Principals.Add(principal);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(programResource);
            context.Resources.Add(projectResource);
            context.Permissions.Add(viewProjectPermission);
            context.Permissions.Add(editProjectPermission);
            context.PermissionAssignments.Add(editProjectPermissionAssignment);
            context.PermissionAssignments.Add(viewProjectPermissionAssignment);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery(context).ToList();
            Assert.AreEqual(2, results.Count);
            var editPermissionAuthorization = results.Where(x => x.PermissionId == editProjectPermission.PermissionId).FirstOrDefault();
            var viewPermissionAuthorization = results.Where(x => x.PermissionId == viewProjectPermission.PermissionId).FirstOrDefault();
            Assert.IsNotNull(editPermissionAuthorization);
            Assert.IsNotNull(viewPermissionAuthorization);
        }
        
        #endregion

        #region CreateGetResourceAuthorizationsQuery
        [TestMethod]
        public void TestCreateGetResourceAuthorizationQuery_InheritedPermissionInRole()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;
            var projectResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Program.Value,
                ResourceTypeId = ResourceType.Program.Id
            };
            var projectResource = new Resource
            {
                ResourceId = 1,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var programResource = new Resource
            {
                ResourceId = 2,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 20
            };
            projectResource.ParentResource = programResource;
            projectResource.ParentResourceId = programResource.ResourceId;
            programResource.ChildResources.Add(projectResource);
            var editProjectPermission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc",
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ParentResourceTypeId = programResourceType.ResourceTypeId,
                ParentResourceType = programResourceType
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = editProjectPermission,
                PermissionId = editProjectPermission.PermissionId,
                Resource = programResource,
                ResourceId = programResource.ResourceId,
                Role = role,
                RoleId = role.RoleId,
                AssignedOn = DateTimeOffset.UtcNow,
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(projectResource);
            context.Resources.Add(programResource);
            context.Permissions.Add(editProjectPermission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var defaultSort = new ExpressionSorter<ResourceAuthorization>(x => x.RoleName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ResourceAuthorization>(0, 1, defaultSort);
            var results = ResourceQueries.CreateGetResourceAuthorizationsQuery(context, queryOperator).ToList();
            Assert.AreEqual(2, results.Count);
            var projectPermission = results.Where(x => x.ForeignResourceId == projectResource.ForeignResourceId).FirstOrDefault();
            var programPermission = results.Where(x => x.ForeignResourceId == programResource.ForeignResourceId).FirstOrDefault();
            Assert.IsNotNull(projectPermission);
            Assert.IsNotNull(programPermission);

            Assert.IsTrue(projectPermission.IsAllowed);
            Assert.IsTrue(projectPermission.IsGrantedByRole);
            Assert.IsTrue(projectPermission.IsGrantedByInheritance);
            Assert.IsFalse(projectPermission.IsGrantedByPermission);

            Assert.IsTrue(programPermission.IsAllowed);
            Assert.IsTrue(programPermission.IsGrantedByRole);
            Assert.IsFalse(programPermission.IsGrantedByInheritance);
            Assert.IsFalse(programPermission.IsGrantedByPermission);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationQuery_InheritedPermission()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                PrincipalId = principal.PrincipalId,
                Principal = principal,
                DisplayName = "display",
                EmailAddress = "email"
            };
            var projectResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Program.Id,
                ResourceTypeName = ResourceType.Program.Value
            };
            var programResource = new Resource
            {
                ResourceId = 1,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 10
            };
            var projectResource = new Resource
            {
                ResourceId = 2,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 20
            };
            projectResource.ParentResourceId = programResource.ResourceId;
            projectResource.ParentResource = programResource;
            programResource.ChildResources.Add(projectResource);

            var viewProjectPermission = new Permission
            {
                PermissionId = Permission.ViewProject.Id,
                PermissionName = Permission.ViewProject.Value,
                PermissionDescription = "desc",
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ParentResourceType = programResourceType,
                ParentResourceTypeId = programResourceType.ResourceTypeId
            };
            var viewProjectPermissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = viewProjectPermission,
                PermissionId = viewProjectPermission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = programResource,
                ResourceId = programResource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow
            };
            context.UserAccounts.Add(userAccount);
            context.Principals.Add(principal);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(programResource);
            context.Resources.Add(projectResource);
            context.Permissions.Add(viewProjectPermission);
            context.PermissionAssignments.Add(viewProjectPermissionAssignment);

            var defaultSort = new ExpressionSorter<ResourceAuthorization>(x => x.RoleName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ResourceAuthorization>(0, 10, defaultSort);
            var results = ResourceQueries.CreateGetResourceAuthorizationsQuery(context, queryOperator).ToList();
            Assert.AreEqual(2, results.Count);
            var projectPermission = results.Where(x => x.ForeignResourceId == projectResource.ForeignResourceId).FirstOrDefault();
            var programPermission = results.Where(x => x.ForeignResourceId == programResource.ForeignResourceId).FirstOrDefault();
            Assert.IsNotNull(projectPermission);
            Assert.IsNotNull(programPermission);

            Assert.IsTrue(projectPermission.IsAllowed);
            Assert.IsFalse(projectPermission.IsGrantedByRole);     
            Assert.IsTrue(projectPermission.IsGrantedByInheritance);
            Assert.IsTrue(projectPermission.IsGrantedByPermission);                   

            Assert.IsTrue(programPermission.IsAllowed);
            Assert.IsFalse(programPermission.IsGrantedByRole);            
            Assert.IsFalse(programPermission.IsGrantedByInheritance);
            Assert.IsTrue(programPermission.IsGrantedByPermission);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsQuery_UserHasRole()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc",
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var defaultSort = new ExpressionSorter<ResourceAuthorization>(x => x.RoleName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ResourceAuthorization>(0, 1, defaultSort);
            var results = ResourceQueries.CreateGetResourceAuthorizationsQuery(context, queryOperator);
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsQuery_UserHasRole_RoleIsInactive()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc",
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = false
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var defaultSort = new ExpressionSorter<ResourceAuthorization>(x => x.RoleName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ResourceAuthorization>(0, 1, defaultSort);
            var results = ResourceQueries.CreateGetResourceAuthorizationsQuery(context, queryOperator);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsQuery_UserHasPermissionAssignment()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            var defaultSort = new ExpressionSorter<ResourceAuthorization>(x => x.RoleName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ResourceAuthorization>(0, 1, defaultSort);
            var results = ResourceQueries.CreateGetResourceAuthorizationsQuery(context, queryOperator);
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsQuery_UserHasPermissionAssignmentAndRole()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            var defaultSort = new ExpressionSorter<ResourceAuthorization>(x => x.RoleName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ResourceAuthorization>(0, 1, defaultSort);
            var results = ResourceQueries.CreateGetResourceAuthorizationsQuery(context, queryOperator);
            Assert.AreEqual(2, results.Count());

            var roleGrantedPermissions = results.Where(x => x.IsGrantedByRole).ToList();
            Assert.AreEqual(1, roleGrantedPermissions.Count);
            var roleGrantedPermission = roleGrantedPermissions.First();
            Assert.AreEqual(true, roleGrantedPermission.IsAllowed);

            var permissionAssignmentGrantedPermissions = results.Where(x => x.IsGrantedByPermission).ToList();
            Assert.AreEqual(1, permissionAssignmentGrantedPermissions.Count);
            var permissionGrantedPermission = roleGrantedPermissions.First();
            Assert.AreEqual(true, permissionGrantedPermission.IsAllowed);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsQuery_UserHasRoleAndNotAllowedPermissionAssignment()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            var defaultSort = new ExpressionSorter<ResourceAuthorization>(x => x.RoleName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ResourceAuthorization>(0, 1, defaultSort);
            var results = ResourceQueries.CreateGetResourceAuthorizationsQuery(context, queryOperator);
            Assert.AreEqual(2, results.Count());

            var roleGrantedPermissions = results.Where(x => x.IsGrantedByRole).ToList();
            Assert.AreEqual(1, roleGrantedPermissions.Count);
            var roleGrantedPermission = roleGrantedPermissions.First();
            Assert.AreEqual(true, roleGrantedPermission.IsAllowed);

            var permissionAssignmentGrantedPermissions = results.Where(x => x.IsGrantedByPermission).ToList();
            Assert.AreEqual(1, permissionAssignmentGrantedPermissions.Count);
            var permissionGrantedPermission = permissionAssignmentGrantedPermissions.First();
            Assert.AreEqual(false, permissionGrantedPermission.IsAllowed);
        }

        #endregion

        #region CreateGetResourcePermissionsQuery
        [TestMethod]
        public void TestCreateGetResourcePermissionsQuery_HasPermission_ResourceIdNull()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc",
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId
            };
            context.Permissions.Add(permission);
            context.ResourceTypes.Add(resourceType);

            var dtos = ResourceQueries.CreateGetResourcePermissionsQuery(context, resourceType.ResourceTypeName, null);
            Assert.AreEqual(1, dtos.Count());
            var firstDto = dtos.First();
            Assert.AreEqual(permission.PermissionDescription, firstDto.PermissionDescription);
            Assert.AreEqual(permission.PermissionId, firstDto.PermissionId);
            Assert.AreEqual(permission.PermissionName, firstDto.PermissionName);
        }

        [TestMethod]
        public void TestCreateGetResourcePermissionsQuery_ParentResourceType()
        {
            var projectResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Program.Value,
                ResourceTypeId = ResourceType.Program.Id
            };
            var viewProjectPermission = new Permission
            {
                PermissionId = Permission.ViewProject.Id,
                PermissionName = Permission.ViewProject.Value,
                PermissionDescription = "desc",
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ParentResourceTypeId = programResourceType.ResourceTypeId,
                ParentResourceType = programResourceType
            };
            context.Permissions.Add(viewProjectPermission);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);

            var dtos = ResourceQueries.CreateGetResourcePermissionsQuery(context, programResourceType.ResourceTypeName, null);
            Assert.AreEqual(1, dtos.Count());
            var firstDto = dtos.First();
            Assert.AreEqual(viewProjectPermission.PermissionDescription, firstDto.PermissionDescription);
            Assert.AreEqual(viewProjectPermission.PermissionId, firstDto.PermissionId);
            Assert.AreEqual(viewProjectPermission.PermissionName, firstDto.PermissionName);
        }

        [TestMethod]
        public void TestCreateGetResourcePermissionsQuery_HasPermission_OtherResourceOfSameTypeShouldNotBeFound()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var permission1 = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc",
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId
            };

            var resource = new Resource
            {
                ResourceId = 1,
            };
            var permission2 = new Permission
            {
                PermissionId = Permission.EditProgram.Id,
                PermissionName = Permission.EditProgram.Value,
                PermissionDescription = "desc",
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceId = resource.ResourceId,
                Resource = resource
            };
            context.Permissions.Add(permission1);
            context.Permissions.Add(permission2);
            context.Resources.Add(resource);
            context.ResourceTypes.Add(resourceType);

            var dtos = ResourceQueries.CreateGetResourcePermissionsQuery(context, resourceType.ResourceTypeName, null);
            Assert.AreEqual(1, dtos.Count());
            var firstDto = dtos.First();
            Assert.AreEqual(permission1.PermissionDescription, firstDto.PermissionDescription);
            Assert.AreEqual(permission1.PermissionId, firstDto.PermissionId);
            Assert.AreEqual(permission1.PermissionName, firstDto.PermissionName);
        }

        [TestMethod]
        public void TestCreateGetResourcePermissionsQuery_NoPermissionsForResourceType()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            context.ResourceTypes.Add(resourceType);
            var dtos = ResourceQueries.CreateGetResourcePermissionsQuery(context, resourceType.ResourceTypeName, null);
            Assert.AreEqual(0, dtos.Count());
        }

        [TestMethod]
        public void TestCreateGetResourcePermissionsQuery_HasPermission_ResourceIdIsNotNull()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc",
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };
            
            context.Permissions.Add(permission);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);

            var dtos = ResourceQueries.CreateGetResourcePermissionsQuery(context, resourceType.ResourceTypeName, resource.ResourceId);
            Assert.AreEqual(1, dtos.Count());
            var firstDto = dtos.First();
            Assert.AreEqual(permission.PermissionDescription, firstDto.PermissionDescription);
            Assert.AreEqual(permission.PermissionId, firstDto.PermissionId);
            Assert.AreEqual(permission.PermissionName, firstDto.PermissionName);
        }

        [TestMethod]
        public void TestCreateGetResourcePermissionsQuery_PermissionHasResource_ResourceIdIsNull()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc",
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };

            context.Permissions.Add(permission);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);

            var dtos = ResourceQueries.CreateGetResourcePermissionsQuery(context, resourceType.ResourceTypeName, null);
            Assert.AreEqual(0, dtos.Count());
        }

        [TestMethod]
        public void TestCreateGetResourcePermissionsQuery_DoesNotHavePermission_ResourceIdIsNotNull()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            context.ResourceTypes.Add(resourceType);
            var dtos = ResourceQueries.CreateGetResourcePermissionsQuery(context, resourceType.ResourceTypeName, 1);
            Assert.AreEqual(0, dtos.Count());
        }
        #endregion

        #region CreateGetResourceAuthorizationInfoDTOQuery
        [TestMethod]
        public void TestCreateGetResourceAuthorizationInfoDTOQuery_PrincipalHasRole()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId,
                AssignedOn = DateTimeOffset.UtcNow,
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var results = ResourceQueries.CreateGetResourceAuthorizationInfoDTOQuery(context, resourceType.ResourceTypeName, resource.ForeignResourceId);
            Assert.AreEqual(1, results.Count());
            var result = results.First();
            Assert.AreEqual(1, result.AllowedPrincipalsCount);
            Assert.AreEqual(roleResourcePermission.AssignedOn, result.LastRevisedOn);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationInfoDTOQuery_PrincipalHasRole_RoleIsInactive()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = false
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId,
                AssignedOn = DateTimeOffset.UtcNow,
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var results = ResourceQueries.CreateGetResourceAuthorizationInfoDTOQuery(context, resourceType.ResourceTypeName, resource.ForeignResourceId);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationInfoDTOQuery_PrincipalHasPermission()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow,
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = ResourceQueries.CreateGetResourceAuthorizationInfoDTOQuery(context, resourceType.ResourceTypeName, resource.ForeignResourceId);
            Assert.AreEqual(1, results.Count());
            var result = results.First();
            Assert.AreEqual(1, result.AllowedPrincipalsCount);
            Assert.AreEqual(permissionAssignment.AssignedOn, result.LastRevisedOn);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationInfoDTOQuery_PrincipalHasInheritedPermission()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var projectResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Program.Value,
                ResourceTypeId = ResourceType.Program.Id
            };
            var projectResource = new Resource
            {
                ResourceId = 1,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var programResource = new Resource
            {
                ResourceId = 2,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 3,
            };
            projectResource.ParentResource = programResource;
            projectResource.ParentResourceId = programResource.ResourceId;
            programResource.ChildResources.Add(projectResource);
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc",
                ParentResourceType = programResourceType,
                ParentResourceTypeId = programResourceType.ResourceTypeId,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = programResource,
                ResourceId = programResource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow,
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(projectResource);
            context.Resources.Add(programResource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = ResourceQueries.CreateGetResourceAuthorizationInfoDTOQuery(context, projectResourceType.ResourceTypeName, projectResource.ForeignResourceId);
            Assert.AreEqual(1, results.Count());
            var result = results.First();
            Assert.AreEqual(1, result.AllowedPrincipalsCount);
            Assert.AreEqual(permissionAssignment.AssignedOn, result.LastRevisedOn);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationInfoDTOQuery_PrincipalHasInheritedRolePermission()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;
            var projectResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Program.Value,
                ResourceTypeId = ResourceType.Program.Id
            };
            var projectResource = new Resource
            {
                ResourceId = 1,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var programResource = new Resource
            {
                ResourceId = 2,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 20
            };
            projectResource.ParentResource = programResource;
            projectResource.ParentResourceId = programResource.ResourceId;
            programResource.ChildResources.Add(projectResource);
            var editProjectPermission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc",
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ParentResourceType = programResourceType,
                ParentResourceTypeId = programResourceType.ResourceTypeId,
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = editProjectPermission,
                PermissionId = editProjectPermission.PermissionId,
                Resource = programResource,
                ResourceId = programResource.ResourceId,
                Role = role,
                RoleId = role.RoleId,
                AssignedOn = DateTimeOffset.UtcNow,
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(projectResource);
            context.Resources.Add(programResource);
            context.Permissions.Add(editProjectPermission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var results = ResourceQueries.CreateGetResourceAuthorizationInfoDTOQuery(context, projectResourceType.ResourceTypeName, projectResource.ForeignResourceId);
            Assert.AreEqual(1, results.Count());
            var result = results.First();
            Assert.AreEqual(1, result.AllowedPrincipalsCount);
            Assert.AreEqual(roleResourcePermission.AssignedOn, result.LastRevisedOn);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationInfoDTOQuery_PrincipalHasInheritedRolePermission_RoleIsInactive()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;
            var projectResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var programResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Program.Value,
                ResourceTypeId = ResourceType.Program.Id
            };
            var projectResource = new Resource
            {
                ResourceId = 1,
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var programResource = new Resource
            {
                ResourceId = 2,
                ResourceType = programResourceType,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ForeignResourceId = 20
            };
            projectResource.ParentResource = programResource;
            projectResource.ParentResourceId = programResource.ResourceId;
            programResource.ChildResources.Add(projectResource);
            var editProjectPermission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc",
                ResourceType = projectResourceType,
                ResourceTypeId = projectResourceType.ResourceTypeId,
                ParentResourceType = programResourceType,
                ParentResourceTypeId = programResourceType.ResourceTypeId,
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = false
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = editProjectPermission,
                PermissionId = editProjectPermission.PermissionId,
                Resource = programResource,
                ResourceId = programResource.ResourceId,
                Role = role,
                RoleId = role.RoleId,
                AssignedOn = DateTimeOffset.UtcNow,
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(projectResourceType);
            context.ResourceTypes.Add(programResourceType);
            context.Resources.Add(projectResource);
            context.Resources.Add(programResource);
            context.Permissions.Add(editProjectPermission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var results = ResourceQueries.CreateGetResourceAuthorizationInfoDTOQuery(context, projectResourceType.ResourceTypeName, projectResource.ForeignResourceId);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationInfoDTOQuery_PrincipalDoesNotHavePermission()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow,
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = ResourceQueries.CreateGetResourceAuthorizationInfoDTOQuery(context, resourceType.ResourceTypeName, resource.ForeignResourceId);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationInfoDTOQuery_ResourceTypeDoesNotExist()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow,
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = ResourceQueries.CreateGetResourceAuthorizationInfoDTOQuery(context, "idontexist", resource.ForeignResourceId);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationInfoDTOQuery_ForeignResourceIdDoesNotExist()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow,
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = ResourceQueries.CreateGetResourceAuthorizationInfoDTOQuery(context, resource.ResourceType.ResourceTypeName, -1);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationInfoDTOQuery_NoDataToQuery()
        {
            var results = ResourceQueries.CreateGetResourceAuthorizationInfoDTOQuery(context, ResourceType.Project.Value, 1);
            Assert.AreEqual(0, results.Count());
        }
        #endregion
    }
}
