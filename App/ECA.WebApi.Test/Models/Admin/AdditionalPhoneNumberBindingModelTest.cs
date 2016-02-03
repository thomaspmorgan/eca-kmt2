using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Admin;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class AdditionalPhoneNumberBindingModelTest
    {
        [TestMethod]
        public void TestToNewPhoneNumber()
        {
            var model = new AdditionalPhoneNumberBindingModel();
            model.Number = "5";
            model.PhoneNumberTypeId = PhoneNumberType.Cell.Id;
            var user = new User(1);

            var instance = model.ToNewPhoneNumber(user);
            Assert.AreEqual(model.Number, instance.Number);
            Assert.AreEqual(model.PhoneNumberTypeId, instance.PhoneNumberTypeId);
            Assert.AreEqual(user.Id, instance.Audit.User.Id);
        }
    }
}
