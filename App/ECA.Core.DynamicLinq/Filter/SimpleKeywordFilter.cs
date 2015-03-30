using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Filter
{
    /// <summary>
    /// A SimpleKeywordFilter is a filter for handling keyword searches on an object.
    /// </summary>
    public class SimpleKeywordFilter : IFilter
    {
        internal SimpleKeywordFilter() { }

        /// <summary>
        /// Creates a new SimpleKeywordFilter with the properties to filter on and the keywords to search for.
        /// </summary>
        /// <param name="properties">The set of properties to filter on.</param>
        /// <param name="keywords">The keywords to search for.</param>
        public SimpleKeywordFilter(ISet<string> properties, ISet<string> keywords)
        {
            Contract.Requires(keywords != null, "The keywords must not be null.");
            Contract.Requires(properties != null, "The properties must not be null.");
            Contract.Requires(properties.Count() > 0, "There must be at least one property.");
            this.Keywords = keywords;
            this.Properties = properties;
        }

        /// <summary>
        /// Gets the keywords to filter on.
        /// </summary>
        public ISet<string> Keywords { get; protected set; }

        /// <summary>
        /// Gets the property names to filter on.
        /// </summary>
        public ISet<string> Properties { get; protected set; }

        /// <summary>
        /// Returns a LinqFilter of this filter to be used in linq queries.
        /// </summary>
        /// <typeparam name="T">The type of object to filter by keywords.</typeparam>
        /// <returns>The linq filter.</returns>
        public LinqFilter<T> ToLinqFilter<T>() where T : class
        {
            return new KeywordFilter<T>(this.Properties, this.Keywords);
        }
    }

    /// <summary>
    /// A typed SimpleKeywordFilter implementation that provides a convience constructor for using
    /// expressions and string keywords on a class to filter.
    /// </summary>
    /// <typeparam name="T">The type to filter.</typeparam>
    public class SimpleKeywordFilter<T> : SimpleKeywordFilter, IFilter where T : class
    {
        /// <summary>
        /// Creates a new SimpleKeywordFilter with the set of keywords and the expressions giving the properties
        /// to filter on.
        /// </summary>
        /// <param name="keywords">The keywords.</param>
        /// <param name="propertySelectors">The expressions giving the properties to filter on.</param>
        public SimpleKeywordFilter(ISet<string> keywords, params Expression<Func<T, string>>[] propertySelectors)
        {
            Contract.Requires(keywords != null, "The keywords must not be null.");
            Contract.Requires(propertySelectors != null, "The properties must not be null.");
            Contract.Requires(propertySelectors.Count() > 0, "There must be at least one property.");
            this.Keywords = keywords;
            this.Properties = new HashSet<string>();
            foreach (var propertySelector in propertySelectors)
            {
                this.Properties.Add(PropertyHelper.GetPropertyName<T>(propertySelector));
            }
        }
    }
}
