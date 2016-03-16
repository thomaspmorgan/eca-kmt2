using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Business.Sevis.Model;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class USGovtTest
    {
        [TestMethod]
        public void TestGetOtherFundsTypeUSGovt_USGovAgency1Only()
        {
            var agencyCode1 = GovAgencyCodeType.DOE;
            var usGov = new USGovt
            {
                Amount1 = "amount 1",
                Org1 = agencyCode1.ToString()
            };
            var instance = usGov.GetOtherFundsTypeUSGovt();
            Assert.AreEqual(usGov.Amount1, instance.Amount1);
            Assert.AreEqual(agencyCode1, instance.Org1);

            Assert.IsNull(instance.OtherName1);
            Assert.IsNull(instance.OtherName2);
            Assert.IsFalse(instance.Org2Specified);
        }

        [TestMethod]
        public void TestGetOtherFundsTypeUSGovt_USGovAgency1And2()
        {
            var agencyCode1 = GovAgencyCodeType.DOE;
            var agencyCode2 = GovAgencyCodeType.DOD;
            var usGov = new USGovt
            {
                Amount1 = "amount 1",
                Org1 = agencyCode1.ToString(),
                Amount2 = "amount 2",
                Org2 = agencyCode2.ToString()
            };
            var instance = usGov.GetOtherFundsTypeUSGovt();
            Assert.AreEqual(usGov.Amount1, instance.Amount1);
            Assert.AreEqual(agencyCode1, instance.Org1);

            Assert.AreEqual(usGov.Amount2, instance.Amount2);
            Assert.AreEqual(agencyCode2, instance.Org2);

            Assert.IsNull(instance.OtherName1);
            Assert.IsNull(instance.OtherName2);
            Assert.IsTrue(instance.Org2Specified);
        }

        [TestMethod]
        public void TestGetOtherFundsTypeUSGovt_OtherUsGovAgency1()
        {
            var agencyCode1 = GovAgencyCodeType.OTHER;
            var usGov = new USGovt
            {
                Amount1 = "amount 1",
                Org1 = agencyCode1.ToString(),
                OtherName1 = "other 1 name"
            };
            var instance = usGov.GetOtherFundsTypeUSGovt();
            Assert.AreEqual(usGov.Amount1, instance.Amount1);
            Assert.AreEqual(agencyCode1, instance.Org1);
            Assert.AreEqual(usGov.OtherName1, instance.OtherName1);

            Assert.IsNull(instance.OtherName2);
            Assert.IsFalse(instance.Org2Specified);
        }

        [TestMethod]
        public void TestGetOtherFundsTypeUSGovt_OtherUsGovAgency1And2()
        {
            var agencyCode1 = GovAgencyCodeType.OTHER;
            var agencyCode2 = GovAgencyCodeType.OTHER;
            var usGov = new USGovt
            {
                Amount1 = "amount 1",
                Org1 = agencyCode1.ToString(),
                OtherName1 = "other 1 name",

                Amount2 = "amount 2",
                Org2 = agencyCode2.ToString(),
                OtherName2 = "other 2 name"
            };
            var instance = usGov.GetOtherFundsTypeUSGovt();
            Assert.AreEqual(usGov.Amount1, instance.Amount1);
            Assert.AreEqual(agencyCode1, instance.Org1);
            Assert.AreEqual(usGov.OtherName1, instance.OtherName1);

            Assert.AreEqual(usGov.Amount2, instance.Amount2);
            Assert.AreEqual(agencyCode2, instance.Org2);
            Assert.AreEqual(usGov.OtherName2, instance.OtherName2);
            Assert.IsTrue(instance.Org2Specified);
        }
    }
}
