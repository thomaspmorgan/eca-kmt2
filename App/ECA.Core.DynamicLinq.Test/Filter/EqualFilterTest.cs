using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test.Filter
{   
    public class EqualFilterTestClass
    {
        public int Int { get; set; }

        public int? NullableInt { get; set; }

        public DateTime Date { get; set; }

        public DateTime? NullableDate { get; set; }

        public string S { get; set; }
    }

    /// <summary>
    /// Summary description for EqualFilterTest
    /// </summary>
    [TestClass]
    public class EqualFilterTest
    {
        [TestMethod]
        public void TestToWhereExpression_DifferentNumericTypes()
        {
            var instance = new EqualFilterTestClass
            {
                Int = 1
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new EqualFilter<EqualFilterTestClass>("Int", 1L);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        #region String

        [TestMethod]
        public void TestEqualFilter_String_EqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                S = "string"
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new EqualFilter<EqualFilterTestClass>("S", instance.S);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestEqualFilter_String_NotEqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                S = "string"
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new EqualFilter<EqualFilterTestClass>("S", "abc");
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        #endregion

        #region Int

        [TestMethod]
        public void TestEqualFilter_NonNullableInteger_EqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                Int = 1
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new EqualFilter<EqualFilterTestClass>("Int", instance.Int);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);

        }

        [TestMethod]
        public void TestEqualFilter_NonNullableInteger_NotEqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                Int = 1
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new EqualFilter<EqualFilterTestClass>("Int", 0);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);

        }

        [TestMethod]
        public void TestEqualFilter_NullableInteger_EqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                NullableInt = 1
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new EqualFilter<EqualFilterTestClass>("NullableInt", instance.NullableInt);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);

        }

        [TestMethod]
        public void TestEqualFilter_NullableInteger_NotEqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                NullableInt = 1
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new EqualFilter<EqualFilterTestClass>("NullableInt", 0);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);

        }

        #endregion

        #region Date
        [TestMethod]
        public void TestEqualFilter_NonNullableDate_EqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                Date = DateTime.UtcNow
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new EqualFilter<EqualFilterTestClass>("Date", instance.Date);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);

        }

        [TestMethod]
        public void TestEqualFilter_NonNullableDate_NotEqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                Date = DateTime.UtcNow
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new EqualFilter<EqualFilterTestClass>("Date", instance.Date.AddDays(1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);

        }

        [TestMethod]
        public void TestEqualFilter_NullableDate_EqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                NullableDate = DateTime.UtcNow
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new EqualFilter<EqualFilterTestClass>("NullableDate", instance.NullableDate);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestEqualFilter_NullableDate_NotEqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                NullableDate = DateTime.UtcNow
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new EqualFilter<EqualFilterTestClass>("NullableDate", instance.NullableDate.Value.AddDays(1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }
        #endregion
    }
}
