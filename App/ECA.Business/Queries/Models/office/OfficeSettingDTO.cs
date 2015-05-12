using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Office
{
    /// <summary>
    /// An OfficeSettingDTO represents a single setting key value pair for an office.
    /// </summary>
    public class OfficeSettingDTO
    {
        /// <summary>
        /// Gets or sets the Id of the key value pair.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the office id.
        /// </summary>
        public int OfficeId { get; set; }

        /// <summary>
        /// Gets or sets the Name i.e. Key.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        public string Value { get; set; }
    }
}
