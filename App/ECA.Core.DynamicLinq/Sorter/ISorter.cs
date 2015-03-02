using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Sorter
{
    /// <summary>
    /// An ISorter is a class that can be converted to sorters used in queries.
    /// </summary>
    public interface ISorter
    {
        /// <summary>
        /// Returns a linq sorter from this ISorter.
        /// </summary>
        /// <typeparam name="T">The type to sort.</typeparam>
        /// <returns>The linq sorter.</returns>
        LinqSorter<T> ToLinqSorter<T>() where T : class;
    }
}
