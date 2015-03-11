using System;
using System.Collections;
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
    public class KeywordFilter<TSource> : LinqFilter<TSource> where TSource : class
    {
        /// <summary>
        /// The maximum number of properties to filter on.
        /// </summary>
        public const int MAX_PROPERTIES_COUNT = 10;

        /// <summary>
        /// The maximum number of keywords to filter with.
        /// </summary>
        public const int MAX_KEYWORDS_COUNT = 10;

        /// <summary>
        /// The maximum length of a keyword.
        /// </summary>
        public const int MAX_KEYWORD_LENGTH = 30;

        public KeywordFilter(ISet<string> properties, ISet<string> keywords)
        {
            Contract.Requires(properties != null, "The properties must not be null.");
            Contract.Requires(properties.Count > 0, "There must be at least one property provided.");            
            Contract.Requires(properties.Count <= MAX_PROPERTIES_COUNT, "The number of properties to filter on must not exceed the max of " + MAX_PROPERTIES_COUNT);

            Contract.Requires(keywords != null, "The keywords must not be null.");
            Contract.Requires(keywords.Count() <= MAX_KEYWORDS_COUNT, "The number of keywords to filter on must not exceed the max of " + MAX_PROPERTIES_COUNT);

            Contract.Requires(keywords.Where(x => x.Count() > MAX_KEYWORD_LENGTH).Count() <= MAX_KEYWORD_LENGTH, 
                "A keyword must not exceed " + MAX_KEYWORD_LENGTH + " in length.");

            InitializeKeywords(keywords);
            InitializeProperties(properties);
            if (this.Properties.Count == 0)
            {
                throw new ArgumentException("The given properties do not match any properties on the object to filter.");
            }
        }

        private IDictionary<string, PropertyInfo> Properties { get; set; }

        private IList<string> Keywords { get; set; }

        public override Expression<Func<TSource, bool>> ToWhereExpression()
        {
            var stringType = typeof(string);
            var xParameter = Expression.Parameter(typeof(TSource), "x");
            var keywordsParameter = Expression.Parameter(typeof(IEnumerable<string>), "keywords");
            var keywordParameter = Expression.Parameter(stringType, "keyword");
            var keywordsConstant = Expression.Constant(this.Keywords);

            var containsExpressions = new List<Expression>();
            foreach (var property in this.Properties.Values)
            {
                containsExpressions.Add(CreateContainsKeywordExpression(xParameter, property, keywordParameter));
            }
            Expression orExpression = null;
            foreach (var containsExpression in containsExpressions)
            {
                if (orExpression == null)
                {
                    orExpression = containsExpression;
                }
                else
                {
                    orExpression = Expression.Or(orExpression, containsExpression);
                }
            }

            var anyMethod = typeof(Enumerable).GetMethods()
                   .Where(m => m.Name == "Any")
                   .Single(m => m.GetParameters().Length == 2)
                   .MakeGenericMethod(stringType);

            var orLambda = Expression.Lambda<Func<string, bool>>(orExpression, keywordParameter);
            var callAnyMethod = Expression.Call(anyMethod, keywordsConstant, orLambda);

            var anyLambda = Expression.Lambda<Func<TSource, bool>>(callAnyMethod, xParameter);
            return anyLambda;
        }

        private Expression CreateContainsKeywordExpression(ParameterExpression xParameter, PropertyInfo property, ParameterExpression keywordParameter)
        {
            var xProperty = Expression.Property(xParameter, property);
            var isNotNullExpression = Expression.NotEqual(xProperty, Expression.Constant(null));
            var toLowerMethod = Expression.Call(xProperty, "ToLower", System.Type.EmptyTypes);

            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsMethodExpression = Expression.Call(toLowerMethod, containsMethod, keywordParameter);
            return Expression.AndAlso(isNotNullExpression, containsMethodExpression);
        }


        private void InitializeKeywords(ISet<string> keywords)
        {
            this.Keywords = new List<string>();
            foreach (var keyword in keywords)
            {
                this.Keywords.Add(keyword.Trim().ToLower());
            }
        }

        private void InitializeProperties(ISet<string> properties)
        {
            this.Properties = new Dictionary<string, PropertyInfo>();
            foreach (var property in properties)
            {
                this.Properties.Add(property, GetPropertyInfo(property));
            }
        }

        /// <summary>
        /// Returns the property info of the property with the given name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The PropertyInfo associated with the property.</returns>
        private PropertyInfo GetPropertyInfo(string propertyName)
        {
            Contract.Assert(propertyName != null, "The property name must not be null.");
            var property = typeof(TSource).GetProperties().Where(x => x.Name.ToLower() == propertyName.ToLower()).FirstOrDefault();
            return property;
        }
    }
}
