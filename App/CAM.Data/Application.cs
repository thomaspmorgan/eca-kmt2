namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// An application is system that is controlled with cam.
    /// </summary>
    [Table("CAM.Application")]
    public partial class Application
    {
        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        [StringLength(50)]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the revised on date.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the revised by.
        /// </summary>
        public int RevisedBy { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        public virtual Resource Resource { get; set; }
    }
}
