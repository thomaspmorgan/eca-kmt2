namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// A PrincipalType details the type of the cam principal such as user, or group.
    /// </summary>
    [Table("CAM.PrincipalType")]
    public partial class PrincipalType
    {
        /// <summary>
        /// Creates a new principal type.
        /// </summary>
        public PrincipalType()
        {
            Principals = new HashSet<Principal>();
        }

        /// <summary>
        /// Gets or sets the principal type id.
        /// </summary>
        public int PrincipalTypeId { get; set; }

        /// <summary>
        /// Gets or sets the principal type name.
        /// </summary>
        [Required]
        [StringLength(10)]
        public string PrincipalTypeName { get; set; }

        /// <summary>
        /// Gets or sets the principal type description.
        /// </summary>
        [StringLength(255)]
        public string PrincipalTypeDescription { get; set; }

        /// <summary>
        /// Gets or sets the created by user id.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created on date.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the revised by user id.
        /// </summary>
        public int RevisedBy { get; set; }

        /// <summary>
        /// Gets or sets the revised on date.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the is active flag.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the principals.
        /// </summary>
        public virtual ICollection<Principal> Principals { get; set; }
    }
}
