namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// A PrincipalRole is a role that has been assigned to a principal
    /// </summary>
    [Table("CAM.PrincipalRole")]
    public partial class PrincipalRole
    {
        /// <summary>
        /// Gets or sets the principal id.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the assigned by user id.
        /// </summary>
        public int AssignedBy { get; set; }

        /// <summary>
        /// Gets or sets the assigned on date.
        /// </summary>
        public DateTimeOffset? AssignedOn { get; set; }

        /// <summary>
        /// Gets or sets the principal.
        /// </summary>
        public virtual Principal Principal { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public virtual Role Role { get; set; }
    }
}
