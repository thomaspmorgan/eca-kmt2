using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class SampleEqualityFilter<T> : EqualityFilter<T> where T : class
    {
        public SampleEqualityFilter(string property, object value) : base(property, value) { }

        protected override System.Linq.Expressions.Expression GetEqualityExpression(System.Linq.Expressions.MemberExpression property)
        {
            throw new NotImplementedException();
        }
    }

    public class EqualityFilterTestClass
    {
        public long L { get; set; }

        public long NullableL { get; set; }

        public string S { get; set; }

        public DateTimeOffset DTOffset { get; set; }
    }


    [TestClass]
    public class EqualityFilterTest
    {


        [TestMethod]
        public void TestConstructor_PropertyIsDateTimeOffset_ValueIsDateTime()
        {
            var instance = new GreaterThanFilterTestClass
            {
                DTOffset = DateTimeOffset.Now
            };
            var date = DateTime.UtcNow;
            var filter = new SampleEqualityFilter<EqualityFilterTestClass>("DTOffset", date);
            Assert.IsInstanceOfType(filter.Value, typeof(DateTimeOffset));
        }


        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestConstructor_PropertyTypeIsNumeric_ValueIsNotNumeric()
        {
            string value = "hello";
            var filter = new SampleEqualityFilter<EqualityFilterTestClass>("L", value);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestConstructor_PropertyTypeIsNonNumeric_ValueIsNumeric()
        {
            int value = 1;
            var filter = new SampleEqualityFilter<EqualityFilterTestClass>("S", value);
        }

        [TestMethod]
        public void TestConstructor_NonNullableNumericProperty()
        {
            int value = 1;
            var filter = new SampleEqualityFilter<EqualityFilterTestClass>("L", value);
            Assert.IsInstanceOfType(filter.Value, typeof(long));
            Assert.AreEqual((long)value, filter.Value);
        }

        [TestMethod]
        public void TestConstructor_NullableNumericProperty()
        {
            int value = 1;
            var filter = new SampleEqualityFilter<EqualityFilterTestClass>("NullableL", value);
            Assert.IsInstanceOfType(filter.Value, typeof(long));
            Assert.AreEqual((long)value, filter.Value);
        }

        [TestMethod]
        public void TestConstructor_NonNumericProperty()
        {
            string value = "hello";
            var filter = new SampleEqualityFilter<EqualityFilterTestClass>("S", value);
            Assert.IsInstanceOfType(filter.Value, typeof(string));
            Assert.AreEqual((string)value, filter.Value);
        }
    }
}
