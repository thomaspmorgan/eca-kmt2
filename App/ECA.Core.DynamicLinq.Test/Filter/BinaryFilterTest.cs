using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class SampleBinaryFilter<T> : BinaryFilter<T> where T : class
    {

        public SampleBinaryFilter(string property, object value) : base(property, value)
        {

        }

        public override System.Linq.Expressions.Expression<Func<T, bool>> ToWhereExpression()
        {
            throw new NotImplementedException();
        }
    }

    public class BinaryFilterTestClass
    {
        public long L { get; set; }

        public long NullableL { get; set; }

        public string S { get; set; }
    }

    [TestClass]
    public class BinaryFilterTest
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestConstructor_PropertyTypeIsNumeric_ValueIsNotNumeric()
        {
            string value = "hello";
            var filter = new SampleBinaryFilter<BinaryFilterTestClass>("L", value);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestConstructor_PropertyTypeIsNonNumeric_ValueIsNumeric()
        {
            int value = 1;
            var filter = new SampleBinaryFilter<BinaryFilterTestClass>("S", value);
        }

        [TestMethod]
        public void TestConstructor_NonNullableNumericProperty()
        {
            int value = 1;
            var filter = new SampleBinaryFilter<BinaryFilterTestClass>("L", value);
            Assert.IsInstanceOfType(filter.Value, typeof(long));
            Assert.AreEqual((long)value, filter.Value);            
        }

        [TestMethod]
        public void TestConstructor_NullableNumericProperty()
        {
            int value = 1;
            var filter = new SampleBinaryFilter<BinaryFilterTestClass>("NullableL", value);
            Assert.IsInstanceOfType(filter.Value, typeof(long));
            Assert.AreEqual((long)value, filter.Value);
        }

        [TestMethod]
        public void TestConstructor_NonNumericProperty()
        {
            string value = "hello";
            var filter = new SampleBinaryFilter<BinaryFilterTestClass>("S", value);
            Assert.IsInstanceOfType(filter.Value, typeof(string));
            Assert.AreEqual((string)value, filter.Value);
        }
    }
}
