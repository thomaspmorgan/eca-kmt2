using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// Data point category property model
    /// </summary>
    public class DataPointCategoryProperty
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        [Key]
        public int DataPointCategoryPropertyId { get; set; }

        /// <summary>
        /// Gets or sets the category id
        /// </summary>
        public int DataPointCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category
        /// </summary>
        public DataPointCategory DataPointCategory { get; set; }

        /// <summary>
        /// Gets or sets the property id
        /// </summary>
        public int DataPointPropertyId { get; set; }
       
        /// <summary>
        /// Gets or sets the property
        /// </summary>
        public DataPointProperty DataPointProperty { get; set; }
    }
}
