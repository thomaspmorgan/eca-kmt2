using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq
{
    /// <summary>
    /// A Comparison type is a simple representation of a comparison test.
    /// </summary>
    public class ComparisonType
    {
        private const string IN = "in";
        private const string IN_DISPLAY = "In";

        private const string NOT_IN = "notin";
        private const string NOT_IN_DISPLAY = "Not In";

        private const string LIKE = "like";
        private const string LIKE_DISPLAY = "Like";

        private const string LESS_THAN = "lt";
        private const string LESS_THAN_DISPLAY = "<";

        private const string GREATER_THAN = "gt";
        private const string GREATER_THAN_DISPLAY = ">";

        private const string EQUAL = "eq";
        private const string EQUAL_DISPLAY = "==";

        private const string IS_NULL = "null";
        private const string IS_NULL_DISPLAY = "Null";

        private const string IS_NOT_NULL = "notnull";
        private const string IS_NOT_NULL_DISPLAY = "Not Null";


        /// <summary>
        /// Gets the not in comparison type.
        /// </summary>
        public static ComparisonType NotIn
        {
            get
            {
                return new ComparisonType(NOT_IN, NOT_IN_DISPLAY);
            }
        }

        /// <summary>
        /// Gets the in comparison type.
        /// </summary>
        public static ComparisonType In
        {
            get
            {
                return new ComparisonType(IN, IN_DISPLAY);
            }
        }

        /// <summary>
        /// Gets the like comparison type.
        /// </summary>
        public static ComparisonType Like
        {
            get
            {
                return new ComparisonType(LIKE, LIKE_DISPLAY);
            }
        }

        /// <summary>
        /// Gets the less than comparison type.
        /// </summary>
        public static ComparisonType LessThan
        {
            get
            {
                return new ComparisonType(LESS_THAN, LESS_THAN_DISPLAY);
            }
        }

        /// <summary>
        /// Gets the greater than comparison type.
        /// </summary>
        public static ComparisonType GreaterThan
        {
            get
            {
                return new ComparisonType(GREATER_THAN, GREATER_THAN_DISPLAY);
            }
        }

        /// <summary>
        /// Gets the equal comparison type.
        /// </summary>
        public static ComparisonType Equal
        {
            get
            {
                return new ComparisonType(EQUAL, EQUAL_DISPLAY);
            }
        }

        /// <summary>
        /// Gets the null comparison type.
        /// </summary>
        public static ComparisonType Null
        {
            get
            {
                return new ComparisonType(IS_NULL, IS_NULL_DISPLAY);
            }
        }

        /// <summary>
        /// Gets the not null comparison type.
        /// </summary>
        public static ComparisonType NotNull
        {
            get
            {
                return new ComparisonType(IS_NOT_NULL, IS_NOT_NULL_DISPLAY);
            }
        }

        private ComparisonType(string value, string displayName)
        {
            Contract.Requires(value != null, "The comparison type must not be null.");
            Contract.Requires(displayName != null, "The displayName must not be null.");
            this.Value = value;
            this.Display = displayName;
        }

        /// <summary>
        /// Gets the string value of the comaprison.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Gets the display name of the comaprison.
        /// </summary>
        private string Display { get; set; }

        /// <summary>
        /// Returns a string of this comparison type.
        /// </summary>
        /// <returns>A string of this comparison type.</returns>
        public override string ToString()
        {
            return this.Display;
        }

        /// <summary>
        /// Returns true if the given object equals this object.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>True if the given object equals this object.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var otherType = obj as ComparisonType;
            if (otherType == null)
            {
                return false;
            }
            return this.Value == otherType.Value;
            
        }

        /// <summary>
        /// Returns a hash of this object.
        /// </summary>
        /// <returns>A hash of this object.</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Returns true if the given instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The second instance.</param>
        /// <returns>True if the given instances are equal.</returns>
        public static bool operator ==(ComparisonType a, ComparisonType b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Value == b.Value;
        }

        /// <summary>
        /// Returns false if the given instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The second instance.</param>
        /// <returns>False if the given instances are equal.</returns>
        public static bool operator !=(ComparisonType a, ComparisonType b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a comparison type whose values is equal to the given string.
        /// </summary>
        /// <param name="comparisonType">The comparison type as a string.</param>
        /// <returns>The comparison type.</returns>
        public static ComparisonType ToComparisonType(string comparisonType)
        {
            Contract.Requires(comparisonType != null, "The comparison type must not be null.");
            var dictionary = new Dictionary<string, ComparisonType>();
            dictionary.Add(LESS_THAN.ToUpper(), ComparisonType.LessThan);
            dictionary.Add(GREATER_THAN.ToUpper(), ComparisonType.GreaterThan);
            dictionary.Add(EQUAL.ToUpper(), ComparisonType.Equal);
            dictionary.Add(IS_NULL.ToUpper(), ComparisonType.Null);
            dictionary.Add(IS_NOT_NULL.ToUpper(), ComparisonType.NotNull);
            dictionary.Add(LIKE.ToUpper(), ComparisonType.Like);
            dictionary.Add(IN.ToUpper(), ComparisonType.In);
            dictionary.Add(NOT_IN.ToUpper(), ComparisonType.NotIn);

            var cType = comparisonType.ToUpper().Trim();
            Contract.Assert(dictionary.ContainsKey(cType), String.Format("The comparison type [{0}] is not recognized.", comparisonType));
            return dictionary[cType];
        }
    }
}
