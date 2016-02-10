using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Data;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class UpdatedEmailAddressTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var email = "someone@isp.com";
            var id = 1;
            var user = new User(5);
            var isPrimary = true;
            var emailAddressTypeId = EmailAddressType.HostEmergency.Id;

            var model = new UpdatedEmailAddress(user, id, email, emailAddressTypeId, isPrimary);
            Assert.AreEqual(id, model.Id);
            Assert.AreEqual(email, model.Address);
            Assert.AreEqual(emailAddressTypeId, model.EmailAddressTypeId);
            Assert.AreEqual(isPrimary, model.IsPrimary);
            Assert.AreEqual(user.Id, model.Audit.User.Id);
            Assert.IsInstanceOfType(model.Audit, typeof(Update));
        }
    }
}
