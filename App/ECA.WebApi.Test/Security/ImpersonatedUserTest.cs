using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class ImpersonatedUserTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var impersonatorId = Guid.NewGuid();
            var impersonatedId = Guid.NewGuid();
            var username = "user";
            var user = new ImpersonatedUser(impersonatorId, impersonatedId, username);
            Assert.AreEqual(impersonatorId, user.ImpersonatorUserId);
            Assert.AreEqual(impersonatedId, user.ImpersonatedUserId);
            Assert.AreEqual(username, user.GetUsername());
        }

        [TestMethod]
        public void TestConstructor_EmptyImpersonatorId()
        {
            var impersonatorId = Guid.Empty;
            var impersonatedId = Guid.NewGuid();
            var username = "user";
            Action a = () => new ImpersonatedUser(impersonatorId, impersonatedId, username);
            a.ShouldThrow<ArgumentException>().WithMessage("The id of the impersonator may not be empty.");
        }

        [TestMethod]
        public void TestConstructor_EmptyImpersonatedId()
        {
            var impersonatorId = Guid.NewGuid();
            var impersonatedId = Guid.Empty;
            var username = "user";
            Action a = () => new ImpersonatedUser(impersonatorId, impersonatedId, username);
            a.ShouldThrow<ArgumentException>().WithMessage("The id of the impersonated may not be empty.");
        }

        [TestMethod]
        public void TestToAzureUser()
        {
            var impersonatorId = Guid.NewGuid();
            var impersonatedId = Guid.NewGuid();
            var username = "user";
            var user = new ImpersonatedUser(impersonatorId, impersonatedId, username);
            user.Invoking(x => x.ToAzureUser()).ShouldThrow<NotSupportedException>().WithMessage("This method should not be executed.  A user must exist for it to be impersonated.");
        }
    }
}
