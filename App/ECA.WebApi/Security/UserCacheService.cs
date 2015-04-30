using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using NLog;
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
    /// <summary>
    /// The UserCacheService is a service to handle caching user details in the web api application.
    /// </summary>
    public class UserCacheService : ECA.WebApi.Security.IUserCacheService
    {
        /// <summary>
        /// The default amount of time to cache a user's details equal to 10 minutes.
        /// </summary>
        public const int DEFAULT_CACHE_TIME_TO_LIVE_IN_SECONDS = 10 * 60;

        private ObjectCache cache;
        private readonly int timeToLiveInSeconds;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new UserCacheService.  If the ObjectCache is null, the MemoryCache.Default instance will be used.  If the time to live
        /// in seconds is null, the default value is used.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="cache">The cache object to store user cache details to.</param>
        /// <param name="timeToLiveInSeconds">The time a cache item is valid before it is invalidated.</param>
        public UserCacheService(ObjectCache cache = null, int timeToLiveInSeconds = DEFAULT_CACHE_TIME_TO_LIVE_IN_SECONDS)
        {
            this.cache = cache ?? MemoryCache.Default;
            this.timeToLiveInSeconds = timeToLiveInSeconds;
        }

        /// <summary>
        /// The user cache item to add.
        /// </summary>
        /// <param name="userCache">The user cache item to add.</param>
        /// <returns>The cacheitem.</returns>
        public CacheItem Add(UserCache userCache)
        {
            var cacheItem = new CacheItem(GetKey(userCache.UserId), userCache);
            cache.Set(cacheItem, GetCacheItemPolicy());
            return cacheItem;
        }

        /// <summary>
        /// Returns the user's cache item.  If the cache item does not exist or has expired it is reloaded.
        /// </summary>
        /// <param name="user">The user to get the cache for.</param>
        /// <returns>The UserCache instance.</returns>
        public UserCache GetUserCache(IWebApiUser user)
        {
            var cachedObject = cache.Get(GetKey(user));
            if (cachedObject == null)
            {
                throw new NotSupportedException("The user should have a cached object in the system cache.  Be sure use to the IsUserCached method and Add method for user cache logic.");
            }
            UserCache userCache = userCache = (UserCache)cachedObject;
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
            return GetKey(user.Id);
        }

        /// <summary>
        /// Returns the string key to use for the cache.
        /// </summary>
        /// <param name="userId">The guid of the user.</param>
        /// <returns>The user id as a string to be used as a key for the cache.</returns>
        public string GetKey(Guid userId)
        {
            return userId.ToString();
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
            policy.RemovedCallback = ItemRemoved;
            return policy;
        }

        private void ItemRemoved(CacheEntryRemovedArguments arguments)
        {
            // The arguments object contains information about the removed item such as: 
            var key = arguments.CacheItem.Key;
            var removedReason = arguments.RemovedReason;
            logger.Info("User cache item with id [{0}] removed because [{1}].", key, removedReason.ToString());
        }

        /// <summary>
        /// Returns true if the user has a cache item.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <returns>True, if a UserCache exists for the given user.</returns>
        public bool IsUserCached(IWebApiUser user)
        {
            return IsUserCached(user.Id);
        }

        /// <summary>
        /// Returns true if the user has a cache item.
        /// </summary>
        /// <param name="userId">The user id to check.</param>
        /// <returns>True, if a UserCache exists for the given user.</returns>
        public bool IsUserCached(Guid userId)
        {
            return this.cache.Get(GetKey(userId)) != null;
        }

        /// <summary>
        /// Removes the user from the cache.
        /// </summary>
        /// <param name="user">The user to remove.</param>
        public void Remove(IWebApiUser user)
        {
            Remove(user.Id);
        }
        /// <summary>
        /// Removes the user from the cache.
        /// </summary>
        /// <param name="userId">The user id of the user to remove.</param>
        public void Remove(Guid userId)
        {            
            this.cache.Remove(GetKey(userId));
            this.logger.Info("Removed user with id [{0}] cache.", userId);
        }

    }
}