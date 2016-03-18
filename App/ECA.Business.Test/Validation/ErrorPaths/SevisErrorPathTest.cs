using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.ErrorPaths;
using System.Reflection;
using ECA.Core.Generation;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class SevisErrorPathTest
    {
        [TestMethod]
        public void TestAllStaticPropertiesUnique()
        {
            var t = typeof(SevisErrorType);
            var staticProperties = t.GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allErrorTypes = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            foreach (var prop in staticProperties)
            {
                var testValue = (StaticLookup)prop.GetValue(null);
                var allValues = allErrorTypes.Where(x => x.Value == testValue.Value).ToList();
                var allHashCodes = allErrorTypes.Where(x => x.GetHashCode() == testValue.GetHashCode()).ToList();

                //check all string values are unique...
                Assert.AreEqual(1, allValues.Count);

                //check all hash codes are unique...
                Assert.AreEqual(1, allHashCodes.Count);

                //check overridden equals are correct...
                var allOtherValues = allErrorTypes.Where(x => x.Value != testValue.Value).ToList();
                allOtherValues.ForEach(x =>
                {
                    Assert.IsFalse(x == testValue);
                    Assert.IsTrue(x != testValue);

                    Assert.IsFalse(x.Equals(testValue));
                });
            }

        }
    }
}
