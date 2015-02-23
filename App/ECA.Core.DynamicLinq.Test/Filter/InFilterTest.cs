using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class InFilterTestClass
    {
        public int Id { get; set; }

        public int? NullableId { get; set; }
    }

    [TestClass]
    public class InFilterTest
    {
        [TestMethod]
        public void TestToWhereExpression_IntProperty_SingleInFilterValue()
        {
            var testValue = 1;
            var list = new List<InFilterTestClass>();
            list.Add(new InFilterTestClass
            {
                Id = 1
            });
            list.Add(new InFilterTestClass
            {
                Id = 2
            });

            var testInList = new List<int> { testValue };
            var filter = new InFilter<InFilterTestClass>("Id", testInList);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_IntProperty_MultipleInFilterValues()
        {
            var list = new List<InFilterTestClass>();
            list.Add(new InFilterTestClass
            {
                Id = 1
            });
            list.Add(new InFilterTestClass
            {
                Id = 2
            });

            var testInList = new List<int> { list.First().Id, list.Last().Id };
            var filter = new InFilter<InFilterTestClass>("Id", testInList);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_IntProperty_FilterValuesAreLong()
        {
            var list = new List<InFilterTestClass>();
            list.Add(new InFilterTestClass
            {
                Id = 1
            });
            list.Add(new InFilterTestClass
            {
                Id = 2
            });

            var testInList = new List<long> { 1L };
            var filter = new InFilter<InFilterTestClass>("Id", testInList);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_NullableIntProperty_PropertyValueIsNull()
        {
            var list = new List<InFilterTestClass>();
            list.Add(new InFilterTestClass
            {
                NullableId = null
            });

            var testInList = new List<int> { 1 };
            var filter = new InFilter<InFilterTestClass>("NullableId", testInList);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestConstructor_ValueIsNotIEnumerable()
        {
            var filter = new InFilter<InFilterTestClass>("Id", 1);
        }
    }
}
