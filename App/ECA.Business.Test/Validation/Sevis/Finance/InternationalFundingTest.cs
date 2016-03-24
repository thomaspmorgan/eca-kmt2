using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Business.Sevis.Model;
using Newtonsoft.Json;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class InternationalFundingTest
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
            var international = new InternationalFunding(org1, otherName1, amount1, org2, otherName2, amount2);
            Assert.AreEqual(org1, international.Org1);
            Assert.AreEqual(org2, international.Org2);
            Assert.AreEqual(amount1, international.Amount1);
            Assert.AreEqual(amount2, international.Amount2);
            Assert.AreEqual(otherName1, international.OtherName1);
            Assert.AreEqual(otherName2, international.OtherName2);
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
            var international = new InternationalFunding(org1, otherName1, amount1, org2, otherName2, amount2);

            var json = JsonConvert.SerializeObject(international);
            var jsonTestObject = JsonConvert.DeserializeObject<InternationalFunding>(json);
            Assert.AreEqual(org1, jsonTestObject.Org1);
            Assert.AreEqual(org2, jsonTestObject.Org2);
            Assert.AreEqual(amount1, jsonTestObject.Amount1);
            Assert.AreEqual(amount2, jsonTestObject.Amount2);
            Assert.AreEqual(otherName1, jsonTestObject.OtherName1);
            Assert.AreEqual(otherName2, jsonTestObject.OtherName2);
        }


        [TestMethod]
        public void TestGetOtherFundsTypeInternational_InternationalOrg1Only()
        {
            var agencyCode1 = InternationalOrgCodeType.EEC;
            var international = new InternationalFunding(
                org1: agencyCode1.ToString(),
                amount1: "amount 1",
                otherName1: null,
                org2: null,
                amount2: null,
                otherName2: null
                );

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
            var international = new InternationalFunding(
                org1: agencyCode1.ToString(),
                amount1: "amount 1",
                otherName1: null,
                org2: agencyCode2.ToString(),
                amount2: "amount 2",
                otherName2: null
                );
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
            var international = new InternationalFunding(
                org1: agencyCode1.ToString(),
                amount1: "amount 1",
                otherName1: "other 1 name",
                org2: null,
                amount2: null,
                otherName2: null
                );
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

            var international = new InternationalFunding(
                org1: agencyCode1.ToString(),
                amount1: "amount 1",
                otherName1: "other 1 name",
                org2: agencyCode2.ToString(),
                amount2: "amount 2",
                otherName2: "other 2 name"
                );
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

            var agencyCode1 = InternationalOrgCodeType.ECOSOC;
            var agencyCode2 = InternationalOrgCodeType.EEC;
            var international = new InternationalFunding(
                org1: agencyCode1.ToString(),
                amount1: "amount 1",
                otherName1: "other 1 name",
                org2: agencyCode2.ToString(),
                amount2: "amount 2",
                otherName2: "other 2 name"
                );

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
