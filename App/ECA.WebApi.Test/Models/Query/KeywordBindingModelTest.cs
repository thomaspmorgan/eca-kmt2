using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Query;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.WebApi.Test.Models.Query
{
    public class KeywordBindingModelTestClass
    {
        public string S { get; set; }

        public string X { get; set; }
    }

    [TestClass]
    public class KeywordBindingModelTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var model = new KeywordBindingModel<KeywordBindingModelTestClass>();
            Assert.IsNotNull(model.Keyword);
        }

        [TestMethod]
        public void TestString_HasNoKeywords()
        {
            var model = new KeywordBindingModel<KeywordBindingModelTestClass>();
            Assert.IsNotNull(model.ToString());
        }

        [TestMethod]
        public void TestGetFilters_OneKeyword()
        {
            var keyword = "hello";
            var model = new KeywordBindingModel<KeywordBindingModelTestClass>();
            model.SetPropertiesToFilter(x => x.S);
            model.Keyword.Add(keyword);

            var filters = model.GetFilters();
            Assert.IsNotNull(filters);
            Assert.AreEqual(1, filters.Count);
            var firstFilter = filters.First();

            Assert.IsInstanceOfType(firstFilter, typeof(SimpleKeywordFilter));
            var keywordFilter = (SimpleKeywordFilter)firstFilter;

            Assert.AreEqual(1, keywordFilter.Keywords.Count);
            Assert.AreEqual(keyword, keywordFilter.Keywords.First());

            Assert.AreEqual(1, keywordFilter.Properties.Count);
            Assert.AreEqual("S", keywordFilter.Properties.First());
        }

        [TestMethod]
        public void TestGetFilters_MulipleProperties()
        {
            var keyword = "hello";
            var model = new KeywordBindingModel<KeywordBindingModelTestClass>();
            model.SetPropertiesToFilter(x => x.S, x => x.X);
            model.Keyword.Add(keyword);

            var filters = model.GetFilters();
            Assert.IsNotNull(filters);
            Assert.AreEqual(1, filters.Count);
            var firstFilter = filters.First();

            Assert.IsInstanceOfType(firstFilter, typeof(SimpleKeywordFilter));
            var keywordFilter = (SimpleKeywordFilter)firstFilter;

            Assert.AreEqual(2, keywordFilter.Properties.Count);
            Assert.AreEqual("S", keywordFilter.Properties.First());
            Assert.AreEqual("X", keywordFilter.Properties.Last());
        }

        [TestMethod]
        public void TestGetFilters_MultipleKeywords()
        {
            var keyword1 = "hello";
            var keyword2 = "hello";
            var model = new KeywordBindingModel<KeywordBindingModelTestClass>();
            model.SetPropertiesToFilter(x => x.S);
            model.Keyword.Add(keyword1);
            model.Keyword.Add(keyword2);

            var filters = model.GetFilters();
            Assert.IsNotNull(filters);
            Assert.AreEqual(1, filters.Count);
            var firstFilter = filters.First();

            Assert.IsInstanceOfType(firstFilter, typeof(SimpleKeywordFilter));
            var keywordFilter = (SimpleKeywordFilter)firstFilter;

            Assert.AreEqual(2, keywordFilter.Keywords.Count);
            Assert.AreEqual(keyword1, keywordFilter.Keywords.First());
            Assert.AreEqual(keyword2, keywordFilter.Keywords.Last());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestGetFilters_PropertiesToFilterNotSet()
        {
            var model = new KeywordBindingModel<KeywordBindingModelTestClass>();
            model.GetFilters();

        }
    }
}
