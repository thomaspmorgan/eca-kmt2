using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Finance;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class OrganizationFundingTest
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
            var instance = new OrganizationFunding(org1, otherName1, amount1, org2, otherName2, amount2);
            Assert.AreEqual(org1, instance.Org1);
            Assert.AreEqual(org2, instance.Org2);
            Assert.AreEqual(amount1, instance.Amount1);
            Assert.AreEqual(amount2, instance.Amount2);
            Assert.AreEqual(otherName1, instance.OtherName1);
            Assert.AreEqual(otherName2, instance.OtherName2);
        }

    }
}
