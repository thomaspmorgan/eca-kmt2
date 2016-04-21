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

        [TestMethod]
        public void TestGetTotalFunding()
        {
            var amount = 1.0m;
            var name = "name";
            var other = new Other(name: name, amount: amount.ToString());
            var total = other.GetTotalFunding();
            Assert.AreEqual(amount, total);
        }

        [TestMethod]
        public void TestGetTotalFunding_WhitespaceAmount()
        {
            var amount = " ";
            var name = "name";
            var other = new Other(name: name, amount: amount.ToString());
            var total = other.GetTotalFunding();
            Assert.AreEqual(0.0m, total);
        }

        [TestMethod]
        public void TestGetTotalFunding_EmptyAmount()
        {
            var amount = String.Empty;
            var name = "name";
            var other = new Other(name: name, amount: amount.ToString());
            var total = other.GetTotalFunding();
            Assert.AreEqual(0.0m, total);
        }

        [TestMethod]
        public void TestGetTotalFunding_NullAmount()
        {
            var name = "name";
            var other = new Other(name: name, amount: null);
            var total = other.GetTotalFunding();
            Assert.AreEqual(0.0m, total);
        }

        [TestMethod]
        public void TestGetTotalFunding_ValueIsNotADouble()
        {
            string amount = "abc";
            var name = "name";
            var other = new Other(name: name, amount: amount.ToString());
            var total = other.GetTotalFunding();
            Assert.AreEqual(0.0m, total);
        }
    }
}
