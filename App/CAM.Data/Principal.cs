namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// A Principal is an actor in the CAM system such as a user or a group.
    /// </summary>
    [Table("CAM.Principal")]
    public partial class Principal
    {
        /// <summary>
        /// Creates a new principal
        /// </summary>
        public Principal()
        {
            PermissionAssignments = new HashSet<PermissionAssignment>();
            PrincipalRoles = new HashSet<PrincipalRole>();
            SevisAccounts = new HashSet<SevisAccount>();
        }

        /// <summary>
        /// Gets or sets the principal id.
        /// </summary>
        public int PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the principal type id.
        /// </summary>
        public int? PrincipalTypeId { get; set; }

        /// <summary>
        /// Gets or sets the permission assignments.
        /// </summary>
        public virtual ICollection<PermissionAssignment> PermissionAssignments { get; set; }

        /// <summary>
        /// Gets or sets the principal type.
        /// </summary>
        public virtual PrincipalType PrincipalType { get; set; }

        /// <summary>
        /// Gets or sets the principal roles.
        /// </summary>
        public virtual ICollection<PrincipalRole> PrincipalRoles { get; set; }

        /// <summary>
        /// Gets or sets the user account.
        /// </summary>
        public virtual UserAccount UserAccount { get; set; }

        /// <summary>
        /// Gets or sets the sevis accounts.
        /// </summary>
        public virtual ICollection<SevisAccount> SevisAccounts { get; set; }
    }
}
