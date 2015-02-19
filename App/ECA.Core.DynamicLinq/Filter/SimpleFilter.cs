﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Filter
{
    /// <summary>
    /// A SimpleFilter is a class used represent a filter on an object.  It implements IFilter so that
    /// it can be used in Linq queries.
    /// </summary>
    public class SimpleFilter : IFilter
    {
        /// <summary>
        /// Gets or sets the Comparison.
        /// </summary>
        public string Comparison { get; set; }

        /// <summary>
        /// Gets or sets the Property.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Returns a LinqFilter to be used in linq queries.
        /// </summary>
        /// <typeparam name="T">The object type to filter.</typeparam>
        /// <returns>The linq filter.</returns>
        public LinqFilter<T> ToLinqFilter<T>() where T : class
        {
            var comparisonType = ComparisonType.ToComparisonType(this.Comparison);
            if (comparisonType == ComparisonType.Equal)
            {
                return new EqualFilter<T>(this.Property, this.Value);
            }
            if (comparisonType == ComparisonType.GreaterThan)
            {
                return new GreaterThanFilter<T>(this.Property, this.Value);
            }
            if (comparisonType == ComparisonType.LessThan)
            {
                return new LessThanFilter<T>(this.Property, this.Value);
            }
            if (comparisonType == ComparisonType.NotNull)
            {
                return new NullFilter<T>(this.Property, true);
            }
            if (comparisonType == ComparisonType.Null)
            {
                return new NullFilter<T>(this.Property, false);
            }
            if (comparisonType == ComparisonType.Like)
            {
                return new LikeFilter<T>(this.Property, this.Value);
            }
            throw new NotSupportedException("The client filter is not supported.");
        }
    }
}
