using CAM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAM.Business.Service
{
    /// <summary>
    /// A SimplePermission contains enough detail for an principal's permission.
    /// </summary>
    public class SimplePermission : IPermission
    {
        /// <summary>
        /// Gets or sets the principal id.
        /// </summary>
        public int PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the permission id.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// Gets or sets is allowed.
        /// </summary>
        public bool IsAllowed { get; set; }

        /// <summary>
        /// Gets or sets the foreign resource id.
        /// </summary>
        public int ForeignResourceId { get; set; }

        /// <summary>
        /// Gets or sets the resource type id.
        /// </summary>
        public int ResourceTypeId { get; set; }

        /// <summary>
        /// Returns a formatted string of this permission.
        /// </summary>
        /// <returns>A formatted string of this permission.</returns>
        public override string ToString()
        {
            var resourceType = ResourceType.GetStaticLookup(this.ResourceTypeId);
            var permission = Permission.GetStaticLookup(this.PermissionId);
            return String.Format("PrincipalId:  [{0}], ResourceId:  [{1}], PermissionId:  [{2}], IsAllowed:  [{3}], ForeignResourceId:  [{4}], ResourceTypeId:  [{5}], ResourceType:  [{6}], Permission:  [{7}]", 
                PrincipalId, 
                ResourceId, 
                PermissionId, 
                IsAllowed,
                ForeignResourceId,
                ResourceTypeId,
                resourceType == null ? "Unknown" : resourceType.Value,
                permission == null ? "Unknown" : permission.Value);
        }
    }
}
