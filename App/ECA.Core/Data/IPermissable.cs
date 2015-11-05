using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Data
{
    /// <summary>
    /// PermissableTypes are objects that must be protected in some form.
    /// </summary>
    public enum PermissableType
    {
        /// <summary>
        /// The Application permissable type.
        /// </summary>
        Application,

        /// <summary>
        /// The Project permissable type.
        /// </summary>
        Project,

        /// <summary>
        /// The Program permissable type.
        /// </summary>
        Program,

        /// <summary>
        /// The Office permissable type.
        /// </summary>
        Office
    }

    /// <summary>
    /// An IPermissable object is an object that must be protected via permissions.  
    /// </summary>
    public interface IPermissable : IIdentifiable
    {
        /// <summary>
        /// Returns the permissable type of this permissable object.
        /// </summary>
        /// <returns>The permissable type.</returns>
        PermissableType GetPermissableType();

        /// <summary>
        /// Returns the parent id of this permissable object or null if it does not have a parent.
        /// </summary>
        /// <returns>The parent id of this permissable object or null if it does not have a parent.</returns>
        int? GetParentId();

        /// <summary>
        /// The parent permissable type.
        /// </summary>
        /// <returns>The parent permissable type.</returns>
        PermissableType GetParentPermissableType();

        /// <summary>
        /// Returns true, if this permissable instance is exempt from protection by permissions.
        /// </summary>
        /// <returns>True, if this permissable instance is exempt from protection by permissions, otherwise, false.</returns>
        bool IsExempt();

        /// <summary>
        /// Returns true, if the permission should be assigned to the role for this resource when the resource is created.
        /// </summary>
        /// <param name="roleName">The name of the role to have a permission assigned to.</param>
        /// <param name="permissionName">The name of the permission that should be granted.</param>
        /// <returns>True, if the role should have the permission granted, otherwise, false.</returns>
        bool AssignPermissionToRoleOnCreate(string roleName, string permissionName);
    }
}
