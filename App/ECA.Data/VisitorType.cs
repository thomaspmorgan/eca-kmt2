using System.ComponentModel.DataAnnotations;

namespace ECA.Data
{
    /// <summary>
    /// Storage for Visitor Type
    /// </summary>
    public class VisitorType
    {
        /// <summary>
        /// Key Id Field for VisitorType
        /// </summary>
        [Key]
        public int VisitorTypeId { get; set; }

        /// <summary>
        /// The Visitor Type name
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string VisitorTypeName { get; set; }

        /// <summary>
        /// Audit fields for VisitorType
        /// </summary>
        public History History { get; set; }
    }
}