using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Query;
using System.Collections.Generic;

namespace ECA.WebApi.Test.Models.Query
{
    [TestClass]
    public class KeywordAttributeTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var maxLength = 10;
            var maxKeywords = 5;
            var attribute = new KeywordAttribute(maxKeywords, maxLength);
            Assert.AreEqual(maxKeywords, attribute.MaxNumberOfKeywords);
            Assert.AreEqual(maxLength, attribute.MaxKeywordLength);
        }

        [TestMethod]
        public void TestIsValid_KeywordExceedsLength()
        {
            var maxLength = 5;
            var maxKeywords = 5;
            var attribute = new KeywordAttribute(maxKeywords, maxLength);

            var searchTerm = new String('s', maxLength);
            var isValid = attribute.IsValid(new List<string> { searchTerm });
            Assert.IsTrue(isValid);

            isValid = attribute.IsValid(new List<string> { searchTerm + "s" });
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void TestIsValid_ToManyKeywords()
        {
            var maxLength = 5;
            var maxKeywords = 5;
            var attribute = new KeywordAttribute(maxKeywords, maxLength);

            var searchTerm = new String('s', maxLength);
            var searchTerms = new List<string>();
            for (var i = 0; i < maxKeywords; i++)
            {
                searchTerms.Add(searchTerm);
            }

            var isValid = attribute.IsValid(searchTerms);
            Assert.IsTrue(isValid);

            searchTerms.Add(searchTerm);
            isValid = attribute.IsValid(searchTerms);
            Assert.IsFalse(isValid);
        }


        [TestMethod]
        public void TestIsValid_NoKeywords()
        {
            var maxLength = 5;
            var maxKeywords = 5;
            var attribute = new KeywordAttribute(maxKeywords, maxLength);

            var searchTerm = new String('s', maxLength);
            var searchTerms = new List<string>();
            for (var i = 0; i < maxKeywords; i++)
            {
                searchTerms.Add(searchTerm);
            }

            var isValid = attribute.IsValid(searchTerms);
            Assert.IsTrue(isValid);

            searchTerms.Clear();
            isValid = attribute.IsValid(searchTerms);
            Assert.IsFalse(isValid);
        }
    }
}
