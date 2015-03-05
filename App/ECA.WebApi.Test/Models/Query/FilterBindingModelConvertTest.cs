using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Query;
using ECA.Core.DynamicLinq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace ECA.WebApi.Test.Models.Query
{
    [TestClass]
    public class FilterBindingModelConvertTest
    {
        [TestMethod]
        public void TestCreate()
        {
            var converter = new FilterBindingModelConverter();
            Assert.IsNotNull(converter.Create(null));
        }

        [TestMethod]
        public void TestCanWrite()
        {
            var converter = new FilterBindingModelConverter();
            Assert.IsFalse(converter.CanWrite);
        }

        [TestMethod]
        public void TestReadJson_SimpleFilter()
        {
            var filterBindingModel = new FilterBindingModel
            {
                Comparison = ComparisonType.Equal.Value,
                Property = "Id",
                Value = 1L
            };
            var json = JsonConvert.SerializeObject(filterBindingModel);

            var testObject = JsonConvert.DeserializeObject<FilterBindingModel>(json, new FilterBindingModelConverter());
            Assert.AreEqual(filterBindingModel.Comparison, testObject.Comparison);
            Assert.AreEqual(filterBindingModel.Property, testObject.Property);
            Assert.AreEqual(filterBindingModel.Value, testObject.Value);
        }

        [TestMethod]
        public void TestReadJson_ArrayOfFilters()
        {
            var filterBindingModel1 = new FilterBindingModel
            {
                Comparison = ComparisonType.Equal.Value,
                Property = "Id",
                Value = 1L
            };
            var filterBindingModel2 = new FilterBindingModel
            {
                Comparison = ComparisonType.Like.Value,
                Property = "Name",
                Value = 2L
            };
            var list = new List<FilterBindingModel> { filterBindingModel1, filterBindingModel2 };
            var json = JsonConvert.SerializeObject(list);

            var testObject = JsonConvert.DeserializeObject<List<FilterBindingModel>>(json, new FilterBindingModelConverter());
            Assert.AreEqual(2, testObject.Count);
            Assert.IsNotNull(testObject.Select(x => x.Property == filterBindingModel1.Property).FirstOrDefault());
            Assert.IsNotNull(testObject.Select(x => x.Property == filterBindingModel2.Property).FirstOrDefault());
        }

        [TestMethod]
        public void TestReadJson_InFilter()
        {
            var list = new List<long> { 1L, 2L};
            var filterBindingModel = new FilterBindingModel
            {
                Comparison = ComparisonType.In.Value,
                Property = "Id",
                Value = list
            };
            var json = JsonConvert.SerializeObject(filterBindingModel);

            var testObject = JsonConvert.DeserializeObject<FilterBindingModel>(json, new FilterBindingModelConverter());
            Assert.AreEqual(filterBindingModel.Comparison, testObject.Comparison);
            Assert.AreEqual(filterBindingModel.Property, testObject.Property);

            var testList = testObject.Value;
            Assert.IsInstanceOfType(testList, typeof(List<long>));
            var testIntList = (List<long>)testList;
            CollectionAssert.AreEqual(list, testIntList);
        }

        #region To List
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestToList_JsonTokenTypeNotSupported()
        {
            var invalidList = new List<object>();
            invalidList.Add(new { Id = 1 });

            var jsonString = JsonConvert.SerializeObject(invalidList);
            var json = JsonConvert.DeserializeObject(jsonString);

            var converter = new FilterBindingModelConverter();
            var testList = converter.ToList(json as JArray);
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

            var converter = new FilterBindingModelConverter();
            var testList = converter.ToList(json as JArray);
        }


        [TestMethod]
        public void TestToList_ArrayOfLongs()
        {
            var intList = new List<long>();
            intList.Add(1L);
            intList.Add(2L);

            var jsonString = JsonConvert.SerializeObject(intList);
            var json = JsonConvert.DeserializeObject(jsonString);

            var converter = new FilterBindingModelConverter();
            var testList = converter.ToList(json as JArray);
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

            var converter = new FilterBindingModelConverter();
            var testList = converter.ToList(json as JArray);
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

            var converter = new FilterBindingModelConverter();
            var testList = converter.ToList(json as JArray);
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

            var converter = new FilterBindingModelConverter();
            var testList = converter.ToList(json as JArray);
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

            var converter = new FilterBindingModelConverter();
            var testList = converter.ToList(json as JArray);
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

            var converter = new FilterBindingModelConverter();
            var testList = converter.ToList(json as JArray);
            Assert.IsInstanceOfType(testList, typeof(List<double>));
            var testDoubleList = (List<double>)testList;
            CollectionAssert.AreEqual(floatList.Select(x => (double)x).ToList(), testDoubleList);

        }
        #endregion
    }
}
