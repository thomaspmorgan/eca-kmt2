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

        public SimpleKeywordFilter(ISet<string> properties, ISet<string> keywords)
        {
            Contract.Requires(keywords != null, "The keywords must not be null.");
            Contract.Requires(properties != null, "The properties must not be null.");
            Contract.Requires(properties.Count() > 0, "There must be at least one property.");
            this.Keywords = keywords;
            this.Properties = properties;
        }

        public ISet<string> Keywords { get; protected set; }

        public ISet<string> Properties { get; protected set; }

        public LinqFilter<T> ToLinqFilter<T>() where T : class
        {
            return new KeywordFilter<T>(this.Properties, this.Keywords);
        }
    }

    public class SimpleKeywordFilter<T> : SimpleKeywordFilter, IFilter where T : class
    {
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
