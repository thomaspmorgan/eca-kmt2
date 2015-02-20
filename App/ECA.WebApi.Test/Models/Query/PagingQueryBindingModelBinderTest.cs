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

namespace ECA.WebApi.Test.Models.Query
{
    [TestClass]
    public class PagingQueryBindingModelBinderTest
    {
        //[TestMethod]
        //public void TestBindModel_OnlyStartAndLimitInitialized()
        //{
        //    var start = 1;
        //    var limit = 10;

        //    //var binder = new PagingQueryBindingModelBinder();
        //    //var httpControllerContext = new HttpControllerContext();
        //    //httpControllerContext.Request = new HttpRequestMessage(HttpMethod.Get, String.Format("http://localhost/action?start={0}&limit={1}", start, limit));
        //    //var actionContext = new HttpActionContext();
        //    //actionContext.ControllerContext = httpControllerContext;

        //    ////var formCollection = new NameValueCollection 
        //    ////{
        //    ////    { "start", start.ToString() },
        //    ////    { "limit", limit.ToString() },
        //    ////};
        //    //var valueProvider = new NameValueCollectionValueProvider(formCollection, CultureInfo.CurrentCulture);
        //    ////var valueProvider = new QueryStringValueProvider()
        //    //var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(PagingQueryBindingModel));
        //    //var bindingContext = new System.Web.Http.ModelBinding.ModelBindingContext
        //    //{
        //    //    ModelName = "",
        //    //    ValueProvider = valueProvider as IValueProvider,
        //    //    ModelMetadata = metadata
        //    //};

        //    //var result = binder.BindModel(actionContext, bindingContext);
        //    //Assert.IsTrue(result);

        //    //var dict = new ValueProviderDictionary(null)
        //    //{
        //    //    {"foo.month1", new ValueProviderResult("2", "2", null)},
        //    //    {"foo.day1", new ValueProviderResult("12", "12", null)},
        //    //    {"foo.year1", new ValueProviderResult("1964", "1964", null)},
        //    //    {"foo.hour1", new ValueProviderResult("13", "13", null)},
        //    //    {"foo.minute1", new ValueProviderResult("44", "44", null)},
        //    //    {"foo.second1", new ValueProviderResult("01", "01", null)},

        //    //    {"foo.month2", new ValueProviderResult("5", "5", null)},
        //    //    {"foo.day2", new ValueProviderResult("9", "9", null)},
        //    //    {"foo.year2", new ValueProviderResult("2014", "2014", null)},
        //    //    {"foo.hour2", new ValueProviderResult("11", "11", null)},
        //    //    {"foo.minute2", new ValueProviderResult("00", "00", null)},
        //    //    {"foo.second2", new ValueProviderResult("01", "01", null)},
        //    //};

        //    //var bindingContext = new ModelBindingContext() { ModelName = "foo", ValueProvider = dict };

        //}


        #region Parse simple filters
        [TestMethod]
        public void TestParseAsSimpleFilters_NullString()
        {
            var binder = new PagingQueryBindingModelBinder();
            var testFilters = binder.ParseFilters(null);
            Assert.AreEqual(0, testFilters.Count);
        }

        [TestMethod]
        public void TestParseAsSimpleFilters_HasOneFilter()
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
        public void TestParseAsSimpleFilters_HasMultipleFilters()
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

    }
}
