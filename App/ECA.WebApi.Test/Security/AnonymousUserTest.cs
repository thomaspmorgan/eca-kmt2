using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using CAM.Business.Service;
using System.Collections.Generic;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class AnonymousUserTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var user = new AnonymousUser();
            Assert.AreEqual(Guid.Empty, user.Id);
        }

        [TestMethod]
        public void TestGetUsername()
        {
            var user = new AnonymousUser();
            Assert.AreEqual(AnonymousUser.ANONYMOUS_USER_NAME, user.GetUsername());
        }

        [TestMethod]
        public void TestHasPermission()
        {
            var user = new AnonymousUser();
            Assert.IsFalse(user.HasPermission(null, null));

            var permission = new Permission
            {

            };
            Assert.IsFalse(user.HasPermission(permission, new List<Permission> { permission }));
        }
    }
}
