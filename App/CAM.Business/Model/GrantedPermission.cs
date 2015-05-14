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
    public class GrantedPermission
    {
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

        public int GranteePrincipalId { get; private set; }

        public int ForeignResourceId { get; private set; }

        public string ResourceTypeAsString { get; private set; }

        public int PermissionId { get; private set; }

        public bool IsAllowed { get; protected set; }

        public Audit Audit { get; private set; }

        public StaticLookup GetResourceType()
        {
            var resourceType = ResourceType.GetStaticLookup(this.ResourceTypeAsString);
            Contract.Assert(resourceType != null, "Only valid static lookups should be found here.");
            return resourceType;
        }
    }
}
