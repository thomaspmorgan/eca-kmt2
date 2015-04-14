using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAM.Business.Service
{
    public class Permission : IPermission, IEquatable<Permission>
    {
        #region Properties

        public int PermissionId { get; set; }
        public int PrincipalId { get; set; }
        public int ResourceId { get; set; }
        public bool IsAllowed { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a permision object, default IsAllowed = true
        /// </summary>
        /// <param name="principalId">user or group Id</param>
        /// <param name="permissionId">Id of the Permission</param>
        /// <param name="resourceId">Id of the resource for the permission</param>
        public Permission(int principalId, int permissionId, int resourceId)
        {
            PrincipalId = principalId;
            PermissionId = permissionId;
            ResourceId = resourceId;
            IsAllowed = true;
        }

        public Permission()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines if the given user has permission for a resource
        /// </summary>
        /// <param name="principalId">Id of the user or group</param>
        /// <param name="permissionId">Id of the permission</param>
        /// <param name="resourceId">Id of the resource</param>
        /// <returns>True if the user or group has permission on the resource, false otherwise</returns>
        public bool HasPermission(int principalId, int permissionId, int resourceId)
        {
            return (PrincipalId == principalId && PermissionId == permissionId && ResourceId == resourceId &&
                    IsAllowed == true);
        }

        /// <summary>
        /// Determines equality of a permission.  The PrincipalId is ignored in the equality check
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool Equals(Permission p)
        {
            return (PermissionId == p.PermissionId &&
                    ResourceId == p.ResourceId &&
                    IsAllowed == p.IsAllowed);
        }

        public override bool Equals(object obj)
        {
            Permission p = obj as Permission;
            if (p != null)
            {
                return Equals(p);
            }
            return false;
        }

        public override int GetHashCode()
        {
            // see Josh Bloch's Effective Java for this hash algorithm
            int hash = 17;
            hash = hash * 23 + PermissionId.GetHashCode();
            hash = hash * 23 + ResourceId.GetHashCode();
            hash = hash * 23 + IsAllowed.GetHashCode();
            return hash;
        }


        #endregion
    }
}
