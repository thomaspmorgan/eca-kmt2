using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// A Focus is a concentration that a project encompasses.
    /// </summary>
    public class Focus : IHistorical
    {
        /// <summary>
        /// The max length of the focus.
        /// </summary>
        public const int NAME_MAX_LENGTH = 4000;

        /// <summary>
        /// Creates a new Focus.
        /// </summary>
        public Focus()
        {
            this.History = new History();
        }

        /// <summary>
        /// Gets or set the Id.
        /// </summary>
        public int FocusId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string FocusName { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }
    }
}
