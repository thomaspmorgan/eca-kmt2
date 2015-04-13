using ECA.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace ECA.WebApi.Security
{
    public interface IBusinessUserService
    {
        Task<List<ResourcePermission>> GetResourcePermissionsAsync(Guid userId);
        List<ResourcePermission> GetResourcePermissions(Guid userId);
    }

    public class TestBusinessUserService : IBusinessUserService
    {

        private List<ResourcePermission> permissions = new List<ResourcePermission>
        {
            new ResourcePermission
            {
                PermissionName = "Edit",
                ResourceId = 1,
                ResourceType = "Project"
            }
        };

        public Task<List<ResourcePermission>> GetResourcePermissionsAsync(Guid userId)
        {
            return Task.FromResult<List<ResourcePermission>>(permissions);
        }

        public List<ResourcePermission> GetResourcePermissions(Guid userId)
        {
            return permissions;
        }
    }

    /// <summary>
    /// The UserCacheService is a service to handle caching user details in the web api application.
    /// </summary>
    public class UserCacheService : ECA.WebApi.Security.IUserCacheService
    {
        /// <summary>
        /// The default amount of time to cache a user's details equal to 5 minutes.
        /// </summary>
        public const int DEFAULT_CACHE_TIME_TO_LIVE_IN_SECONDS = 180;

        private static readonly string COMPONENT_NAME = typeof(UserCacheService).FullName;
        private ObjectCache cache;
        private readonly int timeToLiveInSeconds;
        private readonly IBusinessUserService service;
        private readonly ILogger logger;

        /// <summary>
        /// Creates a new UserCacheService.  If the ObjectCache is null, the MemoryCache.Default instance will be used.  If the time to live
        /// in seconds is null, the default value is used.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="service">The user service.</param>
        /// <param name="cache">The cache object to store user cache details to.</param>
        /// <param name="timeToLiveInSeconds">The time a cache item is valid before it is invalidated.</param>
        public UserCacheService(ILogger logger, IBusinessUserService service, ObjectCache cache = null, int timeToLiveInSeconds = DEFAULT_CACHE_TIME_TO_LIVE_IN_SECONDS)
        {
            Contract.Requires(logger != null, "The logger must not be null.");
            this.cache = cache ?? MemoryCache.Default;
            this.timeToLiveInSeconds = timeToLiveInSeconds;
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// The user cache item to add.
        /// </summary>
        /// <param name="userCache">The user cache item to add.</param>
        /// <returns>The cacheitem.</returns>
        public CacheItem Add(UserCache userCache)
        {
            Contract.Requires(userCache != null, "The user cache item must not be null.");
            var cacheItem = new CacheItem(GetKey(userCache.User), userCache);
            cache.Set(cacheItem, GetCacheItemPolicy());
            return cacheItem;
        }

        /// <summary>
        /// Returns the user's cache item.  If the cache item does not exist or has expired it is reloaded.
        /// </summary>
        /// <param name="user">The user to get the cache for.</param>
        /// <returns>The UserCache instance.</returns>
        public async Task<UserCache> GetUserCacheAsync(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            var stopWatch = Stopwatch.StartNew();
            var cachedObject = cache.Get(GetKey(user));
            UserCache userCache;
            if (cachedObject == null)
            {
                var permissions = await service.GetResourcePermissionsAsync(user.Id);
                userCache = new UserCache(user, permissions);
                Add(userCache);
            }
            else
            {
                userCache = (UserCache)cachedObject;
            }
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return userCache;
        }

        /// <summary>
        /// Returns the user's cache item.  If the cache item does not exist or has expired it is reloaded.
        /// </summary>
        /// <param name="user">The user to get the cache for.</param>
        /// <returns>The UserCache instance.</returns>
        public UserCache GetUserCache(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            var stopWatch = Stopwatch.StartNew();
            var cachedObject = cache.Get(GetKey(user));
            UserCache userCache;
            if (cachedObject == null)
            {
                var permissions = service.GetResourcePermissions(user.Id);
                userCache = new UserCache(user, permissions);
                Add(userCache);
            }
            else
            {
                userCache = (UserCache)cachedObject;
            }
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return userCache;
        }

        /// <summary>
        /// Returns the string key to use for the cache.
        /// </summary>
        /// <param name="user">The user to retrieve a key for.</param>
        /// <returns>The user id as a string to be used as a key for the cache.</returns>
        public string GetKey(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return user.Id.ToString();
        }

        /// <summary>
        /// Returns the number of items in the cache.
        /// </summary>
        /// <returns>The number of items in the cache.</returns>
        public long GetCount()
        {
            return this.cache.GetCount();
        }

        /// <summary>
        /// The cache item policy that will be used for storing user cache.
        /// </summary>
        /// <returns>The cache item policy that will be used for storing user cache.</returns>
        public CacheItemPolicy GetCacheItemPolicy()
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds((double)this.timeToLiveInSeconds);
            return policy;
        }
    }
}