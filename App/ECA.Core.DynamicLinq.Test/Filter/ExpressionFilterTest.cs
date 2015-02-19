using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class ExpressionFilterTestClass
    {
        public int Id { get; set; }

        public DateTime? NullableDate { get; set; }
    }

    [TestClass]
    public class ExpressionFilterTest
    {
        [TestMethod]
        public void TestConstructor_IntProperty()
        {
            var value = 1;
            var comparisonType = ComparisonType.Equal;
            var filter = new ExpressionFilter<ExpressionFilterTestClass>(x => x.Id, comparisonType, value);
            Assert.AreEqual(value, filter.Value);
            Assert.AreEqual("Id", filter.Property);
            Assert.AreEqual(comparisonType.Value, filter.Comparison);
        }
    }
}
