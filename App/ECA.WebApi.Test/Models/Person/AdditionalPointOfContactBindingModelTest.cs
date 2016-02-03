using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Models.Person;
using System.Collections.Generic;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Person
{
    [TestClass]
    public class AdditionalPointOfContactBindingModelTest
    {
        [TestMethod]
        public void TestToAdditionalPointOfContact()
        {
            var email = "someone@isp.com";
            var emailTypeId = EmailAddressType.Business.Id;
            var phoneNumber = "555";
            var phoneNumberTypeId = PhoneNumberType.Work.Id;

            var newEmailAddress = new AdditionalEmailAddressBindingModel
            {
                Address = email,
                EmailAddressTypeId = emailTypeId
            };
            var newPhoneNumber = new AdditionalPhoneNumberBindingModel
            {
                Number = phoneNumber,
                PhoneNumberTypeId = phoneNumberTypeId
            };
            var model = new AdditionalPointOfContactBindingModel
            {
                EmailAddresses = new List<AdditionalEmailAddressBindingModel> { newEmailAddress },
                PhoneNumbers = new List<AdditionalPhoneNumberBindingModel> { newPhoneNumber },
                FullName = "full Name",
                Position = "position"
            };
            var user = new User(1);

            var instance = model.ToAdditionalPointOfContact(user);
            Assert.AreEqual(user.Id, instance.Audit.User.Id);
            Assert.AreEqual(model.FullName, instance.FullName);
            Assert.AreEqual(model.Position, instance.Position);
            Assert.AreEqual(1, instance.EmailAddresses.Count());
            Assert.AreEqual(1, instance.PhoneNumbers.Count());

            var firstEmail = instance.EmailAddresses.First();
            Assert.AreEqual(email, firstEmail.Address);
            Assert.AreEqual(emailTypeId, firstEmail.EmailAddressTypeId);
            Assert.AreEqual(user.Id, firstEmail.Audit.User.Id);

            var firstPhoneNumber = instance.PhoneNumbers.First();
            Assert.AreEqual(phoneNumber, firstPhoneNumber.Number);
            Assert.AreEqual(phoneNumberTypeId, firstPhoneNumber.PhoneNumberTypeId);
            Assert.AreEqual(user.Id, firstPhoneNumber.Audit.User.Id);
        }
    }
}
