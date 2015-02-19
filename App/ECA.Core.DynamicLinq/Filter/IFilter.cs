using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Filter
{
    /// <summary>
    /// An IFilter is a class that can be converted to a filter used against objects.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Returns a LinqFilter to be used in Linq queries.
        /// </summary>
        /// <typeparam name="T">The type to filter on.</typeparam>
        /// <returns>The linq query filter.</returns>
        LinqFilter<T> ToLinqFilter<T>() where T : class;
    }
}
