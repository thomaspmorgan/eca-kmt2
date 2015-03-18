using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Data
{
    /// <summary>
    /// An IConcurrentEntity is a database entity that maintains concurrency.
    /// </summary>
    public interface IConcurrentEntity : IConcurrent
    {
        /// <summary>
        /// Returns the id of the concurrent entity.
        /// </summary>
        /// <returns>The Id of the entity.</returns>
        object GetId();
    }
}
