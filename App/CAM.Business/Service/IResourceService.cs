using CAM.Business.Model;
using CAM.Business.Queries.Models;
using ECA.Core.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace CAM.Business.Service
{
    /// <summary>
    /// An IResourceService is capable of performing lookups for resources.
    /// </summary>
    [ContractClass(typeof(IResourceService))]
    public interface IResourceService
    {
        /// <summary>
        /// Returns the resourceid for the resource with the given foreign resource id and resource type id.
        /// </summary>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <param name="resourceTypeId">The resource type id.</param>
        /// <returns>The resource id.</returns>
        int? GetResourceIdByForeignResourceId(int foreignResourceId, int resourceTypeId);

        /// <summary>
        /// Returns the resourceid for the resource with the given foreign resource id and resource type id.
        /// </summary>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <param name="resourceTypeId">The resource type id.</param>
        /// <returns>The resource id.</returns>
        System.Threading.Tasks.Task<int?> GetResourceIdByForeignResourceIdAsync(int foreignResourceId, int resourceTypeId);

        /// <summary>
        /// Returns the resourceid for the resource with the given application id.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns>The resource id.</returns>
        int? GetResourceIdForApplicationId(int applicationId);

        /// <summary>
        /// Returns the resourceid for the resource with the given application id.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns>The resource id.</returns>
        System.Threading.Tasks.Task<int?> GetResourceIdForApplicationIdAsync(int applicationId);

        /// <summary>
        /// Returns the resource type id for the resource type with the given name.
        /// </summary>
        /// <param name="resourceTypeName">The resource type name.</param>
        /// <returns>The resource type id.</returns>
        int? GetResourceTypeId(string resourceTypeName);

        /// <summary>
        /// Returns resource authorizations given the query operator.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged filtered and sorted resource authorizations.</returns>
        ECA.Core.Query.PagedQueryResults<ResourceAuthorization> GetResourceAuthorizations(QueryableOperator<ResourceAuthorization> queryOperator);

        /// <summary>
        /// Returns resource authorizations given the query operator.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged filtered and sorted resource authorizations.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ResourceAuthorization>> GetResourceAuthorizationsAsync(QueryableOperator<ResourceAuthorization> queryOperator);

        /// <summary>
        /// Returns the permissions that can be set on a resource of the given type and resource id.  If only the 
        /// permissions for the resource type are needed, null can be passed for resource id.  In this case, permission
        /// that have the same resource type but do have a resource id relationship will not be included.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <returns>The available permissions for the given resource type and resource id.</returns>
        System.Collections.Generic.List<ResourcePermissionDTO> GetResourcePermissions(string resourceType, int? foreignResourceId);

        /// <summary>
        /// Returns the permissions that can be set on a resource of the given type and resource id.  If only the 
        /// permissions for the resource type are needed, null can be passed for resource id.  In this case, permission
        /// that have the same resource type but do have a resource id relationship will not be included.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <returns>The available permissions for the given resource type and resource id.</returns>
        Task<System.Collections.Generic.List<ResourcePermissionDTO>> GetResourcePermissionsAsync(string resourceType, int? foreignResourceId);

        /// <summary>
        /// Returns the resource types in CAM.
        /// </summary>
        /// <returns>The resource types.</returns>
        List<ResourceTypeDTO> GetResourceTypes();

        /// <summary>
        /// Returns the resource types in CAM.
        /// </summary>
        /// <returns>The resource types.</returns>
        Task<List<ResourceTypeDTO>> GetResourceTypesAsync();

        /// <summary>
        /// Returns a info object with basic authorization details for a resource by type and foreign resource id.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="foreignResourceId">The resource by foreign resource id.</param>
        /// <returns>The resource authorization info dto.</returns>
        ResourceAuthorizationInfoDTO GetResourceAuthorizationInfoDTO(string resourceType, int foreignResourceId);

        /// <summary>
        /// Returns a info object with basic authorization details for a resource by type and foreign resource id.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="foreignResourceId">The resource by foreign resource id.</param>
        /// <returns>The resource authorization info dto.</returns>
        Task<ResourceAuthorizationInfoDTO> GetResourceAuthorizationInfoDTOAsync(string resourceType, int foreignResourceId);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IResourceService))]
    public abstract class ResourceServiceContract : IResourceService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="foreignResourceId"></param>
        /// <param name="resourceTypeId"></param>
        /// <returns></returns>
        public int? GetResourceIdByForeignResourceId(int foreignResourceId, int resourceTypeId)
        {
            return default(int?);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="foreignResourceId"></param>
        /// <param name="resourceTypeId"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task<int?> GetResourceIdByForeignResourceIdAsync(int foreignResourceId, int resourceTypeId)
        {
            return System.Threading.Tasks.Task.FromResult<int?>(default(int?));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public int? GetResourceIdForApplicationId(int applicationId)
        {
            return default(int?);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task<int?> GetResourceIdForApplicationIdAsync(int applicationId)
        {
            return System.Threading.Tasks.Task.FromResult<int?>(default(int?));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceTypeName"></param>
        /// <returns></returns>
        public int? GetResourceTypeId(string resourceTypeName)
        {
            return default(int?);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public ECA.Core.Query.PagedQueryResults<ResourceAuthorization> GetResourceAuthorizations(QueryableOperator<ResourceAuthorization> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ResourceAuthorization>> GetResourceAuthorizationsAsync(QueryableOperator<ResourceAuthorization> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<ECA.Core.Query.PagedQueryResults<ResourceAuthorization>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="foreignResourceId"></param>
        /// <returns></returns>
        public System.Collections.Generic.List<ResourcePermissionDTO> GetResourcePermissions(string resourceType, int? foreignResourceId)
        {
            return new System.Collections.Generic.List<ResourcePermissionDTO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="foreignResourceId"></param>
        /// <returns></returns>
        public Task<System.Collections.Generic.List<ResourcePermissionDTO>> GetResourcePermissionsAsync(string resourceType, int? foreignResourceId)
        {
            return Task.FromResult<System.Collections.Generic.List<ResourcePermissionDTO>>(new System.Collections.Generic.List<ResourcePermissionDTO>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ResourceTypeDTO> GetResourceTypes()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<List<ResourceTypeDTO>> GetResourceTypesAsync()
        {
            return Task.FromResult<List<ResourceTypeDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="foreignResourceId"></param>
        /// <returns></returns>
        public ResourceAuthorizationInfoDTO GetResourceAuthorizationInfoDTO(string resourceType, int foreignResourceId)
        {
            Contract.Requires(resourceType != null, "The resource type must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="foreignResourceId"></param>
        /// <returns></returns>
        public Task<ResourceAuthorizationInfoDTO> GetResourceAuthorizationInfoDTOAsync(string resourceType, int foreignResourceId)
        {
            Contract.Requires(resourceType != null, "The resource type must not be null.");
            return Task.FromResult<ResourceAuthorizationInfoDTO>(null);
        }
    }
}
