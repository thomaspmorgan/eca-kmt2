using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Service.Admin;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Admin
{
    public class NewPhoneNumberTestClass : NewPhoneNumber<Person>
    {
        public NewPhoneNumberTestClass(User user, int phoneNumberTypeId, string number, bool isPrimary)
            : base(user, phoneNumberTypeId, number, isPrimary)
        {

        }

        public override int GetPhoneNumberableEntityId()
        {
            return 1;
        }
    }

    [TestClass]
    public class NewPhoneNumberTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var number = "1234567890";
            var numberTypeId = PhoneNumberType.Home.Id;
            var isPrimary = true;
            var user = new User(1);
            var model = new NewPhoneNumber(user, numberTypeId, number, isPrimary);
            Assert.AreEqual(number, model.Number);
            Assert.AreEqual(numberTypeId, model.PhoneNumberTypeId);
            Assert.AreEqual(isPrimary, model.IsPrimary);
            Assert.AreEqual(user.Id, model.Audit.User.Id);
            Assert.IsInstanceOfType(model.Audit, typeof(Create));
        }

        [TestMethod]
        public void TestConstructor_GenericArgument()
        {
            var number = "1234567890";
            var numberTypeId = PhoneNumberType.Home.Id;
            var isPrimary = true;
            var user = new User(1);
            var model = new NewPhoneNumberTestClass(user, numberTypeId, number, isPrimary);
            Assert.AreEqual(number, model.Number);
            Assert.AreEqual(numberTypeId, model.PhoneNumberTypeId);
            Assert.AreEqual(isPrimary, model.IsPrimary);
            Assert.AreEqual(user.Id, model.Audit.User.Id);
            Assert.IsInstanceOfType(model.Audit, typeof(Create));
        }

        [TestMethod]
        public void TestAddPhoneNumber()
        {
            var person = new Person();
            var number = "1234567890";
            var numberTypeId = PhoneNumberType.Home.Id;
            var isPrimary = true;
            var user = new User(1);
            var model = new NewPhoneNumberTestClass(user, numberTypeId, number, isPrimary);

            model.AddPhoneNumber(person);
            Assert.AreEqual(1, person.PhoneNumbers.Count);
            var firstPhone = person.PhoneNumbers.First();
            Assert.AreEqual(number, firstPhone.Number);
            Assert.AreEqual(numberTypeId, firstPhone.PhoneNumberTypeId);
            Assert.AreEqual(isPrimary, firstPhone.IsPrimary);

            Assert.AreEqual(user.Id, firstPhone.History.CreatedBy);
            Assert.AreEqual(user.Id, firstPhone.History.RevisedBy);
            DateTimeOffset.Now.Should().BeCloseTo(firstPhone.History.CreatedOn, 20000);
            DateTimeOffset.Now.Should().BeCloseTo(firstPhone.History.RevisedOn, 20000);

        }
    }
}
