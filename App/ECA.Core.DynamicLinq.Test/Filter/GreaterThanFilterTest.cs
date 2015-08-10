using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Filter;
using System.Collections.Generic;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class GreaterThanFilterTestClass
    {
        public int Int { get; set; }

        public int? NullableInt { get; set; }

        public DateTime Date { get; set; }

        public DateTime? NullableDate { get; set; }

        public double D { get; set; }

        public double? NullableD { get; set; }

        public decimal Dec { get; set; }

        public decimal? NullableDec { get; set; }
    }


    [TestClass]
    public class GreaterThanFilterTest
    {

        #region Int

        [TestMethod]
        public void TestGreaterThanFilter_NonNullableInt_ValueIsLessThan()
        {
            var instance = new GreaterThanFilterTestClass
            {
                Int = 1
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("Int", instance.Int - 1);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NonNullableInt_ValueIsEqual()
        {
            var instance = new GreaterThanFilterTestClass
            {
                Int = 1
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("Int", instance.Int);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NonNullableInt_ValueIsGreater()
        {
            var instance = new GreaterThanFilterTestClass
            {
                Int = 1
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("Int", instance.Int + 1);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NullableInt_ValueIsLessThan()
        {
            var instance = new GreaterThanFilterTestClass
            {
                NullableInt = 1
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("NullableInt", instance.NullableInt - 1);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NullableInt_ValueIsEqual()
        {
            var instance = new GreaterThanFilterTestClass
            {
                NullableInt = 1
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("NullableInt", instance.NullableInt);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NullableInt_ValueIsGreater()
        {
            var instance = new GreaterThanFilterTestClass
            {
                NullableInt = 1
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("NullableInt", instance.NullableInt + 1);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        #endregion

        #region Double
        [TestMethod]
        public void TestGreaterThanFilter_NonNullableDouble_ValueIsLessThan()
        {
            var instance = new GreaterThanFilterTestClass
            {
                D = 1.0
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("D", instance.D - 1.0);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NonNullableDouble_ValueIsEqual()
        {
            var instance = new GreaterThanFilterTestClass
            {
                D = 1.0
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("D", instance.D);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NonNullableDouble_ValueIsGreater()
        {
            var instance = new GreaterThanFilterTestClass
            {
                D = 1.0
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("D", instance.D + 1.0);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }



        [TestMethod]
        public void TestGreaterThanFilter_NullableDouble_ValueIsLessThan()
        {
            var instance = new GreaterThanFilterTestClass
            {
                NullableD = 1.0
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("NullableD", instance.NullableD.Value - 1.0);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NullableDouble_ValueIsEqual()
        {
            var instance = new GreaterThanFilterTestClass
            {
                NullableD = 1.0
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("NullableD", instance.NullableD);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NullableDouble_ValueIsGreater()
        {
            var instance = new GreaterThanFilterTestClass
            {
                NullableD = 1.0
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("NullableD", instance.NullableD.Value + 1.0);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        #endregion

        #region Date

        [TestMethod]
        public void TestGreaterThanFilter_NonNullableDate_ValueIsLessThan()
        {
            var instance = new GreaterThanFilterTestClass
            {
                Date = DateTime.UtcNow
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("Date", instance.Date.AddDays(-1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NonNullableDate_ValueIsEqual()
        {
            var instance = new GreaterThanFilterTestClass
            {
                Date = DateTime.UtcNow
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("Date", instance.Date);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NonNullableDate_ValueIsGreater()
        {
            var instance = new GreaterThanFilterTestClass
            {
                Date = DateTime.UtcNow
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("Date", instance.Date.AddDays(1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }



        [TestMethod]
        public void TestGreaterThanFilter_NullableDate_ValueIsLessThan()
        {
            var instance = new GreaterThanFilterTestClass
            {
                NullableDate = DateTime.UtcNow
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("NullableDate", instance.NullableDate.Value.AddDays(-1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NullableDate_ValueIsEqual()
        {
            var instance = new GreaterThanFilterTestClass
            {
                NullableDate = DateTime.UtcNow
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("NullableDate", instance.NullableDate);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NullableDate_ValueIsGreater()
        {
            var instance = new GreaterThanFilterTestClass
            {
                NullableDate = DateTime.UtcNow
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("NullableDate", instance.NullableDate.Value.AddDays(1.0));
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        #endregion

        #region Decimal
        [TestMethod]
        public void TestGreaterThanFilter_NonNullableDecimal_ValueIsLessThan()
        {
            var instance = new GreaterThanFilterTestClass
            {
                Dec = 1.0m
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("Dec", instance.Dec - 1.0m);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NonNullableDecimal_ValueIsEqual()
        {
            var instance = new GreaterThanFilterTestClass
            {
                Dec = 1.0m
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("Dec", instance.Dec);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NonNullableDecimal_ValueIsGreater()
        {
            var instance = new GreaterThanFilterTestClass
            {
                Dec = 1.0m
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("Dec", instance.Dec + 1.0m);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }



        [TestMethod]
        public void TestGreaterThanFilter_NullableDecimal_ValueIsLessThan()
        {
            var instance = new GreaterThanFilterTestClass
            {
                NullableDec = 1.0m
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("NullableDec", instance.NullableDec.Value - 1.0m);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NullableDecimal_ValueIsEqual()
        {
            var instance = new GreaterThanFilterTestClass
            {
                NullableDec = 1.0m
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("NullableDec", instance.NullableDec);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestGreaterThanFilter_NullableDecimal_ValueIsGreater()
        {
            var instance = new GreaterThanFilterTestClass
            {
                NullableDec = 1.0m
            };
            var list = new List<GreaterThanFilterTestClass>();
            list.Add(instance);

            var filter = new GreaterThanFilter<GreaterThanFilterTestClass>("NullableDec", instance.NullableDec.Value + 1.0m);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        #endregion
    }
}
