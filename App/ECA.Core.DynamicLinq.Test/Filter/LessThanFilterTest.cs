using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Filter;
using System.Collections.Generic;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class LessThanFilterTestClass
    {
        public int Int { get; set; }

        public int? NullableInt { get; set; }

        public DateTime Date { get; set; }

        public DateTime? NullableDate { get; set; }

        public double D { get; set; }

        public double? NullableD { get; set; }

        public decimal Dec { get; set; }

        public decimal? NullableDec { get; set; }

        public DateTimeOffset DTOffset { get; set; }

        public DateTimeOffset? NullableDTOffset { get; set; }
    }


    [TestClass]
    public class LessThanFilterTest
    {

        #region Int

        [TestMethod]
        public void TestLessThanFilter_NonNullableInt_ValueIsLessThan()
        {
            var instance = new LessThanFilterTestClass
            {
                Int = 1
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("Int", instance.Int - 1);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NonNullableInt_ValueIsEqual()
        {
            var instance = new LessThanFilterTestClass
            {
                Int = 1
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("Int", instance.Int);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NonNullableInt_ValueIsGreater()
        {
            var instance = new LessThanFilterTestClass
            {
                Int = 1
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("Int", instance.Int + 1);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NullableInt_ValueIsLessThan()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableInt = 1
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableInt", instance.NullableInt - 1);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NullableInt_ValueIsEqual()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableInt = 1
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableInt", instance.NullableInt);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NullableInt_ValueIsGreater()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableInt = 1
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableInt", instance.NullableInt + 1);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        #endregion

        #region Double
        [TestMethod]
        public void TestLessThanFilter_NonNullableDouble_ValueIsLessThan()
        {
            var instance = new LessThanFilterTestClass
            {
                D = 1.0
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("D", instance.D - 1.0);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NonNullableDouble_ValueIsEqual()
        {
            var instance = new LessThanFilterTestClass
            {
                D = 1.0
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("D", instance.D);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NonNullableDouble_ValueIsGreater()
        {
            var instance = new LessThanFilterTestClass
            {
                D = 1.0
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("D", instance.D + 1.0);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }



        [TestMethod]
        public void TestLessThanFilter_NullableDouble_ValueIsLessThan()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableD = 1.0
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableD", instance.NullableD.Value - 1.0);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NullableDouble_ValueIsEqual()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableD = 1.0
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableD", instance.NullableD);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NullableDouble_ValueIsGreater()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableD = 1.0
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableD", instance.NullableD.Value + 1.0);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        #endregion

        #region Date

        [TestMethod]
        public void TestLessThanFilter_NonNullableDate_ValueIsLessThan()
        {
            var instance = new LessThanFilterTestClass
            {
                Date = DateTime.UtcNow
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("Date", instance.Date.AddDays(-1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NonNullableDate_ValueIsEqual()
        {
            var instance = new LessThanFilterTestClass
            {
                Date = DateTime.UtcNow
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("Date", instance.Date);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NonNullableDate_ValueIsGreater()
        {
            var instance = new LessThanFilterTestClass
            {
                Date = DateTime.UtcNow
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("Date", instance.Date.AddDays(1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }



        [TestMethod]
        public void TestLessThanFilter_NullableDate_ValueIsLessThan()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableDate = DateTime.UtcNow
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableDate", instance.NullableDate.Value.AddDays(-1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NullableDate_ValueIsEqual()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableDate = DateTime.UtcNow
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableDate", instance.NullableDate);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NullableDate_ValueIsGreater()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableDate = DateTime.UtcNow
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableDate", instance.NullableDate.Value.AddDays(1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        #endregion

        #region Decimal
        [TestMethod]
        public void TestLessThanFilter_NonNullableDecimal_ValueIsLessThan()
        {
            var instance = new LessThanFilterTestClass
            {
                Dec = 1.0m
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("Dec", instance.Dec - 1.0m);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NonNullableDecimal_ValueIsEqual()
        {
            var instance = new LessThanFilterTestClass
            {
                Dec = 1.0m
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("Dec", instance.Dec);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NonNullableDecimal_ValueIsGreater()
        {
            var instance = new LessThanFilterTestClass
            {
                Dec = 1.0m
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("Dec", instance.Dec+ 1.0m);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }



        [TestMethod]
        public void TestLessThanFilter_NullableDecimal_ValueIsLessThan()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableDec = 1.0m
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableDec", instance.NullableDec.Value - 1.0m);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NullableDecimal_ValueIsEqual()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableDec = 1.0m
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableDec", instance.NullableDec);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NullableDecimal_ValueIsGreater()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableDec = 1.0m
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableDec", instance.NullableDec.Value + 1.0m);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        #endregion

        #region DateTimeOffset
        [TestMethod]
        public void TestLessThanFilter_NonNullableDateTimeOffset_ValueIsLessThan()
        {
            var instance = new LessThanFilterTestClass
            {
                DTOffset = DateTimeOffset.Now
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("DTOffset", instance.DTOffset.AddDays(-1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NonNullableDateTimeOffset_ValueIsEqual()
        {
            var instance = new LessThanFilterTestClass
            {
                DTOffset = DateTimeOffset.Now
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("DTOffset", instance.DTOffset);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NonNullableDateTimeOffset_ValueIsGreater()
        {
            var instance = new LessThanFilterTestClass
            {
                DTOffset = DateTimeOffset.Now
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("DTOffset", instance.DTOffset.AddDays(1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NullableDateTimeOffset_ValueIsLessThan()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableDTOffset = DateTimeOffset.Now
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableDTOffset", instance.NullableDTOffset.Value.AddDays(-1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NullableDateTimeOffset_ValueIsEqual()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableDTOffset = DateTimeOffset.Now
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableDTOffset", instance.NullableDTOffset);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestLessThanFilter_NullableDateTimeOffset_ValueIsGreater()
        {
            var instance = new LessThanFilterTestClass
            {
                NullableDTOffset = DateTimeOffset.Now
            };
            var list = new List<LessThanFilterTestClass>();
            list.Add(instance);

            var filter = new LessThanFilter<LessThanFilterTestClass>("NullableDTOffset", instance.NullableDTOffset.Value.AddDays(1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }
        #endregion
    }
}
