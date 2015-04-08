using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// An IHistorical class is a class that tracks history on its revisions.
    /// </summary>
    public interface IHistorical
    {
        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        History History { get; set; }
    }
}
