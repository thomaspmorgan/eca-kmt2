using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Service
{
    /// <summary>
    /// A ForeignResourceCache is an object that is cached in memory when relating a foreign resource by id
    /// to a resource by id.
    /// </summary>
    public class ForeignResourceCache
    {
        /// <summary>
        /// Creates a new ForeignResourceCache with the given resource ids and type id.
        /// </summary>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="resourceTypeId">The resource type id.</param>
        /// <param name="parentForeignResourceId">The foreign id of the parent's resource.</param>
        /// <param name="parentResourceId">The id of the parent resource.</param>
        /// <param name="parentResourceTypeId">The resource type id of the parent resource.</param>
        public ForeignResourceCache(int foreignResourceId, int resourceId, int resourceTypeId, int? parentForeignResourceId, int? parentResourceId, int? parentResourceTypeId)
        {
            this.ForeignResourceId = foreignResourceId;
            this.ResourceTypeId = resourceTypeId;
            this.ResourceId = resourceId;
            this.ParentForeignResourceId = parentForeignResourceId;
            this.ParentResourceId = parentResourceId;
            this.ParentResourceTypeId = parentResourceTypeId;
        }

        /// <summary>
        /// Gets the foreign resource id.
        /// </summary>
        public int ForeignResourceId { get; private set; }

        /// <summary>
        /// Gets the parent foreign resource id.
        /// </summary>
        public int? ParentForeignResourceId { get; private set; }

        /// <summary>
        /// Gets or sets the parent resource id.
        /// </summary>
        public int? ParentResourceId { get; private set; }

        /// <summary>
        /// Gets the parent resource type id.
        /// </summary>
        public int? ParentResourceTypeId { get; private set; }

        /// <summary>
        /// Gets the resource type id.
        /// </summary>
        public int ResourceTypeId { get; private set; }

        /// <summary>
        /// Gets the resource id.
        /// </summary>
        public int ResourceId { get; private set; }
    }
}
