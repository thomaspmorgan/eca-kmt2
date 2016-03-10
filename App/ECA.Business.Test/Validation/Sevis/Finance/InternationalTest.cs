using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Business.Sevis.Model;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class InternationalTest
    {

        [TestMethod]
        public void TestGetOtherFundsTypeInternational_InternationalOrg1Only()
        {
            var agencyCode1 = InternationalOrgCodeType.EEC;
            var international = new International
            {
                Amount1 = "amount 1",
                Org1 = agencyCode1.ToString()
            };
            var instance = international.GetOtherFundsTypeInternational();
            Assert.AreEqual(international.Amount1, instance.Amount1);
            Assert.AreEqual(agencyCode1, instance.Org1);

            Assert.IsNull(instance.OtherName1);
            Assert.IsNull(instance.OtherName2);
            Assert.IsFalse(instance.Org2Specified);
        }

        [TestMethod]
        public void TestGetOtherFundsTypeInternational_InternationalOrg1And2()
        {
            var agencyCode1 = InternationalOrgCodeType.EEC;
            var agencyCode2 = InternationalOrgCodeType.ESCAP;
            var international = new International
            {
                Amount1 = "amount 1",
                Org1 = agencyCode1.ToString(),
                Amount2 = "amount 2",
                Org2 = agencyCode2.ToString()
            };
            var instance = international.GetOtherFundsTypeInternational();
            Assert.AreEqual(international.Amount1, instance.Amount1);
            Assert.AreEqual(agencyCode1, instance.Org1);

            Assert.AreEqual(international.Amount2, instance.Amount2);
            Assert.AreEqual(agencyCode2, instance.Org2);

            Assert.IsNull(instance.OtherName1);
            Assert.IsNull(instance.OtherName2);
            Assert.IsTrue(instance.Org2Specified);

        }

        [TestMethod]
        public void TestGetOtherFundsTypeInternational_OtherInternationalOrgAgency1()
        {
            var agencyCode1 = InternationalOrgCodeType.OTHER;
            var international = new International
            {
                Amount1 = "amount 1",
                Org1 = agencyCode1.ToString(),
                OtherName1 = "other 1 name"
            };
            var instance = international.GetOtherFundsTypeInternational();
            Assert.AreEqual(international.Amount1, instance.Amount1);
            Assert.AreEqual(agencyCode1, instance.Org1);
            Assert.AreEqual(international.OtherName1, instance.OtherName1);

            Assert.IsNull(instance.OtherName2);
            Assert.IsFalse(instance.Org2Specified);
        }

        [TestMethod]
        public void TestGetOtherFundsTypeInternational_OtherInternationalOrg1And2()
        {
            var agencyCode1 = InternationalOrgCodeType.OTHER;
            var agencyCode2 = InternationalOrgCodeType.OTHER;
            var international = new International
            {
                Amount1 = "amount 1",
                Org1 = agencyCode1.ToString(),
                OtherName1 = "other 1 name",

                Amount2 = "amount 2",
                Org2 = agencyCode2.ToString(),
                OtherName2 = "other 2 name"
            };
            var instance = international.GetOtherFundsTypeInternational();
            Assert.AreEqual(international.Amount1, instance.Amount1);
            Assert.AreEqual(agencyCode1, instance.Org1);
            Assert.AreEqual(international.OtherName1, instance.OtherName1);

            Assert.AreEqual(international.Amount2, instance.Amount2);
            Assert.AreEqual(agencyCode2, instance.Org2);
            Assert.AreEqual(international.OtherName2, instance.OtherName2);
            Assert.IsTrue(instance.Org2Specified);
        }

        [TestMethod]
        public void TestGetOtherFundsNullableTypeUSGovt()
        {
            var international = new International
            {
                Amount1 = "amount 1",
                Org1 = "DOE",
                OtherName1 = "other 1 name",

                Amount2 = "amount 2",
                Org2 = "USDA",
                OtherName2 = "other 2 name"
            };
            var instance = international.GetOtherFundsNullableTypeInternational();
            Assert.AreEqual(international.Amount1, instance.Amount1);
            Assert.AreEqual(international.Org1, instance.Org1);
            Assert.AreEqual(international.OtherName1, instance.OtherName1);
            Assert.AreEqual(international.Amount2, instance.Amount2);
            Assert.AreEqual(international.Org2, instance.Org2);
            Assert.AreEqual(international.OtherName2, instance.OtherName2);
        }
    }
}
