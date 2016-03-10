using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Finance;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class OtherFundsTest
    {
        [TestMethod]
        public void TestGetOtherFundsType()
        {
            var otherFunds = new OtherFunds
            {
                BinationalCommission = "binational commission",
                EVGovt = "ev govt",
                International = new International(),
                Other = new Other(),
                Personal = "personal",
                USGovt = new USGovt()
            };
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
            var otherFunds = new OtherFunds
            {
                International = null,
                Other = new Other(),
                USGovt = new USGovt()
            };
            var instance = otherFunds.GetOtherFundsType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.International);
        }

        [TestMethod]
        public void TestGetOtherFundsType_NullOther()
        {
            var otherFunds = new OtherFunds
            {
                International = new International(),
                Other = null,
                USGovt = new USGovt()
            };
            var instance = otherFunds.GetOtherFundsType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.Other);
        }

        [TestMethod]
        public void TestGetOtherFundsType_NullUSGovt()
        {
            var otherFunds = new OtherFunds
            {
                International = new International(),
                Other = new Other(),
                USGovt = null
            };
            var instance = otherFunds.GetOtherFundsType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.USGovt);
        }


        [TestMethod]
        public void TestGetOtherFundsNullableType()
        {
            var otherFunds = new OtherFunds
            {
                BinationalCommission = "binational commission",
                EVGovt = "ev govt",
                International = new International(),
                Other = new Other(),
                Personal = "personal",
                USGovt = new USGovt()
            };
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
            var otherFunds = new OtherFunds
            {
                International = null,
                Other = new Other(),
                USGovt = new USGovt()
            };
            var instance = otherFunds.GetOtherFundsNullableType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.International);
        }

        [TestMethod]
        public void TestGetOtherFundsNullableType_NullOther()
        {
            var otherFunds = new OtherFunds
            {
                International = new International(),
                Other = null,
                USGovt = new USGovt()
            };
            var instance = otherFunds.GetOtherFundsNullableType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.Other);
        }

        [TestMethod]
        public void TestGetOtherFundsNullableType_NullUSGovt()
        {
            var otherFunds = new OtherFunds
            {
                International = new International(),
                Other = new Other(),
                USGovt = null
            };
            var instance = otherFunds.GetOtherFundsNullableType();
            Assert.IsNotNull(instance);
            Assert.IsNull(otherFunds.USGovt);
        }
    }
}
