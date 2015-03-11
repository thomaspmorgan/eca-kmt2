using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Query
{
    /// <summary>
    /// A ValidationAttribute for the Keyword Property on a KeywordBindingModel class.
    /// </summary>
    public class KeywordAttribute : ValidationAttribute
    {
        /// <summary>
        /// Creates a new KeywordAttribute with the maximum count values.
        /// </summary>
        /// <param name="maxKeywordsCount">The maximum number of individual keywords allowed in a search.</param>
        /// <param name="maxKeywordLength">The cumulative number of characters allowed in a search.</param>
        public KeywordAttribute(int maxKeywordsCount, int maxKeywordLength)
        {
            this.MaxNumberOfKeywords = maxKeywordsCount;
            this.MaxKeywordLength = maxKeywordLength;
        }

        /// <summary>
        /// Gets the maximum number of keywords allowed.
        /// </summary>
        public int MaxNumberOfKeywords { get; private set; }

        /// <summary>
        /// Gets the maximum allowed keyword length.
        /// </summary>
        public int MaxKeywordLength { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var keywordsValue = ((IEnumerable<string>)value).ToList();
            if (keywordsValue.Count == 0)
            {
                return new ValidationResult("There must be at least one keyword.");
            }
            if (keywordsValue.Count > this.MaxNumberOfKeywords)
            {
                return new ValidationResult(String.Format("The number of keywords to search with must not exceed [{0}].", this.MaxNumberOfKeywords));
            }
            var keywordsExceedingLengthCount = keywordsValue.Where(x => x.Count() > this.MaxKeywordLength).Count();
            if (keywordsExceedingLengthCount > 0)
            {
                return new ValidationResult(String.Format("A single keyword's length must not exceed [{0}].", this.MaxKeywordLength));
            }

            return ValidationResult.Success;
        }
    }
}