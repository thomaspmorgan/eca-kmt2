using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.WebApi.Models.Admin;
using ECA.Data;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class UpdatedSocialMediaBindingModelTest
    {
        [TestMethod]
        public void TestToUpdatedSocialMediaPresence()
        {
            var userId = 2;
            var user = new User(userId);
            var id = 1;
            var value = "value";
            var socialMediaTypeId = SocialMediaType.Facebook.Id;
            var model = new UpdatedSocialMediaBindingModel
            {
                Id = id,
                SocialMediaTypeId = socialMediaTypeId,
                Value = value
            };
            var instance = model.ToUpdatedSocialMediaPresence(user);
            Assert.AreEqual(id, instance.Id);
            Assert.AreEqual(socialMediaTypeId, instance.SocialMediaTypeId);
            Assert.AreEqual(value, instance.Value);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Update.User));
        }
    }
}
