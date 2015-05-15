using CAM.Data;
using ECA.Core.Exceptions;
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
    /// A GrantedPermission is a business entity representing a permission that should be given to a principal i.e. Grantee, by another
    /// principal i.e. Grantor, to a resource.  The resource is determined by the foreign resource id and resource type.
    /// </summary>
    public class GrantedPermission
    {
        /// <summary>
        /// Creates a new GrantedPermission business entity.
        /// </summary>
        /// <param name="granteePrincipalId">The principal id of the Grantee, i.e. the user being given the permission.</param>
        /// <param name="permissionId">The permission id.</param>
        /// <param name="foreignResourceId">The id of the resource not in Cam, e.g. the Program Id or the Project Id..</param>
        /// <param name="resourceType">The resource type as a string.</param>
        /// <param name="grantorUserId">The principal id of the Grantor, i.e. the user giving the permission to another principal.</param>
        public GrantedPermission(int granteePrincipalId, int permissionId, int foreignResourceId, string resourceType, int grantorUserId)
        {
            if (ResourceType.GetStaticLookup(resourceType) == null)
            {
                throw new UnknownStaticLookupException(String.Format("The resource type [{0}] is not known.", resourceType));
            }
            this.GranteePrincipalId = granteePrincipalId;
            this.ForeignResourceId = foreignResourceId;
            this.ResourceTypeAsString = resourceType;
            this.PermissionId = permissionId;
            this.Audit = new Audit(grantorUserId);
            this.IsAllowed = true;
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
        /// Returns true, it is assumed a granted permission is allowed.
        /// </summary>
        public bool IsAllowed { get; protected set; }

        /// <summary>
        /// Returns the audit.
        /// </summary>
        public Audit Audit { get; private set; }

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

        /// <summary>
        /// Returns a nicely formatted string of the granted permission.
        /// </summary>
        /// <returns>A nicely formatted string of the granted permission.</returns>
        public override string ToString()
        {
            return String.Format("GranteePrincipalId:  [{0}], ForeignResourceId:  [{1}], ResourceTypeAsString:  [{2}], PermissionId:  [{3}], IsAllowed:  [{4}]",
                this.GranteePrincipalId,
                this.ForeignResourceId,
                this.ResourceTypeAsString,
                this.PermissionId,
                this.IsAllowed);
        }
    }
}
