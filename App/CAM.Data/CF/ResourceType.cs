namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CAM.ResourceType")]
    public partial class ResourceType
    {
        public ResourceType()
        {
            Resources = new HashSet<Resource>();
            ChildResourceTypes = new HashSet<ResourceType>();
            Roles = new HashSet<Role>();
        }

        public int ResourceTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string ResourceTypeName { get; set; }

        [StringLength(255)]
        public string ResourceTypeDescription { get; set; }

        public int? ParentResourceTypeId { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset RevisedOn { get; set; }

        public int RevisedBy { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Resource> Resources { get; set; }

        public virtual ICollection<ResourceType> ChildResourceTypes { get; set; }

        public virtual ResourceType ParentResourceType { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
