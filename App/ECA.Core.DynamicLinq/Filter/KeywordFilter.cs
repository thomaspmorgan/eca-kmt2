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
    /// <summary>
    /// A KeywordFilter is used to filter an object by given properties and their value contained within
    /// any of the keywords.
    /// </summary>
    /// <typeparam name="TSource">The object to filter on.</typeparam>
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

        /// <summary>
        /// Creates a new KeywordFilter for filtering an object by given properties and keywords.  if Any of the properties
        /// match any of the keywords they will be returned as a result.
        /// </summary>
        /// <param name="properties">The properties to filter with.</param>
        /// <param name="keywords">The keywords to filter with.</param>
        public KeywordFilter(ISet<string> properties, ISet<string> keywords)
        {
            Contract.Requires(properties != null, "The properties must not be null.");
            Contract.Requires(properties.Count > 0, "There must be at least one property provided.");            
            Contract.Requires(properties.Count <= MAX_PROPERTIES_COUNT, "The number of properties to filter on must not exceed the max.");

            Contract.Requires(keywords != null, "The keywords must not be null.");
            Contract.Requires(keywords.Count() <= MAX_KEYWORDS_COUNT, "The number of keywords to filter on must not exceed the max.");

            Contract.Requires(keywords.Where(x => x.Count() > MAX_KEYWORD_LENGTH).Count() == 0, 
                "A keyword must not exceed the max length.");

            InitializeKeywords(keywords);
            InitializeProperties(properties);
        }

        /// <summary>
        /// Gets or sets the Properties that are filtered on.
        /// </summary>
        private IDictionary<string, PropertyInfo> Properties { get; set; }

        /// <summary>
        /// Gets or sets the Keywords.
        /// </summary>
        private ISet<string> Keywords { get; set; }

        /// <summary>
        /// Returns an expression that searches the object's properties for a value that
        /// is contained within any of the keywords.  The search is case insensitive.
        /// </summary>
        /// <returns>The where expression.</returns>
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
            Contract.Assert(property.PropertyType == typeof(string), "The property type must be a string.");
            var xProperty = Expression.Property(xParameter, property);
            var isNotNullExpression = Expression.NotEqual(xProperty, Expression.Constant(null));
            var toLowerMethod = Expression.Call(xProperty, "ToLower", System.Type.EmptyTypes);

            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsMethodExpression = Expression.Call(toLowerMethod, containsMethod, keywordParameter);
            return Expression.AndAlso(isNotNullExpression, containsMethodExpression);
        }


        private void InitializeKeywords(ISet<string> keywords)
        {
            this.Keywords = new HashSet<string>();
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
            Contract.Requires(propertyName != null, "The property name must not be null.");
            var property = typeof(TSource).GetProperties().Where(x => x.Name.ToLower() == propertyName.ToLower()).FirstOrDefault();
            if (property.PropertyType != typeof(string))
            {
                throw new NotSupportedException("The property type to filter on must be a string.");
            }
            return property;
        }
    }
}
