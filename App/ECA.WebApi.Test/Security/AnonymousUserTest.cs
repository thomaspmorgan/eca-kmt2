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
    }
}
