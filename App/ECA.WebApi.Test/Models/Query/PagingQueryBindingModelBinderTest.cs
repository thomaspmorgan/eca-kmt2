using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http.Controllers;
using System.Net.Http;
using ECA.WebApi.Models.Query;
using System.Web.Http.ModelBinding;
using System.Web.Http.Metadata.Providers;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.ModelBinding;
using ECA.Core.DynamicLinq.Filter;
using System.Collections.Generic;
using ECA.Core.DynamicLinq;
using Newtonsoft.Json;
using ECA.Core.DynamicLinq.Sorter;
using Newtonsoft.Json.Linq;
using Moq;

namespace ECA.WebApi.Test.Models.Query
{
    [TestClass]
    public class PagingQueryBindingModelBinderTest
    {

        #region Parse simple filters
        [TestMethod]
        public void TestParseFilters_NullString()
        {
            var binder = new PagingQueryBindingModelBinder();
            var testFilters = binder.ParseFilters(null);
            Assert.AreEqual(0, testFilters.Count);
        }

        [TestMethod]
        public void TestParseFilters_HasOneFilter()
        {
            var filter = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.S, ComparisonType.Equal, "S");
            var filters = new List<IFilter>();
            filters.Add(filter);
            var json = JsonConvert.SerializeObject(filters);

            var binder = new PagingQueryBindingModelBinder();
            var testFilters = binder.ParseFilters(json);
            Assert.AreEqual(1, testFilters.Count);
            var firstTestFilter = testFilters.First();

            Assert.AreEqual(filter.Comparison, firstTestFilter.Comparison);
            Assert.AreEqual(filter.Property, firstTestFilter.Property);
            Assert.AreEqual(filter.Value, firstTestFilter.Value);

        }

        [TestMethod]
        public void TestParseFilters_HasMultipleFilters()
        {
            var filter1 = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.S, ComparisonType.Equal, "S");
            var filter2 = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.S, ComparisonType.Equal, "S");
            var filters = new List<IFilter>();
            filters.Add(filter1);
            filters.Add(filter2);
            var json = JsonConvert.SerializeObject(filters);

            var binder = new PagingQueryBindingModelBinder();
            var testFilters = binder.ParseFilters(json);
            Assert.AreEqual(2, testFilters.Count);
        }

        [TestMethod]
        public void TestParseFilters_JArrayOfStrings()
        {
            var s1 = "A";
            var s2 = "B";
            var s3 = "C";
            var stringList = new List<string> { s1, s2, s3 };

            var filter1 = new ExpressionFilter<PagingQueryBindingModelTestClass>(x => x.S, ComparisonType.In, stringList.ToArray());
            var filters = new List<IFilter>();
            filters.Add(filter1);
            var json = JsonConvert.SerializeObject(filters);
            var binder = new PagingQueryBindingModelBinder();
            var testFilters = binder.ParseFilters(json);

            Assert.AreEqual(1, testFilters.Count);
            var firstFilter = testFilters.First();
            Assert.IsInstanceOfType(firstFilter.Value, typeof(List<string>));
            var testStringFilter = (List<string>)firstFilter.Value;
            Assert.IsTrue(testStringFilter.Contains(s1));
            Assert.IsTrue(testStringFilter.Contains(s2));
            Assert.IsTrue(testStringFilter.Contains(s3));
        }

        #endregion

        #region Parse simple sorters
        [TestMethod]
        public void TestParseAsSimpleSorters_NullString()
        {
            var binder = new PagingQueryBindingModelBinder();
            var testSorters = binder.ParseSorters(null);
            Assert.AreEqual(0, testSorters.Count);
        }

        [TestMethod]
        public void TestParseAsSimpleSorters_HasOneSorter()
        {
            var sorter = new ExpressionSorter<PagingQueryBindingModelTestClass>(x => x.S, SortDirection.Ascending);
            var sorters = new List<ISorter>();
            sorters.Add(sorter);
            var json = JsonConvert.SerializeObject(sorters);

            var binder = new PagingQueryBindingModelBinder();
            var testSorters = binder.ParseSorters(json);
            Assert.AreEqual(1, testSorters.Count);
            var firstTestSorter = testSorters.First();

            Assert.AreEqual(sorter.Direction, firstTestSorter.Direction);
            Assert.AreEqual(sorter.Property, firstTestSorter.Property);
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

            var binder = new PagingQueryBindingModelBinder();
            var testSorters = binder.ParseSorters(json);
            Assert.AreEqual(2, testSorters.Count);
            var firstTestFilter = testSorters.First();
        }
        #endregion

        #region To List
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestToList_JsonTokenTypeNotSupported()
        {
            var invalidList = new List<object>();
            invalidList.Add(new { Id = 1 });

            var jsonString = JsonConvert.SerializeObject(invalidList);
            var json = JsonConvert.DeserializeObject(jsonString);

            var model = new PagingQueryBindingModelBinder();
            var testList = model.ToList(json as JArray);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestToList_MultipleTokenTypes()
        {
            var invalidList = new List<object>();
            invalidList.Add(new { Id = 1 });
            invalidList.Add(1);

            var jsonString = JsonConvert.SerializeObject(invalidList);
            var json = JsonConvert.DeserializeObject(jsonString);

            var model = new PagingQueryBindingModelBinder();
            var testList = model.ToList(json as JArray);
        }


        [TestMethod]
        public void TestToList_ArrayOfLongs()
        {
            var intList = new List<long>();
            intList.Add(1L);
            intList.Add(2L);

            var jsonString = JsonConvert.SerializeObject(intList);
            var json = JsonConvert.DeserializeObject(jsonString);

            var model = new PagingQueryBindingModelBinder();
            var testList = model.ToList(json as JArray);
            Assert.IsInstanceOfType(testList, typeof(List<long>));
            var testIntList = (List<long>)testList;
            CollectionAssert.AreEqual(intList, testIntList);

        }

        [TestMethod]
        public void TestToList_ArrayOfIntegers()
        {
            var intList = new List<int>();
            intList.Add(1);
            intList.Add(2);

            var jsonString = JsonConvert.SerializeObject(intList);
            var json = JsonConvert.DeserializeObject(jsonString);

            var model = new PagingQueryBindingModelBinder();
            var testList = model.ToList(json as JArray);
            Assert.IsInstanceOfType(testList, typeof(List<long>));
            var testLongList = (List<long>)testList;
            CollectionAssert.AreEqual(intList.Select(x => (long)x).ToList(), testLongList);

        }

        [TestMethod]
        public void TestToList_ArrayOfStrings()
        {
            var stringList = new List<string>();
            stringList.Add("abc");
            stringList.Add("xyz");

            var jsonString = JsonConvert.SerializeObject(stringList);
            var json = JsonConvert.DeserializeObject(jsonString);

            var model = new PagingQueryBindingModelBinder();
            var testList = model.ToList(json as JArray);
            Assert.IsInstanceOfType(testList, typeof(List<string>));
            var testStringList = (List<string>)testList;
            CollectionAssert.AreEqual(stringList, testStringList);

        }

        [TestMethod]
        public void TestToList_ArrayOfDates()
        {
            var dateList = new List<DateTime>();
            dateList.Add(DateTime.UtcNow);
            dateList.Add(DateTime.UtcNow.AddDays(1.0));

            var jsonString = JsonConvert.SerializeObject(dateList);
            var json = JsonConvert.DeserializeObject(jsonString);

            var model = new PagingQueryBindingModelBinder();
            var testList = model.ToList(json as JArray);
            Assert.IsInstanceOfType(testList, typeof(List<DateTime>));
            var testDateList = (List<DateTime>)testList;
            CollectionAssert.AreEqual(dateList, testDateList);

        }

        [TestMethod]
        public void TestToList_ArrayOfBooleans()
        {
            var boolList = new List<bool>();
            boolList.Add(true);
            boolList.Add(false);

            var jsonString = JsonConvert.SerializeObject(boolList);
            var json = JsonConvert.DeserializeObject(jsonString);

            var model = new PagingQueryBindingModelBinder();
            var testList = model.ToList(json as JArray);
            Assert.IsInstanceOfType(testList, typeof(List<bool>));
            var testBoolList = (List<bool>)testList;
            CollectionAssert.AreEqual(boolList, testBoolList);

        }

        [TestMethod]
        public void TestToList_ArrayOfFloats()
        {
            var floatList = new List<float>();
            floatList.Add(1.0f);
            floatList.Add(2.0f);

            var jsonString = JsonConvert.SerializeObject(floatList);
            var json = JsonConvert.DeserializeObject(jsonString);

            var model = new PagingQueryBindingModelBinder();
            var testList = model.ToList(json as JArray);
            Assert.IsInstanceOfType(testList, typeof(List<double>));
            var testDoubleList = (List<double>)testList;
            CollectionAssert.AreEqual(floatList.Select(x => (double)x).ToList(), testDoubleList);

        }
        #endregion

        #region Bind Model
        [TestMethod]
        public void TestBindModel_MissingStart()
        {
            var limit = 10;
            var valueProvider = new SimpleHttpValueProvider
            {
                { PagingQueryBindingModelBinder.LIMIT_QUERY_KEY, limit.ToString() },
            };
            var actionContext = GetActionContext();
            var bindingContext = GetModelBindingContext(valueProvider);

            var binder = new PagingQueryBindingModelBinder();
            binder.BindModel(actionContext, bindingContext);
            Assert.AreEqual(1, bindingContext.ModelState.Count);
        }

        [TestMethod]
        public void TestBindModel_MissingLimit()
        {
            var start = 10;
            var valueProvider = new SimpleHttpValueProvider
            {
                { PagingQueryBindingModelBinder.START_QUERY_KEY, start.ToString() },
            };
            var actionContext = GetActionContext();
            var bindingContext = GetModelBindingContext(valueProvider);

            var binder = new PagingQueryBindingModelBinder();
            binder.BindModel(actionContext, bindingContext);
            Assert.AreEqual(1, bindingContext.ModelState.Count);
        }

        [TestMethod]
        public void TestBindModel_StartIsNotInt()
        {
            var start = "hello";
            var limit = 10;
            var valueProvider = new SimpleHttpValueProvider
            {
                { PagingQueryBindingModelBinder.START_QUERY_KEY, start.ToString() },
                { PagingQueryBindingModelBinder.LIMIT_QUERY_KEY, limit.ToString() },
            };
            var actionContext = GetActionContext();
            var bindingContext = GetModelBindingContext(valueProvider);

            var binder = new PagingQueryBindingModelBinder();
            binder.BindModel(actionContext, bindingContext);
            Assert.AreEqual(1, bindingContext.ModelState.Count);

        }

        [TestMethod]
        public void TestBindModel_LimitIsNotInt()
        {
            var start = 0;
            var limit = "hello";
            var valueProvider = new SimpleHttpValueProvider
            {
                { PagingQueryBindingModelBinder.START_QUERY_KEY, start.ToString() },
                { PagingQueryBindingModelBinder.LIMIT_QUERY_KEY, limit.ToString() },
            };
            var actionContext = GetActionContext();
            var bindingContext = GetModelBindingContext(valueProvider);

            var binder = new PagingQueryBindingModelBinder();
            binder.BindModel(actionContext, bindingContext);
            Assert.AreEqual(1, bindingContext.ModelState.Count);

        }

        [TestMethod]
        public void TestBindModel_LimitIsBeyondMax()
        {
            var start = 0;
            var limit = PagingQueryBindingModel.MAX_LIMIT + 1;
            var valueProvider = new SimpleHttpValueProvider
            {
                { PagingQueryBindingModelBinder.START_QUERY_KEY, start.ToString() },
                { PagingQueryBindingModelBinder.LIMIT_QUERY_KEY, limit.ToString() },
            };
            var actionContext = GetActionContext();
            var bindingContext = GetModelBindingContext(valueProvider);

            var binder = new PagingQueryBindingModelBinder();
            binder.BindModel(actionContext, bindingContext);
            Assert.AreEqual(1, bindingContext.ModelState.Count);
        }

        [TestMethod]
        public void TestBindModel_LimitLessThanZero()
        {
            var start = 0;
            var limit = -1;
            var valueProvider = new SimpleHttpValueProvider
            {
                { PagingQueryBindingModelBinder.START_QUERY_KEY, start.ToString() },
                { PagingQueryBindingModelBinder.LIMIT_QUERY_KEY, limit.ToString() },
            };
            var actionContext = GetActionContext();
            var bindingContext = GetModelBindingContext(valueProvider);

            var binder = new PagingQueryBindingModelBinder();
            binder.BindModel(actionContext, bindingContext);
            Assert.AreEqual(1, bindingContext.ModelState.Count);
        }

        [TestMethod]
        public void TestBindModel_StartLessThanZero()
        {
            var start = -1;
            var limit = 10;
            var valueProvider = new SimpleHttpValueProvider
            {
                { PagingQueryBindingModelBinder.START_QUERY_KEY, start.ToString() },
                { PagingQueryBindingModelBinder.LIMIT_QUERY_KEY, limit.ToString() },
            };
            var actionContext = GetActionContext();
            var bindingContext = GetModelBindingContext(valueProvider);

            var binder = new PagingQueryBindingModelBinder();
            binder.BindModel(actionContext, bindingContext);
            Assert.AreEqual(1, bindingContext.ModelState.Count);
        }

        [TestMethod]
        public void TestBindModel_OnlyStartAndLimit()
        {
            var start = 0;
            var limit = 10;
            var valueProvider = new SimpleHttpValueProvider
            {
                { PagingQueryBindingModelBinder.START_QUERY_KEY, start.ToString() },
                { PagingQueryBindingModelBinder.LIMIT_QUERY_KEY, limit.ToString() },
            };
            var actionContext = GetActionContext();
            var bindingContext = GetModelBindingContext(valueProvider);

            var binder = new PagingQueryBindingModelBinder();
            binder.BindModel(actionContext, bindingContext);
            Assert.AreEqual(0, bindingContext.ModelState.Count);

            Assert.IsInstanceOfType(bindingContext.Model, typeof(PagingQueryBindingModel));
            var model = (PagingQueryBindingModel)bindingContext.Model;
            Assert.AreEqual(start, model.Start);
            Assert.AreEqual(limit, model.Limit);
        }

        [TestMethod]
        public void TestBindModel_OneFilter()
        {
            var start = 0;
            var limit = 10;
            var filter = new ExpressionFilter<PagingQueryBindingModel>(x => x.Limit, ComparisonType.Equal, 1L);
            var valueProvider = new SimpleHttpValueProvider
            {
                { PagingQueryBindingModelBinder.START_QUERY_KEY, start.ToString() },
                { PagingQueryBindingModelBinder.LIMIT_QUERY_KEY, limit.ToString() },
                { PagingQueryBindingModelBinder.FILTER_QUERY_KEY, JsonConvert.SerializeObject(new List<SimpleFilter>{filter}) },
            };
            var actionContext = GetActionContext();
            var bindingContext = GetModelBindingContext(valueProvider);

            var binder = new PagingQueryBindingModelBinder();
            binder.BindModel(actionContext, bindingContext);
            Assert.AreEqual(0, bindingContext.ModelState.Count);

            Assert.IsInstanceOfType(bindingContext.Model, typeof(PagingQueryBindingModel));
            var model = (PagingQueryBindingModel)bindingContext.Model;
            Assert.AreEqual(start, model.Start);
            Assert.AreEqual(limit, model.Limit);

            Assert.AreEqual(0, model.Sort.Count);
            Assert.AreEqual(1, model.Filter.Count);
            var testFilter = model.Filter.First();
            Assert.AreEqual(filter.Comparison, testFilter.Comparison);
            Assert.AreEqual(filter.Property, testFilter.Property);
            Assert.AreEqual(filter.Value, testFilter.Value);
        }

        [TestMethod]
        public void TestBindModel_MultipleFilters()
        {
            var start = 0;
            var limit = 10;
            var filter1 = new ExpressionFilter<PagingQueryBindingModel>(x => x.Limit, ComparisonType.Equal, 1L);
            var filter2 = new ExpressionFilter<PagingQueryBindingModel>(x => x.Limit, ComparisonType.Equal, 1L);
            var valueProvider = new SimpleHttpValueProvider
            {
                { PagingQueryBindingModelBinder.START_QUERY_KEY, start.ToString() },
                { PagingQueryBindingModelBinder.LIMIT_QUERY_KEY, limit.ToString() },
                { PagingQueryBindingModelBinder.FILTER_QUERY_KEY, JsonConvert.SerializeObject(new List<SimpleFilter>{filter1, filter2}) },
            };
            var actionContext = GetActionContext();
            var bindingContext = GetModelBindingContext(valueProvider);

            var binder = new PagingQueryBindingModelBinder();
            binder.BindModel(actionContext, bindingContext);
            Assert.AreEqual(0, bindingContext.ModelState.Count);

            Assert.IsInstanceOfType(bindingContext.Model, typeof(PagingQueryBindingModel));
            var model = (PagingQueryBindingModel)bindingContext.Model;
            Assert.AreEqual(start, model.Start);
            Assert.AreEqual(limit, model.Limit);
            Assert.AreEqual(0, model.Sort.Count);
            Assert.AreEqual(2, model.Filter.Count);
        }

        [TestMethod]
        public void TestBindModel_OneSorter()
        {
            var start = 0;
            var limit = 10;
            var sorter = new ExpressionSorter<PagingQueryBindingModel>(x => x.Limit, SortDirection.Ascending);
            var valueProvider = new SimpleHttpValueProvider
            {
                { PagingQueryBindingModelBinder.START_QUERY_KEY, start.ToString() },
                { PagingQueryBindingModelBinder.LIMIT_QUERY_KEY, limit.ToString() },
                { PagingQueryBindingModelBinder.SORTER_QUERY_KEY, JsonConvert.SerializeObject(new List<SimpleSorter>{sorter}) },
            };
            var actionContext = GetActionContext();
            var bindingContext = GetModelBindingContext(valueProvider);

            var binder = new PagingQueryBindingModelBinder();
            binder.BindModel(actionContext, bindingContext);
            Assert.AreEqual(0, bindingContext.ModelState.Count);

            Assert.IsInstanceOfType(bindingContext.Model, typeof(PagingQueryBindingModel));
            var model = (PagingQueryBindingModel)bindingContext.Model;
            Assert.AreEqual(start, model.Start);
            Assert.AreEqual(limit, model.Limit);

            Assert.AreEqual(0, model.Filter.Count);
            Assert.AreEqual(1, model.Sort.Count);
            var testSorter = model.Sort.First();
            Assert.AreEqual(sorter.Direction, testSorter.Direction);
            Assert.AreEqual(sorter.Property, testSorter.Property);
        }

        [TestMethod]
        public void TestBindModel_MultipleSorters()
        {
            var start = 0;
            var limit = 10;
            var sorter1 = new ExpressionSorter<PagingQueryBindingModel>(x => x.Limit, SortDirection.Ascending);
            var sorter2 = new ExpressionSorter<PagingQueryBindingModel>(x => x.Limit, SortDirection.Ascending);
            var valueProvider = new SimpleHttpValueProvider
            {
                { PagingQueryBindingModelBinder.START_QUERY_KEY, start.ToString() },
                { PagingQueryBindingModelBinder.LIMIT_QUERY_KEY, limit.ToString() },
                { PagingQueryBindingModelBinder.SORTER_QUERY_KEY, JsonConvert.SerializeObject(new List<SimpleSorter>{sorter1, sorter2}) },
            };
            var actionContext = GetActionContext();
            var bindingContext = GetModelBindingContext(valueProvider);

            var binder = new PagingQueryBindingModelBinder();
            binder.BindModel(actionContext, bindingContext);
            Assert.AreEqual(0, bindingContext.ModelState.Count);

            Assert.IsInstanceOfType(bindingContext.Model, typeof(PagingQueryBindingModel));
            var model = (PagingQueryBindingModel)bindingContext.Model;
            Assert.AreEqual(start, model.Start);
            Assert.AreEqual(limit, model.Limit);

            Assert.AreEqual(0, model.Filter.Count);
            Assert.AreEqual(2, model.Sort.Count);
        }

        private HttpActionContext GetActionContext()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/");
            var httpControllerContext = new HttpControllerContext();
            httpControllerContext.Request = request;
            var actionContext = new HttpActionContext();
            actionContext.ControllerContext = httpControllerContext;
            return actionContext;
        }

        private System.Web.Http.ModelBinding.ModelBindingContext GetModelBindingContext(System.Web.Http.ValueProviders.IValueProvider valueProvider)
        {


            var bindingContext = new System.Web.Http.ModelBinding.ModelBindingContext
            {
                ModelMetadata = new System.Web.Http.Metadata.Providers.EmptyModelMetadataProvider().GetMetadataForType(null, typeof(PagingQueryBindingModel)),
                ModelName = typeof(PagingQueryBindingModel).Name,
                ValueProvider = valueProvider
            };
            return bindingContext;
        }
        #endregion
    }
}
