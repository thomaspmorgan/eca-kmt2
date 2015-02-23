using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Filter;
using System.Collections.Generic;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class SimpleFilterTestClass
    {
        public string S { get; set; }

        public int I { get; set; }
    }

    [TestClass]
    public class SimpleFilterTest
    {
        [TestMethod]
        public void TestToClientFilter_EqualComparison()
        {
            var filter = new SimpleFilter
            {
                Comparison = ComparisonType.Equal.Value,
                Property = "S",
                Value = "hello"
            };

            var linqFilter = filter.ToLinqFilter<SimpleFilterTestClass>();
            Assert.IsInstanceOfType(linqFilter, typeof(EqualFilter<SimpleFilterTestClass>));

            var typedLinqFilter = (EqualFilter<SimpleFilterTestClass>) linqFilter;
            Assert.AreEqual(filter.Value, typedLinqFilter.Value);
            Assert.AreEqual(filter.Property, typedLinqFilter.PropertyInfo.Name);
        }

        [TestMethod]
        public void TestToClientFilter_GreaterThanComparison()
        {
            var filter = new SimpleFilter
            {
                Comparison = ComparisonType.GreaterThan.Value,
                Property = "I",
                Value = 1
            };

            var linqFilter = filter.ToLinqFilter<SimpleFilterTestClass>();
            Assert.IsInstanceOfType(linqFilter, typeof(GreaterThanFilter<SimpleFilterTestClass>));

            var typedLinqFilter = (GreaterThanFilter<SimpleFilterTestClass>)linqFilter;
            Assert.AreEqual(filter.Value, typedLinqFilter.Value);
            Assert.AreEqual(filter.Property, typedLinqFilter.PropertyInfo.Name);
        }

        [TestMethod]
        public void TestToClientFilter_LessThanComparison()
        {
            var filter = new SimpleFilter
            {
                Comparison = ComparisonType.LessThan.Value,
                Property = "I",
                Value = 1
            };

            var linqFilter = filter.ToLinqFilter<SimpleFilterTestClass>();
            Assert.IsInstanceOfType(linqFilter, typeof(LessThanFilter<SimpleFilterTestClass>));

            var typedLinqFilter = (LessThanFilter<SimpleFilterTestClass>)linqFilter;
            Assert.AreEqual(filter.Value, typedLinqFilter.Value);
            Assert.AreEqual(filter.Property, typedLinqFilter.PropertyInfo.Name);
        }

        [TestMethod]
        public void TestToClientFilter_LikeComparison()
        {
            var filter = new SimpleFilter
            {
                Comparison = ComparisonType.Like.Value,
                Property = "S",
                Value = "hello"
            };

            var linqFilter = filter.ToLinqFilter<SimpleFilterTestClass>();
            Assert.IsInstanceOfType(linqFilter, typeof(LikeFilter<SimpleFilterTestClass>));

            var typedLinqFilter = (LikeFilter<SimpleFilterTestClass>)linqFilter;
            Assert.AreEqual(filter.Value, typedLinqFilter.Value);
            Assert.AreEqual(filter.Property, typedLinqFilter.PropertyInfo.Name);
        }

        [TestMethod]
        public void TestToClientFilter_InComparison()
        {
            var filter = new SimpleFilter
            {
                Comparison = ComparisonType.In.Value,
                Property = "I",
                Value = new List<int> { 1}
            };

            var linqFilter = filter.ToLinqFilter<SimpleFilterTestClass>();
            Assert.IsInstanceOfType(linqFilter, typeof(InFilter<SimpleFilterTestClass>));

            var typedLinqFilter = (InFilter<SimpleFilterTestClass>)linqFilter;
            Assert.AreEqual(filter.Value, typedLinqFilter.Value);
            Assert.AreEqual(filter.Property, typedLinqFilter.PropertyInfo.Name);
        }

        [TestMethod]
        public void TestToClientFilter_NullComparison()
        {
            var filter = new SimpleFilter
            {
                Comparison = ComparisonType.Null.Value,
                Property = "S",
                Value = "hello"
            };

            var linqFilter = filter.ToLinqFilter<SimpleFilterTestClass>();
            Assert.IsInstanceOfType(linqFilter, typeof(NullFilter<SimpleFilterTestClass>));

            var typedLinqFilter = (NullFilter<SimpleFilterTestClass>)linqFilter;
            Assert.IsFalse(typedLinqFilter.IsNotNull);
            Assert.AreEqual(filter.Property, typedLinqFilter.PropertyInfo.Name);
        }

        [TestMethod]
        public void TestToClientFilter_NotNullComparison()
        {
            var filter = new SimpleFilter
            {
                Comparison = ComparisonType.NotNull.Value,
                Property = "S",
                Value = "hello"
            };

            var linqFilter = filter.ToLinqFilter<SimpleFilterTestClass>();
            Assert.IsInstanceOfType(linqFilter, typeof(NullFilter<SimpleFilterTestClass>));

            var typedLinqFilter = (NullFilter<SimpleFilterTestClass>)linqFilter;
            Assert.IsTrue(typedLinqFilter.IsNotNull);
            Assert.AreEqual(filter.Property, typedLinqFilter.PropertyInfo.Name);
        }
    }
}
