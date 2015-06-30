namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// A ResourceType is a descriptor of a resource such as project, program, or organization.
    /// </summary>
    [Table("CAM.ResourceType")]
    public partial class ResourceType
    {
        /// <summary>
        /// Creates a new resource type.
        /// </summary>
        public ResourceType()
        {
            Resources = new HashSet<Resource>();
            ChildResourceTypes = new HashSet<ResourceType>();
            Roles = new HashSet<Role>();
        }

        /// <summary>
        /// Gets or sets the resource type id.
        /// </summary>
        public int ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the resource type name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ResourceTypeName { get; set; }

        /// <summary>
        /// Gets or sets the resource type description.
        /// </summary>
        [StringLength(255)]
        public string ResourceTypeDescription { get; set; }

        /// <summary>
        /// Gets or sets the parent resource type id.
        /// </summary>
        public int? ParentResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the created on date.
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
        /// Gets or sets the resources.
        /// </summary>
        public virtual ICollection<Resource> Resources { get; set; }

        /// <summary>
        /// Gets or sets the child resource types.
        /// </summary>
        public virtual ICollection<ResourceType> ChildResourceTypes { get; set; }

        /// <summary>
        /// Gets or sets the parent resource type.
        /// </summary>
        public virtual ResourceType ParentResourceType { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }
    }
}
