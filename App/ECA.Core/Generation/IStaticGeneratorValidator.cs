using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Generation
{
    /// <summary>
    /// Validate a class containing static lookup properties against a repository.
    /// </summary>
    public interface IStaticGeneratorValidator
    {
        /// <summary>
        /// Returns a collection fo error string detailing the static lookups configured for the class of type T.
        /// </summary>
        /// <typeparam name="T">The type of object to validate.</typeparam>
        /// <returns>The list of errors, or zero errors if there are no validation concerns.</returns>
        List<string> Validate<T>() where T : class, IStaticLookup;
    }


}
