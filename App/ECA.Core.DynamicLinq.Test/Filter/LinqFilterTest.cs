using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test.Filter
{

    public class LinqFilterTestClass
    {
        public int Id { get; set; }

        public int? NullableId { get; set; }

        public string S { get; set; }
    }

    public class LinqTestFilter<T> : LinqFilter<T> where T : class
    {

        public LinqTestFilter()
            : base()
        {

        }

        public override System.Linq.Expressions.Expression<Func<T, bool>> ToWhereExpression()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class LinqFilterTest
    {
        

        #region IEnumerable Extension Tests

        [TestMethod]
        public void TestWhereExtension_IEnumerable_SingleLinqFilter()
        {
            var list = new List<LinqFilterTestClass>();
            list.Add(new LinqFilterTestClass
            {
                Id = 1,
                NullableId = 1,
                S = "hello"
            });

            var filter = new LikeFilter<LinqFilterTestClass>("S", "hello");
            var results = list.Where(filter).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestWhereExtension_IEnumerable_MultipleLinqFilters()
        {
            var list = new List<LinqFilterTestClass>();
            list.Add(new LinqFilterTestClass
            {
                Id = 1,
                NullableId = 1,
                S = "hello"
            });

            var filter1 = new LikeFilter<LinqFilterTestClass>("S", "h");
            var filter2 = new LikeFilter<LinqFilterTestClass>("S", "o");
            var results = list.Where(new List<LinqFilter<LinqFilterTestClass>>{filter1, filter2}).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestWhereExtension_IEnumerable_SingleIFilter()
        {
            var list = new List<LinqFilterTestClass>();
            list.Add(new LinqFilterTestClass
            {
                Id = 1,
                NullableId = 1,
                S = "hello"
            });

            var filter = new SimpleFilter
            {
                Comparison = ComparisonType.Like.Value,
                Property = "S",
                Value = "hello"
            };
            var results = list.Where(filter).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestWhereExtension_IEnumerable_MultipleIFilters()
        {
            var list = new List<LinqFilterTestClass>();
            list.Add(new LinqFilterTestClass
            {
                Id = 1,
                NullableId = 1,
                S = "hello"
            });

            var filter1 = new SimpleFilter
            {
                Comparison = ComparisonType.Like.Value,
                Property = "S",
                Value = "h"
            };
            var filter2 = new SimpleFilter
            {
                Comparison = ComparisonType.Like.Value,
                Property = "S",
                Value = "o"
            };
            var results = list.Where(new List<IFilter>{filter1, filter2}).ToList();
            Assert.AreEqual(1, results.Count);
        }

        #endregion

        #region IQueryable Extension Tests
        [TestMethod]
        public void TestWhereExtension_IQueryable_SingleLinqFilter()
        {
            var list = new List<LinqFilterTestClass>();
            list.Add(new LinqFilterTestClass
            {
                Id = 1,
                NullableId = 1,
                S = "hello"
            });

            var filter = new LikeFilter<LinqFilterTestClass>("S", "hello");
            var results = list.AsQueryable().Where(filter).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestWhereExtension_IQueryable_MultipleLinqFilters()
        {
            var list = new List<LinqFilterTestClass>();
            list.Add(new LinqFilterTestClass
            {
                Id = 1,
                NullableId = 1,
                S = "hello"
            });

            var filter1 = new LikeFilter<LinqFilterTestClass>("S", "h");
            var filter2 = new LikeFilter<LinqFilterTestClass>("S", "o");
            var results = list.AsQueryable().Where(new List<LinqFilter<LinqFilterTestClass>> { filter1, filter2 }).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestWhereExtension_IQueryable_SingleIFilter()
        {
            var list = new List<LinqFilterTestClass>();
            list.Add(new LinqFilterTestClass
            {
                Id = 1,
                NullableId = 1,
                S = "hello"
            });

            var filter = new SimpleFilter
            {
                Comparison = ComparisonType.Like.Value,
                Property = "S",
                Value = "hello"
            };
            var results = list.AsQueryable().Where(filter).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestWhereExtension_IQueryable_MultipleIFilters()
        {
            var list = new List<LinqFilterTestClass>();
            list.Add(new LinqFilterTestClass
            {
                Id = 1,
                NullableId = 1,
                S = "hello"
            });

            var filter1 = new SimpleFilter
            {
                Comparison = ComparisonType.Like.Value,
                Property = "S",
                Value = "h"
            };
            var filter2 = new SimpleFilter
            {
                Comparison = ComparisonType.Like.Value,
                Property = "S",
                Value = "o"
            };
            var results = list.AsQueryable().Where(new List<IFilter> { filter1, filter2 }).ToList();
            Assert.AreEqual(1, results.Count);
        }

        #endregion

    }
}
