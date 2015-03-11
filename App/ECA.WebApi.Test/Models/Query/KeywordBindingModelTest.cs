using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Query;
using ECA.Core.DynamicLinq.Filter;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace ECA.WebApi.Test.Models.Query
{
    public class KeywordBindingModelTestClass
    {
        public string A { get; set; }

        public string B { get; set; }

        public string C { get; set; }

        public string D { get; set; }

        public string E { get; set; }

        public string F { get; set; }

        public string G { get; set; }

        public string H { get; set; }

        public string I { get; set; }

        public string J { get; set; }

        public string K { get; set; }
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
            model.SetPropertiesToFilter(x => x.A);
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
            Assert.AreEqual("A", keywordFilter.Properties.First());
        }

        [TestMethod]
        public void TestGetFilters_MulipleProperties()
        {
            var keyword = "hello";
            var model = new KeywordBindingModel<KeywordBindingModelTestClass>();
            model.SetPropertiesToFilter(x => x.A, x => x.B);
            model.Keyword.Add(keyword);

            var filters = model.GetFilters();
            Assert.IsNotNull(filters);
            Assert.AreEqual(1, filters.Count);
            var firstFilter = filters.First();

            Assert.IsInstanceOfType(firstFilter, typeof(SimpleKeywordFilter));
            var keywordFilter = (SimpleKeywordFilter)firstFilter;

            Assert.AreEqual(2, keywordFilter.Properties.Count);
            Assert.AreEqual("A", keywordFilter.Properties.First());
            Assert.AreEqual("B", keywordFilter.Properties.Last());
        }

        [TestMethod]
        public void TestGetFilters_MultipleKeywords()
        {
            var keyword1 = "hello";
            var keyword2 = "world";
            var model = new KeywordBindingModel<KeywordBindingModelTestClass>();
            model.SetPropertiesToFilter(x => x.A);
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSetPropertiesToFilter_NoProperties()
        {
            var model = new KeywordBindingModel<KeywordBindingModelTestClass>();
            model.SetPropertiesToFilter(new Expression<Func<KeywordBindingModelTestClass, string>>[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetPropertiesToFilter_NullProperties()
        {
            var model = new KeywordBindingModel<KeywordBindingModelTestClass>();
            model.SetPropertiesToFilter(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSetPropertiesToFilter_ToManyProperties()
        {
            var model = new KeywordBindingModel<KeywordBindingModelTestClass>();
            model.SetPropertiesToFilter(
                x => x.A, 
                x => x.B, 
                x => x.C, 
                x => x.D,
                x => x.E,
                x => x.F,
                x => x.G,
                x => x.H,
                x => x.I,
                x => x.J,
                x => x.K
                );
        }

        [TestMethod]
        public void TestSetPropertiesToFilter_DistinctProperties()
        {
            var model = new KeywordBindingModel<KeywordBindingModelTestClass>();
            model.SetPropertiesToFilter(
                x => x.A,
                x => x.A
                );
            var filters = model.GetFilters();
            Assert.AreEqual(1, filters.Count);

            var simpleKeywordFilter = filters.First() as SimpleKeywordFilter;
            Assert.IsNotNull(simpleKeywordFilter);
            Assert.AreEqual(1, simpleKeywordFilter.Properties.Count);
        }
    }
}
