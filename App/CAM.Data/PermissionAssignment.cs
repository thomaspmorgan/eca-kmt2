namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// A Permission Assignment is a singular granting or revoking of a permission to a resource for a principal, for example,
    /// view project on project resource with id 10 to principal with principal id = 1.
    /// </summary>
    [Table("CAM.PermissionAssignment")]
    public partial class PermissionAssignment
    {
        /// <summary>
        /// Gets or sets the principal id.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the permission id.
        /// </summary>
        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the assigned on date.
        /// </summary>
        public DateTimeOffset AssignedOn { get; set; }

        /// <summary>
        /// Gets or sets the assigned by date.
        /// </summary>
        public int AssignedBy { get; set; }

        /// <summary>
        /// Gets or sets the is allowed flag.
        /// </summary>
        public bool IsAllowed { get; set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        public virtual Permission Permission { get; set; }

        /// <summary>
        /// Gets or sets the principal.
        /// </summary>
        public virtual Principal Principal { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        public virtual Resource Resource { get; set; }
    }
}
