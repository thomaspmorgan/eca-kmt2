using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Finance;
using Newtonsoft.Json;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class OtherTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var amount = "amount";
            var name = "name";
            var other = new Other(name, amount);
            var json = JsonConvert.SerializeObject(other);
            var instance = JsonConvert.DeserializeObject<Other>(json);
            Assert.AreEqual(name, instance.Name);
            Assert.AreEqual(amount, instance.Amount);
        }

        [TestMethod]
        public void TestJsonSerialization()
        {
            var amount = "amount";
            var name = "name";
            var other = new Other(name, amount);

            var json = JsonConvert.SerializeObject(other);
            var instance = JsonConvert.DeserializeObject<Other>(json);
            Assert.AreEqual(name, instance.Name);
            Assert.AreEqual(amount, instance.Amount);
        }

        [TestMethod]
        public void TestGetOtherFundsTypeOther()
        {
            var amount = "amount";
            var name = "name";
            var other = new Other(name: name, amount: amount);

            var instance = other.GetOtherFundsTypeOther();
            Assert.AreEqual(other.Amount, instance.Amount);
            Assert.AreEqual(other.Name, instance.Name);
        }

        [TestMethod]
        public void TestGetOtherFundsNullableTypeInternational()
        {
            var amount = "amount";
            var name = "name";
            var other = new Other(name: name, amount: amount);

            var instance = other.GetOtherFundsNullableTypeInternational();
            Assert.AreEqual(other.Amount, instance.Amount);
            Assert.AreEqual(other.Name, instance.Name);
        }
    }
}
