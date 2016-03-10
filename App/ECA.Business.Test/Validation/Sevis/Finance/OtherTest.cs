using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Finance;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class OtherTest
    {
        [TestMethod]
        public void TestGetOtherFundsTypeOther()
        {
            var other = new Other();
            other.Amount = "amount";
            other.Name = "name";

            var instance = other.GetOtherFundsTypeOther();
            Assert.AreEqual(other.Amount, instance.Amount);
            Assert.AreEqual(other.Name, instance.Name);
        }

        [TestMethod]
        public void TestGetOtherFundsNullableTypeInternational()
        {
            var other = new Other();
            other.Amount = "amount";
            other.Name = "name";

            var instance = other.GetOtherFundsNullableTypeInternational();
            Assert.AreEqual(other.Amount, instance.Amount);
            Assert.AreEqual(other.Name, instance.Name);
        }
    }
}
