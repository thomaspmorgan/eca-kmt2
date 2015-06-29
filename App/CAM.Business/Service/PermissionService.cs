using CAM.Data;
using System.Data.Entity;
using ECA.Core.Service;
using NLog.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using CAM.Business.Queries;

namespace CAM.Business.Service
{
    /// <summary>
    /// The PermissionService is used to maintain references to a permission and its parent permission if it has one and
    /// help determine whether a permission can be granted based on a resource and its parent.
    /// </summary>
    public class PermissionService : DbContextService<CamModel>, IPermissionService
    {
        /// <summary>
        /// The format string for the permission model key in the cache.
        /// </summary>
        public const string PERMISSION_CACHE_KEY_FORMAT = "id:{0}|permission:{1}";

        /// <summary>
        /// The default amount of time to cache a resource equal to 10 minutes.
        /// </summary>
        public const int DEFAULT_CACHE_TIME_TO_LIVE_IN_SECONDS = 10 * 60;

        private readonly ILogger logger = new LoggerAdapter(NLog.LogManager.GetCurrentClassLogger());
        private readonly ObjectCache cache;
        private readonly int timeToLiveInSeconds;

        /// <summary>
        /// Creates a new PermissionService with the given context and caching parameters.
        /// </summary>
        /// <param name="model">The context to operate against.</param>
        /// <param name="objectCache">The caching mechanism.</param>
        /// <param name="timeToLiveInSeconds">The length of time a permission will exist in the cache.</param>
        public PermissionService(CamModel model, ObjectCache objectCache = null, int timeToLiveInSeconds = DEFAULT_CACHE_TIME_TO_LIVE_IN_SECONDS)
            : base(model)
        {
            Contract.Requires(model != null, "The model must not be null.");
            this.cache = objectCache ?? MemoryCache.Default;
            this.timeToLiveInSeconds = timeToLiveInSeconds;
        }

        #region Caching

        private void ItemRemoved(CacheEntryRemovedArguments arguments)
        {
            var key = arguments.CacheItem.Key;
            var removedReason = arguments.RemovedReason;
            logger.Info("Permission cache with id [{0}] removed because [{1}].", key, removedReason.ToString());
        }

        /// <summary>
        /// The cache item policy that will be used for storing permission cache.
        /// </summary>
        /// <returns>The cache item policy that will be used for storing permission cache.</returns>
        public CacheItemPolicy GetCacheItemPolicy()
        {
            var policy = new CacheItemPolicy();
            policy.SlidingExpiration = TimeSpan.FromSeconds((double)this.timeToLiveInSeconds);
            policy.RemovedCallback = ItemRemoved;
            return policy;
        }

        /// <summary>
        /// The permission model cache item to add.
        /// </summary>
        /// <param name="permissionModel">The permission model cache item to add.</param>
        /// <returns>The cache item.</returns>
        public CacheItem Add(PermissionModel permissionModel)
        {
            Contract.Requires(permissionModel != null, "The permission model must not be null.");
            var cacheItem = new CacheItem(GetKey(permissionModel), permissionModel);
            cache.Set(cacheItem, GetCacheItemPolicy());
            return cacheItem;
        }
        
        /// <summary>
        /// Returns a key for the cache.
        /// </summary>
        /// <param name="permission">The permission to get a key for.</param>
        /// <returns>The cache key.</returns>
        public string GetKey(PermissionModel permission)
        {
            Contract.Requires(permission != null, "The permission must not be null.");
            return GetKey(permission.Id, permission.Name);
        }
        
        /// <summary>
        /// Returns a key for the cache.
        /// </summary>
        /// <param name="permissionId">The id of the permission.</param>
        /// <param name="permissionName">The name of the permission.</param>
        /// <returns>The cache key.</returns>
        public string GetKey(int permissionId, string permissionName)
        {
            return String.Format(PERMISSION_CACHE_KEY_FORMAT, permissionId, permissionName);
        }

        /// <summary>
        /// Returns a key for the cache.
        /// </summary>
        /// <param name="permissionId">The id of the permission.</param>
        /// <returns>The cache key.</returns>
        public string GetKey(int permissionId)
        {
            var lookup = Permission.GetStaticLookup(permissionId);
            return GetKey(lookup.Id, lookup.Value);
        }

        /// <summary>
        /// Returns a key for the cache.
        /// </summary>
        /// <param name="permissionName">The name of the permission.</param>
        /// <returns>The cache key.</returns>
        public string GetKey(string permissionName)
        {
            var lookup = Permission.GetStaticLookup(permissionName);
            return GetKey(lookup.Id, lookup.Value);
        }
        #endregion

        #region Get

        private IQueryable<PermissionModel> CreateGetPermissionModelsQuery()
        {
            var query = from permission in Context.Permissions
                        
                        join parentPermission in Context.Permissions
                        on permission.ParentResourceTypeId equals parentPermission.ResourceTypeId into pp
                        from tempParentPermission in pp.DefaultIfEmpty()

                        select new PermissionModel
                        {
                            Id = permission.PermissionId,
                            Name = permission.PermissionName,
                            ParentResourceTypeId = permission.ParentResourceTypeId,
                            ResourceTypeId = permission.ResourceTypeId,
                            ParentPermissionId = tempParentPermission != null ? tempParentPermission.PermissionId : default(int?)
                        };
            return query;
        }

        /// <summary>
        /// Returns a query to retrieve permissions with the given name.
        /// </summary>
        /// <param name="name">The name of the permission.</param>
        /// <returns>The query to get permissions with the given name.</returns>
        public IQueryable<PermissionModel> CreateGetPermissionModelsByNameQuery(string name)
        {
            return CreateGetPermissionModelsQuery().Where(x => x.Name == name);
        }

        /// <summary>
        /// Returns a query to retrieve permissions with the given id.
        /// </summary>
        /// <param name="id">The id of the permission.</param>
        /// <returns>The query to get permissions with the given id.</returns>
        public IQueryable<PermissionModel> CreateGetPermissionModelsByPermissionIdQuery(int id)
        {
            return CreateGetPermissionModelsQuery().Where(x => x.Id == id);
        }
        
        /// <summary>
        /// Returns the permission with the given name.
        /// </summary>
        /// <param name="permissionName">The name of the permission.</param>
        /// <returns>The permission.</returns>
        public PermissionModel GetPermissionByName(string permissionName)
        {
            var cacheItem = this.cache.Get(GetKey(permissionName));
            if (cacheItem == null)
            {
                var permissionModel = CreateGetPermissionModelsByNameQuery(permissionName).FirstOrDefault();
                Add(permissionModel);
                return permissionModel;
            }
            else
            {
                return (PermissionModel)cacheItem;
            }
        }

        /// <summary>
        /// Returns the permission with the given name.
        /// </summary>
        /// <param name="permissionName">The name of the permission.</param>
        /// <returns>The permission.</returns>
        public async Task<PermissionModel> GetPermissionByNameAsync(string permissionName)
        {
            var cacheItem = this.cache.Get(GetKey(permissionName));
            if (cacheItem == null)
            {
                var permissionModel = await CreateGetPermissionModelsByNameQuery(permissionName).FirstOrDefaultAsync();
                Add(permissionModel);
                return permissionModel;
            }
            else
            {
                return (PermissionModel)cacheItem;
            }
        }

        /// <summary>
        /// Returns the permission with the given id.
        /// </summary>
        /// <param name="id">The id of the permission.</param>
        /// <returns>The permission.</returns>
        public PermissionModel GetPermissionById(int id)
        {
            var cacheItem = this.cache.Get(GetKey(id));
            if (cacheItem == null)
            {
                var permissionModel = CreateGetPermissionModelsByPermissionIdQuery(id).FirstOrDefault();
                Add(permissionModel);
                return permissionModel;
            }
            else
            {
                return (PermissionModel)cacheItem;
            }
        }

        /// <summary>
        /// Returns the permission with the given id.
        /// </summary>
        /// <param name="id">The id of the permission.</param>
        /// <returns>The permission.</returns>
        public async Task<PermissionModel> GetPermissionByIdAsync(int id)
        {
            var cacheItem = this.cache.Get(GetKey(id));
            if (cacheItem == null)
            {
                var permissionModel = await CreateGetPermissionModelsByPermissionIdQuery(id).FirstOrDefaultAsync();
                Add(permissionModel);
                return permissionModel;
            }
            else
            {
                return (PermissionModel)cacheItem;
            }
        }
        #endregion

        #region Get User Permissions

        /// <summary>
        /// Returns a query that gets all permissions (granted, revoked, and inherited) for the user with the given principal id.
        /// </summary>
        /// <param name="principalId">The principal id.</param>
        /// <returns>The granted, revoked, and inherited permissions of the principal with the given id.</returns>
        public IQueryable<IPermission> CreateGetAllowedPermissionsByPrincipalIdQuery(int principalId)
        {
            var query = ResourceQueries.CreateGetResourceAuthorizationsQuery(this.Context)
                .Where(x => x.PrincipalId == principalId);
            var groupedQuery = CreateCollapsePermissionsQuery(query)
                .OrderBy(x => x.PrincipalId)
                .ThenBy(x => x.ResourceId)
                .ThenBy(x => x.PermissionId);
            return groupedQuery;
        }

        /// <summary>
        /// Returns the permissions currently inherited, granted, and revoked for the principal with the given id.
        /// </summary>
        /// <param name="principalId">The principal id.</param>
        /// <returns>The permissions of the principal with the given id.</returns>
        public List<IPermission> GetAllowedPermissionsByPrincipalId(int principalId)
        {
            return CreateGetAllowedPermissionsByPrincipalIdQuery(principalId).ToList();
        }

        /// <summary>
        /// Returns the permissions currently inherited, granted, and revoked for the principal with the given id.
        /// </summary>
        /// <param name="principalId">The principal id.</param>
        /// <returns>The permissions of the principal with the given id.</returns>
        public Task<List<IPermission>> GetAllowedPermissionsByPrincipalIdAsync(int principalId)
        {
            return CreateGetAllowedPermissionsByPrincipalIdQuery(principalId).ToListAsync();
        }

        /// <summary>
        /// Creates a query to group the given permissions by principal, resource, and permission and calculate whether
        /// the permission is allowed.
        /// </summary>
        /// <param name="permissions">The permissions to group.</param>
        /// <returns>A query to group the given permissions and calculate whether that permission is allowed.</returns>
        public IQueryable<SimplePermission> CreateCollapsePermissionsQuery(IQueryable<IPermission> permissions)
        {
            Contract.Requires(permissions != null, "The permissions must not be null.");
            var groupedPermissionsQuery = from permission in permissions
                                         group permission by new
                                         {
                                             ResourceId = permission.ResourceId,
                                             PermissionId = permission.PermissionId,
                                             PrincipalId = permission.PrincipalId,
                                             ForeignResourceId = permission.ForeignResourceId,
                                             ResourceTypeId = permission.ResourceTypeId
                                         } into g
                                         select new SimplePermission
                                         {
                                             PermissionId = g.Key.PermissionId,
                                             PrincipalId = g.Key.PrincipalId,
                                             ResourceId = g.Key.ResourceId,
                                             ForeignResourceId = g.Key.ForeignResourceId,
                                             ResourceTypeId = g.Key.ResourceTypeId,
                                             IsAllowed = !(g.Where(x => !x.IsAllowed).Count() > 0)
                                         };
            return groupedPermissionsQuery;
        }

        /// <summary>
        /// Returns true, if the given permissions contain a permission for the given resource and permission by id.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="parentResourceId">The parent resource id of the resource.</param>
        /// <param name="permissionId">The permission by id to check.</param>
        /// <param name="permissions">The list of permissions to check.</param>
        /// <returns>True, if the given permissions contains the desired permission, otherwise, false.</returns>
        public bool HasPermission(int resourceId, int? parentResourceId, int permissionId, List<IPermission> permissions)
        {
            var queryablePermissions = permissions.AsQueryable();
            var groupedPermissions = CreateCollapsePermissionsQuery(queryablePermissions).Where(x => x.PermissionId == permissionId);
            var resoucePermission = groupedPermissions.Where(x => x.ResourceId == resourceId).FirstOrDefault();
            IPermission parentPermission = null;
            if (parentResourceId.HasValue)
            {
                parentPermission = groupedPermissions.Where(x => x.ResourceId == parentResourceId.Value).FirstOrDefault();
            }
            if (resoucePermission != null)
            {
                return resoucePermission.IsAllowed;
            }
            else if(parentPermission != null)
            {
                return parentPermission.IsAllowed;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
