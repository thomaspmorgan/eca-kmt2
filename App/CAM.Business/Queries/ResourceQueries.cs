using CAM.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Queries
{
    /// <summary>
    /// The ResourceQueries class contains queries for CAM resources in a CamModel.
    /// </summary>
    public static class ResourceQueries
    {
        /// <summary>
        /// Returns a query to retrieve a resource by foreign resource id and resource type id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <param name="resourceTypeId">The resource type id.</param>
        /// <returns>The query to retrieve the resource.</returns>
        public static IQueryable<Resource> CreateGetResourceByForeignResourceIdQuery(CamModel context, int foreignResourceId, int resourceTypeId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from resource in context.Resources
                        where
                            resource.ResourceTypeId == resourceTypeId &&
                            resource.ForeignResourceId == foreignResourceId
                        select resource;
            return query;
        }
    }
}
