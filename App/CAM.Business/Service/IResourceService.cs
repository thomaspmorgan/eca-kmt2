using System;
using System.Diagnostics.Contracts;
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
    }
}
