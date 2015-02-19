using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Sorter
{
    /// <summary>
    /// A LinqSorter is a property operator that is used to sort a collection of objects on that property.
    /// </summary>
    /// <typeparam name="T">The type to sort.</typeparam>
    public class LinqSorter<T> : PropertyOperator<T> where T : class
    {
        /// <summary>
        /// Creates a new LinqSorter with the given property name and direction.
        /// </summary>
        /// <param name="property">The name of the property to sort on.</param>
        /// <param name="direction">The direction to sort on.</param>
        public LinqSorter(string property, SortDirection direction) : base(property)
        {
            Contract.Requires(property != null, "The property must not be null.");
            Contract.Requires(direction != null, "The sort direction must not be null.");
            this.Direction = direction;
        }

        /// <summary>
        /// Gets the direction to sort.
        /// </summary>
        public SortDirection Direction { get; private set; }

        /// <summary>
        /// Returns an Expression to be used in a linq query to sort IQueryables with.
        /// </summary>
        /// <returns>An expression to sort IQueryables.</returns>
        public Expression<Func<T, object>> ToOrderByExpression()
        {
            var xParameter = Expression.Parameter(typeof(T), "x");
            var xProperty = Expression.Property(xParameter, this.PropertyInfo);
            var xPropertyObject = Expression.Convert(xProperty, typeof(object));
            return Expression.Lambda<Func<T, object>>(xPropertyObject, xParameter);
        }
    }

    /// <summary>
    /// Adds additional linq order operations to a LinqSorter.
    /// </summary>
    public static class LinqSorterExtensions
    {
        #region IQueryable
        /// <summary>
        /// Orders the given IQueryable with the given linq sorter.
        /// </summary>
        /// <typeparam name="T">The type to sort.</typeparam>
        /// <param name="source">The collection to sort.</param>
        /// <param name="sorter">The sorter.</param>
        /// <returns>The ordered query.</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, LinqSorter<T> sorter) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(sorter != null, "The sorters must not be null.");
            var expression = sorter.ToOrderByExpression();
            if (sorter.Direction == SortDirection.Ascending)
            {
                return source.OrderBy(expression);
            }
            else
            {
                return source.OrderByDescending(expression);
            }
        }

        /// <summary>
        /// Orders the given IQueryable with the given linq sorters.
        /// </summary>
        /// <typeparam name="T">The type to sort.</typeparam>
        /// <param name="source">The collection to sort.</param>
        /// <param name="sorters">The sorters.</param>
        /// <returns>The ordered query.</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<LinqSorter<T>> sorters) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(sorters != null, "The sorters must not be null.");
            var count = 0;
            using (var enumerator = sorters.GetEnumerator())
            {
                IOrderedQueryable<T> orderedQuery = null;
                while (enumerator.MoveNext())
                {
                    var sorter = enumerator.Current;
                    var expression = sorter.ToOrderByExpression();
                    if (count == 0)
                    {
                        if (sorter.Direction == SortDirection.Ascending)
                        {
                            orderedQuery = source.OrderBy(expression);
                        }
                        else
                        {
                            orderedQuery = source.OrderByDescending(expression);
                        }
                    }
                    else
                    {
                        Debug.Assert(orderedQuery != null, "The ordered query should be initialized.");
                        if (sorter.Direction == SortDirection.Ascending)
                        {
                            orderedQuery = orderedQuery.ThenBy(expression);
                        }
                        else
                        {
                            orderedQuery = orderedQuery.ThenByDescending(expression);
                        }
                    }
                    count++;
                }
                return orderedQuery;
            }
        }

        /// <summary>
        /// Orders the given IQueryable with the given sorter.
        /// </summary>
        /// <typeparam name="T">The type to sort.</typeparam>
        /// <param name="source">The collection to sort.</param>
        /// <param name="sorter">The sorter.</param>
        /// <returns>The ordered query.</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, ISorter sorter) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(sorter != null, "The sorter must not be null.");
            return OrderBy<T>(source, sorter.ToLinqSorter<T>());
        }

        /// <summary>
        /// Orders the given IQueryable with the given sorters.
        /// </summary>
        /// <typeparam name="T">The type to sort.</typeparam>
        /// <param name="source">The collection to sort.</param>
        /// <param name="sorters">The sorters.</param>
        /// <returns>The ordered query.</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<ISorter> sorters) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(sorters != null, "The sorters must not be null.");
            var linqSorters = sorters.ToList().Select(x => x.ToLinqSorter<T>()).ToList();
            return OrderBy<T>(source, linqSorters);
        }

        #endregion
    }
}
