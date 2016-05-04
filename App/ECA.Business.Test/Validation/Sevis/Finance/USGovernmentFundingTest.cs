using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Business.Sevis.Model;
using Newtonsoft.Json;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class USGovernmentFundingTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var org1 = "org1";
            var org2 = "org2";
            var amount1 = "amount 1";
            var amount2 = "amount 2";
            var otherName1 = "other 1";
            var otherName2 = "other 2";
            var instance = new USGovernmentFunding(org1, otherName1, amount1, org2, otherName2, amount2);
            Assert.AreEqual(org1, instance.Org1);
            Assert.AreEqual(org2, instance.Org2);
            Assert.AreEqual(amount1, instance.Amount1);
            Assert.AreEqual(amount2, instance.Amount2);
            Assert.AreEqual(otherName1, instance.OtherName1);
            Assert.AreEqual(otherName2, instance.OtherName2);

            var json = JsonConvert.SerializeObject(instance);
            var jsonTestObject = JsonConvert.DeserializeObject<USGovernmentFunding>(json);
            Assert.AreEqual(org1, jsonTestObject.Org1);
            Assert.AreEqual(org2, jsonTestObject.Org2);
            Assert.AreEqual(amount1, jsonTestObject.Amount1);
            Assert.AreEqual(amount2, jsonTestObject.Amount2);
            Assert.AreEqual(otherName1, jsonTestObject.OtherName1);
            Assert.AreEqual(otherName2, jsonTestObject.OtherName2);
        }

        [TestMethod]
        public void TestJsonSerialization()
        {
            var org1 = "org1";
            var org2 = "org2";
            var amount1 = "amount 1";
            var amount2 = "amount 2";
            var otherName1 = "other 1";
            var otherName2 = "other 2";
            var instance = new USGovernmentFunding(org1, otherName1, amount1, org2, otherName2, amount2);

            var json = JsonConvert.SerializeObject(instance);
            var jsonTestObject = JsonConvert.DeserializeObject<USGovernmentFunding>(json);
            Assert.AreEqual(org1, jsonTestObject.Org1);
            Assert.AreEqual(org2, jsonTestObject.Org2);
            Assert.AreEqual(amount1, jsonTestObject.Amount1);
            Assert.AreEqual(amount2, jsonTestObject.Amount2);
            Assert.AreEqual(otherName1, jsonTestObject.OtherName1);
            Assert.AreEqual(otherName2, jsonTestObject.OtherName2);
        }

        [TestMethod]
        public void TestGetOtherFundsTypeUSGovt_USGovAgency1Only()
        {
            var agencyCode1 = GovAgencyCodeType.DOE;
            var usGov = new USGovernmentFunding(
               org1: agencyCode1.ToString(),
               amount1: "amount 1",
               otherName1: null,
               org2: null,
               amount2: null,
               otherName2: null
               );
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
            var usGov = new USGovernmentFunding(
               org1: agencyCode1.ToString(),
               amount1: "amount 1",
               otherName1: null,
               org2: agencyCode2.ToString(),
               amount2: "amount 2",
               otherName2: null
               );
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
            var usGov = new USGovernmentFunding(
               org1: agencyCode1.ToString(),
               amount1: "amount 1",
               otherName1: "other 1 name",
               org2: null,
               amount2: null,
               otherName2: null
               );
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
            var usGov = new USGovernmentFunding(
               org1: agencyCode1.ToString(),
               amount1: "amount 1",
               otherName1: "other 1 name",
               org2: agencyCode2.ToString(),
               amount2: "amount 2",
               otherName2: "other 2 name"
               );
            var instance = usGov.GetOtherFundsTypeUSGovt();
            Assert.AreEqual(usGov.Amount1, instance.Amount1);
            Assert.AreEqual(agencyCode1, instance.Org1);
            Assert.AreEqual(usGov.OtherName1, instance.OtherName1);

            Assert.AreEqual(usGov.Amount2, instance.Amount2);
            Assert.AreEqual(agencyCode2, instance.Org2);
            Assert.AreEqual(usGov.OtherName2, instance.OtherName2);
            Assert.IsTrue(instance.Org2Specified);
        }

        [TestMethod]
        public void TestGetOtherFundsNullableTypeUSGovt()
        {
            var agencyCode1 = GovAgencyCodeType.BBG;
            var agencyCode2 = GovAgencyCodeType.DOED;

            var usGov = new USGovernmentFunding(
               org1: agencyCode1.ToString(),
               amount1: "amount 1",
               otherName1: "other 1 name",
               org2: agencyCode2.ToString(),
               amount2: "amount 2",
               otherName2: "other 2 name"
               );
            var instance = usGov.GetOtherFundsNullableTypeUSGovt();
            Assert.AreEqual(usGov.Amount1, instance.Amount1);
            Assert.AreEqual(usGov.Org1, instance.Org1);
            Assert.AreEqual(usGov.OtherName1, instance.OtherName1);
            Assert.AreEqual(usGov.Amount2, instance.Amount2);
            Assert.AreEqual(usGov.Org2, instance.Org2);
            Assert.AreEqual(usGov.OtherName2, instance.OtherName2);
        }

        #region GetTotalFunding
        [TestMethod]
        public void TestGetTotalFunding_HasAmount1Funding()
        {
            var agencyCode1 = GovAgencyCodeType.OTHER;
            var agencyCode2 = GovAgencyCodeType.OTHER;
            var usGov = new USGovernmentFunding(
                org1: agencyCode1.ToString(),
                amount1: "1.0",
                otherName1: "other 1 name",
                org2: agencyCode2.ToString(),
                amount2: "amount 2",
                otherName2: "other 2 name"
                );

            Assert.AreEqual(1.0m, usGov.GetTotalFunding());
        }

        [TestMethod]
        public void TestGetTotalFunding_HasNullAmount1Funding()
        {
            var agencyCode1 = GovAgencyCodeType.OTHER;
            var agencyCode2 = GovAgencyCodeType.OTHER;
            var usGov = new USGovernmentFunding(
                org1: agencyCode1.ToString(),
                amount1: null,
                otherName1: "other 1 name",
                org2: agencyCode2.ToString(),
                amount2: "amount 2",
                otherName2: "other 2 name"
                );

            Assert.AreEqual(0.0m, usGov.GetTotalFunding());
        }

        [TestMethod]
        public void TestGetTotalFunding_HasEmptyStringAmount1Funding()
        {
            var agencyCode1 = GovAgencyCodeType.OTHER;
            var agencyCode2 = GovAgencyCodeType.OTHER;
            var usGov = new USGovernmentFunding(
                org1: agencyCode1.ToString(),
                amount1: String.Empty,
                otherName1: "other 1 name",
                org2: agencyCode2.ToString(),
                amount2: "amount 2",
                otherName2: "other 2 name"
                );

            Assert.AreEqual(0.0m, usGov.GetTotalFunding());
        }

        [TestMethod]
        public void TestGetTotalFunding_HasWhitespaceAmount1Funding()
        {
            var agencyCode1 = GovAgencyCodeType.OTHER;
            var agencyCode2 = GovAgencyCodeType.OTHER;
            var usGov = new USGovernmentFunding(
                org1: agencyCode1.ToString(),
                amount1: " ",
                otherName1: "other 1 name",
                org2: agencyCode2.ToString(),
                amount2: "amount 2",
                otherName2: "other 2 name"
                );

            Assert.AreEqual(0.0m, usGov.GetTotalFunding());
        }

        [TestMethod]
        public void TestGetTotalFunding_HasAmount2Funding()
        {
            var agencyCode1 = GovAgencyCodeType.OTHER;
            var agencyCode2 = GovAgencyCodeType.OTHER;
            var usGov = new USGovernmentFunding(
                org1: agencyCode1.ToString(),
                amount1: null,
                otherName1: "other 1 name",
                org2: agencyCode2.ToString(),
                amount2: "1.0",
                otherName2: "Other name 2"
                );

            Assert.AreEqual(1.0m, usGov.GetTotalFunding());
        }

        [TestMethod]
        public void TestGetTotalFunding_HasNullAmount2Funding()
        {
            var agencyCode1 = GovAgencyCodeType.OTHER;
            var agencyCode2 = GovAgencyCodeType.OTHER;
            var usGov = new USGovernmentFunding(
                org1: agencyCode1.ToString(),
                amount1: null,
                otherName1: "other 1 name",
                org2: agencyCode2.ToString(),
                amount2: "amount 2",
                otherName2: null
                );

            Assert.AreEqual(0.0m, usGov.GetTotalFunding());
        }

        [TestMethod]
        public void TestGetTotalFunding_HasEmptyStringAmount2Funding()
        {
            var agencyCode1 = GovAgencyCodeType.OTHER;
            var agencyCode2 = GovAgencyCodeType.OTHER;
            var usGov = new USGovernmentFunding(
                org1: agencyCode1.ToString(),
                amount1: null,
                otherName1: "other 1 name",
                org2: agencyCode2.ToString(),
                amount2: "amount 2",
                otherName2: string.Empty
                );

            Assert.AreEqual(0.0m, usGov.GetTotalFunding());
        }

        [TestMethod]
        public void TestGetTotalFunding_HasWhitespaceAmount2Funding()
        {
            var agencyCode1 = GovAgencyCodeType.OTHER;
            var agencyCode2 = GovAgencyCodeType.OTHER;
            var usGov = new USGovernmentFunding(
                org1: agencyCode1.ToString(),
                amount1: null,
                otherName1: "other 1 name",
                org2: agencyCode2.ToString(),
                amount2: "amount 2",
                otherName2: " "
                );

            Assert.AreEqual(0.0m, usGov.GetTotalFunding());
        }
        #endregion
    }
}

