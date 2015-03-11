using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Query
{
    public class KeywordAttribute : ValidationAttribute
    {
        public KeywordAttribute(int maxKeywordsCount, int maxKeywordLength)
        {
            this.MaxKeywordsCount = maxKeywordsCount;
            this.MaxKeywordsLength = maxKeywordLength;
        }

        public int MaxKeywordsCount { get; private set; }

        public int MaxKeywordsLength { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var keywordsValue = ((IEnumerable<string>)value).ToList();
            if (keywordsValue.Count == 0)
            {
                return new ValidationResult("There must be at least one keyword.");
            }
            if (keywordsValue.Count > this.MaxKeywordsCount)
            {
                return new ValidationResult(String.Format("The number of keywords exceeds the maximum length [{0}].", this.MaxKeywordsLength));
            }
            var allCharsLength = keywordsValue.SelectMany(x => x.ToCharArray()).Count();
            if (allCharsLength > this.MaxKeywordsLength)
            {
                return new ValidationResult(String.Format("The number of characters to search for exceeds the maximum length [{0}].", this.MaxKeywordsLength));
            }

            return ValidationResult.Success;
        }
    }
}