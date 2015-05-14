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
        private Action<GrantedPermission, int?> throwIfForeignResourceNotFound;
        private Action<GrantedPermission, Principal> throwIfGranteeNotFound;
        private Action<GrantedPermission, Principal> throwIfGrantorNotFound;
        private Action<GrantedPermission, CAM.Data.Permission> throwIfPermissionNotFound;

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
            throwIfForeignResourceNotFound = (grantedPermission, resourceId) =>
            {
                if (!resourceId.HasValue)
                {
                    throw new ModelNotFoundException(
                    String.Format(
                    "The foreign resource with id [{0}] and resource type [{1}] does not exist in CAM.",
                    grantedPermission.ForeignResourceId,
                    grantedPermission.ResourceTypeAsString));
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
                    throw new ModelNotFoundException(String.Format("The permission with id [{0}] was found.", grantedPermission.PermissionId));
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
            var resourceId = resourceService.GetResourceIdByForeignResourceId(grantedPermission.ForeignResourceId, grantedPermission.GetResourceType().Id);
            throwIfForeignResourceNotFound(grantedPermission, resourceId);

            var grantee = CreateGetPrincipalByIdQuery(grantedPermission.GranteePrincipalId).FirstOrDefault();
            throwIfGranteeNotFound(grantedPermission, grantee);

            var camPermission = CreateGetPermissionByIdQuery(grantedPermission.PermissionId).FirstOrDefault();
            throwIfPermissionNotFound(grantedPermission, camPermission);

            var grantor = CreateGetPrincipalByIdQuery(grantedPermission.Audit.UserId).FirstOrDefault();
            throwIfGrantorNotFound(grantedPermission, grantor);

            var existingPermissions = CreateGetPermissionAssignmentQuery(grantee.PrincipalId, camPermission.PermissionId, resourceId.Value).ToList();
            if (existingPermissions.Count == 0)
            {
                DoGrantPermission(grantee, grantor, resourceId, grantedPermission);
            }
            else
            {
                SetIsAllowed(existingPermissions);
            }
        }

        /// <summary>
        /// Grants a permission to a principal in the system.  If the permission had been previously granted it is set active.
        /// </summary>
        /// <param name="grantedPermission">The permission granted to a principal by another principal.</param>
        public async Task GrantPermissionsAsync(GrantedPermission grantedPermission)
        {
            var resourceId = await resourceService.GetResourceIdByForeignResourceIdAsync(grantedPermission.ForeignResourceId, grantedPermission.GetResourceType().Id);
            throwIfForeignResourceNotFound(grantedPermission, resourceId);

            var grantee = await CreateGetPrincipalByIdQuery(grantedPermission.GranteePrincipalId).FirstOrDefaultAsync();
            throwIfGranteeNotFound(grantedPermission, grantee);

            var camPermission = await CreateGetPermissionByIdQuery(grantedPermission.PermissionId).FirstOrDefaultAsync();
            throwIfPermissionNotFound(grantedPermission, camPermission);

            var grantor = await CreateGetPrincipalByIdQuery(grantedPermission.Audit.UserId).FirstOrDefaultAsync();
            throwIfGrantorNotFound(grantedPermission, grantor);

            var existingPermissions = await CreateGetPermissionAssignmentQuery(grantee.PrincipalId, camPermission.PermissionId, resourceId.Value).ToListAsync();
            if (existingPermissions.Count == 0)
            {
                DoGrantPermission(grantee, grantor, resourceId, grantedPermission);
            }
            else
            {
                SetIsAllowed(existingPermissions);
            }
        }

        private void SetIsAllowed(List<PermissionAssignment> permissionAssignments)
        {
            permissionAssignments.ForEach(x => x.IsAllowed = true);
            if (permissionAssignments.Count > 1)
            {
                throw new NotSupportedException("There should not be more than one permission assignment to set is allowed true.");
            }
        }

        private PermissionAssignment DoGrantPermission(
            Principal grantee,
            Principal grantor,
            int? resourceId,
            GrantedPermission grantedPermission)
        {
            Contract.Requires(grantedPermission != null, "The granted permission must not be null.");
            Contract.Requires(resourceId.HasValue, "The resource id should have a value here.");
            Contract.Requires(grantee != null, "The grantee must not be null.");
            Contract.Requires(grantor != null, "The grantor must not be null.");
            var permissionAssignment = new PermissionAssignment
            {
                AssignedBy = grantedPermission.Audit.UserId,
                AssignedOn = grantedPermission.Audit.Date,
                IsAllowed = true,
                PermissionId = grantedPermission.PermissionId,
                PrincipalId = grantedPermission.GranteePrincipalId,
                ResourceId = resourceId.Value
            };
            Context.PermissionAssignments.Add(permissionAssignment);
            return permissionAssignment;
        }
        #endregion
    }
}
