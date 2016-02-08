using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Admin;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class PersonPhoneNumberBindingModelTest
    {
        [TestMethod]
        public void TestToPhoneNumber()
        {
            var model = new PersonPhoneNumberBindingModel();
            model.IsPrimary = true;
            model.Number = "123";
            model.PhoneNumberableId = 10;
            model.PhoneNumberTypeId = PhoneNumberType.Home.Id;
            var user = new User(1);
            var instance = model.ToPhoneNumber(user);
            Assert.AreEqual(model.IsPrimary, instance.IsPrimary);
            Assert.AreEqual(model.PhoneNumberableId, instance.GetPhoneNumberableEntityId());
            Assert.AreEqual(model.PhoneNumberTypeId, instance.PhoneNumberTypeId);
            Assert.AreEqual(user.Id, instance.Audit.User.Id);
        }
    }
}
