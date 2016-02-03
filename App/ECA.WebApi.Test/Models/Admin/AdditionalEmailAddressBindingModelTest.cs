using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.WebApi.Models.Admin;
using ECA.Data;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class AdditionalEmailAddressBindingModelTest
    {
        [TestMethod]
        public void TestToNewEmailAddress()
        {
            var user = new User(1);
            var model = new AdditionalEmailAddressBindingModel();
            model.Address = "someone@isp.com";
            model.EmailAddressTypeId = EmailAddressType.Business.Id;
            var instance = model.ToNewEmailAddress(user);
            Assert.AreEqual(model.Address, instance.Address);
            Assert.AreEqual(model.EmailAddressTypeId, instance.EmailAddressTypeId);
            Assert.AreEqual(user.Id, instance.Audit.User.Id);
        }
    }
}
