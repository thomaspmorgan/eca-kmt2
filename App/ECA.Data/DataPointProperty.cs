using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// Data point property model
    /// </summary>
    public partial class DataPointProperty
    {
        /// <summary>
        /// Gets or sets the data point property id
        /// </summary>
        [Key]
        public int DataPointPropertyId { get; set; }

        /// <summary>
        /// Gets or sets the data point property name
        /// </summary>
        [Required]
        public string DataPointPropertyName { get; set; }
    }
}
