using ECA.Business.Validation;
using ECA.Business.Validation.Sevis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.Sevis
{
    [TestClass]
    public class USAddressTest
    {
        [TestMethod]
        public void TestGetUSAddressDoctorType()
        {
            var address = new USAddress();
            address.Address1 = "1";
            address.Address2 = "2";
            address.City = "city";
            address.State = "FL";
            address.PostalCode = "12345";

            var instance = address.GetUSAddressDoctorType();
            Assert.AreEqual(address.Address1, instance.Address1);
            Assert.AreEqual(address.Address2, instance.Address2);
            Assert.AreEqual(address.City, instance.City);
            Assert.AreEqual(address.State.GetStateCodeType(), instance.State);
            Assert.AreEqual(address.PostalCode, instance.PostalCode);
            Assert.IsNull(instance.Explanation);
            Assert.IsNull(instance.ExplanationCode);
        }
    }
}
