using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Sorter
{
    /// <summary>
    /// An ExpressionSorter is a convience class for creating a sorter using an Expression.
    /// </summary>
    /// <typeparam name="TSource">The type of object to sort.</typeparam>
    public class ExpressionSorter<TSource> : SimpleSorter where TSource : class
    {
        /// <summary>
        /// Creates a new expression sorter.
        /// </summary>
        /// <param name="propertySelector">The property expression for selecting the property to sort on.</param>
        /// <param name="direction">The sort direction.</param>
        public ExpressionSorter(Expression<Func<TSource, object>> propertySelector, SortDirection direction)
        {
            Contract.Requires(propertySelector != null, "The property selector must not be null.");
            Contract.Requires(direction != null, "The direction must not be null.");
            this.Direction = direction.Value;
            this.Property = PropertyOperator<TSource>.GetPropertyName(propertySelector);
        }
    }
}
