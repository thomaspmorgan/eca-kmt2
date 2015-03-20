using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Generation
{
    /// <summary>
    /// An IStaticLookup class is a class that was generated via the StaticDataGenerator and can return the StaticLookupConfig used
    /// for generation.
    /// </summary>
    public interface IStaticLookup
    {
        /// <summary>
        /// Gets the StaticLookupConfig used at generation for the lookups in this type.
        /// </summary>
        /// <returns>The StaticLookupConfig used to generate lookups.</returns>
        StaticLookupConfig GetConfig();
    }
}
