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
    public class SimplePermission : IPermission, IEquatable<SimplePermission>
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

        /// <summary>
        /// Determines equality of this given permission to this permission.
        /// </summary>
        /// <param name="p">The permisison to check.</param>
        /// <returns>True if the given permission equals this permission, otherwise, false.</returns>
        public bool Equals(SimplePermission p)
        {
            if (p == null)
            {
                return false;
            }
            return (PermissionId == p.PermissionId &&
                    ResourceId == p.ResourceId &&
                    PrincipalId == p.PrincipalId &&
                    IsAllowed == p.IsAllowed);
        }

        /// <summary>
        /// Determines the equality of this permission to the given object.
        /// </summary>
        /// <param name="obj">The objec to test.</param>
        /// <returns>True if this permission equals the given permission, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            SimplePermission p = obj as SimplePermission;
            if (p != null)
            {
                return Equals(p);
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code of this permission.
        /// </summary>
        /// <returns>A has code of this permission.</returns>
        public override int GetHashCode()
        {
            // see Josh Bloch's Effective Java for this hash algorithm
            int hash = 17;
            hash = hash * 23 + PermissionId.GetHashCode();
            hash = hash * 23 + ResourceId.GetHashCode();
            hash = hash * 23 + IsAllowed.GetHashCode();
            hash = hash * 23 + PrincipalId.GetHashCode();
            return hash;
        }
    }
}
