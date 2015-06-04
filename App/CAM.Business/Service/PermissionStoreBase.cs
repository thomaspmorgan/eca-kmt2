using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAM.Data;
using System.Diagnostics;
using NLog.Interface;
using ECA.Core.Service;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq;
using CAM.Business.Model;
using CAM.Business.Queries;

namespace CAM.Business.Service
{
    public class PermissionStoreBase : DbContextService<CamModel>
    {
        private readonly ILogger logger = new LoggerAdapter(NLog.LogManager.GetCurrentClassLogger());


        public PermissionStoreBase(CamModel camModel, IPermissionModelService permissionModelService, IResourceService resourceService)
            : base(camModel)
        {
            Contract.Requires(permissionModelService != null, "The permission model service must not be null.");
            Contract.Requires(resourceService != null, "The resource service must not be null.");
            this.PermissionModelService = permissionModelService;
            this.ResourceService = resourceService;
        }

        /// <summary>
        /// Gets the permission model service.
        /// </summary>
        public IPermissionModelService PermissionModelService { get; private set; }

        /// <summary>
        /// Gets the Resource Service.
        /// </summary>
        public IResourceService ResourceService { get; private set; }

        /// <summary>
        /// List of Permissions that have been loaded for the user, from one of the Load methods
        /// </summary>
        public List<IPermission> Permissions { get; set; }

        /// <summary>
        /// Application Id property (optional), allows access to simpler HasPermission methods
        /// </summary>
        public int? ResourceId { get; set; }

        /// <summary>
        /// Principal Id property (optional), allows access to simpler HasPermission methods
        /// </summary>
        public int? PrincipalId { get; set; }

        /// <summary>
        /// Given a PermissionName, and property ApplicationResourceId and PrincipalId, determines if that permission exists in the list of permissions
        /// </summary>
        /// <param name="permissionName">Permission Name as string to check</param>
        /// <returns>true if permission is found in list of permissions for the property ApplicationResourceId and PrincipalId</returns>
        public bool HasPermission(string permissionName)
        {
            if (ResourceId == null || PrincipalId == null)
                return false;
            var permissionId = this.PermissionModelService.GetPermissionIdByName(permissionName);
            return HasPermission(PrincipalId.Value, permissionId, ResourceId.Value);
        }

        /// <summary>
        /// Given a permission object, determines if that permission exists in the list of permissions
        /// </summary>
        /// <param name="permission"></param>
        /// <returns>true if permission is found in list of permissions</returns>
        public bool HasPermission(IPermission permission)
        {
            return Permissions.Contains(permission);
        }

        /// <summary>
        /// Given a PrincipalId, PermissionName, and ResourceId, determines if that permission exists in the list of permissions
        /// </summary>
        /// <param name="principalId">User or Group Id</param>
        /// <param name="permissionName">Permission Name as a string, i.e. 'Can Access Application'</param>
        /// <param name="resourceId">Id of the Resource</param>
        /// <returns>true if permission is found in list of permissions</returns>
        public bool HasPermission(int principalId, string permissionName, int resourceId)
        {
            var permissionId = this.PermissionModelService.GetPermissionIdByName(permissionName);
            return HasPermission(principalId, permissionId, resourceId);
        }

        /// <summary>
        /// Given a PrincipalId, PermissionId, and ResourceId, determines if that permission exists in the list of permissions
        /// </summary>
        /// <param name="principalId">User or Group Id</param>
        /// <param name="permissionId">Permission Id to check</param>
        /// <param name="resourceId">Id of the Resource</param>
        /// <returns>true if permission is found in list of permissions</returns>
        public bool HasPermission(int principalId, int permissionId, int resourceId)
        {
            IPermission permission = new SimplePermission(principalId, permissionId, resourceId);
            return Permissions.Contains(permission);
        }

        /// <summary>
        /// Given a PrincipalId, and ResourceId, determines if a permission exists in the list of permissions
        /// </summary>
        /// <param name="principalId">User or Group Id</param>
        /// <param name="resourceId">Id of the Resource</param>
        /// <returns>true if user has any permissions for a given resource</returns>
        public bool HasPermission(int principalId, int resourceId)
        {
            return Permissions.Find(p => p.PrincipalId == principalId && p.ResourceId == resourceId) != null;
        }

        /// <summary>
        /// Given a PrincipalId, PermissionName, and ApplicationId, determines if that permission exists in the list of permissions
        /// </summary>
        /// <param name="principalId">User or Group Id</param>
        /// <param name="permissionName">Permission Name as string to check</param>
        /// <param name="applicationId">Id of the Application, from the Application table</param>
        /// <returns>true if permission is found in list of permissions for the giving applicationId</returns>
        public bool HasPermissionForApplication(int principalId, string permissionName, int applicationId)
        {
            int? resourceId = this.ResourceService.GetResourceIdForApplicationId(applicationId);
            Contract.Assert(resourceId.HasValue, "The application resource id should have a value.");
            var permissionId = this.PermissionModelService.GetPermissionIdByName(permissionName);
            return HasPermission(principalId, permissionId, resourceId.Value);
        }

        /// <summary>
        /// Given a PrincipalId, PermissionName, and property ApplicationResourceId, determines if that permission exists in the list of permissions
        /// </summary>
        /// <param name="principalId">User or Group Id</param>
        /// <param name="permissionName">Permission Name as string to check</param>
        /// <returns>true if permission is found in list of permissions for the property ApplicationResourceId</returns>
        public bool HasPermissionForApplication(int principalId, string permissionName)
        {
            if (ResourceId == null)
                return false;
            var permissionId = this.PermissionModelService.GetPermissionIdByName(permissionName);
            return HasPermission(principalId, permissionId, ResourceId.Value);
        }

        /// <summary>
        /// Given a PrincipalId, and ApplicationResourceId, determines if there are permission other than those in the 
        /// excluded list of permissions
        /// </summary>
        /// <param name="excludedPermissionNames">A list of Permission Name as string to be excluded from this check</param>
        /// <returns>true if user has any other permissions for a given application</returns>               
        public bool HasPermissionForApplication(List<string> excludedPermissionNames)
        {
            bool retCode = false;
            if (ResourceId != null && PrincipalId != null)
            {
                int maxPermissions = Permissions.Count;
                foreach (string item in excludedPermissionNames)
                {
                    var permissionId = this.PermissionModelService.GetPermissionIdByName(item);
                    if (HasPermission(PrincipalId.Value, permissionId, ResourceId.Value))
                        --maxPermissions;
                }
                if (maxPermissions > 0)
                    retCode = true;
            }
            return retCode;
        }

        /// <summary>
        /// Get User Permissions For all resources - internal, used to populate PermissionsStore.Perissions
        /// </summary>
        /// <param name="principalId"></param>
        /// <returns></returns>
        protected async Task<List<IPermission>> GetUserPermissionsAsync(int principalId)
        {
            var stopwatch = Stopwatch.StartNew();
            var permissionsList = await CreateGetAllowedPermissionsByPrincipalIdQuery(principalId).ToListAsync();
            stopwatch.Stop();
            logger.Trace("Time Elasped: {0}", stopwatch.Elapsed);
            return permissionsList;
        }

        /// <summary>
        /// Get User Permissions For all resources - internal, used to populate PermissionsStore.Perissions
        /// </summary>
        /// <param name="principalId"></param>
        /// <returns></returns>
        protected List<IPermission> GetUserPermissions(int principalId)
        {
            var stopwatch = Stopwatch.StartNew();
            var permissionsList = CreateGetAllowedPermissionsByPrincipalIdQuery(principalId).ToList();
            stopwatch.Stop();
            logger.Trace("Time Elasped: {0}", stopwatch.Elapsed);
            return permissionsList;
        }

        private IQueryable<IPermission> CreateGetAllowedPermissionsByPrincipalIdQuery(int principalId)
        {
            var query = ResourceQueries.CreateGetResourceAuthorizationsQuery(this.Context);
            var permissionsQuery = query
                .Where(x => x.PrincipalId == principalId)
                .Where(x => x.IsAllowed)
                .OrderBy(x => x.PrincipalId)
                .ThenBy(x => x.ResourceId)
                .ThenBy(x => x.PermissionId)
                .Select(x => new CAM.Business.Service.SimplePermission
                {
                    IsAllowed = x.IsAllowed,
                    PermissionId = x.PermissionId,
                    PrincipalId = x.PrincipalId,
                    ResourceId = x.ResourceId
                }).Distinct();
            return permissionsQuery;
        }
    }
}

