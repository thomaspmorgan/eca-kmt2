namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// A RoleResourcePermission is a way to assign a role to a permission and a resource directly.
    /// </summary>
    [Table("CAM.RoleResourcePermission")]
    public partial class RoleResourcePermission
    {
        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleId { get; set; }

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
        /// Gets or set the assigned by user id.
        /// </summary>
        public int AssignedBy { get; set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        public virtual Permission Permission { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        public virtual Resource Resource { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public virtual Role Role { get; set; }
    }
}
