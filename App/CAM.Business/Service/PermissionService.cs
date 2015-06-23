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
    public class PermissionService : DbContextService<CamModel>, IPermissionService
    {
        /// <summary>
        /// The format string for the permission model key in the cache.
        /// </summary>
        public const string PERMISSION_CACHE_KEY_FORMAT = "id:{0}|permissionName:{1}";

        /// <summary>
        /// The default amount of time to cache a resource equal to 10 minutes.
        /// </summary>
        public const int DEFAULT_CACHE_TIME_TO_LIVE_IN_SECONDS = 10 * 60;

        private readonly ILogger logger = new LoggerAdapter(NLog.LogManager.GetCurrentClassLogger());
        private readonly ObjectCache cache;
        private readonly int timeToLiveInSeconds;

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
            logger.Info("Foreign resource cache with id [{0}] removed because [{1}].", key, removedReason.ToString());
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

        public IQueryable<PermissionModel> CreateGetPermissionModelsByNameQuery(string name)
        {
            return CreateGetPermissionModelsQuery().Where(x => x.Name == name);
        }

        public IQueryable<PermissionModel> CreateGetPermissionModelsByPermissionIdQuery(int id)
        {
            return CreateGetPermissionModelsQuery().Where(x => x.Id == id);
        }
        
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

        public IQueryable<IPermission> CreateGetAllowedPermissionsByPrincipalIdQuery(int principalId)
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

        public List<IPermission> GetAllowedPermissionsByPrincipalId(int principalId)
        {
            return CreateGetAllowedPermissionsByPrincipalIdQuery(principalId).ToList();
        }

        public Task<List<IPermission>> GetAllowedPermissionsByPrincipalIdAsync(int principalId)
        {
            return CreateGetAllowedPermissionsByPrincipalIdQuery(principalId).ToListAsync();
        }

        public IQueryable<IPermission> CreateHasPermissionQuery(int resourceId, int? parentResourceId, int permissionId, IQueryable<IPermission> grantedPermissions)
        {
            var groupedPermissionsQuery = from permission in grantedPermissions
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

        public bool HasPermission(int resourceId, int? parentResourceId, int permissionId, List<IPermission> grantedPermissions)
        {
            var query = CreateHasPermissionQuery(resourceId, parentResourceId, permissionId, grantedPermissions.AsQueryable()).FirstOrDefault();
            return query != null;
        }
        #endregion
    }
}
