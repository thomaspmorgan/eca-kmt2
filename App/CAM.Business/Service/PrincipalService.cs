using CAM.Business.Model;
using System.Data.Entity;
using CAM.Data;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAM.Business.Queries.Models;

namespace CAM.Business.Service
{
    /// <summary>
    /// The PrincipalService is responsible for handling crud operations on Principals in CAM.
    /// </summary>
    public partial class PrincipalService : DbContextService<CamModel>, CAM.Business.Service.IPrincipalService
    {
        private const string FOREIGN_RESOURCE_DOES_NOT_EXIST_FORMAT_STRING = "The foreign resource with id [{0}] and resource type [{1}] does not exist in CAM.";

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IResourceService resourceService;
        private Action<GrantedPermission, ForeignResourceCache> throwIfForeignResourceNotFoundByGrantedPermission;
        private Action<DeletedPermission, ForeignResourceCache> throwIfForeignResourceNotFoundByDeletedPermission;
        private Action<GrantedPermission, Principal> throwIfGranteeNotFound;
        private Action<GrantedPermission, Principal> throwIfGrantorNotFound;
        private Action<GrantedPermission, CAM.Data.Permission> throwIfPermissionNotFound;
        private Action<PermissionAssignment> throwIfPermissionAssignmentNotFound;
        private Action<int, List<ResourcePermissionDTO>> throwIfRequestedPermissionIsNotAvailable;

        /// <summary>
        /// Creats a new PrincipalService with the given CamModel to perform operations against and the resource service
        /// to lookup resources in CAM.
        /// </summary>
        /// <param name="model">The CamModel to perform crud operations with.</param>
        /// <param name="resourceService">The resource service.</param>
        public PrincipalService(CamModel model, IResourceService resourceService)
            : base(model)
        {
            Contract.Requires(model != null, "The model must not be null.");
            Contract.Requires(resourceService != null, "The resource service must not be null.");
            this.resourceService = resourceService;
            throwIfRequestedPermissionIsNotAvailable = (permissionId, availablePermisions) =>
            {
                if (!availablePermisions.Select(x => x.PermissionId).ToList().Contains(permissionId))
                {
                    throw new NotSupportedException(String.Format("The requested permission with id [{0}] is not a valid permission for the resource.", permissionId));
                }
            };
            throwIfForeignResourceNotFoundByGrantedPermission = (grantedPermission, resource) =>
            {
                if (resource == null)
                {
                    throw new ModelNotFoundException(
                    String.Format(
                    "The foreign resource with id [{0}] and resource type [{1}] does not exist in CAM.",
                    grantedPermission.ForeignResourceId,
                    grantedPermission.ResourceTypeAsString));
                }
            };
            throwIfForeignResourceNotFoundByDeletedPermission = (deletedPermission, resource) =>
            {
                if (resource == null)
                {
                    throw new ModelNotFoundException(
                    String.Format(
                    "The foreign resource with id [{0}] and resource type [{1}] does not exist in CAM.",
                    deletedPermission.ForeignResourceId,
                    deletedPermission.ResourceTypeAsString));
                }
            };
            throwIfGranteeNotFound = (grantedPermission, grantee) =>
            {
                if (grantee == null)
                {
                    throw new ModelNotFoundException(
                        String.Format("The user with id [{0}] being granted the permission was not found.", grantedPermission.GranteePrincipalId));
                }
            };
            throwIfGrantorNotFound = (grantedPermission, grantor) =>
            {
                if (grantor == null)
                {
                    throw new ModelNotFoundException(
                        String.Format("The user with id [{0}] granting the permission could not be found.", grantedPermission.Audit.UserId));
                }
            };
            throwIfPermissionNotFound = (grantedPermission, permission) =>
            {
                if (permission == null)
                {
                    throw new ModelNotFoundException(String.Format("The permission with id [{0}] was not found.", grantedPermission.PermissionId));
                }
            };
            throwIfPermissionAssignmentNotFound = (permissionAssignment) => 
            {
                if (permissionAssignment == null)
                {
                    throw new ModelNotFoundException("The permission assignment was not found.");
                }
            };
        }

        private IQueryable<CAM.Data.Permission> CreateGetPermissionByIdQuery(int permissionId)
        {
            return this.Context.Permissions.Where(x => x.PermissionId == permissionId);
        }

        private IQueryable<Principal> CreateGetPrincipalByIdQuery(int principalId)
        {
            return this.Context.Principals.Where(x => x.PrincipalId == principalId);
        }

        private IQueryable<PermissionAssignment> CreateGetPermissionAssignmentQuery(int principalId, int permissionId, int? resourceId)
        {
            Contract.Requires(resourceId.HasValue, "The resource id should have a value here.");
            return this.Context.PermissionAssignments.Where(x => x.ResourceId == resourceId.Value && x.PrincipalId == principalId && x.PermissionId == permissionId);
        }

        #region Grant Permission

        /// <summary>
        /// Grants a permission to a principal in the system.  If the permission had been previously granted it is set active.
        /// </summary>
        /// <param name="grantedPermission">The permission granted to a principal by another principal.</param>
        public void GrantPermission(GrantedPermission grantedPermission)
        {
            Handle(grantedPermission);
        }

        /// <summary>
        /// Grants a permission to a principal in the system.  If the permission had been previously granted it is set active.
        /// </summary>
        /// <param name="grantedPermission">The permission granted to a principal by another principal.</param>
        public Task GrantPermissionsAsync(GrantedPermission grantedPermission)
        {
            return HandleAsync(grantedPermission);
        }

        private async Task HandleAsync(GrantedPermission grantedPermission)
        {
            logger.Info("Handling granted permission [{0}].", grantedPermission);
            var resource = await resourceService.GetResourceByForeignResourceIdAsync(grantedPermission.ForeignResourceId, grantedPermission.GetResourceType().Id);
            throwIfForeignResourceNotFoundByGrantedPermission(grantedPermission, resource);

            var grantee = await CreateGetPrincipalByIdQuery(grantedPermission.GranteePrincipalId).FirstOrDefaultAsync();
            throwIfGranteeNotFound(grantedPermission, grantee);

            var userAccount = await Context.UserAccounts.FindAsync(grantedPermission.GranteePrincipalId);
            UpdatePermissionsRevisedOn(userAccount);

            var camPermission = await CreateGetPermissionByIdQuery(grantedPermission.PermissionId).FirstOrDefaultAsync();
            throwIfPermissionNotFound(grantedPermission, camPermission);

            var grantor = await CreateGetPrincipalByIdQuery(grantedPermission.Audit.UserId).FirstOrDefaultAsync();
            throwIfGrantorNotFound(grantedPermission, grantor);

            var existingPermissions = await CreateGetPermissionAssignmentQuery(grantee.PrincipalId, camPermission.PermissionId, resource.ResourceId).ToListAsync();
            if (existingPermissions.Count == 0)
            {
                var availablePermissions = await resourceService.GetResourcePermissionsAsync(grantedPermission.ResourceTypeAsString, grantedPermission.ForeignResourceId);
                throwIfRequestedPermissionIsNotAvailable(grantedPermission.PermissionId, availablePermissions);
                DoInsertPermissionAssignment(grantee, grantor, resource.ResourceId, grantedPermission);
            }
            else
            {
                UpdateIsAllowed(grantedPermission, existingPermissions);
            }

            
        }

        private void Handle(GrantedPermission grantedPermission)
        {
            logger.Info("Handling granted permission [{0}].", grantedPermission);
            var resource = resourceService.GetResourceByForeignResourceId(grantedPermission.ForeignResourceId, grantedPermission.GetResourceType().Id);
            throwIfForeignResourceNotFoundByGrantedPermission(grantedPermission, resource);

            var grantee = CreateGetPrincipalByIdQuery(grantedPermission.GranteePrincipalId).FirstOrDefault();
            throwIfGranteeNotFound(grantedPermission, grantee);

            var camPermission = CreateGetPermissionByIdQuery(grantedPermission.PermissionId).FirstOrDefault();
            throwIfPermissionNotFound(grantedPermission, camPermission);

            var grantor = CreateGetPrincipalByIdQuery(grantedPermission.Audit.UserId).FirstOrDefault();
            throwIfGrantorNotFound(grantedPermission, grantor);

            var userAccount = Context.UserAccounts.Find(grantedPermission.GranteePrincipalId);
            UpdatePermissionsRevisedOn(userAccount);

            var existingPermissions = CreateGetPermissionAssignmentQuery(grantee.PrincipalId, camPermission.PermissionId, resource.ResourceId).ToList();
            if (existingPermissions.Count == 0)
            {
                var availablePermissions = resourceService.GetResourcePermissions(grantedPermission.ResourceTypeAsString, grantedPermission.ForeignResourceId);
                throwIfRequestedPermissionIsNotAvailable(grantedPermission.PermissionId, availablePermissions);
                DoInsertPermissionAssignment(grantee, grantor, resource.ResourceId, grantedPermission);
            }
            else
            {
                UpdateIsAllowed(grantedPermission, existingPermissions);
            }
        }

        private void UpdateIsAllowed(GrantedPermission grantedPermission, List<PermissionAssignment> permissionAssignments)
        {
            if (permissionAssignments.Count > 1)
            {
                throw new NotSupportedException("There should not be more than one permission assignment to set is allowed true.");
            }
            permissionAssignments.ForEach(x => {
                logger.Info("Setting IsAllowed [{0}] to permission with id [PermissionId:  {1}, ResourceId:  {2}, PrincipalId:  {3}].", 
                    grantedPermission.IsAllowed, 
                    x.PermissionId, 
                    x.ResourceId, 
                    x.PrincipalId);
                x.IsAllowed = grantedPermission.IsAllowed;
            });
        }

        private PermissionAssignment DoInsertPermissionAssignment(
            Principal grantee,
            Principal grantor,
            int resourceId,
            GrantedPermission grantedPermission)
        {
            Contract.Requires(grantedPermission != null, "The granted permission must not be null.");
            Contract.Requires(grantee != null, "The grantee must not be null.");
            Contract.Requires(grantor != null, "The grantor must not be null.");
            logger.Info("Inserting permission assignment for granted permission [{0}].", grantedPermission);
            var permissionAssignment = new PermissionAssignment
            {
                AssignedBy = grantedPermission.Audit.UserId,
                AssignedOn = grantedPermission.Audit.Date,
                IsAllowed = grantedPermission.IsAllowed,
                PermissionId = grantedPermission.PermissionId,
                PrincipalId = grantedPermission.GranteePrincipalId,
                ResourceId = resourceId
            };
            Context.PermissionAssignments.Add(permissionAssignment);
            return permissionAssignment;
        }
        #endregion

        /// <summary>
        /// Revoke a permission explicity from a user.
        /// </summary>
        /// <param name="revokedPermission">The revoked permission.</param>
        public void RevokePermission(RevokedPermission revokedPermission)
        {
            Handle(revokedPermission);
        }

        /// <summary>
        /// Revoke a permission explicity from a user.
        /// </summary>
        /// <param name="revokedPermission">The revoked permission.</param>
        public Task RevokePermissionAsync(RevokedPermission revokedPermission)
        {
            return HandleAsync(revokedPermission);
        }

        #region Delete

        /// <summary>
        /// Deletes a permission assignment.
        /// </summary>
        /// <param name="permission">The deleted permission.</param>
        public void DeletePermission(DeletedPermission permission)
        {
            var user = Context.UserAccounts.Find(permission.GranteePrincipalId);

            var resource = resourceService.GetResourceByForeignResourceId(permission.ForeignResourceId, permission.GetResourceType().Id);
            throwIfForeignResourceNotFoundByDeletedPermission(permission, resource);

            var permissionAssignment = CreateGetPermissionAssignmentQuery(permission.GranteePrincipalId, permission.PermissionId, resource.ResourceId).FirstOrDefault();
            throwIfPermissionAssignmentNotFound(permissionAssignment);

            DoDeletePermissionAssignment(permissionAssignment, user);
        }

        /// <summary>
        /// Deletes a permission assignment.
        /// </summary>
        /// <param name="permission">The deleted permission.</param>
        public async Task DeletePermissionAsync(DeletedPermission permission)
        {
            var user = await Context.UserAccounts.FindAsync(permission.GranteePrincipalId);

            var resource = await resourceService.GetResourceByForeignResourceIdAsync(permission.ForeignResourceId, permission.GetResourceType().Id);
            throwIfForeignResourceNotFoundByDeletedPermission(permission, resource);

            var permissionAssignment = await CreateGetPermissionAssignmentQuery(permission.GranteePrincipalId, permission.PermissionId, resource.ResourceId).FirstOrDefaultAsync();
            throwIfPermissionAssignmentNotFound(permissionAssignment);            

            DoDeletePermissionAssignment(permissionAssignment, user);
        }

        private void DoDeletePermissionAssignment(PermissionAssignment permissionAssignment, UserAccount user)
        {
            Contract.Requires(permissionAssignment != null, "The permission assignment must not be null.");
            UpdatePermissionsRevisedOn(user);
            this.Context.PermissionAssignments.Remove(permissionAssignment);
        }
        #endregion

        private void UpdatePermissionsRevisedOn(UserAccount modifiedUser)
        {
            if (modifiedUser != null)
            {
                modifiedUser.PermissionsRevisedOn = DateTimeOffset.UtcNow;
            }
        }
    }
}
