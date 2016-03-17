using ECA.Business.Sevis.Model;
using ECA.Business.Validation;
using ECA.Business.Validation.Sevis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.Sevis
{
    [TestClass]
    public class USAddressTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var address1 = "1";
            var address2 = "2";
            var city = "city";
            var state = "FL";
            var postalCode = "12345";
            var explanationCode = "code";
            var explanation = "explanation";
            var address = new USAddress(
                address1,
                address2,
                city,
                state,
                postalCode,
                explanationCode,
                explanation
                );

            Assert.AreEqual(address1, address.Address1);
            Assert.AreEqual(address2, address.Address2);
            Assert.AreEqual(city, address.City);
            Assert.AreEqual(state.GetStateCodeType().ToString(), address.State);
            Assert.AreEqual(postalCode, address.PostalCode);
            Assert.AreEqual(explanation, address.Explanation);
            Assert.AreEqual(explanationCode, address.ExplanationCode);
        }

        [TestMethod]
        public void TestGetUSAddressDoctorType_HasExplanationCode()
        {
            var address1 = "1";
            var address2 = "2";
            var city = "city";
            var state = "FL";
            var postalCode = "12345";
            var explanationCode = USAddrDoctorTypeExplanationCode.OM;
            var explanation = "explanation";
            var address = new USAddress(
                address1: address1,
                address2: address2,
                city: city,
                state: state,
                postalCode: postalCode,
                explanationCode: explanationCode.ToString(),
                explanation: explanation
                );

            var instance = address.GetUSAddressDoctorType();
            Assert.AreEqual(address.Address1, instance.Address1);
            Assert.AreEqual(address.Address2, instance.Address2);
            Assert.AreEqual(address.City, instance.City);
            Assert.AreEqual(address.State.GetStateCodeType(), instance.State);
            Assert.AreEqual(address.PostalCode, instance.PostalCode);
            Assert.AreEqual(address.Explanation, instance.Explanation);

            Assert.IsTrue(instance.ExplanationCodeSpecified);
            Assert.AreEqual(explanationCode, instance.ExplanationCode);
        }

        [TestMethod]
        public void TestGetUSAddressDoctorType_DoesNotHaveExplanationCode()
        {
            var address1 = "1";
            var address2 = "2";
            var city = "city";
            var state = "FL";
            var postalCode = "12345";
            var explanation = "explanation";
            var address = new USAddress(
                address1: address1,
                address2: address2,
                city: city,
                state: state,
                postalCode: postalCode,
                explanationCode: null,
                explanation: explanation
                );

            var instance = address.GetUSAddressDoctorType();

            Assert.IsFalse(instance.ExplanationCodeSpecified);
            Assert.IsNull(instance.ExplanationCode);
        }
    }
}
