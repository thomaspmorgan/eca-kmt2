using ECA.Core.DynamicLinq.Filter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class SimpleKeywordFilterTestClass
    {
        public string S1 { get; set; }

        public string S2 { get; set; }
    }

    [TestClass]
    public class SimpleKeywordFilterTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var properties = new HashSet<string> { "s1", "s2" };
            var keywords = new HashSet<string> { "hello", "world" };
            var filter = new SimpleKeywordFilter(properties, keywords);

            CollectionAssert.AreEqual(properties.ToList(), filter.Properties.ToList());
            CollectionAssert.AreEqual(keywords.ToList(), filter.Keywords.ToList());
        }

        [TestMethod]
        public void TestTypedConstructor()
        {
            var keywords = new HashSet<string> { "hello", "world" };
            var filter = new SimpleKeywordFilter<SimpleKeywordFilterTestClass>(keywords, x => x.S1, x => x.S2);

            Assert.IsTrue(filter.Properties.Contains("S1"));
            Assert.IsTrue(filter.Properties.Contains("S2"));
            CollectionAssert.AreEqual(keywords.ToList(), filter.Keywords.ToList());
        }

        [TestMethod]
        public void TestToLinqFilter()
        {
            var properties = new HashSet<string> { "s1", "s2" };
            var keywords = new HashSet<string> { "hello", "world" };
            var filter = new SimpleKeywordFilter(properties, keywords);

            var linqFilter = filter.ToLinqFilter<SimpleKeywordFilterTestClass>();
            Assert.IsInstanceOfType(linqFilter, typeof(KeywordFilter<SimpleKeywordFilterTestClass>));
            Assert.IsNotNull(linqFilter.ToWhereExpression());
        }
    }
}
