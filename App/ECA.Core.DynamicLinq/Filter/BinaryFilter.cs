using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Filter
{
    /// <summary>
    /// A BinaryFilter is used to compare an object's property value to another value of the same type.
    /// </summary>
    /// <typeparam name="T">The type to filter on.</typeparam>
    public abstract class BinaryFilter<T> : PropertyOperator<T> where T : class
    {
        /// <summary>
        /// Creates a new BinaryFilter with the property to filter on the value to filter with.  The
        /// property type and value type must be the same.
        /// </summary>
        /// <param name="property">The property to filter on.</param>
        /// <param name="value">The value to filter with.</param>
        public BinaryFilter(string property, object value)
            : base(property)
        {
            Contract.Requires(property != null, "The property must not be null.");
            Contract.Requires(value != null, "The value must not be null.");
        }

        /// <summary>
        /// Gets the value to filter with.
        /// </summary>
        public object Value { get; protected set; }

    }
}
