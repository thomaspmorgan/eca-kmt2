using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.Data;
using ECA.Business.Service.Admin;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class UpdatedSocialMediaPresenceTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var userId = 1;
            var user = new User(userId);
            var id = 2;
            var value = "value";
            var socialMediaTypeId = SocialMediaType.Facebook.Id;
            var instance = new UpdatedSocialMediaPresence(user, id, value, socialMediaTypeId);
            Assert.AreEqual(value, instance.Value);
            Assert.AreEqual(id, instance.Id);
            Assert.AreEqual(socialMediaTypeId, instance.SocialMediaTypeId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Update.User));
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownStaticLookupException))]
        public void TestConstructor_UnknownSocialMediaType()
        {
            var id = 10;
            var userId = 1;
            var socialMediaTypeId = -1;
            var value = "facebook.com/someone";
            var user = new User(userId);
            var instance = new UpdatedSocialMediaPresence(user, socialMediaTypeId, value, id);
        }
    }
}
