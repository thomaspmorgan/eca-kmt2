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

            var results = ResourceQueries.CreateGetResourceAuthorizationsByRoleQuery(context);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(roleResourcePermission.AssignedOn, firstResult.AssignedOn);
            Assert.AreEqual(userAccount.DisplayName, firstResult.DisplayName);
            Assert.AreEqual(userAccount.EmailAddress, firstResult.EmailAddress);
            Assert.AreEqual(resource.ForeignResourceId, firstResult.ForeignResourceId);
            Assert.AreEqual(true, firstResult.IsAllowed);
            Assert.AreEqual(false, firstResult.IsGrantedByPermission);
            Assert.AreEqual(true, firstResult.IsGrantedByRole);
            Assert.AreEqual(permission.PermissionDescription, firstResult.PermissionDescription);
            Assert.AreEqual(permission.PermissionId, firstResult.PermissionId);
            Assert.AreEqual(permission.PermissionName, firstResult.PermissionName);
            Assert.AreEqual(principal.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(resource.ResourceId, firstResult.ResourceId);
            Assert.AreEqual(resourceType.ResourceTypeName, firstResult.ResourceType);
            Assert.AreEqual(resourceType.ResourceTypeId, firstResult.ResourceTypeId);
            Assert.AreEqual(role.RoleId, firstResult.RoleId);
            Assert.AreEqual(role.RoleName, firstResult.RoleName);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByRoleQuery_PrincipalHasRole_PrincipalHasAllowedPermissionAssignment()
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
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
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
            context.PermissionAssignments.Add(permissionAssignment);
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByRoleQuery(context);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(true, firstResult.IsAllowed);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByRoleQuery_PrincipalHasRole_PrincipalHasNotAllowedPermissionAssignment()
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
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
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
            context.PermissionAssignments.Add(permissionAssignment);
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByRoleQuery(context);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(false, firstResult.IsAllowed);
        }

        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByRoleQuery_PrincipalHasRole_SomeOtherPermissionAssignmentNotAllowed()
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
            };
            var someOtherPermission = new Permission
            {
                PermissionId = Permission.EditProgram.Id,
                PermissionName = Permission.EditProgram.Value
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
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
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = someOtherPermission,
                PermissionId = someOtherPermission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };
            context.PermissionAssignments.Add(permissionAssignment);
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByRoleQuery(context);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(true, firstResult.IsAllowed);
        }


        [TestMethod]
        public void TestCreateGetResourceAuthorizationsByRoleQuery_PrincipalHasRole_OtherPrincipalHasNotAllowedPermissionAssignment()
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
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
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
            var someOtherPrincipal = new Principal
            {
                PrincipalId = 100,
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = someOtherPrincipal,
                PrincipalId = someOtherPrincipal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };
            context.Principals.Add(someOtherPrincipal);
            context.PermissionAssignments.Add(permissionAssignment);
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            var results = ResourceQueries.CreateGetResourceAuthorizationsByRoleQuery(context);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(true, firstResult.IsAllowed);
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
            Assert.AreEqual(userAccount.DisplayName, firstResult.DisplayName);
            Assert.AreEqual(userAccount.EmailAddress, firstResult.EmailAddress);
            Assert.AreEqual(resource.ForeignResourceId, firstResult.ForeignResourceId);
            Assert.AreEqual(permissionAssignment.IsAllowed, firstResult.IsAllowed);
            Assert.AreEqual(true, firstResult.IsGrantedByPermission);
            Assert.AreEqual(false, firstResult.IsGrantedByRole);
            Assert.AreEqual(permission.PermissionDescription, firstResult.PermissionDescription);
            Assert.AreEqual(permission.PermissionId, firstResult.PermissionId);
            Assert.AreEqual(permission.PermissionName, firstResult.PermissionName);
            Assert.AreEqual(principal.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(resource.ResourceId, firstResult.ResourceId);
            Assert.AreEqual(resourceType.ResourceTypeName, firstResult.ResourceType);
            Assert.AreEqual(resourceType.ResourceTypeId, firstResult.ResourceTypeId);
            Assert.IsNull(firstResult.RoleName);
            Assert.AreEqual(-1, firstResult.RoleId);
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

        #region CreateGetResourceAuthorizationsQuery
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
                PermissionDescription = "desc"
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
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
            Assert.AreEqual(false, roleGrantedPermission.IsAllowed);

            var permissionAssignmentGrantedPermissions = results.Where(x => x.IsGrantedByPermission).ToList();
            Assert.AreEqual(1, permissionAssignmentGrantedPermissions.Count);
            var permissionGrantedPermission = roleGrantedPermissions.First();
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
