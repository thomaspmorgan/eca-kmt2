using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// Data point category model
    /// </summary>
    public partial class DataPointCategory
    {
        /// <summary>
        /// Gets or sets the data point category id
        /// </summary>
        [Key]
        public int DataPointCategoryId { get; set; }
        /// <summary>
        /// Gets or sets the data point category name
        /// </summary>
        [Required]
        public string DataPointCategoryName { get; set; }
    }
}
