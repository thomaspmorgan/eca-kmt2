using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System;
using System.Diagnostics.Contracts;
using System.Runtime.Caching;
using System.Threading.Tasks;
namespace ECA.WebApi.Security
{
    /// <summary>
    /// An IUserCacheService is capable of caching a user's UserCache in the system for a temporary amount of time.
    /// </summary>
    [ContractClass(typeof(UserCacheServiceContract))]
    public interface IUserCacheService
    {
        /// <summary>
        /// Adds the given UserCache object to the system cache and returns the resultant CacheItem.
        /// </summary>
        /// <param name="userCache">The user cache object to cache.</param>
        /// <returns>The system's cache item.</returns>
        CacheItem Add(UserCache userCache);

        /// <summary>
        /// Returns the user's cache object from system cache.
        /// </summary>
        /// <param name="user">The user to retrieve a cache object for.</param>
        /// <returns>The user cache.</returns>
        UserCache GetUserCache(IWebApiUser user);

        /// <summary>
        /// Removes all cached items for the given user.
        /// </summary>
        /// <param name="user">The user to remove cache for.</param>
        void Remove(IWebApiUser user);

        /// <summary>
        /// Removes all cached items for the given user.
        /// </summary>
        /// <param name="userId">The user id to remove cache for.</param>
        void Remove(Guid userId);

    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IUserCacheService))]
    public abstract class UserCacheServiceContract : IUserCacheService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userCache"></param>
        /// <returns></returns>
        public CacheItem Add(UserCache userCache)
        {
            Contract.Requires(userCache != null, "The user cache must not be null.");
            Contract.Ensures(Contract.Result<CacheItem>() != null, "The cache item returned must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserCache GetUserCache(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task<UserCache> GetUserCacheAsync(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return Task.FromResult<UserCache>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void Remove(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public void Remove(Guid userId)
        {
            
        }
    }
}
