using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ECA.WebApi.Test.Models.Query
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
            var filters = new List<FilterBindingModel>{new FilterBindingModel
            {
                Comparison = filter.Comparison,
                Property = filter.Property,
                Value = filter.Value
            }};
            var jsonStrings = new List<string>();
            jsonStrings.AddRange(filters.Select(x => JsonConvert.SerializeObject(x)).ToList());
            var model = new PagingQueryBindingModel
            {
                Start = start,
                Limit = limit,
                Filter = jsonStrings
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
            var sorters = new List<SorterBindingModel>{new SorterBindingModel
            {
                Property = sorter.Property,
                Direction = sorter.Direction
            }};
            var jsonStrings = new List<string>();
            jsonStrings.AddRange(sorters.Select(x => JsonConvert.SerializeObject(x)).ToList());
            var model = new PagingQueryBindingModel
            {
                Start = start,
                Limit = limit,
                Sort = jsonStrings
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
    }

}
