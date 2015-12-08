using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// Data point configuration data transfer object
    /// </summary>
    public class DataPointConfigurationDTO
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public int? DataPointConfigurationId { get; set; }

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
        public int CategoryPropertyId { get; set; }

        /// <summary>
        /// Gets or sets the category id
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Gets or sets the category name
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// Gets or sets the property id
        /// </summary>
        public int PropertyId { get; set; }
        /// <summary>
        /// Gets or sets the property name
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the required flag
        /// </summary>
        public bool IsRequired { get; set; }
    }
}
