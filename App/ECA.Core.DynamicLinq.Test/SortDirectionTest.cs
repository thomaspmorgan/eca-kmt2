using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace ECA.Core.DynamicLinq.Test
{
    [TestClass]
    public class SortDirectionTest
    {
        [TestMethod]
        public void TestAllStaticPropertiesUnique()
        {
            var t = typeof(SortDirection);
            var staticProperties = t.GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allSortDirections = staticProperties.Select(x => x.GetValue(null) as SortDirection).ToList();
            foreach (var prop in staticProperties)
            {
                var testValue = (SortDirection)prop.GetValue(null);
                var allValues = allSortDirections.Where(x => x.Value == testValue.Value).ToList();
                var allHashCodes = allSortDirections.Where(x => x.GetHashCode() == testValue.GetHashCode()).ToList();

                //check all string values are unique...
                Assert.AreEqual(1, allValues.Count);

                //check all hash codes are unique...
                Assert.AreEqual(1, allHashCodes.Count);

                //check overridden equals are correct...
                var allOtherValues = allSortDirections.Where(x => x.Value != testValue.Value).ToList();
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
            var d = SortDirection.Ascending;
            Assert.AreEqual(d.Value.GetHashCode(), d.GetHashCode());
        }

        [TestMethod]
        public void TestEquals()
        {
            var d = SortDirection.Ascending;
            var otherC = SortDirection.Ascending;
            Assert.IsTrue(d.Equals(otherC));
        }

        [TestMethod]
        public void TestEquals_NullTestObject()
        {
            var d = SortDirection.Ascending;
            Assert.IsFalse(d.Equals(null));
        }

        [TestMethod]
        public void TestEquals_DifferentTypeObject()
        {
            var d = SortDirection.Ascending;
            Assert.IsFalse(d.Equals(1));
        }

        [TestMethod]
        public void TestEqualOperator()
        {
            var d = SortDirection.Ascending;
            var otherD = SortDirection.Ascending;
            Assert.IsTrue(d == otherD);
        }

        [TestMethod]
        public void TestNotEqualOperator()
        {
            var d = SortDirection.Ascending;
            var otherD = SortDirection.Ascending;
            Assert.IsFalse(d != otherD);
        }

        [TestMethod]
        public void TestToSortDirection_Ascending()
        {
            var key = SortDirection.Ascending.Value;
            var testDirection = SortDirection.ToSortDirection(key);
            Assert.AreEqual(SortDirection.Ascending, testDirection);

        }

        [TestMethod]
        public void TestToSortDirection_Descending()
        {
            var key = SortDirection.Descending.Value;
            var testDirection = SortDirection.ToSortDirection(key);
            Assert.AreEqual(SortDirection.Descending, testDirection);
        }

        [TestMethod]
        public void TestToSortDirection_CaseInsensitiveTest()
        {
            var key = "AsC";
            Assert.AreEqual(SortDirection.Ascending, SortDirection.ToSortDirection(key));
        }
    }
}
