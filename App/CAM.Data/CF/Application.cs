namespace CAM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CAM.Application")]
    public partial class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ResourceId { get; set; }

        [StringLength(50)]
        public string ApplicationName { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset RevisedOn { get; set; }

        public int RevisedBy { get; set; }

        public virtual Resource Resource { get; set; }
    }
}
