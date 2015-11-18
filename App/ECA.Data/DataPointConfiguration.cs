using System;
using System.Collections.Generic;
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
        /// Gets or sets the category
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Gets or sets the property
        /// </summary>
        public string Property { get; set; }
        /// <summary>
        /// Gets or sets the isHidden flag
        /// </summary>
        public bool IsHidden { get; set; }
    }
}
