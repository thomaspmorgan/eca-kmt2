using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Query;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq;
using System.Collections.Generic;
using Newtonsoft.Json;
using ECA.Core.DynamicLinq.Sorter;

namespace ECA.WebApi.Test.Models.Query
{
    public class MultipleFilterBindingModelTestClass
    {
        public string S { get; set; }

        public int Id { get; set; }
    }

    [TestClass]
    public class MultipleFilterBindingModelTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var model = new MultipleFilterBindingModel();
            Assert.IsNotNull(model.Filter);
        }

        [TestMethod]
        public void TestGetFilters_MulitpleFilters()
        {
            var expressionFilter1 = new ExpressionFilter<MultipleFilterBindingModelTestClass>(x => x.S, ComparisonType.Equal, "S");

            var longValue = 1L;
            var expressionFilter2 = new ExpressionFilter<MultipleFilterBindingModelTestClass>(x => x.Id, ComparisonType.Equal, longValue);

            var json1 = JsonConvert.SerializeObject(expressionFilter1);
            var json2 = JsonConvert.SerializeObject(expressionFilter2);
            var model = new MultipleFilterBindingModel();
            model.Filter = new List<string> { json1, json2 };

            var parsedFilters = model.GetFilters();
            Assert.AreEqual(2, parsedFilters.Count);
        }

        [TestMethod]
        public void TestGetFilters_OneFilter()
        {
            var expressionFilter = new ExpressionFilter<MultipleFilterBindingModelTestClass>(x => x.S, ComparisonType.Equal, "S");

            var json = JsonConvert.SerializeObject(expressionFilter);
            var model = new MultipleFilterBindingModel();
            model.Filter = new List<string> { json };

            var parsedFilters = model.GetFilters();
            Assert.AreEqual(1, parsedFilters.Count);
            var firstParsedFilter = parsedFilters.First();
            Assert.IsInstanceOfType(firstParsedFilter, typeof(SimpleFilter));

            var equalFilter = (SimpleFilter)firstParsedFilter;
            Assert.AreEqual(expressionFilter.Comparison, equalFilter.Comparison);
            Assert.AreEqual(expressionFilter.Value, equalFilter.Value);
            Assert.AreEqual(expressionFilter.Property, equalFilter.Property);
        }

        [TestMethod]
        public void TestParseFilters_OneFilter()
        {
            var expressionFilter = new ExpressionFilter<MultipleFilterBindingModelTestClass>(x => x.S, ComparisonType.Equal, "S");

            var json = JsonConvert.SerializeObject(expressionFilter);
            var model = new MultipleFilterBindingModel();
            model.Filter = new List<string> { json };

            var parsedFilters = model.ParseFilters(model.Filter);
            Assert.AreEqual(1, parsedFilters.Count);
            var firstParsedFilter = parsedFilters.First();
            Assert.AreEqual(expressionFilter.Comparison, firstParsedFilter.Comparison);
            Assert.AreEqual(expressionFilter.Value, firstParsedFilter.Value);
            Assert.AreEqual(expressionFilter.Property, firstParsedFilter.Property);
        }

        [TestMethod]
        public void TestParseFilters_MulitpleFilters()
        {
            var expressionFilter1 = new ExpressionFilter<MultipleFilterBindingModelTestClass>(x => x.S, ComparisonType.Equal, "S");

            var longValue = 1L;
            var expressionFilter2 = new ExpressionFilter<MultipleFilterBindingModelTestClass>(x => x.Id, ComparisonType.Equal, longValue);

            var json1 = JsonConvert.SerializeObject(expressionFilter1);
            var json2 = JsonConvert.SerializeObject(expressionFilter2);
            var model = new MultipleFilterBindingModel();
            model.Filter = new List<string> { json1, json2 };

            var parsedFilters = model.ParseFilters(model.Filter);
            Assert.AreEqual(2, parsedFilters.Count);
            var firstParsedFilter = parsedFilters.First();
            Assert.AreEqual(expressionFilter1.Comparison, firstParsedFilter.Comparison);
            Assert.AreEqual(expressionFilter1.Value, firstParsedFilter.Value);
            Assert.AreEqual(expressionFilter1.Property, firstParsedFilter.Property);

            var lastParsedFilter = parsedFilters.Last();
            Assert.AreEqual(expressionFilter2.Comparison, lastParsedFilter.Comparison);
            Assert.AreEqual(expressionFilter2.Value, lastParsedFilter.Value);
            Assert.AreEqual(expressionFilter2.Property, lastParsedFilter.Property);
        }

        [TestMethod]
        public void TestToString_NullFilter_NullSort()
        {
            var model = new MultipleFilterBindingModel();
            model.Start = 0;
            model.Limit = 10;
            Assert.IsNotNull(model.ToString());
        }

        [TestMethod]
        public void TestToString_HasFiltersHasSorters()
        {
            var model = new MultipleFilterBindingModel();
            model.Start = 0;
            model.Limit = 10;

            var expressionFilter = new ExpressionFilter<MultipleFilterBindingModelTestClass>(x => x.S, ComparisonType.Equal, "S");
            var filterJson = JsonConvert.SerializeObject(expressionFilter);
            model.Filter = new List<string> { filterJson };

            var expressionSort = new ExpressionSorter<MultipleFilterBindingModelTestClass>(x => x.S, SortDirection.Ascending);
            var sortJson = JsonConvert.SerializeObject(expressionSort);
            model.Sort = new List<string> { sortJson };

            Assert.IsNotNull(model.ToString());
        }
    }
}
