using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Service
{
    /// <summary>
    /// An AddedPermissableEntityResult is a class containing the PermissableEntity that was added via a PermissableService
    /// and the roles that have been impacted by such.
    /// </summary>
    public class AddedPermissableEntityResult
    {
        /// <summary>
        /// Creates a new default instance and initializes the AffectedRolesById collection.
        /// </summary>
        /// <param name="permissable">The permissable entity.</param>
        /// <param name="affectedRolesById">The roles by id that have had permissions added to it.</param>
        public AddedPermissableEntityResult(IPermissable permissable, IEnumerable<int> affectedRolesById)
        {
            Contract.Requires(permissable != null, "The permissable entity must not be null.");
            this.PermissableEntity = permissable;
            this.AffectedRolesById = affectedRolesById ?? new List<int>();
        }

        /// <summary>
        /// Gets the permissable entity.
        /// </summary>
        public IPermissable PermissableEntity { get; private set; }

        /// <summary>
        /// Gets the roles by id that have had permissions added to it.
        /// </summary>
        public IEnumerable<int> AffectedRolesById { get; private set; }
    }
}
