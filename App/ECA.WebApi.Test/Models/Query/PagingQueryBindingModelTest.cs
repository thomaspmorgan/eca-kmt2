using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ECA.WebApi.Test.Models.Query
{
    public class PagingQueryBindingModelTestClass
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

        public int Id { get; set; }
    }

    [TestClass]
    public class PagingQueryBindingModelTest
    {
        #region Constructor
        [TestMethod]
        public void TestConstructor()
        {
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            Assert.IsNotNull(model.Filter);
            Assert.IsNotNull(model.Keyword);
        }
        #endregion

        #region Test To Queryable Operator Only Sorter
        [TestMethod]
        public void TestToQueryableOperator_NoFilterNoSorter_OnlyDefaultSort()
        {
            var start = 0;
            var limit = 10;
            var defaultSorter = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.A, SortDirection.Ascending);
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>
            {
                Start = start,
                Limit = limit
            };
            var queryOperator = model.ToQueryableOperator(defaultSorter);
            Assert.AreEqual(start, queryOperator.Start);
            Assert.AreEqual(limit, queryOperator.Limit);

            var testDefaultSorter = queryOperator.DefaultSorter;
            Assert.IsInstanceOfType(testDefaultSorter, typeof(SimpleSorter));
            var testDefaultSimpleSorter = (SimpleSorter)testDefaultSorter;
            Assert.AreEqual(defaultSorter.Direction, testDefaultSimpleSorter.Direction);
            Assert.AreEqual(defaultSorter.Property, testDefaultSimpleSorter.Property);
        }

        [TestMethod]
        public void TestToQueryableOperator_HasSorter()
        {
            var start = 0;
            var limit = 10;
            var defaultSorter = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.A, SortDirection.Ascending);
            var sorter = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.A, SortDirection.Ascending);
            var sorters = new List<SorterBindingModel>{new SorterBindingModel
            {
                Property = sorter.Property,
                Direction = sorter.Direction
            }};
            var jsonStrings = new List<string>();
            jsonStrings.AddRange(sorters.Select(x => JsonConvert.SerializeObject(x)).ToList());
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>
            {
                Start = start,
                Limit = limit,
                Sort = jsonStrings
            };
            var queryOperator = model.ToQueryableOperator(defaultSorter);
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

        [TestMethod]
        public void TestToQueryableOperator_HasSorterAndFilters()
        {
            var start = 0;
            var limit = 10;
            var defaultSorter = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.A, SortDirection.Ascending);
            var sorter = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.A, SortDirection.Ascending);
            var sorters = new List<SorterBindingModel>{new SorterBindingModel
            {
                Property = sorter.Property,
                Direction = sorter.Direction
            }};
            var jsonStrings = new List<string>();
            jsonStrings.AddRange(sorters.Select(x => JsonConvert.SerializeObject(x)).ToList());

            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>
            {
                Start = start,
                Limit = limit,
                Sort = jsonStrings
            };

            var keyword = "hello";
            model.SetPropertiesToSearch(x => x.A);
            model.Keyword.Add(keyword);

            var expressionFilter = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.A, ComparisonType.Equal, "S");
            var json = JsonConvert.SerializeObject(expressionFilter);
            model.Filter = new List<string> { json };

            var queryOperator = model.ToQueryableOperator(defaultSorter);
            Assert.AreEqual(start, queryOperator.Start);
            Assert.AreEqual(limit, queryOperator.Limit);
            Assert.AreEqual(1, queryOperator.Sorters.Count);
            Assert.AreEqual(2, queryOperator.Filters.Count);
        }

        #endregion

        #region Get Keyword Filter
        [TestMethod]
        public void TestGetKeywordFilter_OneKeyword()
        {
            var keyword = "hello";
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.SetPropertiesToSearch(x => x.A);
            model.Keyword.Add(keyword);

            var filter = model.GetKeywordFilter();
            Assert.IsNotNull(filter);

            Assert.IsInstanceOfType(filter, typeof(SimpleKeywordFilter));
            var keywordFilter = (SimpleKeywordFilter)filter;

            Assert.AreEqual(1, keywordFilter.Keywords.Count);
            Assert.AreEqual(keyword, keywordFilter.Keywords.First());

            Assert.AreEqual(1, keywordFilter.Properties.Count);
            Assert.AreEqual("A", keywordFilter.Properties.First());
        }

        [TestMethod]
        public void TestGetKeywordFilter_MulipleProperties()
        {
            var keyword = "hello";
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.SetPropertiesToSearch(x => x.A, x => x.B);
            model.Keyword.Add(keyword);

            var filter = model.GetKeywordFilter();
            Assert.IsNotNull(filter);

            Assert.IsInstanceOfType(filter, typeof(SimpleKeywordFilter));
            var keywordFilter = (SimpleKeywordFilter)filter;

            Assert.AreEqual(2, keywordFilter.Properties.Count);
            Assert.AreEqual("A", keywordFilter.Properties.First());
            Assert.AreEqual("B", keywordFilter.Properties.Last());
        }

        [TestMethod]
        public void TestGetKeywordFilter_MultipleKeywords()
        {
            var keyword1 = "hello";
            var keyword2 = "world";
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.SetPropertiesToSearch(x => x.A);
            model.Keyword.Add(keyword1);
            model.Keyword.Add(keyword2);

            var filter = model.GetKeywordFilter();
            Assert.IsNotNull(filter);

            Assert.IsInstanceOfType(filter, typeof(SimpleKeywordFilter));
            var keywordFilter = (SimpleKeywordFilter)filter;

            Assert.AreEqual(2, keywordFilter.Keywords.Count);
            Assert.AreEqual(keyword1, keywordFilter.Keywords.First());
            Assert.AreEqual(keyword2, keywordFilter.Keywords.Last());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestGetKeywordFilter_PropertiesToFilterNotSet()
        {
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.GetKeywordFilter();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSetPropertiesToSearch_NoProperties()
        {
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.SetPropertiesToSearch(new Expression<Func<PagingQueryBindingModelTestClass, string>>[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetPropertiesToSearch_NullProperties()
        {
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.SetPropertiesToSearch(null);
        }

        #endregion

        #region Set Properties To Search
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSetPropertiesToSearch_ToManyProperties()
        {
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.SetPropertiesToSearch(
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
        public void TestSetPropertiesToSearch_DistinctProperties()
        {
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.SetPropertiesToSearch(
                x => x.A,
                x => x.A
                );
            var filter = model.GetKeywordFilter();
            var simpleKeywordFilter = filter as SimpleKeywordFilter;
            Assert.IsNotNull(simpleKeywordFilter);
            Assert.AreEqual(1, simpleKeywordFilter.Properties.Count);
        }
        #endregion

        #region Parse Filters
        [TestMethod]
        public void TestParseFilters_OneFilter()
        {
            var expressionFilter = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.A, ComparisonType.Equal, "S");

            var json = JsonConvert.SerializeObject(expressionFilter);
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.Filter = new List<string> { json };

            var parsedFilters = model.ParseFilters(model.Filter);
            Assert.AreEqual(1, parsedFilters.Count);
            var firstParsedFilter = parsedFilters.First();
            Assert.AreEqual(expressionFilter.Comparison, firstParsedFilter.Comparison);
            Assert.AreEqual(expressionFilter.Value, firstParsedFilter.Value);
            Assert.AreEqual(expressionFilter.Property, firstParsedFilter.Property);
        }

        [TestMethod]
        public void TestParseFilters_MultipleFilters()
        {
            var expressionFilter1 = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.A, ComparisonType.Equal, "S");

            var longValue = 1L;
            var expressionFilter2 = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.Id, ComparisonType.Equal, longValue);

            var json1 = JsonConvert.SerializeObject(expressionFilter1);
            var json2 = JsonConvert.SerializeObject(expressionFilter2);
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
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

        #endregion

        #region Get Filters
        [TestMethod]
        public void TestGetFilters_KeywordAndFilterNull()
        {
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.Filter = null;
            model.Keyword = null;
            Assert.IsNotNull(model.GetFilters());
        }

        [TestMethod]
        public void TestGetFilters_NoKeywordAndNoFilter()
        {
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.Filter = new List<string>();
            model.Keyword = new List<string>();
            Assert.IsNotNull(model.GetFilters());
            Assert.AreEqual(0, model.GetFilters().Count);
        }

        [TestMethod]
        public void TestGetFilters_HasFilter()
        {   
            var expressionFilter = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.A, ComparisonType.Equal, "S");
            var json = JsonConvert.SerializeObject(expressionFilter);
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.Filter = new List<string> { json };

            var filters = model.GetFilters();
            Assert.AreEqual(1, filters.Count);
            var firstFilter = filters.First();
            Assert.IsInstanceOfType(firstFilter, typeof(SimpleFilter));

            var firstSimpleFilter = (SimpleFilter)firstFilter;
            Assert.AreEqual(expressionFilter.Comparison, firstSimpleFilter.Comparison);
            Assert.AreEqual(expressionFilter.Value, firstSimpleFilter.Value);
            Assert.AreEqual(expressionFilter.Property, firstSimpleFilter.Property);
        }

        [TestMethod]
        public void TestGetFilters_HasKeyword()
        {
            var keyword = "hello";
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.SetPropertiesToSearch(x => x.A);
            model.Keyword.Add(keyword);

            var filters = model.GetFilters();
            Assert.AreEqual(1, filters.Count);
            var firstFilter = filters.First();
            Assert.IsInstanceOfType(firstFilter, typeof(SimpleKeywordFilter));

            var firstKeywordFilter = (SimpleKeywordFilter)firstFilter;
            Assert.AreEqual(1, firstKeywordFilter.Keywords.Count);
            Assert.AreEqual(1, firstKeywordFilter.Properties.Count);

            Assert.AreEqual(keyword, firstKeywordFilter.Keywords.First());
            Assert.AreEqual("A", firstKeywordFilter.Properties.First());
        }

        [TestMethod]
        public void TestGetFilters_HasKeywordAndFilter()
        {
            var keyword = "hello";
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.SetPropertiesToSearch(x => x.A);
            model.Keyword.Add(keyword);

            var expressionFilter = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.A, ComparisonType.Equal, "S");
            var json = JsonConvert.SerializeObject(expressionFilter);
            model.Filter = new List<string> { json };

            var filters = model.GetFilters();
            Assert.AreEqual(2, filters.Count);
        }
        #endregion

        #region ToString
        [TestMethod]
        public void TestToString_OnlyStartAndLimitInitialized()
        {
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.Start = 0;
            model.Limit = 10;
            Assert.IsNotNull(model.ToString());
        }

        [TestMethod]
        public void TestToString_SorterStrings()
        {
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.Start = 0;
            model.Limit = 10;
            model.Sort = new List<string> { "sort1" };
            Assert.IsNotNull(model.ToString());
        }


        [TestMethod]
        public void TestToString_HasFiltersHasSorters()
        {
            var model = new PagingQueryBindingModel<PagingQueryBindingModelTestClass>();
            model.Start = 0;
            model.Limit = 10;

            var expressionFilter = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.A, ComparisonType.Equal, "A");
            var filterJson = JsonConvert.SerializeObject(expressionFilter);
            model.Filter = new List<string> { filterJson };

            var expressionSort = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.A, SortDirection.Ascending);
            var sortJson = JsonConvert.SerializeObject(expressionSort);
            model.Sort = new List<string> { sortJson };

            Assert.IsNotNull(model.ToString());
        }
        #endregion
    }

}
