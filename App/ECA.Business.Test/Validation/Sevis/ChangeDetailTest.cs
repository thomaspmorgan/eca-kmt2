using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KellermanSoftware.CompareNetObjects;
using Microsoft.QualityTools.Testing.Fakes;
using ECA.Business.Validation.Sevis;

namespace ECA.Business.Test.Validation.Sevis
{
    [TestClass]
    public class ChangeDetailTest
    {
        [TestMethod]
        public void TestConstructor_ComparisonResult_AreNotEqual()
        {
            using (ShimsContext.Create())
            {
                var result = new KellermanSoftware.CompareNetObjects.Fakes.ShimComparisonResult
                {
                    AreEqualGet = () => false
                };

                var instance = new ChangeDetail(result);
                Assert.IsTrue(instance.HasChanges());
            }
        }

        [TestMethod]
        public void TestConstructor_ComparisonResult_AreEqual()
        {
            using (ShimsContext.Create())
            {
                var result = new KellermanSoftware.CompareNetObjects.Fakes.ShimComparisonResult
                {
                    AreEqualGet = () => true
                };

                var instance = new ChangeDetail(result);
                Assert.IsFalse(instance.HasChanges());
            }
        }

        [TestMethod]
        public void TestConstructor_Bool()
        {
            var expected = true;
            var instance = new ChangeDetail(expected);
            Assert.AreEqual(expected, instance.HasChanges());

            expected = false;
            instance = new ChangeDetail(expected);
            Assert.AreEqual(expected, instance.HasChanges());
        }
    }
}
