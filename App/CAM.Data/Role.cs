namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// A Role is a collection of permissions and permission assignments.
    /// </summary>
    [Table("CAM.Role")]
    public partial class Role
    {
        /// <summary>
        /// Creates a new role.
        /// </summary>
        public Role()
        {
            PrincipalRoles = new HashSet<PrincipalRole>();
            RoleResourcePermissions = new HashSet<RoleResourcePermission>();
        }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the role name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the role description.
        /// </summary>
        [StringLength(255)]
        public string RoleDescription { get; set; }

        /// <summary>
        /// Gets or sets the crated on date.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the created by user id.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the revised on date.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the revised by user id.
        /// </summary>
        public int RevisedBy { get; set; }

        /// <summary>
        /// Gets or sets the is active flag.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        public int? ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the resource type id.
        /// </summary>
        public int? ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the principal roles.
        /// </summary>
        public virtual ICollection<PrincipalRole> PrincipalRoles { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        public virtual Resource Resource { get; set; }

        /// <summary>
        /// Gets or sets the resource type.
        /// </summary>
        public virtual ResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the role resource permissions.
        /// </summary>
        public virtual ICollection<RoleResourcePermission> RoleResourcePermissions { get; set; }
    }
}
