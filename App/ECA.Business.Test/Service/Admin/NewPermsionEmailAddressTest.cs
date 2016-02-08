using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class NewPermsionEmailAddressTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var emailTypeId = EmailAddressType.Home.Id;
            var email = "someone@isp.com";
            var isPrimary = true;
            var user = new User(1);
            var personId = 10;
            var model = new NewPersonEmailAddress(user, emailTypeId, email, isPrimary, personId);
            Assert.AreEqual(personId, model.PersonId);
            Assert.AreEqual(personId, model.GetEmailAddressableEntityId());
        }
    }
}
