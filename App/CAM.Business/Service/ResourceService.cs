﻿using CAM.Data;
using System.Data.Entity;
using ECA.Core.Service;
using NLog.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace CAM.Business.Service
{
    /// <summary>
    /// A ResourceService is used to track resource's and their related types and foreign resource ids.  The 
    /// resources are cached in the given object cache.
    /// </summary>
    public class ResourceService : DbContextService<CamModel>, CAM.Business.Service.IResourceService
    {
        /// <summary>
        /// The format string for the resource key in the cache.
        /// </summary>
        public const string CACHE_KEY_FORMAT = "id:{0}|type:{1}";

        /// <summary>
        /// The default amount of time to cache a resource equal to 10 minutes.
        /// </summary>
        public const int DEFAULT_CACHE_TIME_TO_LIVE_IN_SECONDS = 10 * 60;

        private readonly ILogger logger = new LoggerAdapter(NLog.LogManager.GetCurrentClassLogger());
        private readonly ObjectCache cache;
        private readonly int timeToLiveInSeconds;

        /// <summary>
        /// Creates a new ResourceService with the given cam model, the caching object and the lifespan of a cached resource.
        /// </summary>
        /// <param name="model">The model to query for resources and types.</param>
        /// <param name="objectCache">The cache to store resources in, or the default memory cache is none is provided.</param>
        /// <param name="timeToLiveInSeconds">The sliding expiration time span in seconds.</param>
        public ResourceService(CamModel model, ObjectCache objectCache = null, int timeToLiveInSeconds = DEFAULT_CACHE_TIME_TO_LIVE_IN_SECONDS)
            : base(model)
        {
            Contract.Requires(model != null, "The model must not be null.");
            Contract.Requires(objectCache != null, "The object cache must not be null.");
            this.cache = objectCache ?? MemoryCache.Default;
            this.timeToLiveInSeconds = timeToLiveInSeconds;
        }

        /// <summary>
        /// Returns the resourceId for a given applicationId
        /// </summary>
        /// <param name="applicationId">ApplicationId (from table Application)</param>
        /// <returns>ResourceId</returns>
        public int? GetResourceIdForApplicationId(int applicationId)
        {
            return GetResourceIdByForeignResourceId(applicationId, ResourceType.Application.Id);
        }

        /// <summary>
        /// Returns the resourceId for a given applicationId
        /// </summary>
        /// <param name="applicationId">ApplicationId (from table Application)</param>
        /// <returns>ResourceId</returns>
        public Task<int?> GetResourceIdForApplicationIdAsync(int applicationId)
        {
            return GetResourceIdByForeignResourceIdAsync(applicationId, ResourceType.Application.Id);
        }


        private IQueryable<Resource> CreateGetResourceByForeignResourceIdQuery(int foreignResourceId, int resourceTypeId)
        {
            var query = from p in this.Context.Resources
                        where
                            p.ResourceTypeId == resourceTypeId &&
                            p.ForeignResourceId == foreignResourceId
                        select p;
            return query;
        }

        /// <summary>
        /// Get a ResourceId giving a foreignResourceId and a ResourceTypeId
        /// </summary>
        /// <param name="foreignResourceId"></param>
        /// <param name="resourceTypeId"></param>
        /// <returns></returns>
        public int? GetResourceIdByForeignResourceId(int foreignResourceId, int resourceTypeId)
        {
            if (IsCached(foreignResourceId, resourceTypeId))
            {
                return HandleCachedResource(foreignResourceId, resourceTypeId);
            }
            else
            {
                var resource = CreateGetResourceByForeignResourceIdQuery(foreignResourceId, resourceTypeId).FirstOrDefault();
                var resourceId = resource != null ? resource.ResourceId : default(int?);
                return HandleNonCachedResource(foreignResourceId, resourceId, resourceTypeId);
            }
        }

        /// <summary>
        /// Get a ResourceId giving a foreignResourceId and a ResourceTypeId
        /// </summary>
        /// <param name="foreignResourceId"></param>
        /// <param name="resourceTypeId"></param>
        /// <returns></returns>
        public async Task<int?> GetResourceIdByForeignResourceIdAsync(int foreignResourceId, int resourceTypeId)
        {
            if (IsCached(foreignResourceId, resourceTypeId))
            {
                return HandleCachedResource(foreignResourceId, resourceTypeId);
            }
            else
            {
                var resource = await CreateGetResourceByForeignResourceIdQuery(foreignResourceId, resourceTypeId).FirstOrDefaultAsync();
                var resourceId = resource != null ? resource.ResourceId : default(int?);
                return HandleNonCachedResource(foreignResourceId, resourceId, resourceTypeId);
            }
        }

        private int? HandleCachedResource(int foreignResourceId, int resourceTypeId)
        {
            var item = GetForeignResourceCache(foreignResourceId, resourceTypeId);
            Contract.Assert(item != null, "The item must not be null.");
            logger.Info("The foreign resource with id [{0}] of type [{1}] was cached with resource id [{2}].", foreignResourceId, resourceTypeId, item.ResourceId);
            return item.ResourceId;
        }

        private int? HandleNonCachedResource(int foreignResourceId, int? resourceId, int resourceTypeId)
        {
            if (!resourceId.HasValue)
            {
                logger.Warn("ResourceId not found for foreignResourceId = '{0}', resourceTypeId='{1}'", foreignResourceId, resourceTypeId);
            }
            else
            {
                Add(foreignResourceId, resourceId.Value, resourceTypeId);
            }
            return resourceId;
        }

        /// <summary>
        /// Returns the resource type id given the name or null of if the name is not found.
        /// </summary>
        /// <param name="resourceTypeName">The resource type name.</param>
        /// <returns>The resource type id.</returns>
        public int? GetResourceTypeId(string resourceTypeName)
        {
            var resourceType = ResourceType.GetStaticLookup(resourceTypeName);
            if (resourceType == null)
            {
                logger.Warn("ResourceTypeId not found for resourceTypeName = '{0}'", resourceTypeName);
            }
            return resourceType == null ? default(int?) : resourceType.Id;
        }

        /// <summary>
        /// Returns the cache key for the given resource.
        /// </summary>
        /// <param name="resourceCache">The resource cache object.</param>
        /// <returns>The key.</returns>
        public string GetKey(ForeignResourceCache resourceCache)
        {
            return GetKey(resourceCache.ForeignResourceId, resourceCache.ResourceTypeId);
        }

        /// <summary>
        /// Returns the cache key for the foreign resource and resource type ids.
        /// </summary>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <param name="resourceTypeId">The resource type id.</param>
        /// <returns>The cache key.</returns>
        public string GetKey(int foreignResourceId, int resourceTypeId)
        {
            return String.Format(CACHE_KEY_FORMAT, foreignResourceId, resourceTypeId);
        }

        /// <summary>
        /// Returns true if a cached object exists for an item with the given foreign resource id and resource type id.
        /// </summary>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <param name="resourceTypeId">The resource type id.</param>
        /// <returns>True, if a cached object exists, otherwise, false.</returns>
        public bool IsCached(int foreignResourceId, int resourceTypeId)
        {
            var key = GetKey(foreignResourceId, resourceTypeId);
            return cache.Get(key) != null;
        }

        /// <summary>
        /// Adds a ForeignResourceCache object with the given foreign resource id, resource id, and
        /// resource type id.
        /// </summary>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="resourceTypeId">The resource type id.</param>
        /// <returns>The cache item added.</returns>
        public CacheItem Add(int foreignResourceId, int resourceId, int resourceTypeId)
        {
            return Add(new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId));
        }

        /// <summary>
        /// The cache item policy that will be used for storing resource cache.
        /// </summary>
        /// <returns>The cache item policy that will be used for storing resource cache.</returns>
        public CacheItemPolicy GetCacheItemPolicy()
        {
            var policy = new CacheItemPolicy();
            policy.SlidingExpiration = TimeSpan.FromSeconds((double)this.timeToLiveInSeconds);
            policy.RemovedCallback = ItemRemoved;
            return policy;
        }

        private void ItemRemoved(CacheEntryRemovedArguments arguments)
        {
            // The arguments object contains information about the removed item such as: 
            var key = arguments.CacheItem.Key;
            var removedReason = arguments.RemovedReason;
            logger.Info("Foreign resource cache with id [{0}] removed because [{1}].", key, removedReason.ToString());
        }

        /// <summary>
        /// The resource cache item to add.
        /// </summary>
        /// <param name="resourceCache">The resource cache item to add.</param>
        /// <returns>The cacheitem.</returns>
        public CacheItem Add(ForeignResourceCache resourceCache)
        {
            var cacheItem = new CacheItem(GetKey(resourceCache), resourceCache);
            cache.Set(cacheItem, GetCacheItemPolicy());
            return cacheItem;
        }

        /// <summary>
        /// Returns the ForeignResourceCache object from the given foreign resource id and resource type id.
        /// </summary>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <param name="resourceTypeId">The resource type id.</param>
        /// <returns>The cached ForeignResourceCache instance, or null if none exists.</returns>
        public ForeignResourceCache GetForeignResourceCache(int foreignResourceId, int resourceTypeId)
        {
            var key = GetKey(foreignResourceId, resourceTypeId);
            var item = cache.Get(key);
            if (item != null)
            {
                return (ForeignResourceCache)item;
            }
            else
            {
                return null;
            }
        }
    }
}
