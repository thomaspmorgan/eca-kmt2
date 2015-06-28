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
    public class JustificationObjectiveDTO : BaseDTO
    {
        /// <summary>
        /// Gets or sets the justification name.
        /// </summary>
        public string JustificationName { get; set; }
    }
}
