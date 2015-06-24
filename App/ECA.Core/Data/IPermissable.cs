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
    public interface IPermissable
    {
        /// <summary>
        /// Returns the id of the IPermissable object.
        /// </summary>
        /// <returns>The id.</returns>
        int GetId();

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
    }
}
