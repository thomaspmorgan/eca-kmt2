using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Admin;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class PersonEmailAddressBindingModelTest
    {
        [TestMethod]
        public void TestToToEmailAddress()
        {
            var model = new PersonEmailAddressBindingModel();
            model.Address = "someone@isp.com";
            model.EMailAddressableId = 10;
            model.EmailAddressTypeId = EmailAddressType.Home.Id;
            model.IsPrimary = true;
            var user = new User(1);

            var instance = model.ToEmailAddress(user);
            Assert.AreEqual(model.Address, instance.Address);
            Assert.AreEqual(model.EMailAddressableId, instance.GetEmailAddressableEntityId());
            Assert.AreEqual(model.EmailAddressTypeId, instance.EmailAddressTypeId);
            Assert.AreEqual(model.IsPrimary, instance.IsPrimary);
        }
    }
}
