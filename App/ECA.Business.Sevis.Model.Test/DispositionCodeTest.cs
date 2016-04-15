using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Sevis.Model;
using System.Reflection;

namespace ECA.Business.Sevis.Model.Test
{
    [TestClass]
    public class DispositionCodeTest
    {
        [TestMethod]
        public void TestAllStaticPropertiesUnique()
        {
            var t = typeof(DispositionCode);
            var staticProperties = t.GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allComparisonTypes = staticProperties.Select(x => x.GetValue(null) as DispositionCode).ToList();
            foreach (var prop in staticProperties)
            {
                var testValue = (DispositionCode)prop.GetValue(null);
                var allValues = allComparisonTypes.Where(x => x.Code == testValue.Code).ToList();
                var allHashCodes = allComparisonTypes.Where(x => x.GetHashCode() == testValue.GetHashCode()).ToList();

                //check all string values are unique...
                Assert.AreEqual(1, allValues.Count);

                //check all hash codes are unique...
                Assert.AreEqual(1, allHashCodes.Count);

                //check overridden equals are correct...
                var allOtherValues = allComparisonTypes.Where(x => x.Code != testValue.Code).ToList();
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
            var c = DispositionCode.Success;
            Assert.AreEqual(c.Code.GetHashCode(), c.GetHashCode());
        }

        [TestMethod]
        public void TestEquals()
        {
            var c = DispositionCode.Success;
            var otherC = DispositionCode.Success;
            Assert.IsTrue(c.Equals(otherC));
        }

        [TestMethod]
        public void TestEquals_NullTestObject()
        {
            var c = DispositionCode.Success;
            Assert.IsFalse(c.Equals(null));
        }

        [TestMethod]
        public void TestEquals_DifferentTypeObject()
        {
            var c = DispositionCode.Success;
            Assert.IsFalse(c.Equals(1));
        }

        [TestMethod]
        public void TestEqualOperator()
        {
            var c = DispositionCode.Success;
            var otherC = DispositionCode.Success;
            Assert.IsTrue(c == otherC);
        }

        [TestMethod]
        public void TestNotEqualOperator()
        {
            var c = DispositionCode.Success;
            var otherC = DispositionCode.Success;
            Assert.IsFalse(c != otherC);
        }

        [TestMethod]
        public void TestToDispositionCode()
        {
            Assert.AreEqual(DispositionCode.BatchNeverSubmitted, DispositionCode.ToDispositionCode(DispositionCode.BatchNeverSubmitted.Code));
            Assert.AreEqual(DispositionCode.BatchNotYetProcessed, DispositionCode.ToDispositionCode(DispositionCode.BatchNotYetProcessed.Code));
            Assert.AreEqual(DispositionCode.BusinessRuleViolations, DispositionCode.ToDispositionCode(DispositionCode.BusinessRuleViolations.Code));
            Assert.AreEqual(DispositionCode.DocumentNameInvalid, DispositionCode.ToDispositionCode(DispositionCode.DocumentNameInvalid.Code));
            Assert.AreEqual(DispositionCode.DuplicateBatchId, DispositionCode.ToDispositionCode(DispositionCode.DuplicateBatchId.Code));
            Assert.AreEqual(DispositionCode.GeneralUploadDownloadFailure, DispositionCode.ToDispositionCode(DispositionCode.GeneralUploadDownloadFailure.Code));
            Assert.AreEqual(DispositionCode.InvalidOrganizationInformation, DispositionCode.ToDispositionCode(DispositionCode.InvalidOrganizationInformation.Code));
            Assert.AreEqual(DispositionCode.InvalidUserId, DispositionCode.ToDispositionCode(DispositionCode.InvalidUserId.Code));
            Assert.AreEqual(DispositionCode.InvalidXml, DispositionCode.ToDispositionCode(DispositionCode.InvalidXml.Code));
            Assert.AreEqual(DispositionCode.MalformedXml, DispositionCode.ToDispositionCode(DispositionCode.MalformedXml.Code));
            Assert.AreEqual(DispositionCode.Success, DispositionCode.ToDispositionCode(DispositionCode.Success.Code));

        }

        [TestMethod]
        public void TestToDipositionCode_CaseInsensitive()
        {
            var key = DispositionCode.Success.Code.ToLower();
            Assert.AreEqual(DispositionCode.Success, DispositionCode.ToDispositionCode(key));
        }

        [TestMethod]
        public void TestToString()
        {
            Assert.IsNotNull(DispositionCode.Success.ToString());
        }
    }
}
