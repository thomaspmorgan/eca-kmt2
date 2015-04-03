using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace ECA.Core.DynamicLinq.Test
{
    [TestClass]
    public class ComparisonTypeTest
    {
        [TestMethod]
        public void TestAllStaticPropertiesUnique()
        {
            var t = typeof(ComparisonType);
            var staticProperties = t.GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allComparisonTypes = staticProperties.Select(x => x.GetValue(null) as ComparisonType).ToList();
            foreach (var prop in staticProperties)
            {
                var testValue = (ComparisonType)prop.GetValue(null);
                var allValues = allComparisonTypes.Where(x => x.Value == testValue.Value).ToList();
                var allHashCodes = allComparisonTypes.Where(x => x.GetHashCode() == testValue.GetHashCode()).ToList();

                //check all string values are unique...
                Assert.AreEqual(1, allValues.Count);

                //check all hash codes are unique...
                Assert.AreEqual(1, allHashCodes.Count);

                //check overridden equals are correct...
                var allOtherValues = allComparisonTypes.Where(x => x.Value != testValue.Value).ToList();
                allOtherValues.ForEach(x =>
                {
                    Assert.IsFalse(x == testValue);
                    Assert.IsTrue(x != testValue);

                    Assert.IsFalse(x.Equals(testValue));
                });
            }

        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var c = ComparisonType.Equal;
            Assert.AreEqual(c.Value.GetHashCode(), c.GetHashCode());
        }

        [TestMethod]
        public void TestEquals()
        {
            var c = ComparisonType.Equal;
            var otherC = ComparisonType.Equal;
            Assert.IsTrue(c.Equals(otherC));
        }

        [TestMethod]
        public void TestEquals_NullTestObject()
        {
            var c = ComparisonType.Equal;
            Assert.IsFalse(c.Equals(null));
        }

        [TestMethod]
        public void TestEquals_DifferentTypeObject()
        {
            var c = ComparisonType.Equal;
            Assert.IsFalse(c.Equals(1));
        }

        [TestMethod]
        public void TestEqualOperator()
        {
            var c = ComparisonType.Equal;
            var otherC = ComparisonType.Equal;
            Assert.IsTrue(c == otherC);
        }

        [TestMethod]
        public void TestNotEqualOperator()
        {
            var c = ComparisonType.Equal;
            var otherC = ComparisonType.Equal;
            Assert.IsFalse(c != otherC);
        }

        [TestMethod]
        public void TestToComparisonType()
        {
            Assert.AreEqual(ComparisonType.Equal, ComparisonType.ToComparisonType(ComparisonType.Equal.Value));
            Assert.AreEqual(ComparisonType.GreaterThan, ComparisonType.ToComparisonType(ComparisonType.GreaterThan.Value));
            Assert.AreEqual(ComparisonType.In, ComparisonType.ToComparisonType(ComparisonType.In.Value));
            Assert.AreEqual(ComparisonType.LessThan, ComparisonType.ToComparisonType(ComparisonType.LessThan.Value));
            Assert.AreEqual(ComparisonType.Like, ComparisonType.ToComparisonType(ComparisonType.Like.Value));
            Assert.AreEqual(ComparisonType.NotNull, ComparisonType.ToComparisonType(ComparisonType.NotNull.Value));
            Assert.AreEqual(ComparisonType.Null, ComparisonType.ToComparisonType(ComparisonType.Null.Value));
            Assert.AreEqual(ComparisonType.NotIn, ComparisonType.ToComparisonType(ComparisonType.NotIn.Value));
        }

        [TestMethod]
        public void TestToComparisonType_CaseInsensitive()
        {
            var key = "eQ";
            Assert.AreEqual(ComparisonType.Equal, ComparisonType.ToComparisonType(key));
        }

        [TestMethod]
        public void TestToString()
        {
            Assert.IsNotNull(ComparisonType.Equal.ToString());
        }
    }
}
