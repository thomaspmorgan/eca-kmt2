using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class PersonSocialMediaPresenceTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var id = 10;
            var userId = 1;
            var socialMediaTypeId = SocialMediaType.Facebook.Id;
            var value = "facebook.com/someone";
            var user = new User(userId);
            var instance = new PersonSocialMediaPresence(user, socialMediaTypeId, value, id);

            Assert.AreEqual(id, instance.PersonId);
            //Assert the base constructor is called.
            Assert.AreEqual(value, instance.Value);
        }

        [TestMethod]
        public void TestGetSocialableEntityId()
        {
            var id = 10;
            var userId = 1;
            var socialMediaTypeId = SocialMediaType.Facebook.Id;
            var value = "facebook.com/someone";
            var user = new User(userId);
            var instance = new PersonSocialMediaPresence(user, socialMediaTypeId, value, id);

            Assert.AreEqual(id, instance.GetSocialableEntityId());
        }
    }
}
