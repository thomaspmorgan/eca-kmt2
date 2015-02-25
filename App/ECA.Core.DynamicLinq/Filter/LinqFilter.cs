﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Filter
{
    /// <summary>
    /// A LinqFilter is a property operator that will filter instances on a object's property.
    /// </summary>
    /// <typeparam name="T">The object type to filter on.</typeparam>
    public abstract class LinqFilter<T> : PropertyOperator<T> where T : class
    {
        /// <summary>
        /// Creates a new LinqFilter with the property name to filter on.
        /// </summary>
        /// <param name="property">The name of the property to filter on.</param>
        public LinqFilter(string property) : base(property)
        {
            Contract.Requires(property != null, "The property must not be null.");
        }

        /// <summary>
        /// Returns an expression to filter with in a linq query.
        /// </summary>
        /// <returns>An expression to filter with in a linq query.</returns>
        public abstract Expression<Func<T, bool>> ToWhereExpression();

        /// <summary>
        /// Returns a string representation of this linq filter.
        /// </summary>
        /// <returns>A string representation of this linq filter.</returns>
        public override string ToString()
        {
            return ToWhereExpression().ToString();
        }
    }

    /// <summary>
    /// Add additional linq query expressions.
    /// </summary>
    public static class LinqFilterExtensions
    {
        #region IEnumerable
        /// <summary>
        /// Filters an IEnumerable with the given linq filter.
        /// </summary>
        /// <typeparam name="T">The type to filter.</typeparam>
        /// <param name="source">The collection of instances to filter.</param>
        /// <param name="filter">The linq query filter.</param>
        /// <returns>The filtered collection.</returns>
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, LinqFilter<T> filter) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(filter != null, "The filter must not be null.");
            return source.Where(filter.ToWhereExpression().Compile());
        }

        /// <summary>
        /// Filters an IEnumerable with the given linq filters.
        /// </summary>
        /// <typeparam name="T">The type to filter.</typeparam>
        /// <param name="source">The collection of instances to filter.</param>
        /// <param name="filters">The linq query filters.</param>
        /// <returns>The filtered collection.</returns>
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, IEnumerable<LinqFilter<T>> filters) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(filters != null, "The filters must not be null.");
            foreach (var filter in filters)
            {
                source = source.Where(filter);
            }
            return source;
        }

        /// <summary>
        /// Filters an IEnumerable with the given IFilter.
        /// </summary>
        /// <typeparam name="T">The type to filter.</typeparam>
        /// <param name="source">The collection of objects to filter.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>The filtered collection.</returns>
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, IFilter filter) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(filter != null, "The filter must not be null.");
            return source.Where(filter.ToLinqFilter<T>());
        }

        /// <summary>
        /// Filters an IEnumerable with the given IFilters.
        /// </summary>
        /// <typeparam name="T">The type to filter.</typeparam>
        /// <param name="source">The collection of objects to filter.</param>
        /// <param name="filter">The filters.</param>
        /// <returns>The filtered collection.</returns>
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, IEnumerable<IFilter> filters) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(filters != null, "The filters must not be null.");
            foreach (var filter in filters)
            {
                source = source.Where(filter);
            }
            return source;
        }
        #endregion

        #region IQueryable
        /// <summary>
        /// Filters an IQueryable with the given linq filter.
        /// </summary>
        /// <typeparam name="T">The type to filter.</typeparam>
        /// <param name="source">The collection of instances to filter.</param>
        /// <param name="filter">The linq query filter.</param>
        /// <returns>The filtered collection.</returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> source, LinqFilter<T> filter) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(filter != null, "The filter must not be null.");
            return source.Where(filter.ToWhereExpression());
        }

        /// <summary>
        /// Filters an IQueryable with the given linq filters.
        /// </summary>
        /// <typeparam name="T">The type to filter.</typeparam>
        /// <param name="source">The collection of instances to filter.</param>
        /// <param name="filters">The linq query filters.</param>
        /// <returns>The filtered collection.</returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> source, IEnumerable<LinqFilter<T>> filters) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(filters != null, "The filters must not be null.");
            foreach (var filter in filters)
            {
                source = source.Where(filter);
            }
            return source;
        }

        /// <summary>
        /// Filters an IQueryable with the given IFilter.
        /// </summary>
        /// <typeparam name="T">The type to filter.</typeparam>
        /// <param name="source">The collection of objects to filter.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>The filtered collection.</returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> source, IFilter filter) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(filter != null, "The filter must not be null.");
            return source.Where(filter.ToLinqFilter<T>());
        }

        /// <summary>
        /// Filters an IQueryable with the given IFilters.
        /// </summary>
        /// <typeparam name="T">The type to filter.</typeparam>
        /// <param name="source">The collection of objects to filter.</param>
        /// <param name="filter">The filters.</param>
        /// <returns>The filtered collection.</returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> source, IEnumerable<IFilter> filters) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(filters != null, "The filters must not be null.");
            foreach (var filter in filters)
            {
                source = source.Where(filter);
            }
            return source;
        }

        #endregion
    }
}