using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Data
{
    /// <summary>
    /// An IIdentifiable object is an object that has a unique Id.
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        /// Returns the id of the concurrent entity.
        /// </summary>
        /// <returns>The Id of the entity.</returns>
        int GetId();
    }
}
