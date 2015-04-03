using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class NotInFilterTestClass
    {
        public int Id { get; set; }

        public int? NullableId { get; set; }

        public string S { get; set; }
    }

    [TestClass]
    public class NotInFilterTest
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestConstructor_ValueIsNotIEnumerable()
        {
            var filter = new NotInFilter<NotInFilterTestClass>("Id", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestConstructor_PropertyTypeIsNumeric_CollectionIsNot()
        {
            var filter = new NotInFilter<NotInFilterTestClass>("Id", new List<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestConstructor_PropertyTypeIsNotNumeric_CollectionIsNumeric()
        {
            var filter = new NotInFilter<NotInFilterTestClass>("S", new List<int>());
        }

        [TestMethod]
        public void TestToWhereExpression_IntProperty_SingleNotInFilterValue()
        {
            var testValue = 1;
            var list = new List<NotInFilterTestClass>();
            list.Add(new NotInFilterTestClass
            {
                Id = testValue
            });

            var testInList = new List<int> { testValue };
            var filter = new NotInFilter<NotInFilterTestClass>("Id", testInList);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_IntProperty_MultipleNotInFilterValues()
        {
            var testValue = 1;
            var secondTestValue = 2;
            var list = new List<NotInFilterTestClass>();
            list.Add(new NotInFilterTestClass
            {
                Id = testValue
            });
            list.Add(new NotInFilterTestClass
            {
                Id = secondTestValue
            });

            var testInList = new List<int> { testValue, secondTestValue };
            var filter = new NotInFilter<NotInFilterTestClass>("Id", testInList);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_IntProperty_SingleNotInFilter_MultipleListValues()
        {
            var testValue = 1;
            var secondTestValue = 2;
            var list = new List<NotInFilterTestClass>();
            list.Add(new NotInFilterTestClass
            {
                Id = testValue
            });
            list.Add(new NotInFilterTestClass
            {
                Id = secondTestValue
            });

            var testInList = new List<int> { testValue };
            var filter = new NotInFilter<NotInFilterTestClass>("Id", testInList);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(secondTestValue, results.First().Id);
        }

        [TestMethod]
        public void TestToWhereExpression_NullableIntProperty_SingleNotInFilter_NullValue()
        {
            var testValue = 1;
            var list = new List<NotInFilterTestClass>();
            list.Add(new NotInFilterTestClass
            {
                Id= 1,
                NullableId = testValue
            });
            list.Add(new NotInFilterTestClass
            {
                Id = 2,
                NullableId = null
            });

            var testInList = new List<int> { testValue };
            var filter = new NotInFilter<NotInFilterTestClass>("NullableId", testInList);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(2, results.First().Id);
        }

        [TestMethod]
        public void TestToWhereExpression_NullableIntProperty_MultipleNotInFilters_NullValue()
        {
            var testValue = 1;
            var secondTestValue = 2;
            var list = new List<NotInFilterTestClass>();
            list.Add(new NotInFilterTestClass
            {
                Id = 1,
                NullableId = null
            });
            list.Add(new NotInFilterTestClass
            {
                Id = 2,
                NullableId = null
            });

            var testInList = new List<int> { testValue, secondTestValue };
            var filter = new NotInFilter<NotInFilterTestClass>("NullableId", testInList);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(2, results.Count);
        }
    }
}
