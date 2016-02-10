using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Admin;
using ECA.Business.Service;
using ECA.Data;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class UpdatedPhoneNumberBindingModelTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var model = new UpdatedPhoneNumberBindingModel
            {
                Number = "123-456-7890",
                PhoneNumberTypeId = PhoneNumberType.Home.Id,
                Id = 100,
                IsPrimary = true
            };
            var user = new User(10);
            var instance = model.ToUpdatedPhoneNumber(user);
            Assert.AreEqual(model.Number, instance.Number);
            Assert.AreEqual(model.PhoneNumberTypeId, instance.PhoneNumberTypeId);
            Assert.AreEqual(model.Id, instance.Id);
            Assert.AreEqual(model.IsPrimary, instance.IsPrimary);
            Assert.AreEqual(user.Id, instance.Audit.User.Id);
        }
    }
}
