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
        /// Gets or sets the office id
        /// </summary>
        public int? OfficeId { get; set; }

        /// <summary>
        /// Gets or sets the program id
        /// </summary>
        public int? ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the project id 
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the category property id
        /// </summary>
        public int DataPointCategoryPropertyId { get; set; }

        /// <summary>
        /// Gets or sets the category property
        /// </summary>
        public DataPointCategoryProperty DataPointCategoryProperty { get; set; }
    }
}
