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
        public ForeignResourceCache(int foreignResourceId, int resourceId, int resourceTypeId)
        {
            this.ForeignResourceId = foreignResourceId;
            this.ResourceTypeId = resourceTypeId;
            this.ResourceId = resourceId;
        }

        /// <summary>
        /// Gets the foreign resource id.
        /// </summary>
        public int ForeignResourceId { get; private set; }

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
