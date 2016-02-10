using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Admin;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class UpdatedEmailAddressBindingModelTest
    {
        [TestMethod]
        public void TestToUpdatedEmailAddress()
        {
            var model = new UpdatedEmailAddressBindingModel
            {
                Address = "someone@isp.com",
                EmailAddressTypeId = EmailAddressType.Home.Id,
                Id = 100,
                IsPrimary = true
            };
            var user = new User(10);
            var instance = model.ToUpdatedEmailAddress(user);
            Assert.AreEqual(model.Address, instance.Address);
            Assert.AreEqual(model.EmailAddressTypeId, instance.EmailAddressTypeId);
            Assert.AreEqual(model.Id, instance.Id);
            Assert.AreEqual(model.IsPrimary, instance.IsPrimary);
            Assert.AreEqual(user.Id, instance.Audit.User.Id);
        }
    }
}
