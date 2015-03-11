using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Filter
{
    public class SimpleKeywordFilter : IFilter
    {

        internal SimpleKeywordFilter() { }

        public SimpleKeywordFilter(IList<string> properties, IList<string> keywords)
        {
            Contract.Requires(keywords != null, "The keywords must not be null.");
            Contract.Requires(properties != null, "The properties must not be null.");
            Contract.Requires(properties.Count() > 0, "There must be at least one property.");
            this.Keywords = keywords;
            this.Properties = properties;
        }

        public IList<string> Keywords { get; protected set; }

        public IList<string> Properties { get; protected set; }

        public LinqFilter<T> ToLinqFilter<T>() where T : class
        {
            return new KeywordFilter<T>(this.Properties, this.Keywords);
        }
    }

    public class SimpleKeywordFilter<T> : SimpleKeywordFilter, IFilter where T : class
    {
        public SimpleKeywordFilter(IList<string> keywords, params Expression<Func<T, object>>[] propertySelectors)
        {
            Contract.Requires(keywords != null, "The keywords must not be null.");
            Contract.Requires(propertySelectors != null, "The properties must not be null.");
            Contract.Requires(propertySelectors.Count() > 0, "There must be at least one property.");
            this.Keywords = keywords;
            this.Properties = new List<string>();
            foreach (var propertySelector in propertySelectors)
            {
                this.Properties.Add(PropertyHelper.GetPropertyName<T>(propertySelector));
            }
        }
    }
}
