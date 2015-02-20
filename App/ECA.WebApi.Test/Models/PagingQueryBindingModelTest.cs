using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq;
using System.Collections.Generic;
using Newtonsoft.Json;
using ECA.Core.DynamicLinq.Sorter;

namespace ECA.WebApi.Test.Models
{
    public class PagingQueryBindingModelTestClass
    {
        public string S { get; set; }
    }

    [TestClass]
    public class PagingQueryBindingModelTest
    {

        #region Test To Queryable Operator
        [TestMethod]
        public void TestToQueryableOperator_NoFilterNoSorter_OnlyDefaultSort()
        {
            var start = 0;
            var limit = 10;
            var defaultSorter = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.S, SortDirection.Ascending);
            var model = new PagingQueryBindingModel
            {
                Start = start,
                Limit = limit
            };
            var queryOperator = model.ToQueryableOperator<PagingQueryBindingModelTestClass>(defaultSorter);
            Assert.AreEqual(start, queryOperator.Start);
            Assert.AreEqual(limit, queryOperator.Limit);

            var testDefaultSorter = queryOperator.DefaultSorter;
            Assert.IsInstanceOfType(testDefaultSorter, typeof(SimpleSorter));
            var testDefaultSimpleSorter = (SimpleSorter)testDefaultSorter;
            Assert.AreEqual(defaultSorter.Direction, testDefaultSimpleSorter.Direction);
            Assert.AreEqual(defaultSorter.Property, testDefaultSimpleSorter.Property);
        }

        [TestMethod]
        public void TestToQueryableOperator_HasFilter()
        {
            var start = 0;
            var limit = 10;
            var defaultSorter = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.S, SortDirection.Ascending);
            var filter = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.S, ComparisonType.Equal, "s");
            var filterJson = JsonConvert.SerializeObject(new List<SimpleFilter> { filter });
            var model = new PagingQueryBindingModel
            {
                Start = start,
                Limit = limit,
                Filter = filterJson
            };
            var queryOperator = model.ToQueryableOperator<PagingQueryBindingModelTestClass>(defaultSorter);
            Assert.AreEqual(start, queryOperator.Start);
            Assert.AreEqual(limit, queryOperator.Limit);
            Assert.AreEqual(1, queryOperator.Filters.Count);
            Assert.AreEqual(0, queryOperator.Sorters.Count);

            var testFilter = queryOperator.Filters.First();
            Assert.IsInstanceOfType(testFilter, typeof(SimpleFilter));
            var simpleTestFilter = (SimpleFilter)testFilter;
            Assert.AreEqual(filter.Comparison, simpleTestFilter.Comparison);
            Assert.AreEqual(filter.Property, simpleTestFilter.Property);
            Assert.AreEqual(filter.Value, simpleTestFilter.Value);
        }

        [TestMethod]
        public void TestToQueryableOperator_HasSorter()
        {
            var start = 0;
            var limit = 10;
            var defaultSorter = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.S, SortDirection.Ascending);
            var sorter = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.S, SortDirection.Ascending);
            var sorterJson = JsonConvert.SerializeObject(new List<SimpleSorter> { sorter });
            var model = new PagingQueryBindingModel
            {
                Start = start,
                Limit = limit,
                Sort = sorterJson
            };
            var queryOperator = model.ToQueryableOperator<PagingQueryBindingModelTestClass>(defaultSorter);
            Assert.AreEqual(start, queryOperator.Start);
            Assert.AreEqual(limit, queryOperator.Limit);
            Assert.AreEqual(1, queryOperator.Sorters.Count);
            Assert.AreEqual(0, queryOperator.Filters.Count);

            var testFilter = queryOperator.Sorters.First();
            Assert.IsInstanceOfType(testFilter, typeof(SimpleSorter));
            var simpleTestFilter = (SimpleSorter)testFilter;
            Assert.AreEqual(sorter.Property, simpleTestFilter.Property);
            Assert.AreEqual(sorter.Direction, simpleTestFilter.Direction);
        }

        #endregion

        #region Parse simple filters
        [TestMethod]
        public void TestParseAsSimpleFilters_NullString()
        {
            var model = new PagingQueryBindingModel();
            var testFilters = model.ParseAsSimpleFilters(null);
            Assert.AreEqual(0, testFilters.Count);
        }

        [TestMethod]
        public void TestParseAsSimpleFilters_HasOneFilter()
        {
            var filter = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.S, ComparisonType.Equal, "S");
            var filters = new List<IFilter>();
            filters.Add(filter);
            var json = JsonConvert.SerializeObject(filters);

            var model = new PagingQueryBindingModel();
            var testFilters = model.ParseAsSimpleFilters(json);
            Assert.AreEqual(1, testFilters.Count);
            var firstTestFilter = testFilters.First();

            Assert.IsInstanceOfType(firstTestFilter, typeof(SimpleFilter));
            var firstSimpleTestFilter = (SimpleFilter)firstTestFilter;
            Assert.AreEqual(filter.Comparison, firstSimpleTestFilter.Comparison);
            Assert.AreEqual(filter.Property, firstSimpleTestFilter.Property);
            Assert.AreEqual(filter.Value, firstSimpleTestFilter.Value);
            
        }

        [TestMethod]
        public void TestParseAsSimpleFilters_HasMultipleFilters()
        {
            var filter1 = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.S, ComparisonType.Equal, "S");
            var filter2 = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.S, ComparisonType.Equal, "S");
            var filters = new List<IFilter>();
            filters.Add(filter1);
            filters.Add(filter2);
            var json = JsonConvert.SerializeObject(filters);

            var model = new PagingQueryBindingModel();
            var testFilters = model.ParseAsSimpleFilters(json);
            Assert.AreEqual(2, testFilters.Count);

        }

        #endregion

        #region Parse simple sorters
        [TestMethod]
        public void TestParseAsSimpleSorters_NullString()
        {
            var model = new PagingQueryBindingModel();
            var testSorters = model.ParseAsSimpleSorters(null);
            Assert.AreEqual(0, testSorters.Count);
        }

        [TestMethod]
        public void TestParseAsSimpleSorters_HasOneSorter()
        {
            var sorter = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.S, SortDirection.Ascending);
            var sorters = new List<ISorter>();
            sorters.Add(sorter);
            var json = JsonConvert.SerializeObject(sorters);

            var model = new PagingQueryBindingModel();
            var testSorters = model.ParseAsSimpleSorters(json);
            Assert.AreEqual(1, testSorters.Count);
            var firstTestFilter = testSorters.First();

            Assert.IsInstanceOfType(firstTestFilter, typeof(SimpleSorter));
            var firstSimpleTestSorter = (SimpleSorter)firstTestFilter;
            Assert.AreEqual(sorter.Direction, firstSimpleTestSorter.Direction);
            Assert.AreEqual(sorter.Property, firstSimpleTestSorter.Property);
        }

        [TestMethod]
        public void TestParseAsSimpleSorters_HasMultipleSorters()
        {
            var sorter1 = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.S, SortDirection.Ascending);
            var sorter2 = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.S, SortDirection.Ascending);
            var sorters = new List<ISorter>();
            sorters.Add(sorter1);
            sorters.Add(sorter2);
            var json = JsonConvert.SerializeObject(sorters);

            var model = new PagingQueryBindingModel();
            var testSorters = model.ParseAsSimpleSorters(json);
            Assert.AreEqual(2, testSorters.Count);
            var firstTestFilter = testSorters.First();
        }
        #endregion

    }
}
