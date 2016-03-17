using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Finance;
using Newtonsoft.Json;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class OtherFundsTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var binationalCommission = "commission";
            var personal = "peronsal";
            var evGovt = "ev gov";
            var other = new Other("name", "amount");
            var usGovt = new USGovt("us gov 1", null, "us gov value 1", null, null, null);
            var international = new International("international org 1", null, "internation org 1 value", null, null, null);
            var instance = new OtherFunds(evGovt, binationalCommission, personal, usGovt, international, other);

            Assert.AreEqual(binationalCommission, instance.BinationalCommission);
            Assert.AreEqual(personal, instance.Personal);
            Assert.AreEqual(evGovt, instance.EVGovt);
            Assert.IsTrue(Object.ReferenceEquals(usGovt, instance.USGovt));
            Assert.IsTrue(Object.ReferenceEquals(international, instance.International));
            Assert.IsTrue(Object.ReferenceEquals(other, instance.Other));

            var json = JsonConvert.SerializeObject(instance);
            var jsonObject = JsonConvert.DeserializeObject<OtherFunds>(json);
            Assert.AreEqual(binationalCommission, jsonObject.BinationalCommission);
            Assert.AreEqual(personal, jsonObject.Personal);
            Assert.AreEqual(evGovt, jsonObject.EVGovt);
            Assert.IsNotNull(jsonObject.International);
            Assert.IsNotNull(jsonObject.Other);
            Assert.IsNotNull(jsonObject.USGovt);
            Assert.AreEqual(usGovt.Org1, jsonObject.USGovt.Org1);
            Assert.AreEqual(usGovt.Amount1, jsonObject.USGovt.Amount1);
            Assert.AreEqual(international.Org1, jsonObject.International.Org1);
            Assert.AreEqual(international.Amount1, jsonObject.International.Amount1);
        }

        [TestMethod]
        public void TestJsonSerialization()
        {
            var binationalCommission = "commission";
            var personal = "peronsal";
            var evGovt = "ev gov";
            var other = new Other("name", "amount");
            var usGovt = new USGovt("us gov 1", null, "us gov value 1", null, null, null);
            var international = new International("international org 1", null, "internation org 1 value", null, null, null);
            var instance = new OtherFunds(evGovt, binationalCommission, personal, usGovt, international, other);

            var json = JsonConvert.SerializeObject(instance);
            var jsonObject = JsonConvert.DeserializeObject<OtherFunds>(json);
            Assert.AreEqual(binationalCommission, jsonObject.BinationalCommission);
            Assert.AreEqual(personal, jsonObject.Personal);
            Assert.AreEqual(evGovt, jsonObject.EVGovt);
            Assert.IsNotNull(jsonObject.International);
            Assert.IsNotNull(jsonObject.Other);
            Assert.IsNotNull(jsonObject.USGovt);
            Assert.AreEqual(usGovt.Org1, jsonObject.USGovt.Org1);
            Assert.AreEqual(usGovt.Amount1, jsonObject.USGovt.Amount1);
            Assert.AreEqual(international.Org1, jsonObject.International.Org1);
            Assert.AreEqual(international.Amount1, jsonObject.International.Amount1);
        }

        [TestMethod]
        public void TestGetOtherFundsType()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovt usGovt = new USGovt(null, null, null, null, null, null);
            International international = new International(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);

            var instance = otherFunds.GetOtherFundsType();
            Assert.IsNotNull(instance);
            Assert.AreEqual(otherFunds.BinationalCommission, instance.BinationalCommission);
            Assert.AreEqual(otherFunds.EVGovt, instance.EVGovt);
            Assert.AreEqual(otherFunds.Personal, instance.Personal);

            Assert.IsNotNull(otherFunds.International);
            Assert.IsNotNull(otherFunds.Personal);
            Assert.IsNotNull(otherFunds.USGovt);
        }

        [TestMethod]
        public void TestGetOtherFundsType_NullInternational()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovt usGovt = new USGovt(null, null, null, null, null, null);
            International international = null;

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);

            var instance = otherFunds.GetOtherFundsType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.International);
        }

        [TestMethod]
        public void TestGetOtherFundsType_NullOther()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = null;
            USGovt usGovt = new USGovt(null, null, null, null, null, null);
            International international = new International(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);
            var instance = otherFunds.GetOtherFundsType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.Other);
        }

        [TestMethod]
        public void TestGetOtherFundsType_NullUSGovt()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovt usGovt = null;
            International international = new International(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);
            var instance = otherFunds.GetOtherFundsType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.USGovt);
        }


        [TestMethod]
        public void TestGetOtherFundsNullableType()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovt usGovt = new USGovt(null, null, null, null, null, null);
            International international = new International(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);
            var instance = otherFunds.GetOtherFundsNullableType();
            Assert.IsNotNull(instance);
            Assert.AreEqual(otherFunds.BinationalCommission, instance.BinationalCommission);
            Assert.AreEqual(otherFunds.EVGovt, instance.EVGovt);
            Assert.AreEqual(otherFunds.Personal, instance.Personal);

            Assert.IsNotNull(otherFunds.International);
            Assert.IsNotNull(otherFunds.Personal);
            Assert.IsNotNull(otherFunds.USGovt);
        }

        [TestMethod]
        public void TestGetOtherFundsNullableType_NullInternational()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovt usGovt = new USGovt(null, null, null, null, null, null);
            International international = null;

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);
            var instance = otherFunds.GetOtherFundsNullableType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.International);
        }

        [TestMethod]
        public void TestGetOtherFundsNullableType_NullOther()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = null;
            USGovt usGovt = new USGovt(null, null, null, null, null, null);
            International international = new International(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);
            var instance = otherFunds.GetOtherFundsNullableType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.Other);
        }

        [TestMethod]
        public void TestGetOtherFundsNullableType_NullUSGovt()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovt usGovt = null;
            International international = new International(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);
            var instance = otherFunds.GetOtherFundsNullableType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.USGovt);
        }
    }
}
