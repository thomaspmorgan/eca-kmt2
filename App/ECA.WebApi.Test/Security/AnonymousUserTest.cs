using System;
using FluentAssertions;
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
        public void TestToAzureUser()
        {
            var user = new AnonymousUser();
            user.Invoking(x => x.ToAzureUser()).ShouldThrow<NotSupportedException>().WithMessage("This method should not be executed.  An anonymous user must not be inserted into CAM.");
        }
    }
}
