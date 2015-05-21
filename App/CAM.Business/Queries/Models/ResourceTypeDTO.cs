using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Queries.Models
{
    /// <summary>
    /// The ResourceTypeDTO is a simple class to detail resource types in CAM.
    /// </summary>
    public class ResourceTypeDTO
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type name.
        /// </summary>
        public string Name { get; set; }
    }
}
