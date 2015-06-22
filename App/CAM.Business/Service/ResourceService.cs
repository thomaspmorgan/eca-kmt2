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
using CAM.Business.Queries;
using CAM.Business.Model;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using CAM.Business.Queries.Models;
using ECA.Core.Exceptions;
using ECA.Core.Data;

namespace CAM.Business.Service
{
    /// <summary>
    /// A ResourceService is used to track resource's and their related types and foreign resource ids.  The 
    /// resources are cached in the given object cache.
    /// </summary>
    public class ResourceService : DbContextService<CamModel>, CAM.Business.Service.IResourceService, ECA.Core.Service.IPermissableService
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
        private readonly Action<string> throwIfResourceTypeIsNotKnown;

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
            throwIfResourceTypeIsNotKnown = (resourceType) =>
            {
                var lookup = ResourceType.GetStaticLookup(resourceType);
                if (lookup == null)
                {
                    throw new UnknownStaticLookupException(String.Format("The resource type [{0}] is not known.", resourceType));
                }
            };
        }

        #region Resource/Foreign ResourceId

        private IQueryable<Resource> CreateGetResourceByResourceIdQuery(int resourceId)
        {
            return this.Context.Resources.Where(x => x.ResourceId == resourceId)
                .Include(x => x.ResourceType);
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
                var resource = ResourceQueries.CreateGetResourceByForeignResourceIdQuery(this.Context, foreignResourceId, resourceTypeId).FirstOrDefault();
                int? parentResourceId = null;
                int? parentForeignResourceId = null;
                int? parentResourceTypeId = null;
                if (resource != null && resource.ParentResourceId.HasValue)
                {
                    var parentResource = CreateGetResourceByResourceIdQuery(resource.ParentResourceId.Value).FirstOrDefault();
                    Contract.Assert(parentResource != null, "The parent resource should not be null.");
                    parentResourceId = parentResource.ResourceId;
                    parentForeignResourceId = parentResource.ForeignResourceId;
                    parentResourceTypeId = parentResource.ResourceTypeId;
                }
                var resourceId = resource != null ? resource.ResourceId : default(int?);
                return HandleNonCachedResource(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
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
                var resource = await ResourceQueries.CreateGetResourceByForeignResourceIdQuery(this.Context, foreignResourceId, resourceTypeId).FirstOrDefaultAsync();
                int? parentResourceId = null;
                int? parentForeignResourceId = null;
                int? parentResourceTypeId = null;
                if (resource != null && resource.ParentResourceId.HasValue)
                {
                    var parentResource = await CreateGetResourceByResourceIdQuery(resource.ParentResourceId.Value).FirstOrDefaultAsync();
                    Contract.Assert(parentResource != null, "The parent resource should not be null.");
                    parentResourceId = parentResource.ResourceId;
                    parentForeignResourceId = parentResource.ForeignResourceId;
                    parentResourceTypeId = parentResource.ResourceTypeId;
                }
                var resourceId = resource != null ? resource.ResourceId : default(int?);
                return HandleNonCachedResource(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
            }
        }

        private int? HandleCachedResource(int foreignResourceId, int resourceTypeId)
        {
            var item = GetForeignResourceCache(foreignResourceId, resourceTypeId);
            Contract.Assert(item != null, "The item must not be null.");
            logger.Info("The foreign resource with id [{0}] of type [{1}] was cached with resource id [{2}].", foreignResourceId, resourceTypeId, item.ResourceId);
            return item.ResourceId;
        }

        private int? HandleNonCachedResource(int foreignResourceId, int? resourceId, int resourceTypeId, int? parentForeignResourceId, int? parentResourceId, int? parentResourceTypeId)
        {
            if (!resourceId.HasValue)
            {
                logger.Warn("ResourceId not found for foreignResourceId = '{0}', resourceTypeId='{1}'", foreignResourceId, resourceTypeId);
            }
            else
            {
                Add(foreignResourceId, resourceId.Value, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
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
        #endregion

        #region Caching

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
        public CacheItem Add(int foreignResourceId, int resourceId, int resourceTypeId, int? parentForeignResourceId, int? parentResourceId, int? parentResourceTypeId)
        {
            return Add(new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId));
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
        #endregion

        #region Resource Authorizations

        /// <summary>
        /// Returns a info object with basic authorization details for a resource by type and foreign resource id.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="foreignResourceId">The resource by foreign resource id.</param>
        /// <returns>The resource authorization info dto.</returns>
        public ResourceAuthorizationInfoDTO GetResourceAuthorizationInfoDTO(string resourceType, int foreignResourceId)
        {
            var dto = ResourceQueries.CreateGetResourceAuthorizationInfoDTOQuery(this.Context, resourceType, foreignResourceId).FirstOrDefault();
            logger.Trace("Retrieved resource authorization info dto for resource type [{0}] and foreign resource id [{1}].", resourceType, foreignResourceId);
            return dto;
        }

        /// <summary>
        /// Returns a info object with basic authorization details for a resource by type and foreign resource id.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="foreignResourceId">The resource by foreign resource id.</param>
        /// <returns>The resource authorization info dto.</returns>
        public async Task<ResourceAuthorizationInfoDTO> GetResourceAuthorizationInfoDTOAsync(string resourceType, int foreignResourceId)
        {
            var dto = await ResourceQueries.CreateGetResourceAuthorizationInfoDTOQuery(this.Context, resourceType, foreignResourceId).FirstOrDefaultAsync();
            logger.Trace("Retrieved resource authorization info dto for resource type [{0}] and foreign resource id [{1}].", resourceType, foreignResourceId);
            return dto;
        }

        /// <summary>
        /// Returns resource authorizations given the query operator.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged filtered and sorted resource authorizations.</returns>
        public PagedQueryResults<ResourceAuthorization> GetResourceAuthorizations(QueryableOperator<ResourceAuthorization> queryOperator)
        {
            var results = ResourceQueries.CreateGetResourceAuthorizationsQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Retrieved resource authorizations using query operator [{0}].", queryOperator);
            return results;
        }

        /// <summary>
        /// Returns resource authorizations given the query operator.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged filtered and sorted resource authorizations.</returns>
        public async Task<PagedQueryResults<ResourceAuthorization>> GetResourceAuthorizationsAsync(QueryableOperator<ResourceAuthorization> queryOperator)
        {
            var results = await ResourceQueries.CreateGetResourceAuthorizationsQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Retrieved resource authorizations using query operator [{0}].", queryOperator);
            return results;
        }
        #endregion

        /// <summary>
        /// Returns the permissions that can be set on a resource of the given type and resource id.  If only the 
        /// permissions for the resource type are needed, null can be passed for resource id.  In this case, permission
        /// that have the same resource type but do have a resource id relationship will not be included.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <returns>The available permissions for the given resource type and resource id.</returns>
        public List<ResourcePermissionDTO> GetResourcePermissions(string resourceType, int? foreignResourceId)
        {
            throwIfResourceTypeIsNotKnown(resourceType);
            int? resourceId = null;
            if (foreignResourceId.HasValue)
            {
                resourceId = GetResourceIdByForeignResourceId(foreignResourceId.Value, ResourceType.GetStaticLookup(resourceType).Id);
                logger.Trace("Retrieved resourceId [{0}] for foreign resource id [{1}].", resourceId, foreignResourceId);
            }
            var permissions = ResourceQueries.CreateGetResourcePermissionsQuery(this.Context, resourceType, resourceId).ToList();
            logger.Trace("Retrieved resource permissions for resource type [{0}] and foreign resource id [{1}].", resourceType, foreignResourceId);
            return permissions;
        }

        /// <summary>
        /// Returns the permissions that can be set on a resource of the given type and resource id.  If only the 
        /// permissions for the resource type are needed, null can be passed for resource id.  In this case, permission
        /// that have the same resource type but do have a resource id relationship will not be included.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <returns>The available permissions for the given resource type and resource id.</returns>
        public async Task<List<ResourcePermissionDTO>> GetResourcePermissionsAsync(string resourceType, int? foreignResourceId)
        {
            throwIfResourceTypeIsNotKnown(resourceType);
            int? resourceId = null;
            if (foreignResourceId.HasValue)
            {
                resourceId = GetResourceIdByForeignResourceId(foreignResourceId.Value, ResourceType.GetStaticLookup(resourceType).Id);
                logger.Trace("Retrieved resourceId [{0}] for foreign resource id [{1}].", resourceId, foreignResourceId);
            }
            var permissions = await ResourceQueries.CreateGetResourcePermissionsQuery(this.Context, resourceType, resourceId).ToListAsync();
            logger.Trace("Retrieved resource permissions for resource type [{0}] and resource id [{1}].", resourceType, resourceId);
            return permissions;
        }

        private IQueryable<ResourceTypeDTO> CreateGetResourceTypesQuery()
        {
            return this.Context.ResourceTypes.OrderBy(x => x.ResourceTypeName).Select(x => new ResourceTypeDTO
            {
                Id = x.ResourceTypeId,
                Name = x.ResourceTypeName
            });
        }

        /// <summary>
        /// Returns the resource types in CAM.
        /// </summary>
        /// <returns>The resource types.</returns>
        public List<ResourceTypeDTO> GetResourceTypes()
        {
            var resourceTypes = CreateGetResourceTypesQuery().ToList();
            logger.Trace("Successfully retrieved resource types.");
            return resourceTypes;
        }

        /// <summary>
        /// Returns the resource types in CAM.
        /// </summary>
        /// <returns>The resource types.</returns>
        public async Task<List<ResourceTypeDTO>> GetResourceTypesAsync()
        {
            var resourceTypes = await CreateGetResourceTypesQuery().ToListAsync();
            logger.Trace("Successfully retrieved resource types.");
            return resourceTypes;
        }

        #region IPermissableService


        public void OnAdded(IList<IPermissable> addedEntities)
        {
            if (addedEntities.Count > 3)
            {
                logger.Warn("There are more than the recommended number of entities [{0}] being added to CAM performance issues may be arise.", addedEntities.Count);
            }
            foreach (var addedEntity in addedEntities.ToList())
            {
                OnAdded(addedEntity);
            }
        }

        public async Task OnAddedAsync(IList<IPermissable> addedEntities)
        {
            if (addedEntities.Count > 3)
            {
                logger.Warn("There are more than the recommended number of entities [{0}] being added to CAM performance issues may be arise.", addedEntities.Count);
            }
            foreach (var addedEntity in addedEntities.ToList())
            {
                await OnAddedAsync(addedEntity);
            }
        }

        public void OnAdded(IPermissable addedEntity)
        {
            var existingResource = ResourceQueries.CreateGetResourceByForeignResourceIdQuery(this.Context, addedEntity.GetId(), addedEntity.GetPermissableType().GetResourceTypeId())
                .FirstOrDefault();
            Resource parentResource = null;
            if (addedEntity.GetParentId().HasValue)
            {
                parentResource = ResourceQueries.CreateGetResourceByForeignResourceIdQuery(
                    this.Context,
                    addedEntity.GetParentId().Value,
                    addedEntity.GetParentPermissableType().GetResourceTypeId())
                .FirstOrDefault();
            }
            DoOnAdded(addedEntity, existingResource, parentResource);
        }

        public async Task OnAddedAsync(IPermissable addedEntity)
        {
            var existingResource = await ResourceQueries.CreateGetResourceByForeignResourceIdQuery(this.Context, addedEntity.GetId(), addedEntity.GetPermissableType().GetResourceTypeId())
                .FirstOrDefaultAsync();
            Resource parentResource = null;
            if (addedEntity.GetParentId().HasValue)
            {
                parentResource = await ResourceQueries.CreateGetResourceByForeignResourceIdQuery(
                    this.Context,
                    addedEntity.GetParentId().Value,
                    addedEntity.GetParentPermissableType().GetResourceTypeId())
                .FirstOrDefaultAsync();
            }
            DoOnAdded(addedEntity, existingResource, parentResource);
        }

        private void DoOnAdded(IPermissable addedEntity, Resource existingResource, Resource parentResource)
        {
            if (existingResource == null)
            {
                var newResource = AddResourceToCAM(addedEntity, parentResource);                                
                this.Context.SaveChanges();
            } 
            RemoveFromCache(addedEntity);
        }

        private Resource AddResourceToCAM(IPermissable permissable, Resource parentResource)
        {
            var resource = new Resource
            {
                ForeignResourceId = permissable.GetId(),
                ResourceTypeId = permissable.GetPermissableType().GetResourceTypeId()
            };
            if (parentResource != null)
            {
                resource.ParentResourceId = parentResource.ResourceId;
                resource.ParentResource = parentResource;
            }
            else if (permissable.GetParentId().HasValue && parentResource == null)
            {
                var newParentResource = new Resource
                {
                    ForeignResourceId = permissable.GetParentId().Value,
                    ResourceTypeId = permissable.GetParentPermissableType().GetResourceTypeId()
                };
                resource.ParentResource = newParentResource;
                newParentResource.ChildResources.Add(resource);
                Context.Resources.Add(newParentResource);
            }
            Context.Resources.Add(resource);
            return resource;
        }

        public void OnUpdated(IList<IPermissable> updatedEntities)
        {
            foreach (var updatedEntity in updatedEntities.ToList())
            {
                OnUpdated(updatedEntity);
            }
        }

        public async Task OnUpdatedAsync(IList<IPermissable> updatedEntities)
        {
            foreach (var updatedEntity in updatedEntities.ToList())
            {
                await OnUpdatedAsync(updatedEntity);
            }
        }

        public async Task OnUpdatedAsync(IPermissable updatedEntity)
        {
            var resource = await ResourceQueries.CreateGetResourceByForeignResourceIdQuery(this.Context, updatedEntity.GetId(), updatedEntity.GetPermissableType().GetResourceTypeId()).FirstOrDefaultAsync();
            Resource parentResource = null;
            if (updatedEntity.GetParentId().HasValue)
            {
                parentResource = await ResourceQueries.CreateGetResourceByForeignResourceIdQuery(this.Context, updatedEntity.GetParentId().Value, updatedEntity.GetParentPermissableType().GetResourceTypeId()).FirstOrDefaultAsync();
            }

            DoUpdate(updatedEntity, resource, parentResource);
        }

        public void OnUpdated(IPermissable updatedEntity)
        {
            var resource = ResourceQueries.CreateGetResourceByForeignResourceIdQuery(this.Context, updatedEntity.GetId(), updatedEntity.GetPermissableType().GetResourceTypeId()).FirstOrDefault();
            Resource parentResource = null;
            if (updatedEntity.GetParentId().HasValue)
            {
                parentResource = ResourceQueries.CreateGetResourceByForeignResourceIdQuery(this.Context, updatedEntity.GetParentId().Value, updatedEntity.GetParentPermissableType().GetResourceTypeId()).FirstOrDefault();
            }
            
            DoUpdate(updatedEntity, resource, parentResource);
        }

        private bool DoUpdate(IPermissable updatedEntity, Resource resourceToUpdate, Resource parentResource)
        {
            if (resourceToUpdate == null)
            {
                throw new ModelNotFoundException(String.Format("The resource with foreign id [{0}] and resource type id [{1}] was not found.", 
                    updatedEntity.GetId(), 
                    updatedEntity.GetPermissableType().GetResourceTypeId()));
            }
            var saveChangesRequired = false;
            if (resourceToUpdate.ResourceId != parentResource.ResourceId)
            {
                saveChangesRequired = true;
                resourceToUpdate.ParentResource = parentResource;
                resourceToUpdate.ParentResourceId = parentResource.ResourceId;
                RemoveFromCache(resourceToUpdate);
                RemoveFromCache(parentResource);
                RemoveFromCache(updatedEntity);
            }
            if (updatedEntity.GetPermissableType().GetResourceTypeId() != resourceToUpdate.ResourceTypeId)
            {
                saveChangesRequired = true;
                resourceToUpdate.ResourceTypeId = updatedEntity.GetPermissableType().GetResourceTypeId();
                RemoveFromCache(updatedEntity);
            }
            
            return saveChangesRequired;
        }

        public void RemoveFromCache(Resource resource)
        {
            if (resource != null)
            {
                cache.Remove(GetKey(resource.ForeignResourceId, resource.ResourceTypeId));
            }
        }

        public void RemoveFromCache(IPermissable permissable)
        {
            if (permissable != null)
            {
                cache.Remove(GetKey(permissable.GetId(), permissable.GetPermissableType().GetResourceTypeId()));
                if (permissable.GetParentId().HasValue)
                {
                    cache.Remove(GetKey(permissable.GetParentId().Value, permissable.GetParentPermissableType().GetResourceTypeId()));
                }
            }
        }

        #endregion
    }
}
