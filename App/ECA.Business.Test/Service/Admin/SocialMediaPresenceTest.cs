using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.Data;
using ECA.Business.Service.Admin;
using System.Collections.Generic;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Admin
{
    public class SocialableTestClass : ISocialable
    {
        public SocialableTestClass()
        {
            this.SocialMedias = new List<SocialMedia>();
        }

        public int Id { get; set; }

        public ICollection<SocialMedia> SocialMedias { get; set; }

        public int GetId()
        {
            return this.Id;
        }
    }

    public class SocialMediaPresenceTestClass : SocialMediaPresence<SocialableTestClass>
    {
        public SocialMediaPresenceTestClass(User user, int socialMediaTypeId, string value, int id)
            :base(user, socialMediaTypeId, value)
        {

        }

        public int Id { get; set; }

        public override int GetSocialableEntityId()
        {
            return Id;
        }
    }

    [TestClass]
    public class SocialMediaPresenceTest
    {
        [TestMethod]
        [ExpectedException(typeof(UnknownStaticLookupException))]
        public void TestConstructor_UnknownSocialMediaType()
        {
            var id = 10;
            var userId = 1;
            var socialMediaTypeId = -1;
            var value = "facebook.com/someone";
            var user = new User(userId);
            var instance = new SocialMediaPresenceTestClass(user, socialMediaTypeId, value, id);
        }

        [TestMethod]
        public void TestConstructor()
        {
            var id = 10;
            var userId = 1;
            var socialMediaTypeId = SocialMediaType.Facebook.Id;
            var value = "facebook.com/someone";
            var user = new User(userId);
            var instance = new SocialMediaPresenceTestClass(user, socialMediaTypeId, value, id);
            Assert.AreEqual(socialMediaTypeId, instance.SocialMediaTypeId);
            Assert.AreEqual(value, instance.Value);
            Assert.IsInstanceOfType(instance.Create, typeof(Create));
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Create.User));
        }

        [TestMethod]
        public void TestAddSocialMediaPresence()
        {
            var id = 10;
            var userId = 1;
            var socialMediaTypeId = SocialMediaType.Facebook.Id;
            var value = "facebook.com/someone";
            var user = new User(userId);
            var instance = new SocialMediaPresenceTestClass(user, socialMediaTypeId, value, id);

            var iSocialable = new SocialableTestClass();
            Assert.AreEqual(0, iSocialable.SocialMedias.Count);

            instance.AddSocialMediaPresence(iSocialable);
            Assert.AreEqual(1, iSocialable.SocialMedias.Count);
            var addedSocialMedia = iSocialable.SocialMedias.First();
            Assert.AreEqual(socialMediaTypeId, addedSocialMedia.SocialMediaTypeId);
            Assert.AreEqual(value, addedSocialMedia.SocialMediaValue);
            Assert.IsTrue(Object.ReferenceEquals(addedSocialMedia, iSocialable.SocialMedias.First()));
        }
    }
}
