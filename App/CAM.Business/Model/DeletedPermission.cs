using CAM.Data;
using ECA.Core.Generation;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Model
{
    /// <summary>
    /// A DeletedPermission is a permission that should be removed from the PermissionAssignments.
    /// </summary>
    public class DeletedPermission
    {
        /// <summary>
        /// Creates a new DeletedPermission instance.
        /// </summary>
        /// <param name="granteePrincipalId">The principal id of the user whose permission should be removed.</param>
        /// <param name="foreignResourceId">The foreign key id of the resource.</param>
        /// <param name="permissionId">The permission id.</param>
        public DeletedPermission(int granteePrincipalId, int foreignResourceId, int permissionId, string resourceTypeAsString)
        {
            this.GranteePrincipalId = granteePrincipalId;
            this.ForeignResourceId = foreignResourceId;
            this.PermissionId = permissionId;
            this.ResourceTypeAsString = resourceTypeAsString;
        }

        /// <summary>
        /// Gets the grantee principal id.
        /// </summary>
        public int GranteePrincipalId { get; private set; }

        /// <summary>
        /// Gets the foreign resource id.
        /// </summary>
        public int ForeignResourceId { get; private set; }

        /// <summary>
        /// Gets the resource type as a string.
        /// </summary>
        public string ResourceTypeAsString { get; private set; }

        /// <summary>
        /// Gets the permission id.
        /// </summary>
        public int PermissionId { get; private set; }

        /// <summary>
        /// Returns the lookup associated to the resource type.
        /// </summary>
        /// <returns>The resource type static lookup.</returns>
        public StaticLookup GetResourceType()
        {
            var resourceType = ResourceType.GetStaticLookup(this.ResourceTypeAsString);
            Contract.Assert(resourceType != null, "Only valid static lookups should be found here.");
            return resourceType;
        }
    }
}
