using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test.Filter
{

    /// <summary>
    /// Summary description for EqualFilterTest
    /// </summary>
    [TestClass]
    public class NotEqualFilterTest
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

            var filter = new NotEqualFilter<EqualFilterTestClass>("Int", 1L);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        #region String

        [TestMethod]
        public void TestNotEqualFilter_String_EqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                S = "string"
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new NotEqualFilter<EqualFilterTestClass>("S", instance.S);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestNotEqualFilter_String_NotEqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                S = "string"
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new NotEqualFilter<EqualFilterTestClass>("S", "abc");
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        #endregion

        #region Int

        [TestMethod]
        public void TestNotEqualFilter_NonNullableInteger_EqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                Int = 1
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new NotEqualFilter<EqualFilterTestClass>("Int", instance.Int);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);

        }

        [TestMethod]
        public void TestNotEqualFilter_NonNullableInteger_NotEqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                Int = 1
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new NotEqualFilter<EqualFilterTestClass>("Int", 0);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);

        }

        [TestMethod]
        public void TestNotEqualFilter_NullableInteger_EqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                NullableInt = 1
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new NotEqualFilter<EqualFilterTestClass>("NullableInt", instance.NullableInt);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);

        }

        [TestMethod]
        public void TestNotEqualFilter_NullableInteger_NotEqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                NullableInt = 1
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new NotEqualFilter<EqualFilterTestClass>("NullableInt", 0);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);

        }

        #endregion

        #region Date
        [TestMethod]
        public void TestNotEqualFilter_NonNullableDate_EqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                Date = DateTime.UtcNow
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new NotEqualFilter<EqualFilterTestClass>("Date", instance.Date);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);

        }

        [TestMethod]
        public void TestNotEqualFilter_NonNullableDate_NotEqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                Date = DateTime.UtcNow
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new NotEqualFilter<EqualFilterTestClass>("Date", instance.Date.AddDays(1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);

        }

        [TestMethod]
        public void TestNotEqualFilter_NullableDate_EqualValue()
        {
            var instance = new EqualFilterTestClass
            {
                NullableDate = DateTime.UtcNow
            };
            var list = new List<EqualFilterTestClass>();
            list.Add(instance);

            var filter = new NotEqualFilter<EqualFilterTestClass>("NullableDate", instance.NullableDate);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
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

            var filter = new NotEqualFilter<EqualFilterTestClass>("NullableDate", instance.NullableDate.Value.AddDays(1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }
        #endregion
    }
}
