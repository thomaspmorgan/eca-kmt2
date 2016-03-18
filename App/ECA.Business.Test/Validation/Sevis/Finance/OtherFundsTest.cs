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
            var usGovt = new USGovernmentFunding("us gov 1", null, "us gov value 1", null, null, null);
            var international = new InternationalFunding("international org 1", null, "internation org 1 value", null, null, null);
            var instance = new OtherFunds(evGovt, binationalCommission, personal, usGovt, international, other);

            Assert.AreEqual(binationalCommission, instance.BinationalCommission);
            Assert.AreEqual(personal, instance.Personal);
            Assert.AreEqual(evGovt, instance.ExchangeVisitorGovernment);
            Assert.IsTrue(Object.ReferenceEquals(usGovt, instance.USGovernmentFunding));
            Assert.IsTrue(Object.ReferenceEquals(international, instance.InternationalFunding));
            Assert.IsTrue(Object.ReferenceEquals(other, instance.Other));

            var json = JsonConvert.SerializeObject(instance);
            var jsonObject = JsonConvert.DeserializeObject<OtherFunds>(json);
            Assert.AreEqual(binationalCommission, jsonObject.BinationalCommission);
            Assert.AreEqual(personal, jsonObject.Personal);
            Assert.AreEqual(evGovt, jsonObject.ExchangeVisitorGovernment);
            Assert.IsNotNull(jsonObject.InternationalFunding);
            Assert.IsNotNull(jsonObject.Other);
            Assert.IsNotNull(jsonObject.USGovernmentFunding);
            Assert.AreEqual(usGovt.Org1, jsonObject.USGovernmentFunding.Org1);
            Assert.AreEqual(usGovt.Amount1, jsonObject.USGovernmentFunding.Amount1);
            Assert.AreEqual(international.Org1, jsonObject.InternationalFunding.Org1);
            Assert.AreEqual(international.Amount1, jsonObject.InternationalFunding.Amount1);
        }

        [TestMethod]
        public void TestJsonSerialization()
        {
            var binationalCommission = "commission";
            var personal = "peronsal";
            var evGovt = "ev gov";
            var other = new Other("name", "amount");
            var usGovt = new USGovernmentFunding("us gov 1", null, "us gov value 1", null, null, null);
            var international = new InternationalFunding("international org 1", null, "internation org 1 value", null, null, null);
            var instance = new OtherFunds(evGovt, binationalCommission, personal, usGovt, international, other);

            var json = JsonConvert.SerializeObject(instance);
            var jsonObject = JsonConvert.DeserializeObject<OtherFunds>(json);
            Assert.AreEqual(binationalCommission, jsonObject.BinationalCommission);
            Assert.AreEqual(personal, jsonObject.Personal);
            Assert.AreEqual(evGovt, jsonObject.ExchangeVisitorGovernment);
            Assert.IsNotNull(jsonObject.InternationalFunding);
            Assert.IsNotNull(jsonObject.Other);
            Assert.IsNotNull(jsonObject.USGovernmentFunding);
            Assert.AreEqual(usGovt.Org1, jsonObject.USGovernmentFunding.Org1);
            Assert.AreEqual(usGovt.Amount1, jsonObject.USGovernmentFunding.Amount1);
            Assert.AreEqual(international.Org1, jsonObject.InternationalFunding.Org1);
            Assert.AreEqual(international.Amount1, jsonObject.InternationalFunding.Amount1);
        }

        [TestMethod]
        public void TestGetOtherFundsType()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovernmentFunding usGovt = new USGovernmentFunding(null, null, null, null, null, null);
            InternationalFunding international = new InternationalFunding(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                exchangeVisitorGovernment: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovernmentFunding: usGovt,
                internationalFunding: international,
                other: other);

            var instance = otherFunds.GetOtherFundsType();
            Assert.IsNotNull(instance);
            Assert.AreEqual(otherFunds.BinationalCommission, instance.BinationalCommission);
            Assert.AreEqual(otherFunds.ExchangeVisitorGovernment, instance.EVGovt);
            Assert.AreEqual(otherFunds.Personal, instance.Personal);

            Assert.IsNotNull(otherFunds.InternationalFunding);
            Assert.IsNotNull(otherFunds.Personal);
            Assert.IsNotNull(otherFunds.USGovernmentFunding);
        }

        [TestMethod]
        public void TestGetOtherFundsType_NullInternational()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovernmentFunding usGovt = new USGovernmentFunding(null, null, null, null, null, null);
            InternationalFunding international = null;

            var otherFunds = new OtherFunds(
                exchangeVisitorGovernment: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovernmentFunding: usGovt,
                internationalFunding: international,
                other: other);

            var instance = otherFunds.GetOtherFundsType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.InternationalFunding);
        }

        [TestMethod]
        public void TestGetOtherFundsType_NullOther()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = null;
            USGovernmentFunding usGovt = new USGovernmentFunding(null, null, null, null, null, null);
            InternationalFunding international = new InternationalFunding(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                exchangeVisitorGovernment: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovernmentFunding: usGovt,
                internationalFunding: international,
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
            USGovernmentFunding usGovt = null;
            InternationalFunding international = new InternationalFunding(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                exchangeVisitorGovernment: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovernmentFunding: usGovt,
                internationalFunding: international,
                other: other);
            var instance = otherFunds.GetOtherFundsType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.USGovernmentFunding);
        }


        [TestMethod]
        public void TestGetOtherFundsNullableType()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovernmentFunding usGovt = new USGovernmentFunding(null, null, null, null, null, null);
            InternationalFunding international = new InternationalFunding(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                exchangeVisitorGovernment: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovernmentFunding: usGovt,
                internationalFunding: international,
                other: other);
            var instance = otherFunds.GetOtherFundsNullableType();
            Assert.IsNotNull(instance);
            Assert.AreEqual(otherFunds.BinationalCommission, instance.BinationalCommission);
            Assert.AreEqual(otherFunds.ExchangeVisitorGovernment, instance.EVGovt);
            Assert.AreEqual(otherFunds.Personal, instance.Personal);

            Assert.IsNotNull(otherFunds.InternationalFunding);
            Assert.IsNotNull(otherFunds.Personal);
            Assert.IsNotNull(otherFunds.USGovernmentFunding);
        }

        [TestMethod]
        public void TestGetOtherFundsNullableType_NullInternational()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovernmentFunding usGovt = new USGovernmentFunding(null, null, null, null, null, null);
            InternationalFunding international = null;

            var otherFunds = new OtherFunds(
                exchangeVisitorGovernment: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovernmentFunding: usGovt,
                internationalFunding: international,
                other: other);
            var instance = otherFunds.GetOtherFundsNullableType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.InternationalFunding);
        }

        [TestMethod]
        public void TestGetOtherFundsNullableType_NullOther()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = null;
            USGovernmentFunding usGovt = new USGovernmentFunding(null, null, null, null, null, null);
            InternationalFunding international = new InternationalFunding(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                exchangeVisitorGovernment: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovernmentFunding: usGovt,
                internationalFunding: international,
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
            USGovernmentFunding usGovt = null;
            InternationalFunding international = new InternationalFunding(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                exchangeVisitorGovernment: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovernmentFunding: usGovt,
                internationalFunding: international,
                other: other);
            var instance = otherFunds.GetOtherFundsNullableType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.USGovernmentFunding);
        }
    }
}
