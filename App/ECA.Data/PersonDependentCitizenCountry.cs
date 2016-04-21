using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class PersonDependentCitizenCountry
    {        
        /// <summary>
        /// Gets or sets the dependent id.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DependentId { get; set; }

        /// <summary>
        /// Gets or sets the Location Id.
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LocationId { get; set; }

        /// <summary>
        /// Gets or sets the is primary flag.
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets the citizenship country dependent
        /// </summary>
        [ForeignKey("DependentId")]
        public virtual PersonDependent Dependent { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

    }
}
