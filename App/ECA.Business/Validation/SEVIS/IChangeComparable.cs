using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// An IChangeComparable class can determine what instance changes hae occurred given a previous version
    /// of that instance.
    /// </summary>
    /// <typeparam name="TChangeComparable">The class that is IChangeComparable.</typeparam>
    /// <typeparam name="TChangeDetail">The change detail type that is returned.</typeparam>
    public interface IChangeComparable<TChangeComparable, TChangeDetail>
        where TChangeDetail : ChangeDetail
        where TChangeComparable : class
    {
        /// <summary>
        /// Returns a change detail instance with all change information of this instance and the given instance.
        /// </summary>
        /// <param name="otherChangeComparable">The instance to compare for changes.</param>
        /// <returns>The object detailing the changes between this instance the given instance.</returns>
        TChangeDetail GetChangeDetail(TChangeComparable otherChangeComparable);
    }
}
