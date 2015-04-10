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

    public class UserPermissionsService// : IDisposable
    {
        private static readonly string COMPONENT_NAME = typeof(UserPermissionsService).FullName;
        private ObjectCache cache;
        private readonly int timeToLiveInSeconds;
        private readonly IBusinessUserService service;
        private readonly ILogger logger;

        public UserPermissionsService(ILogger logger, int timeToLiveInSeconds, IBusinessUserService service, ObjectCache cache = null)
        {
            this.cache = cache ?? MemoryCache.Default;
            this.timeToLiveInSeconds = timeToLiveInSeconds;
            this.service = service;
            this.logger = logger;
        }

        public CacheItem Add(IWebApiUser user, IEnumerable<ResourcePermission> permissions)
        {
            var cacheItem = new CacheItem(GetCacheKey(user), permissions.ToList());
            cache.Set(cacheItem, GetCacheItemPolicy());
            return cacheItem;
        }

        public async Task<IEnumerable<ResourcePermission>> GetResourcePermissionsAsync(IWebApiUser user)
        {
            var stopWatch = Stopwatch.StartNew();
            var cachedObject = cache.Get(GetCacheKey(user));
            IEnumerable<ResourcePermission> permissions;
            if (cachedObject != null)
            {
                Contract.Assert(cachedObject is IEnumerable<ResourcePermission>, "The cached user permissions should be a list of resource permissions.");
                permissions = (IEnumerable<ResourcePermission>)cachedObject;
            }
            else
            {
                permissions = await service.GetResourcePermissionsAsync(user.Id);
                Add(user, permissions);
            }
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return permissions;
        }

        public IEnumerable<ResourcePermission> GetResourcePermissions(IWebApiUser user)
        {
            var stopWatch = Stopwatch.StartNew();
            var cachedObject = cache.Get(GetCacheKey(user));
            IEnumerable<ResourcePermission> permissions;
            if (cachedObject != null)
            {
                Contract.Assert(cachedObject is IEnumerable<ResourcePermission>, "The cached user permissions should be a list of resource permissions.");
                permissions = (IEnumerable<ResourcePermission>)cachedObject;
            }
            else
            {
                permissions = service.GetResourcePermissions(user.Id);
                Add(user, permissions);
            }
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return permissions;
        }


        public long GetCount()
        {
            return this.cache.GetCount();
        }

        public string GetCacheKey(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return user.Id.ToString();
        }

        public CacheItemPolicy GetCacheItemPolicy()
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(this.timeToLiveInSeconds);
            return policy;
        }

        //#region IDispose

        ///// <summary>
        ///// 
        ///// </summary>
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="disposing"></param>
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        this.cache.Dispose();
        //        this.cache = null;
        //    }
        //}

        //#endregion
    }
}