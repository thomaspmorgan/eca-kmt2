using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// Data point configuration model
    /// </summary>
    public class DataPointConfiguration
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        [Key]
        public int DataPointConfigurationId { get; set; }
        /// <summary>
        /// Gets or sets the officeId
        /// </summary>
        public int? OfficeId { get; set; }
        /// <summary>
        /// Gets or sets the programId
        /// </summary>
        public int? ProgramId { get; set; }
        /// <summary>
        /// Gets or sets the projectId
        /// </summary>
        public int? ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the category id
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category
        /// </summary>
        public DataPointCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the property id
        /// </summary>
        [Required]
        public int PropertyId { get; set; }

        /// <summary>
        /// Gets or sets the property
        /// </summary>
        public DataPointProperty Property { get; set; }

        /// <summary>
        /// Gets or sets the isHidden flag
        /// </summary>
        public bool IsHidden { get; set; }
    }
}
