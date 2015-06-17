using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// A FocusDTO is a simple representation of a focus in the ECA system.
    /// </summary>
    public class FocusCategoryDTO : BaseDTO
    {
        /// <summary>
        /// Gets or sets the focus name.
        /// </summary>
        public string FocusName { get; set; }
    }
}
