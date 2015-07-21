﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Admin;
using ECA.Data;
using ECA.Business.Service;
using ECA.Business.Service.Admin;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class OrganizationSocialMediaPresenceBindingModelTest
    {
        [TestMethod]
        public void TestToSocialMediaPresence()
        {
            var userId = 2;
            var user = new User(userId);
            var id = 1;
            var value = "value";
            var socialMediaTypeId = SocialMediaType.Facebook.Id;
            var model = new OrganizationSocialMediaPresenceBindingModel
            {
                SocialableId = id,
                SocialMediaTypeId = socialMediaTypeId,
                Value = value
            };
            var instance = model.ToSocialMediaPresence(user);
            Assert.IsInstanceOfType(instance, typeof(OrganizationSocialMediaPresence));
            var socialMediaPresence = (OrganizationSocialMediaPresence)instance;
            Assert.AreEqual(id, socialMediaPresence.OrganizationId);
            Assert.AreEqual(socialMediaTypeId, socialMediaPresence.SocialMediaTypeId);
            Assert.AreEqual(value, socialMediaPresence.Value);
            Assert.IsTrue(Object.ReferenceEquals(user, socialMediaPresence.Create.User));
        }
    }
}
