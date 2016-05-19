using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class NewPersonPhoneNumberTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var number = "1234567890";
            var ext = "123";
            var numberTypeId = PhoneNumberType.Home.Id;
            var isPrimary = true;
            var user = new User(1);
            var personId = 10;
            var model = new NewPersonPhoneNumber(user, numberTypeId, number, ext, personId, isPrimary);
            Assert.AreEqual(number, model.Number);
            Assert.AreEqual(ext, model.Extension);
            Assert.AreEqual(numberTypeId, model.PhoneNumberTypeId);
            Assert.AreEqual(isPrimary, model.IsPrimary);
            Assert.AreEqual(user.Id, model.Audit.User.Id);
            Assert.IsInstanceOfType(model.Audit, typeof(Create));
            Assert.AreEqual(personId, model.PersonId);
        }

        [TestMethod]
        public void TestGetPhoneNumberableEntityId()
        {
            var number = "1234567890";
            var ext = "123";
            var numberTypeId = PhoneNumberType.Home.Id;
            var isPrimary = true;
            var user = new User(1);
            var personId = 10;
            var model = new NewPersonPhoneNumber(user, numberTypeId, number, ext, personId, isPrimary);
            Assert.AreEqual(personId, model.GetPhoneNumberableEntityId());
        }
    }
}
