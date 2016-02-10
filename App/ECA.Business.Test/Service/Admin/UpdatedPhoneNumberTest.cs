using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Service.Admin;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class UpdatedPhoneNumberTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var userId = 10;
            var number = "12345";
            var numberTypeId = PhoneNumberType.Home.Id;
            var isPrimary = true;
            var updator = new User(userId);
            var id = 1;

            var model = new UpdatedPhoneNumber(updator, id, number, numberTypeId, isPrimary);
            Assert.AreEqual(userId, model.Audit.User.Id);
            Assert.AreEqual(id, model.Id);
            Assert.AreEqual(number, model.Number);
            Assert.AreEqual(isPrimary, model.IsPrimary);
            Assert.AreEqual(numberTypeId, model.PhoneNumberTypeId);
            Assert.IsInstanceOfType(model.Audit, typeof(Update));
        }
    }
}
