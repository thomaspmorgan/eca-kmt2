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
        /// Returns a query that gets all allowed permissions for the user with the given principal id.
        /// </summary>
        /// <param name="principalId">The principal id.</param>
        /// <returns>The allowed permissions of the principal with the given id.</returns>
        public IQueryable<IPermission> CreateGetAllowedPermissionsByPrincipalIdQuery(int principalId)
        {
            var query = ResourceQueries.CreateGetResourceAuthorizationsQuery(this.Context);
            var permissionsQuery = query
                .Where(x => x.PrincipalId == principalId)
                .Where(x => x.IsAllowed)
                .OrderBy(x => x.PrincipalId)
                .ThenBy(x => x.ResourceId)
                .ThenBy(x => x.PermissionId)
                .Select(x => new SimplePermission
                {
                    ForeignResourceId = x.ForeignResourceId,
                    IsAllowed = x.IsAllowed,
                    PermissionId = x.PermissionId,
                    PrincipalId = x.PrincipalId,
                    ResourceId = x.ResourceId,
                    ResourceTypeId = x.ResourceTypeId
                }).Distinct();
            return permissionsQuery;
        }

        /// <summary>
        /// Returns the permissions currently allowed of the principal with the given id.
        /// </summary>
        /// <param name="principalId">The principal id.</param>
        /// <returns>The permissions of the principal with the given id.</returns>
        public List<IPermission> GetAllowedPermissionsByPrincipalId(int principalId)
        {
            return CreateGetAllowedPermissionsByPrincipalIdQuery(principalId).ToList();
        }

        /// <summary>
        /// Returns the permissions currently allowed of the principal with the given id.
        /// </summary>
        /// <param name="principalId">The principal id.</param>
        /// <returns>The permissions of the principal with the given id.</returns>
        public Task<List<IPermission>> GetAllowedPermissionsByPrincipalIdAsync(int principalId)
        {
            return CreateGetAllowedPermissionsByPrincipalIdQuery(principalId).ToListAsync();
        }

        /// <summary>
        /// Returns a query that filters the given granted permissions into only allowed permissions
        /// with the given resourceId, parentResourceId, and PermissionId.  Use this method to determine
        /// if a permission exists for either the resource directly or the resource's parent.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="parentResourceId">The parent resource id.</param>
        /// <param name="permissionId">The permission id.</param>
        /// <param name="permissions">The permissions to locate the allowed permission in.</param>
        /// <returns>A query to determine if a permission exists in the given permissions for the resource and its parent by id.</returns>
        public IQueryable<IPermission> CreateHasPermissionQuery(int resourceId, int? parentResourceId, int permissionId, IQueryable<IPermission> permissions)
        {
            var groupedPermissionsQuery = from permission in permissions
                                         group permission by new
                                         {
                                             ResourceId = permission.ResourceId,
                                             PermissionId = permission.PermissionId,
                                             PrincipalId = permission.PrincipalId
                                         } into g
                                         select new SimplePermission
                                         {
                                             PermissionId = g.Key.PermissionId,
                                             PrincipalId = g.Key.PrincipalId,
                                             ResourceId = g.Key.ResourceId,
                                             IsAllowed = !(g.Where(x => !x.IsAllowed).Count() > 0)
                                         };

            var query = groupedPermissionsQuery.Where(x =>
                x.IsAllowed
                && x.PermissionId == permissionId
                && (x.ResourceId == resourceId || (parentResourceId.HasValue && x.ResourceId == parentResourceId.Value)));
            return query;
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
            var allowedPermission = CreateHasPermissionQuery(resourceId, parentResourceId, permissionId, permissions.AsQueryable()).FirstOrDefault();
            return allowedPermission != null;
        }
        #endregion
    }
}
