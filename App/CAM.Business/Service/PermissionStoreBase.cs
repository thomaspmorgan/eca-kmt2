﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAM.Data;
using System.Diagnostics;
using NLog.Interface;
using ECA.Core.Service;
using System.Diagnostics.Contracts;

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
        /// List all the PermissionModel (permission Ids and Names)  Allows PermissionName to Id lookup and vice versa.
        /// </summary>
        public List<PermissionModel> PermissionLookup { get; set; }

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
            return HasPermission(PrincipalId.Value, GetPermissionIdByName(permissionName), ResourceId.Value);
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
            return HasPermission(principalId, GetPermissionIdByName(permissionName), resourceId);
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
            IPermission permission = new Permission(principalId, permissionId, resourceId);
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
            return HasPermission(principalId, GetPermissionIdByName(permissionName), resourceId.Value);
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
            return HasPermission(principalId, GetPermissionIdByName(permissionName), ResourceId.Value);
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
                    if (HasPermission(PrincipalId.Value, GetPermissionIdByName(item), ResourceId.Value))
                        --maxPermissions;
                }
                if (maxPermissions > 0)
                    retCode = true;
            }
            return retCode;
        }

        /// <summary>
        /// Gets a permission Id given the Name of the permission
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>PermissionId</returns>
        public int GetPermissionIdByName(string permissionName)
        {
            Contract.Assert(PermissionLookup != null, "The permission lookup must not be null.");
            PermissionModel permission = PermissionLookup.Find(p => p.PermissionName == permissionName);
            if (permission == null)
                logger.Warn("Permission not found for PermissionName = '{0}'", permissionName);
            return permission == null ? 0 : permission.PermissionId;
        }

        /// <summary>
        /// Gets a permission Name given a permission Id
        /// </summary>
        /// <param name="permissionId">PermissionId</param>
        /// <returns>PermissionName</returns>
        public string GetPermissionNameById(int permissionId)
        {
            Contract.Assert(PermissionLookup != null, "The permission lookup must not be null.");
            PermissionModel permission = PermissionLookup.Find(p => p.PermissionId == permissionId);
            if (permission == null)
                logger.Warn("Permission not found for Permissionid = '{0}'", permissionId);
            return permission == null ? string.Empty : permission.PermissionName;
        }

        /// <summary>
        /// Get User Permissions For a resource - internal, used to populate PermissionsStore.Perissions
        /// </summary>
        /// <param name="principalId"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        protected List<IPermission> GetUserPermissionsForResource(int principalId, int resourceId)
        {
            var stopwatch = Stopwatch.StartNew();
            IEnumerable<IPermission> rolePermissions = (from p in this.Context.RoleResourcePermissions
                                                        join r in this.Context.PrincipalRoles on p.RoleId equals r.RoleId
                                                    where r.PrincipalId == principalId && p.ResourceId== resourceId
                                                    select new CAM.Business.Service.Permission
                                                    {
                                                        PrincipalId = r.PrincipalId,
                                                        IsAllowed = true,
                                                        PermissionId = p.PermissionId,
                                                        ResourceId = p.ResourceId,
                                                    });

            IEnumerable<IPermission> userPermissions = (from p in this.Context.PermissionAssignments
                                                 where p.PrincipalId == principalId && p.ResourceId == resourceId
                                                 select new CAM.Business.Service.Permission
                                                 {
                                                     PrincipalId = p.PrincipalId,
                                                     IsAllowed = p.IsAllowed,
                                                     PermissionId = p.PermissionId,
                                                     ResourceId = p.ResourceId,
                                                 }).ToList();

            var permissions = rolePermissions.Union(userPermissions);
            List<IPermission> uList = userPermissions.ToList();
            List<IPermission> permissionsList = permissions.ToList();
            List<IPermission> removeList;

            // remove negative permissions from list for user
            removeList = uList.FindAll(u => u.IsAllowed == false);
            foreach (IPermission perm in removeList)
            {
                while (permissionsList.Remove(permissionsList.Find(s => s.PrincipalId == perm.PrincipalId &&
                                                                s.PermissionId == perm.PermissionId &&
                                                                s.ResourceId == perm.ResourceId))) ;
            }
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
            IEnumerable<IPermission> rolePermissions = (from p in this.Context.RoleResourcePermissions
                                                        join r in this.Context.PrincipalRoles on p.RoleId equals r.RoleId
                                                    where r.PrincipalId == principalId 
                                                    select new CAM.Business.Service.Permission
                                                    {
                                                        PrincipalId = r.PrincipalId,
                                                        IsAllowed = true,
                                                        PermissionId = p.PermissionId,
                                                        ResourceId = p.ResourceId,
                                                    });

            IEnumerable<IPermission> userPermissions = (from p in this.Context.PermissionAssignments
                                                 where p.PrincipalId == principalId 
                                                 select new CAM.Business.Service.Permission
                                                 {
                                                     PrincipalId = p.PrincipalId,
                                                     IsAllowed = p.IsAllowed,
                                                     PermissionId = p.PermissionId,
                                                     ResourceId = p.ResourceId,
                                                 }).ToList();

            var permissions = rolePermissions.Union(userPermissions);
            List<IPermission> uList = userPermissions.ToList();
            List<IPermission> permissionsList = permissions.ToList();
            List<IPermission> removeList;

            // remove negative permissions from list for user
            removeList = uList.FindAll(u => u.IsAllowed == false);
            foreach (IPermission perm in removeList)
            {
                while (permissionsList.Remove(permissionsList.Find(s => s.PrincipalId == perm.PrincipalId &&
                                                                s.PermissionId == perm.PermissionId &&
                                                                s.ResourceId == perm.ResourceId))) ;
            }
            stopwatch.Stop();
            logger.Trace("Time Elasped: {0}", stopwatch.Elapsed);
            return permissionsList;
        }
      
    }
}

