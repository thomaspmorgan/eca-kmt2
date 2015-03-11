using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class KeywordFilterTestClass
    {
        public string S1 { get; set; }

        public string S2 { get; set; }
    }

    [TestClass]
    public class KeywordFilterTest
    {
        [TestMethod]
        public void TestToWhereExpression()
        {
            var properties = new HashSet<string> { "S1", "S2" };
            var keywords = new HashSet<string> { "A", "B" };

            var set = new HashSet<KeywordFilterTestClass>();
            set.Add(new KeywordFilterTestClass
            {
                S1 = "A",
                S2 = "hello world"
            });

            var expectedList = set;

            var filter = new KeywordFilter<KeywordFilterTestClass>(properties, keywords);
            var where = filter.ToWhereExpression();

            var results = set.Where(where.Compile()).ToList();
            CollectionAssert.AreEqual(expectedList.ToList(), results);

        }

        [TestMethod]
        public void TestToWhereExpression_NullPropertyValues()
        {
            var properties = new HashSet<string> { "S1", "S2" };
            var keywords = new HashSet<string> { "A", "B" };

            var list = new List<KeywordFilterTestClass>();
            list.Add(new KeywordFilterTestClass
            {
                S1 = null,
                S2 = null
            });

            var filter = new KeywordFilter<KeywordFilterTestClass>(properties, keywords);
            var where = filter.ToWhereExpression();

            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);

        }

        [TestMethod]
        public void TestToWhereExpression_CaseInsensitivity()
        {
            var properties = new HashSet<string> { "S1", "S2" };
            var keywords = new HashSet<string> { "A", "b" };

            var list = new List<KeywordFilterTestClass>();
            list.Add(new KeywordFilterTestClass
            {
                S1 = "a world",
                S2 = null
            });

            list.Add(new KeywordFilterTestClass
            {
                S1 = null,
                S2 = "B hello"
            });

            var filter = new KeywordFilter<KeywordFilterTestClass>(properties, keywords);
            var where = filter.ToWhereExpression();

            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(2, results.Count);

        }

        [TestMethod]
        public void TestToWhereExpression_NoStringMatches()
        {
            var properties = new HashSet<string> { "S1", "S2" };
            var keywords = new HashSet<string> { "ABC", "XYZ" };

            var list = new List<KeywordFilterTestClass>();
            list.Add(new KeywordFilterTestClass
            {
                S1 = "hello",
                S2 = null
            });

            list.Add(new KeywordFilterTestClass
            {
                S1 = null,
                S2 = "world"
            });

            var filter = new KeywordFilter<KeywordFilterTestClass>(properties, keywords);
            var where = filter.ToWhereExpression();

            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_NoKeywords()
        {
            var properties = new HashSet<string> { "S1", "S2" };
            var keywords = new HashSet<string>();

            var list = new List<KeywordFilterTestClass>();
            list.Add(new KeywordFilterTestClass
            {
                S1 = "hello",
                S2 = "world"
            });


            var filter = new KeywordFilter<KeywordFilterTestClass>(properties, keywords);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestToWhereExpression_NoProperties()
        {
            var properties = new HashSet<string>();
            var keywords = new HashSet<string>();

            var list = new List<KeywordFilterTestClass>();
            list.Add(new KeywordFilterTestClass
            {
                S1 = "hello",
                S2 = "world"
            });
            var filter = new KeywordFilter<KeywordFilterTestClass>(properties, keywords);
        }
    }
}
